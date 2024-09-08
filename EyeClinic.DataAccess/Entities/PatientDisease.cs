using System;
using System.Collections.Generic;

#nullable disable

namespace EyeClinic.DataAccess.Entities
{
    public partial class PatientDisease : IEntityModel
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DiseaseId { get; set; }
        public string AgeOfInjury { get; set; }
        public string MaxMeasure { get; set; }
        public string LastCheckMeasure { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        public virtual Disease Disease { get; set; }
        public virtual Patient Patient { get; set; }
    }
}
