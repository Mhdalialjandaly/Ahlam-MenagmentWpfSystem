using System;
using System.Collections.Generic;

#nullable disable

namespace EyeClinic.DataAccess.Entities
{
    public partial class LabTest : IEntityModel
    {
        public LabTest()
        {
            PatientVisitLabTests = new HashSet<PatientVisitLabTest>();
        }

        public int Id { get; set; }
        public string LabTestName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        public virtual ICollection<PatientVisitLabTest> PatientVisitLabTests { get; set; }
    }
}
