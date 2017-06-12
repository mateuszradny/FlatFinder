using System.Collections.Generic;

namespace FlatFinder.Model
{
    public class Role : EntityBase
    {
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}