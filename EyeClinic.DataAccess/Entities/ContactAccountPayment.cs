using System;
using System.Collections.Generic;

#nullable disable

namespace EyeClinic.DataAccess.Entities
{
    public partial class ContactAccountPayment : IEntityModel
    {
        public int Id { get; set; }
        public int? ContactAccountId { get; set; }
        public DateTime PaymentDate { get; set; }
        public int PaymentValue { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int ContactId { get; set; }
        public bool PayoutTransaction { get; set; }
        public int CategoryId { get; set; }

        public virtual AccountPaymentCategory Category { get; set; }
        public virtual Contact Contact { get; set; }
        public virtual ContactAccount ContactAccount { get; set; }
    }
}
