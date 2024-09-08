using System;
using System.Collections.Generic;

namespace EyeClinic.Model
{
    public class ContactAccountDto
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public string AccountName { get; set; }
        public int TotalCost { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool PayOutAccount { get; set; }
        public int Remaining { get; set; }

        public ContactDto Contact { get; set; }
        public List<ContactAccountPaymentDto> ContactAccountPayments { get; set; }


        public override string ToString() {
            if (Id == -1)
                return AccountName;

            return $"{AccountName} - Type : {(PayOutAccount ? "Payout" : "Receive")} - Remaining : {Remaining}";
        }
    }
}
