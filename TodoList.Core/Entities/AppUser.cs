using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TodoList.Core.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
