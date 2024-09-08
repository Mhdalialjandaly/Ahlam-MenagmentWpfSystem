using System;
using System.Collections.Generic;

#nullable disable

namespace EyeClinic.DataAccess.Entities
{
    public partial class PatientVisitEyeTest : IEntityModel
    {
        public int Id { get; set; }
        public int PatientVisitId { get; set; }
        public int EyeTestId { get; set; }
        public string LeftEyeResult { get; set; }
        public string RightEyeResult { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        public virtual EyeTest EyeTest { get; set; }
        public virtual PatientVisit PatientVisit { get; set; }
    }
}
