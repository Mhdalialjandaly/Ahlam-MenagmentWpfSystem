using System;

namespace EyeClinic.Model
{
    public class OldMedicineViewTableDto
    {
        public int Id { get; set; }
        public int PatientVisitId { get; set; }
        public DateTime VisitDate { get; set; }
        public string MedicineName { get; set; }
        public string MedicineType { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? Index { get; set; }
        public int RowIndex { get; set; }

        public PatientVisitDto PatientVisit { get; set; }
    }
}
