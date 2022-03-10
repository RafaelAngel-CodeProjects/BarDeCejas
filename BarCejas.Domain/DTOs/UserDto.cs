using System;

using BarCejas.Core.Enumerations;

namespace BarCejas.Core.DTOs
{
    public class UserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBrith { get; set; }
        public string Telephone { get; set; }
        public string Password { get; set; }
        public RoleType Role { get; set; }
    }
}
