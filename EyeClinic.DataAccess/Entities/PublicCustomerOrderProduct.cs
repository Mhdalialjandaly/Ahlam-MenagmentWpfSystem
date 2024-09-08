using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeClinic.DataAccess.Entities
{
  
        public partial class PublicCustomerOrderProduct : IEntityModel
        {
            public int Id { get; set; }
            public int PublicCustomerOrderId { get; set; }
            public int ProductId { get; set; }
            public int ProductDescriptionId { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime? LastModifiedDate { get; set; }
            public int RowIndex { get; set; }
            public string DeletedBy { get; set; }
            public DateTime? DeletedDate { get; set; }

            public virtual Product Product { get; set; }
            public virtual ProductDescription ProductDescription { get; set; }
            public virtual PublicCustomerOrder PublicCustomerOrder { get; set; }
        
    }
}
