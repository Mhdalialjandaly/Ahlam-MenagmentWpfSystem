using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeClinic.DataAccess.Entities
{
    
        public partial class ProductDescription : IEntityModel
        {
            public ProductDescription()
            {
                PublicCustomerOrderProducts = new HashSet<PublicCustomerOrderProduct>();
                ReadyProductOrders = new HashSet<ReadyProductOrder>();
            }

            public int Id { get; set; }
            public string Code { get; set; }
            public string DescriptionName { get; set; }
            public int ProductDescriptionDepartmentId { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime? LastModifiedDate { get; set; }
            public string DeletedBy { get; set; }
            public DateTime? DeletedDate { get; set; }

            public virtual ProductDepartment DescriptionProductDepartment { get; set; }
            public virtual ICollection<PublicCustomerOrderProduct> PublicCustomerOrderProducts { get; set; }
            public virtual ICollection<ReadyProductOrder> ReadyProductOrders { get; set; }
        }
    
}
