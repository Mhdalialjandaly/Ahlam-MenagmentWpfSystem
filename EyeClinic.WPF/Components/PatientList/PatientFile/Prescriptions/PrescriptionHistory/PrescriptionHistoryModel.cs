using System;
using System.Collections.Generic;
using EyeClinic.Model;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.Prescriptions.PrescriptionHistory
{
    public class PrescriptionHistoryModel
    {
        public DateTime VisitDate { get; set; }
        public List<OldMedicineViewTableDto> Prescriptions { get; set; }
    }
}