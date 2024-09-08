using System;
using System.Collections.Generic;

#nullable disable

namespace EyeClinic.DataAccess.Entities
{
    public partial class ContactAccount : IEntityModel
    {
        public ContactAccount()
        {
            ContactAccountPayments = new HashSet<ContactAccountPayment>();
        }

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

        public virtual Contact Contact { get; set; }
        public virtual ICollection<ContactAccountPayment> ContactAccountPayments { get; set; }
    }
}
