using System;
using System.Collections.Generic;

#nullable disable

namespace EyeClinic.DataAccess.Entities
{
    public partial class PatientVisitLabTest : IEntityModel
    {
        public int Id { get; set; }
        public int PatientVisitId { get; set; }
        public int LabTestId { get; set; }
        public string Result { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public bool Done { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        public virtual LabTest LabTest { get; set; }
        public virtual PatientVisit PatientVisit { get; set; }
    }
}
