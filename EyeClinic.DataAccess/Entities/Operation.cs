using System;
using System.Collections.Generic;

#nullable disable

namespace EyeClinic.DataAccess.Entities
{
    public partial class Operation : IEntityModel
    {
        public Operation()
        {
            PatientOperationLeftEyeOperations = new HashSet<PatientOperation>();
            PatientOperationRightEyeOperations = new HashSet<PatientOperation>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string EnName { get; set; }
        public string ArName { get; set; }
        public bool IsSergical { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        public virtual ICollection<PatientOperation> PatientOperationLeftEyeOperations { get; set; }
        public virtual ICollection<PatientOperation> PatientOperationRightEyeOperations { get; set; }
    }
}
