using Application.DTOs.Employee;
using Application.DTOs.TaskItemDTOs;
using Application.Interfaces;
using Application.Services.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUserRepository _userRepo;

        public EmployeeService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<List<GetEmployees>> GetAllEmployees()
        {
            var appUsers = await _userRepo.GetAllEmployeesAsync();
            return appUsers.Select(t => MapUser(t)).ToList();
        }

        private GetEmployees MapUser(AppUser user) => new GetEmployees
        {
            Id = user.Id,
            name = user.FirstName,
            email = user.Email,
        };
    }
}
