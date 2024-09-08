using System;
using System.Collections.Generic;

namespace EyeClinic.Model
{
    public class PaymentTypeDto
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public string BeneficiaryName { get; set; }
        public int TotalCost { get; set; }
        public bool Debt { get; set; }
        public int? TotalPayments { get; set; }
        public int? Remaining { get; set; }
        public string Note { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public List<PaymentDto> Payments { get; set; }
    }
}
