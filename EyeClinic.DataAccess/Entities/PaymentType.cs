using System;
using System.Collections.Generic;

#nullable disable

namespace EyeClinic.DataAccess.Entities
{
    public partial class PaymentType : IEntityModel
    {
        public PaymentType()
        {
            Payments = new HashSet<Payment>();
        }

        public int Id { get; set; }
        public string TypeName { get; set; }
        public string BeneficiaryName { get; set; }
        public int TotalCost { get; set; }
        public string Note { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public bool? Debt { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int Remaining { get; set; }
        public int? TotalPayments { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
    }
}
