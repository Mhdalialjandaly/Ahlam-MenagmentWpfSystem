using System;
using System.Collections.Generic;

#nullable disable

namespace EyeClinic.DataAccess.Entities
{
    public partial class PatientOperation : IEntityModel
    {
        public PatientOperation() {
            PatientOperationSessions = new HashSet<PatientOperationSession>();
        }

        public int Id { get; set; }
        public int PatientId { get; set; }
        public DateTime OperationDate { get; set; }
        public int? LeftEyeOperationId { get; set; }
        public int? RightEyeOperationId { get; set; }
        public int MedicalCenterId { get; set; }
        public bool MedicalCenterReserved { get; set; }
        public int TotalSessions { get; set; }
        public string PhotoSource { get; set; }
        public string PaymentLocation { get; set; }
        public int TotalCost { get; set; }
        public string LeftEyeNote { get; set; }
        public string RightEyeNote { get; set; }
        public string LeftEyeLens { get; set; }
        public string LeftEyeLensType { get; set; }
        public string RightEyeLens { get; set; }
        public string RightEyeLensType { get; set; }
        public string Report { get; set; }
        public bool IsFinish { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int CenterCost { get; set; }
        public int ClinicCost { get; set; }
        public int? DownPayment { get; set; }
        public int? Revenue { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int Remaining { get; set; }

        public virtual Operation LeftEyeOperation { get; set; }
        public virtual MedicalCenter MedicalCenter { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual Operation RightEyeOperation { get; set; }
        public virtual ICollection<PatientOperationSession> PatientOperationSessions { get; set; }
    }
}
