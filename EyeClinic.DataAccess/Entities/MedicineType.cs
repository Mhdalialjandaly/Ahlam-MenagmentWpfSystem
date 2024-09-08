using System;
using System.Collections.Generic;

#nullable disable

namespace EyeClinic.DataAccess.Entities
{
    public partial class MedicineType : IEntityModel
    {
        public MedicineType()
        {
            MedicineUsages = new HashSet<MedicineUsage>();
            Medicines = new HashSet<Medicine>();
        }

        public int Id { get; set; }
        public string EnName { get; set; }
        public string ArName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        public virtual ICollection<MedicineUsage> MedicineUsages { get; set; }
        public virtual ICollection<Medicine> Medicines { get; set; }
    }
}
