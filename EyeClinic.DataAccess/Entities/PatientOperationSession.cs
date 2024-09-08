using System;
using System.Collections.Generic;

#nullable disable

namespace EyeClinic.DataAccess.Entities
{
    public partial class PatientOperationSession : IEntityModel
    {
        public int Id { get; set; }
        public int PatientOperationId { get; set; }
        public DateTime SessionDate { get; set; }
        public int PaymentValue { get; set; }
        public string Note { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int SessionNumber { get; set; }

        public virtual PatientOperation PatientOperation { get; set; }
    }
}
