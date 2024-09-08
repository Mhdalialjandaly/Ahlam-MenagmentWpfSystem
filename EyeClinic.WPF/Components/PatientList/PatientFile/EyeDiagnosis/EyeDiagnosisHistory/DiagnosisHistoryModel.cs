using System;
using System.Collections.Generic;
using EyeClinic.Model;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.EyeDiagnosis.EyeDiagnosisHistory
{
    public class DiagnosisHistoryModel
    {
        public DateTime VisitDate { get; set; }
        public List<PatientVisitDiagnosis> LeftEyeDiagnoses { get; set; }
        public List<PatientVisitDiagnosis> RightEyeDiagnoses { get; set; }
    }
}
