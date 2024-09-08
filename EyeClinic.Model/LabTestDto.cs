using System;

namespace EyeClinic.Model
{
    public class LabTestDto
    {
        public int Id { get; set; }
        public string LabTestName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }


        public override string ToString() {
            return LabTestName;
        }
    }
}
