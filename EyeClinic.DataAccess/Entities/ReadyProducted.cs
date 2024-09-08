using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeClinic.DataAccess.Entities
{
    
        public partial class ReadyProducted : IEntityModel
        {
            public ReadyProducted()
            {
                ReadyProductOrders = new HashSet<ReadyProductOrder>();
            }

            public int Id { get; set; }
            public string EnName { get; set; }
            public string ArName { get; set; }
            public bool Disabled { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime? LastModifiedDate { get; set; }
            public string DeletedBy { get; set; }
            public DateTime? DeletedDate { get; set; }

            public virtual ICollection<ReadyProductOrder> ReadyProductOrders { get; set; }
        }
    
}
