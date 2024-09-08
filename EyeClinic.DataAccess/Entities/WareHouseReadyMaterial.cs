using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeClinic.DataAccess.Entities
{
    public partial class WareHouseReadyMaterial:IEntityModel
    {
        public WareHouseReadyMaterial()
        {
            PatientVisitsTests = new HashSet<PatientVisitsTest>();
        }

        public int Id { get; set; }
        public string EnName { get; set; }
        public string ArName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string Code { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public double Quintity { get; set; }
        public string Unit { get; set; }
        public double UnitValue { get; set; }
        public double TotalResult { get; set; }
        public string FirstTermBalance { get; set; }
        public double WasteValue { set; get; }
        public bool IsProduct { get; set; }
        public int TestId { get; set; }
        public virtual ICollection<PatientVisitsTest> PatientVisitsTests { get; set; }
        public virtual Test Test { get; set; }
    }
}
