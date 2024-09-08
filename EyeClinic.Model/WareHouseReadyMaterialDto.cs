using EyeClinic.Model.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeClinic.Model
{
    public class WareHouseReadyMaterialDto
    {
        
            public int Id { get; set; }
            public string Code { get; set; }
            public string EnName { get; set; }
            public string ArName { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime? LastModifiedDate { get; set; }
            public string DeletedBy { get; set; }
            public DateTime? DeletedDate { get; set; }           
            public double TotalWight { get; set; }
            public double TotalWaste { get; set; }
            public double TotalValue { get; set; }
            public double TotalResult { get; set; }
            public double Quintity { get; set; }
            public string Unit { get; set; }
            public double ReayProductsValue { get; set; }
            public string FirstTermBalance { get; set; }
            public double WasteValue { set; get; }
            public double UnitValue { get; set; }
            public bool IsProduct { get; set; }
            public int IndexN { get; set; }
            public int TestId { get; set; }

        public override string ToString()
            {
                var code = CurrentLanguageCodeHandler.ArLanguageCode;

                return CurrentLanguageCodeHandler.CurrentLanguageCode == code
                    ? $"{Code} - {ArName}"
                    : $"{Code} - {EnName}";
            }
        }
    
}
