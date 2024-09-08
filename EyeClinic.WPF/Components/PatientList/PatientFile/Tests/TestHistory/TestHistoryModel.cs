using System;
using System.Collections.Generic;
using EyeClinic.DataAccess.Entities;
using EyeClinic.Model;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.Tests.TestHistory
{
    public class TestHistoryModel
    {
        public DateTime VisitDate { get; set; }
        public List<PatientVisitsTest> PatientVisitLabTests { get; set; }
        public List<PatientVisitsTestDto> PatientVisitsTests { get; internal set; }
    }
}
