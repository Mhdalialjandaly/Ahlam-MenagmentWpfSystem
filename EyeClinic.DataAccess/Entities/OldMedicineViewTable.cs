using System;
using System.Collections.Generic;

#nullable disable

namespace EyeClinic.DataAccess.Entities
{
    public partial class OldMedicineViewTable : IEntityModel
    {
        public int Id { get; set; }
        public int PatientVisitId { get; set; }
        public DateTime VisitDate { get; set; }
        public string MedicineName { get; set; }
        public string MedicineType { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? Index { get; set; }
        public int? TempPrescriptionId { get; set; }

        public virtual PatientVisit PatientVisit { get; set; }
    }
}
