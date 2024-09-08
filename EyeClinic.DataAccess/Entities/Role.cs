using System;
using System.Collections.Generic;

#nullable disable

namespace EyeClinic.DataAccess.Entities
{
    public partial class Role : IEntityModel
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public DateTime PassWrong { get; set; }
        public string EnName { get; set; }
        public string ArName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
