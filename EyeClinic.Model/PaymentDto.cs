using System;

namespace EyeClinic.Model
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public int PaymentTypeId { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime? ReminderDate { get; set; }
        public int PaymentValue { get; set; }
        public string Notes { get; set; }
        public bool Paid { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public PaymentTypeDto PaymentType { get; set; }
    }
}
