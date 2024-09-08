using System;
using System.Collections.Generic;

#nullable disable

namespace EyeClinic.DataAccess.Entities
{
    public partial class PatientSickStory : IEntityModel
    {
        public int PatientId { get; set; }
        public string SickStory { get; set; }
      
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        public virtual Patient Patient { get; set; }
    }
}
