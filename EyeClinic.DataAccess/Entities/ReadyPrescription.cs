using System;
using System.Collections.Generic;

#nullable disable

namespace EyeClinic.DataAccess.Entities
{
    public partial class ReadyPrescription : IEntityModel
    {
        public ReadyPrescription()
        {
            ReadyPrescriptionMedicines = new HashSet<ReadyPrescriptionMedicine>();
        }

        public int Id { get; set; }
        public string EnName { get; set; }
        public string ArName { get; set; }
        public bool Disabled { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        public virtual ICollection<ReadyPrescriptionMedicine> ReadyPrescriptionMedicines { get; set; }
    }
}
