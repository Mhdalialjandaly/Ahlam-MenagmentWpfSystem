using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeClinic.DataAccess.Entities
{
   
        public partial class Product : IEntityModel
        {
            public Product()
            {
                PublicCustomerOrderProducts = new HashSet<PublicCustomerOrderProduct>();
                ReadyProductOrders = new HashSet<ReadyProductOrder>();
            }

            public int Id { get; set; }
            public string Code { get; set; }
            public string ProductName { get; set; }
            public int ProductDepartmentId { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime? LastModifiedDate { get; set; }
            public string DeletedBy { get; set; }
            public DateTime? DeletedDate { get; set; }

            public virtual ProductDepartment ProductDepartment { get; set; }
            public virtual ICollection<PublicCustomerOrderProduct> PublicCustomerOrderProducts { get; set; }
            public virtual ICollection<ReadyProductOrder> ReadyProductOrders { get; set; }
        }
    
}
