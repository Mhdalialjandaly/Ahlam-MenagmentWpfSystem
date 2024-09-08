using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeClinic.DataAccess.Entities
{
    public partial class MarketsEntry :IEntityModel
    {
        public int Id { get; set; }
        public double Quinttity { get; set; }
        public string UnitName { get; set; }
        public int CodeNumber { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int MarketsProductsId { get; set; }
        
    }
}
