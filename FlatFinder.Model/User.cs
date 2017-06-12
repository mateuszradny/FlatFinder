using System.Collections.Generic;

namespace FlatFinder.Model
{
    public class User : EntityBase
    {
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public byte[] Salt { get; set; }
        public string PhoneNumber { get; set; }

        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
    }
}