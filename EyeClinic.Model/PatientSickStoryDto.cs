using System;

namespace EyeClinic.Model
{
    public class PatientSickStoryDto
    {
        public int PatientId { get; set; }
        public string SickStory { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
