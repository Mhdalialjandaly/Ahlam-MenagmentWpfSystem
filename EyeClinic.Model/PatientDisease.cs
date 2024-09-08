using System;

namespace EyeClinic.Model
{
    public class PatientDisease
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DiseaseId { get; set; }
        public string AgeOfInjury { get; set; }
        public string MaxMeasure { get; set; }
        public string LastCheckMeasure { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public DiseaseDto Disease { get; set; }
    }
}
