using System.Collections.Generic;
using EyeClinic.Model;
using EyeClinic.WPF.Components.Management;

namespace EyeClinic.WPF.AppServices.Print
{
    public interface IPrintService
    {
        List<Printer> GetAllPrinters();
        void PrintPrescription(PatientDto patient, List<PatientVisitPrescriptionDto> prescription);
        void PrintPrescription(PatientDto patient, List<OldMedicineViewTableDto> prescription);
        void PrintGlass(PatientDto patient, PatientVisitGlassDto glass);
        void PrintGlass(PatientDto patient, PatientGlassDto glass);
        void PrintNote(PatientDto patient, string note);
        void PrintLabTests(PatientDto patient, List<PatientVisitLabTestDto> labTests);
        void PrintTests(PatientDto patient, List<PatientVisitsTestDto> patientVisitsTestDtos);
    }
}
