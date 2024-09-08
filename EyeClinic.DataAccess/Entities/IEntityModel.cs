using System;

namespace EyeClinic.DataAccess.Entities
{
    public interface IEntityModel
    {
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string DeletedBy { get; set; }
    }
}
