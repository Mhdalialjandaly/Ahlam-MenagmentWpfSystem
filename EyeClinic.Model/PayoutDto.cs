using System;

#nullable disable

namespace EyeClinic.Model
{
    public class PayoutDto
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public int Amount { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
