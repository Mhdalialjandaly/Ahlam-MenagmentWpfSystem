using System;

namespace EyeClinic.Model
{
    public class PatientVisitLabTestDto
    {
        public int Id { get; set; }
        public int PatientVisitId { get; set; }
        public int LabTestId { get; set; }
        public string Result { get; set; }
        public bool Done { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }


        public PatientVisitDto PatientVisit { get; set; }
        public LabTestDto LabTest { get; set; }


        public DateTime RequestDate { get; set; }
    }
}
