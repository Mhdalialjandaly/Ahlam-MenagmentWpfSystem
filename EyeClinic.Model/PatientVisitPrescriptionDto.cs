using System;

namespace EyeClinic.Model
{
    public class PatientVisitPrescriptionDto
    {
        public int Id { get; set; }
        public int PatientVisitId { get; set; }
        public int MedicineId { get; set; }
        public int MedicineUsageId { get; set; }
        public int RowIndex { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public MedicineDto Medicine { get; set; }
        public MedicineUsageDto MedicineUsage { get; set; }
        public PatientVisitDto PatientVisit { get; set; }
    }
}
