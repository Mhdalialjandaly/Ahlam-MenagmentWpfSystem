using System;
namespace EyeClinic.Model
{
    public class PatientVisitDto
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public DateTime VisitDate { get; set; }
        public DateTime? ReviewDate { get; set; }
        
        //public string SelectedPatinetReviewTime { set; get; }
        public int Cost { get; set; }
        public int Payment { get; set; }
        public int? NotPaymentReasonId { get; set; }
        public int Remaining { get; set; }
        public bool RemainPayed { get; set; }
        public int RemainPayValue { get; set; }
        public byte VisitStatus { get; set; }
        public int QueueIndex { get; set; }
        public int VisitIndex { get; set; }
        public string Notes { get; set; }
        public byte[] PdfFile { get; set; }
        public string PdfPath { get; set; }
        public string Why { get; set; }
        public string ImageUploaded { get; set; }
        public string MedicalReport { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        public PatientDto Patient { get; set; }
        public NotPayReasonDto NotPaymentReason { get; set; }

        public bool IsChecked { get; set; }
        public string PatientVisitType { get; set; }

    }
}
