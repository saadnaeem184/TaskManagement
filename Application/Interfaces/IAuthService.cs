using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    namespace Application.Contracts.Identity
    {
        /// <summary>
        /// Interface for authentication services.
        /// </summary>
        public interface IAuthService
        {
            Task<ServiceResponse<AuthResponse>> RegisterAsync(RegisterRequest model);
            Task<ServiceResponse<AuthResponse>> LoginAsync(LoginRequest model);
            // Potentially add methods for refreshing tokens, forgot password, etc.
        }
    }
}
