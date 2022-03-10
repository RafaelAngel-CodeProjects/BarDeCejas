using BarCejas.Core.Enumerations;

using System;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BarCejas.Core.Entities
{
    public partial class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBrith { get; set; }
        public string Telephone { get; set; }
        public string Password { get; set; }
        public RoleType Role { get; set; }
        public string Gender { get; set; }
        public bool IsActive { get; set; }
    }
}
