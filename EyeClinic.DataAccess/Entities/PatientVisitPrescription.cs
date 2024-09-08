using System;
using System.Collections.Generic;

#nullable disable

namespace EyeClinic.DataAccess.Entities
{
    public partial class PatientVisitPrescription : IEntityModel
    {
        public int Id { get; set; }
        public int PatientVisitId { get; set; }
        public int MedicineId { get; set; }
        public int MedicineUsageId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int RowIndex { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        public virtual Medicine Medicine { get; set; }
        public virtual MedicineUsage MedicineUsage { get; set; }
        public virtual PatientVisit PatientVisit { get; set; }
    }
}
