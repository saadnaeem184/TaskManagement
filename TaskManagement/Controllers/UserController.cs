using Application.Services;
using Application.Services.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagement.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly UserManager<AppUser> _userManager;

        public UserController(UserManager<AppUser> userManager, IEmployeeService employeeService)
        {
            _userManager = userManager;
            _employeeService = employeeService;
        }

        //[Authorize(Roles = "Employer")]
        [HttpGet("employees")]
        public async Task<IActionResult> Get()
        {
            var employees = await _employeeService.GetAllEmployees();

            return Ok(employees);
        }
    }
}
