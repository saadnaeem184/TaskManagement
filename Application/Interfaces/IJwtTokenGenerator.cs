using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    /// <summary>
    /// Interface for generating JWT tokens.
    /// </summary>
    public interface IJwtTokenGenerator
    {
        string GenerateToken(AppUser user, IEnumerable<string> roles, IEnumerable<Claim>? additionalClaims = null);
    }
}
