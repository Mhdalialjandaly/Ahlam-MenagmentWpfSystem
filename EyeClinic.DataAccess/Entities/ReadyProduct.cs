using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeClinic.DataAccess.Entities
{
   

        public partial class ReadyProduct : IEntityModel
        {
            public ReadyProduct()
            {

            }

            public int Id { get; set; }
            public string ArName { get; set; }
            public int ProductId { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime? LastModifiedDate { get; set; }
            public string DeletedBy { get; set; }
            public DateTime? DeletedDate { get; set; }
            public double CreatedValue { set; get; }
            public string ProductedValueUnit { get; set; }
            public string ExportedValueUnit { get; set; }
            public string WasteValueUnit { get; set; }
            public string CreatedValueUnit { get; set; }
            public double ProductedValue { set; get; }
            public double ExportedValue { get; set; }
            public double WasteValue { get; set; }
            public double TotalWight { get; set; }
            public double TotalWaste { get; set; }
            public double TotalValue { get; set; }
            public double TotalResult { get; set; }
            public string Note { set; get; }
            public bool IsIncreaseDogma { get; set; }
            public double IsIncreaseDogmaValue { get; set; }

    }

}
