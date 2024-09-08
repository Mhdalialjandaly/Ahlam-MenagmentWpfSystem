using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeClinic.Model
{
    public partial class CustomerDto
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public string AccountName { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool PayOutAccount { get; set; }
        public int Remaining { get; set; }
    }
}
