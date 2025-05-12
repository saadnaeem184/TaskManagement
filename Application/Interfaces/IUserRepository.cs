﻿using Application.DTOs.Employee;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        //Task<AppUser?> GetByIdAsync(int id);
        Task<List<AppUser>> GetAllEmployeesAsync();
    }
}
