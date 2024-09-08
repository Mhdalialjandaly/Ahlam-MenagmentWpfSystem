using System;
using System.Collections.Generic;

#nullable disable

namespace EyeClinic.DataAccess.Entities
{
    public partial class Test : IEntityModel
    {
        public Test()
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

        public string ImagePath { get; set; }
        public string ImagePath2 { get; set; }
        public string ImagePath3 { get; set; }
        public byte Imagex { get; set; }
        public byte[] Imagex2 { get; set; }
        public byte[] Imagex3 { get; set; }
        public byte[] Imagex4 { get; set; }

        public double Quintity { get; set; }
        public string Unit { get; set; }
        public double UnitValue { get; set; }
        public double TotalResult { get; set; }

        public double FirstTermBalanceWareHouseValue { get; set; }
        public double TotalWasteWareHouseValue { get; set; }
        public double TotalWareHouseValue { get; set; }
        public double TotalEntryWareHouseValue { get; set; }
        public string FirstTermBalance { get; set; }
        public double WasteValue { set; get; }
        public bool IsProduct { get; set; }

        public virtual ICollection<PatientVisitsTest> PatientVisitsTests { get; set; }
    }
}
