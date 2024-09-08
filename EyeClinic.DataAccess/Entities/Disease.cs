using System;
using System.Collections.Generic;

#nullable disable

namespace EyeClinic.DataAccess.Entities
{
    public partial class Disease : IEntityModel
    {
        public Disease()
        {
            PatientDiseases = new HashSet<PatientDisease>();
        }

        public int Id { get; set; }
        public string EnName { get; set; }
        public string ArName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int ThePNumber { set; get; }
        public string Enrty { set; get; }
        public string Extry { set; get; }
        public string FirstDayValue { set; get; }
        public string Note { set; get; }
        public double FirstValue { set; get; }
        public double CurrentValue { get; set; }


        public virtual ICollection<PatientDisease> PatientDiseases { get; set; }
    }
}
