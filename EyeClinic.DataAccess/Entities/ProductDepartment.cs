using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeClinic.DataAccess.Entities
{
  
        public partial class ProductDepartment : IEntityModel
        {
            public ProductDepartment()
            {
                ProductDescriptions = new HashSet<ProductDescription>();
                Products = new HashSet<Product>();
            }

            public int Id { get; set; }
            public string EnName { get; set; }
            public string ArName { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime? LastModifiedDate { get; set; }
            public string DeletedBy { get; set; }
            public DateTime? DeletedDate { get; set; }

            public virtual ICollection<ProductDescription> ProductDescriptions { get; set; }
            public virtual ICollection<Product> Products { get; set; }
        }
    
}
