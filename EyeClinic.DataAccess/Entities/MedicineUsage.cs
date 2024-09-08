using System;
using System.Collections.Generic;

#nullable disable

namespace EyeClinic.DataAccess.Entities
{
    public partial class MedicineUsage : IEntityModel
    {
        public MedicineUsage()
        {
            PatientVisitPrescriptions = new HashSet<PatientVisitPrescription>();
            ReadyPrescriptionMedicines = new HashSet<ReadyPrescriptionMedicine>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string UsageName { get; set; }
        public int UsageMedicineTypeId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        public virtual MedicineType UsageMedicineType { get; set; }
        public virtual ICollection<PatientVisitPrescription> PatientVisitPrescriptions { get; set; }
        public virtual ICollection<ReadyPrescriptionMedicine> ReadyPrescriptionMedicines { get; set; }
    }
}
