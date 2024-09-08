using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EyeClinic.DataAccess.Entities;
using EyeClinic.Model;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Home.Reports.PatientVisitTestReport;
using EyeClinic.WPF.Components.PatientList.PatientFile.EyeDiagnosis.EyeDiagnosisHistory;
using EyeClinic.WPF.Components.PatientList.PatientFile.EyeTests.EyeTestHistory;
using EyeClinic.WPF.Components.PatientList.PatientFile.LabTests.LabTestHistory;
using EyeClinic.WPF.Components.PatientList.PatientFile.Operations.OperationList.OperationHistory;
using EyeClinic.WPF.Components.PatientList.PatientFile.Prescriptions.PrescriptionHistory;
using PatientVisitDiagnosis = EyeClinic.Model.PatientVisitDiagnosis;

namespace EyeClinic.WPF.Components.PatientList.PatientVisitList.SummeryVisits
{
    public class SummeryVisitModel : Bindable
    {
        public ObservableCollection<LabTestHistoryModel> PatientVisitLabTests { get; set; }
        public List<DiagnosisHistoryModel> PatientVisitDiagnoses { get; set; }
        public List<TestHistoryModel> PatientVisitEyeTests { get; set; }
        public List<OperationHistoryModel> Operations { get; set; }
        public List<TestHistoryModel> Historyimagetest { set; get; }
        public List<PrescriptionHistoryModel> PrescriptionHistory { get; set; }
        public ObservableCollection<PatientFile.Tests.TestHistory.TestHistoryModel> PatientVisitsTest { get; set; }
        public ObservableCollection<PatientVisitGlassDto> PatientVisitGlass { get; set; }
        public List<PatientVisitsTestDto> PatientImagesTest { get;  set; }
    }

    public class SummeryDetailModel : Bindable
    {
      
        public DateTime VisitDate { get; set; }
        public string VisitNote { get; set; }

        public string PatientOpertionReport { get; set; }

        public string SpecialNote { get; set; }
        public string MedicalHistory { get; set; }

        public List<PatientVisitsTestDto> PatientImagesTest { set; get; }
        public List<PatientVisitLabTestDto> PatientVisitLabTests { get; set; }
        public List<PatientVisitDiagnosis> LeftEyeDiagnoses { get; set; }
        public List<PatientVisitDiagnosis> RightEyeDiagnoses { get; set; }
        public List<PatientVisitEyeTestDto> PatientVisitEyeTests { get; set; }
        public List<PatientVisitsTest> PatientVisitsTests { get; set; }
        public List<PatientOperationDto> Operations { get; set; }
        public List<OldMedicineViewTableDto> Prescriptions { get; set; }
        public PatientVisitGlassDto PatientVisitGlass { get; set; }
    }
}
