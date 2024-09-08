using System;
using System.Collections.Generic;
using EyeClinic.Model;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.EyeTests.EyeTestHistory
{
    public class TestHistoryModel
    {
        public DateTime VisitDate { get; set; }
        public List<PatientVisitEyeTestDto> PatientVisitEyeTests { get; set; }
        public List<PatientVisitsTestDto> PatientImagesTest { get; internal set; }
    }
}
