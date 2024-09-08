using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeClinic.Model
{
    public class OrderQueueDto
    {
        public int Id { get; set; }
        public int PublicCustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? UpComingDate { get; set; }
        public int Cost { get; set; }
        public int Payment { get; set; }
        public int Remaining { get; set; }
        public byte OrderStatus { get; set; }
        public int OrderIndex { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
      
    }
}
