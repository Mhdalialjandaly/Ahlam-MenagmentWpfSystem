using System;
using System.Collections.Generic;

#nullable disable

namespace EyeClinic.DataAccess.Entities
{
    public partial class PatientVisitDiagnosis : IEntityModel
    {
        public int Id { get; set; }
        public int PatientVisitId { get; set; }
        public int DiagnosisId { get; set; }
        public bool LeftEye { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        public virtual Diagnosis Diagnosis { get; set; }
        public virtual PatientVisit PatientVisit { get; set; }
    }
}
