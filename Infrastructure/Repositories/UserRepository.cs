using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AppUser>> GetAllEmployeesAsync()
        {
            List<AppUser> users = new List<AppUser>();
            //Get Role Id.
            var roleId = await _context.Roles.Where(x => x.Name.Equals(UserRoles.Employee.ToString())).FirstOrDefaultAsync();

            if(roleId != null)
            {
                var userIds = await _context.UserRoles.Where(x => x.RoleId.Equals(roleId.Id)).Select(i=>i.UserId).ToListAsync();
                if (userIds != null && userIds.Count > 0)
                {
                    users =  await _context.Users.Where(x=>userIds.Contains(x.Id)).ToListAsync();
                }
            }
            return users;
        }
    }
}
