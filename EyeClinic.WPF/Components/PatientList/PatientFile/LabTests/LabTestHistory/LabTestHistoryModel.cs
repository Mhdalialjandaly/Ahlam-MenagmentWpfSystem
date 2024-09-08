using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EyeClinic.Model;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.LabTests.LabTestHistory
{
    public class LabTestHistoryModel
    {
        public DateTime VisitDate { get; set; }
        public List<PatientVisitLabTestDto> PatientVisitLabTests { get; set; }
    }
}
