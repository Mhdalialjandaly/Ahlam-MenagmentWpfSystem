using System;

namespace EyeClinic.Model
{
    public class PatientVisitDiagnosis
    {
        public int Id { get; set; }
        public int PatientVisitId { get; set; }
        public int DiagnosisId { get; set; }
        public bool LeftEye { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }


        public PatientVisitDto PatientVisit { get; set; }
        public DiagnosisDto Diagnosis { get; set; }
    }
}
