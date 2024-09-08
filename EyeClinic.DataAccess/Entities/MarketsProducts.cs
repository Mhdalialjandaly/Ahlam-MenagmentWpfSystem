using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeClinic.DataAccess.Entities
{
    public partial class MarketsProducts :IEntityModel
    {
        public int Id { get; set; }
        public string MarketsProductName { get; set; }
        public double FirstTermValue { get; set; }
        public double TheMinimumValue { get; set; }
        public double Entry { get; set; }
        public double Extry { get; set; }
        public double RealValue { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        public virtual ICollection<MarketsEntry> MarketsEntries { get; set; }
        public virtual ICollection<MarketsExtry> MarketsExtries { get; set; }
    }
}
