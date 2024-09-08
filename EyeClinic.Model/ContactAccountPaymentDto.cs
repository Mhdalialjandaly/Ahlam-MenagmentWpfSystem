using System;

namespace EyeClinic.Model
{
    public class ContactAccountPaymentDto
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

        public AccountPaymentCategoryDto Category { get; set; }
        public ContactDto Contact { get; set; }
        public ContactAccountDto ContactAccount { get; set; }


        public string PayType => PayoutTransaction ? "Pay" : "Receive";
    }
}
