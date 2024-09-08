using System;
using System.Collections.Generic;

#nullable disable

namespace EyeClinic.DataAccess.Entities
{
    public partial class PatientVisit : IEntityModel
    {
        public PatientVisit() {
            OldMedicineViewTables = new HashSet<OldMedicineViewTable>();
            PatientVisitDiagnoses = new HashSet<PatientVisitDiagnosis>();
            PatientVisitEyeTests = new HashSet<PatientVisitEyeTest>();
            PatientVisitGlasses = new HashSet<PatientVisitGlass>();
            PatientVisitLabTests = new HashSet<PatientVisitLabTest>();
            PatientVisitPrescriptions = new HashSet<PatientVisitPrescription>();
            PatientVisitsTests = new HashSet<PatientVisitsTest>();
        }

        public int Id { get; set; }
        public int PatientId { get; set; }
        public DateTime VisitDate { get; set; }
        public DateTime? ReviewDate { get; set; }
        public int Cost { get; set; }
        public int Payment { get; set; }
        public int? NotPaymentReasonId { get; set; }
        public int Remaining { get; set; }
        public bool RemainPayed { get; set; }
        public int RemainPayValue { get; set; }
        public byte VisitStatus { get; set; }
        public int VisitIndex { get; set; }
        public int QueueIndex { get; set; }
        public string Notes { get; set; }
        public byte[] PdfFile { get; set; }
        public string PdfPath { get; set; }

        public string Why { get; set; }
        public string MedicalReport { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string PatientVisitType { get; set; }
     
        public virtual NotPayReason NotPaymentReason { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual ICollection<OldMedicineViewTable> OldMedicineViewTables { get; set; }
        public virtual ICollection<PatientVisitDiagnosis> PatientVisitDiagnoses { get; set; }
        public virtual ICollection<PatientVisitEyeTest> PatientVisitEyeTests { get; set; }
        public virtual ICollection<PatientVisitGlass> PatientVisitGlasses { get; set; }
        public virtual ICollection<PatientVisitLabTest> PatientVisitLabTests { get; set; }
        public virtual ICollection<PatientVisitPrescription> PatientVisitPrescriptions { get; set; }
        public virtual ICollection<PatientVisitsTest> PatientVisitsTests { get; set; }
    }
}
