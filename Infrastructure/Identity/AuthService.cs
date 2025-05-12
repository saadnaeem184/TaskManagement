using Application.DTOs;
using Application.Interfaces.Application.Contracts.Identity;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Infrastructure.Identity
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager; // For managing roles
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly JwtSettings _jwtSettings; // For accessing DurationInMinutes directly

        public AuthService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager, // Injected RoleManager
            IJwtTokenGenerator jwtTokenGenerator,
            IOptions<JwtSettings> jwtOptions) // Injected IOptions<JwtSettings>
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager; // Initialize RoleManager
            _jwtTokenGenerator = jwtTokenGenerator;
            _jwtSettings = jwtOptions.Value; // Get JwtSettings value
        }

        public async Task<ServiceResponse<AuthResponse>> LoginAsync(LoginRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Avoid indicating whether the email or password was wrong for security
                return new ServiceResponse<AuthResponse>(false, "Invalid credentials.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                // Avoid indicating whether the email or password was wrong
                return new ServiceResponse<AuthResponse>(false, "Invalid credentials.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenGenerator.GenerateToken(user, roles);

            var authResponse = new AuthResponse
            {
                UserId = user.Id,
                Username = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes), // Used injected settings
                Roles = roles
            };

            return new ServiceResponse<AuthResponse>(authResponse, message: "Login successful.");
        }

        public async Task<ServiceResponse<AuthResponse>> RegisterAsync(RegisterRequest model)
        {
            // Validate Role from the DTO
            if (string.IsNullOrWhiteSpace(model.Role) || (model.Role != UserRoles.Employer.ToString() && model.Role != UserRoles.Employee.ToString()))
            {
                return new ServiceResponse<AuthResponse>(false, "Invalid role specified. Must be 'Employer' or 'Employee'.");
            }

            var existingUserByEmail = await _userManager.FindByEmailAsync(model.Email);
            if (existingUserByEmail != null)
            {
                return new ServiceResponse<AuthResponse>(false, "Email already exists.", new List<string> { "An account with this email address already exists." });
            }

            var existingUserByUsername = await _userManager.FindByNameAsync(model.Username);
            if (existingUserByUsername != null)
            {
                return new ServiceResponse<AuthResponse>(false, "Username already exists.", new List<string> { "This username is already taken." });
            }

            var newUser = new AppUser
            {
                Email = model.Email,
                UserName = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailConfirmed = true // Set to true for simplicity; implement email confirmation for production
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return new ServiceResponse<AuthResponse>(false, "User registration failed.", errors);
            }

            // Ensure the role exists (roles should be seeded at startup, but this is a fallback)
            if (!await _roleManager.RoleExistsAsync(model.Role))
            {
                // Log this occurrence, as roles should ideally be pre-seeded
                // For production, you might not want to auto-create roles here.
                await _roleManager.CreateAsync(new IdentityRole(model.Role));
            }
            // Add user to the specified role
            var addToRoleResult = await _userManager.AddToRoleAsync(newUser, model.Role);
            if (!addToRoleResult.Succeeded)
            {
                // Handle failure to add role (e.g., log it, potentially rollback user creation or inform admin)
                var roleErrors = addToRoleResult.Errors.Select(e => e.Description).ToList();
                // For now, we'll proceed but return an error message indicating role assignment failed
                // You might want to delete the user if role assignment is critical
                return new ServiceResponse<AuthResponse>(false, $"User registered, but failed to assign role '{model.Role}'.", roleErrors);
            }


            var roles = await _userManager.GetRolesAsync(newUser); // Get roles to include in the response
            var token = _jwtTokenGenerator.GenerateToken(newUser, roles);

            var authResponse = new AuthResponse
            {
                UserId = newUser.Id,
                Username = newUser.UserName ?? string.Empty,
                Email = newUser.Email ?? string.Empty,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes), // Used injected settings
                Roles = roles
            };

            return new ServiceResponse<AuthResponse>(authResponse, message: $"User registered successfully and assigned to role: {model.Role}");
        }
    }
}
