using System;

namespace EyeClinic.Model
{
    public class PatientVisitEyeTestDto
    {
        public int Id { get; set; }
        public int PatientVisitId { get; set; }
        public int EyeTestId { get; set; }
        public string LeftEyeResult { get; set; }
        public string RightEyeResult { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public PatientVisitDto PatientVisit { get; set; }
        public EyeTestDto EyeTest { get; set; }
    }
}
