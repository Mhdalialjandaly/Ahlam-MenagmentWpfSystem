using System;
using System.Collections.Generic;

#nullable disable

namespace EyeClinic.DataAccess.Entities
{
    public partial class Contact : IEntityModel
    {
        public Contact()
        {
            ContactAccountPayments = new HashSet<ContactAccountPayment>();
            ContactAccounts = new HashSet<ContactAccount>();
        }

        public int Id { get; set; }
        public string ContactName { get; set; }
        public string ContactPhones { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string CompanyType { get; set; }
        public string EmailAdress { get; set; }
        public string WebSite { get; set; }
        public string note { get; set; }
        public virtual ICollection<ContactAccountPayment> ContactAccountPayments { get; set; }
        public virtual ICollection<ContactAccount> ContactAccounts { get; set; }
    }
}
