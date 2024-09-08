using System;
using System.Collections.Generic;

#nullable disable

namespace EyeClinic.DataAccess.Entities
{
    public partial class Diagnosis : IEntityModel
    {
        public Diagnosis()
        {
            PatientVisitDiagnoses = new HashSet<PatientVisitDiagnosis>();
        }

        public int Id { get; set; }
        public string EnName { get; set; }
        public string ArName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string Code { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        public virtual ICollection<PatientVisitDiagnosis> PatientVisitDiagnoses { get; set; }
    }
}
