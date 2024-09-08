using EyeClinic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeClinic.DataAccess.Entities
{
    public class PublicCustomer : IEntityModel
    {
        public PublicCustomer()
        {         
            CustomerOrders = new HashSet<PublicCustomerOrder>();
            Queues = new HashSet<Queue>();
        }
        public int Id { get; set; }
        public int Number { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string Type { get; set; }       
       
        public string HomePhone { get; set; }
        public string WorkPhone { get; set; }
        public string Nationality { get; set; }
        public int LocationId { get; set; }      
        public string Notes { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public bool Referral { get; set; }
        public int? TempId { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public virtual Location Location { get; set; }           
        public virtual CustomerNoteStory CustomerNoteStory  { get; set; } 
        public virtual ICollection<PublicCustomerOrder> CustomerOrders { get; set; }
        public virtual ICollection<Queue> Queues { get; set; }
    }
}
