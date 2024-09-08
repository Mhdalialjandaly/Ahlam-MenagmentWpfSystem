using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EyeClinic.Model
{
    public class PatientOperationDto
    {
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
        public int CenterCost { get; set; }
        public int ClinicCost { get; set; }
        public int? DownPayment { get; set; }
        public int Remaining { get; set; }
        public int Revenue { get; set; }
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


        public PatientDto Patient { get; set; }
        public OperationDto LeftEyeOperation { get; set; }
        public OperationDto RightEyeOperation { get; set; }
        public MedicalCenterDto MedicalCenter { get; set; }


        public List<PatientOperationSessionDto> PatientOperationSessions { get; set; }



        [NotMapped] public int? SessionNumber { get; set; }

        [NotMapped]
        public string EyeOperationDisplayName
        {
            get
            {
                var displayName = "";
                if (LeftEyeOperation != null)
                    displayName += LeftEyeOperation.EnName;
                if (RightEyeOperation != null)
                displayName += LeftEyeOperation == null ? $"{RightEyeOperation.EnName}" : $"\n {RightEyeOperation.EnName}";

                return displayName;
            }
        }

        [NotMapped]
        public string EyeDisplayName
        {
            get
            {
                var displayName = "";
                if (LeftEyeOperationId != null)
                    displayName += "L";
                if (RightEyeOperationId != null)
                    displayName += LeftEyeOperationId != null ? "+ R" : "R";

                return displayName;
            }
        }
    }
}
