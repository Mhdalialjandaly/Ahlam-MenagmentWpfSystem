using System;
using System.Collections.Generic;

#nullable disable

namespace EyeClinic.DataAccess.Entities
{
    public partial class AccountPaymentCategory : IEntityModel
    {
        public AccountPaymentCategory()
        {
            ContactAccountPayments = new HashSet<ContactAccountPayment>();
        }

        public int Id { get; set; }
        public string CategoryName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string DeletedBy { get; set; }


        public virtual ICollection<ContactAccountPayment> ContactAccountPayments { get; set; }
    }
}
