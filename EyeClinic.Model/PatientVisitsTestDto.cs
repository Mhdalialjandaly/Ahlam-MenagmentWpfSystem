using System;
using System.IO;

namespace EyeClinic.Model
{
    public class PatientVisitsTestDto
    {
        public int Id { get; set; }
        public int PatientVisitId { get; set; }
        public int TestId { get; set; }
        public bool LeftEye { get; set; }
        public string LeftEyeNote { get; set; }
        public bool RightEye { get; set; }
        public string RightEyeNote { get; set; }
        public string ImageNumber { get; set; }
        public int CostPayment { get; set; }
        public bool Dropped { get; set; }
        public bool Locked { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string ImageName { get; set; }
        public string PdfSource { get; set; }
        public string ImageNameRight { get; set; }
        public string ImageNameLeft { get; set; }
        public string ImageNameBoth { get; set; }
        public int? MedicalCenterId { get; set; }        
        public PatientVisitDto PatientVisit { get; set; }
        public TestDto Test { get; set; }
        public DateTime VisitDate { get; set; }
        private MedicalCenterDto _medicalCenter;
        public int BoxSize { set; get; }
        public string SelectedMedicalCenter { set; get; }
        public MedicalCenterDto MedicalCenter {
            get => _medicalCenter;
            set {
                _medicalCenter = value;
                MedicalCenterId = value?.Id;
            }
        }

        public bool CostVisible => Test.EnName.ToLower().Contains("retinal");

        public string ImageSource1 => Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Images", "Tests", ImageNameRight ?? "");

        public string ImageSource2 => Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Images", "Tests", ImageNameLeft ?? "");

        public string ImageSource3 => Path.Combine(
           AppDomain.CurrentDomain.BaseDirectory,
           "Images", "Tests", ImageNameBoth ?? "");

    }
}
