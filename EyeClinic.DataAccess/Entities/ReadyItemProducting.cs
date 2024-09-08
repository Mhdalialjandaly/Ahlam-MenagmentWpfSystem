using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeClinic.DataAccess.Entities
{
    public partial class ReadyItemProducting :IEntityModel
    {
        public ReadyItemProducting() { }
        public int Id { get; set; }
        public string EnName { get; set; }
        public string ArName { get; set; }
        public double ProductingValue { set; get; }
        public string Code { get; set; }
        public string DeletedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }       
        public DateTime? DeletedDate { get; set; }


    }
}
