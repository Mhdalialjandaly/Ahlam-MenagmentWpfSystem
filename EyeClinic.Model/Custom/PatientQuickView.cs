using System.Collections.Generic;

namespace EyeClinic.Model.Custom
{
    public class PatientQuickInfo
    {
        public List<string> Prescriptions { get; set; }
        public List<string> Tests { get; set; }
        public List<string> EyeTests { get; set; }
        public List<string> Diagnosis { get; set; }
    }
}
