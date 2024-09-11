using System;
using System.Collections.Generic;

#nullable disable

namespace EyeClinic.DataAccess.Entities
{
    public partial class Medicine : IEntityModel
    {
        public Medicine()
        {
            PatientVisitPrescriptions = new HashSet<PatientVisitPrescription>();
            ReadyPrescriptionMedicines = new HashSet<ReadyPrescriptionMedicine>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string MedicineName { get; set; }
        public int MedicineTypeId { get; set; }
        public double MedicinCount { get; set; }
        public double AvailibleMedicinCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        public virtual MedicineType MedicineType { get; set; }
        public virtual ICollection<PatientVisitPrescription> PatientVisitPrescriptions { get; set; }
        public virtual ICollection<ReadyPrescriptionMedicine> ReadyPrescriptionMedicines { get; set; }
    }
}
