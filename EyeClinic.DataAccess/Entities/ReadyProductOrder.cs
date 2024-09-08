using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeClinic.DataAccess.Entities
{
   
        public partial class ReadyProductOrder : IEntityModel
        {
            public int Id { get; set; }
            public int ReadyProductedId { get; set; }
            public int ProductId { get; set; }
            public int ProductDescriptionId { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime? LastModifiedDate { get; set; }
            public string DeletedBy { get; set; }
            public DateTime? DeletedDate { get; set; }

            public virtual Product Product { get; set; }
            public virtual ProductDescription GetProductDescription { get; set; }
            public virtual ReadyProducted ReadyProducted { get; set; }
        }
    
}
