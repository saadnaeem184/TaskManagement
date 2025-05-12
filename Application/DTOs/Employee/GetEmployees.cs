using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Employee
{
    public class GetEmployees
    {
        public string Id { get; set; }
        public string? name { get; set; }
        public string? email { get; set; }
        public string? role { get; set; }
    }
}
