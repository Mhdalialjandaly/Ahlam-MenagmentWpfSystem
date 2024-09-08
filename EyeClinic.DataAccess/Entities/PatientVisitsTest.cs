using System;
using System.Collections.Generic;

#nullable disable

namespace EyeClinic.DataAccess.Entities
{
    public partial class PatientVisitsTest : IEntityModel
    {
        public int Id { get; set; }
        public int PatientVisitId { get; set; }
        public int TestId { get; set; }
        public bool LeftEye { get; set; }
        public string LeftEyeNote { get; set; }
        public bool RightEye { get; set; }
        public string RightEyeNote { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string ImageNumber { get; set; }
        public bool? Dropped { get; set; }
        public bool Locked { get; set; }
        public int CostPayment { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string ImageName { get; set; }

        public string ImageNameRight { get; set; }
        public string ImageNameLeft { get; set; }
        public string ImageNameBoth { get; set; }
        public string PdfSource { get; set; }

        public int? MedicalCenterId { get; set; }
        public MedicalCenter MedicalCenter { get; set; }
        public virtual PatientVisit PatientVisit { get; set; }
        public virtual Test Test { get; set; }
    }
}
