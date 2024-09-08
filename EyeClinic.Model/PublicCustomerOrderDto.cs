using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeClinic.Model
{
    public class PublicCustomerOrderDto
    {

        public int Id { get; set; }
        public int PublicCustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? UpComingDate { get; set; }
        public byte OrderStatus { get; set; }
        public int OrderIndex { get; set; }
        public int QueueIndex { get; set; }
        public string Notes { get; set; }
        public string Why { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string PublicCustomerOrderType { get; set; }
        public PublicCustomerDto PublicCustomer { set; get; } 
    }
}
