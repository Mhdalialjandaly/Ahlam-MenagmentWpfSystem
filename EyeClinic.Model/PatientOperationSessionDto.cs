using System;

namespace EyeClinic.Model
{
    public class PatientOperationSessionDto
    {
        public int Id { get; set; }
        public int PatientOperationId { get; set; }
        public DateTime SessionDate { get; set; }
        public int PaymentValue { get; set; }
        public string Note { get; set; }
        public int SessionNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }


        public PatientOperationDto PatientOperation { get; set; }
    }
}
