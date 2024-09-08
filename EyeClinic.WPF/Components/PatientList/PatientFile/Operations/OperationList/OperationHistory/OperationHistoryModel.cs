using System;
using System.Collections.Generic;
using EyeClinic.Model;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.Operations.OperationList.OperationHistory
{
    public class OperationHistoryModel
    {
        public DateTime OperationDate { get; set; }
        public List<PatientOperationDto> Operations { get; set; }
    }
}
