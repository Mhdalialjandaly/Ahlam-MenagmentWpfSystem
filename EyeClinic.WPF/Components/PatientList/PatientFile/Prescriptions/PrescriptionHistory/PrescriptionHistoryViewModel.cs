using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.Base;
using Microsoft.EntityFrameworkCore;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.Prescriptions.PrescriptionHistory
{
    public partial class PrescriptionHistoryViewModel
    {
        public PrescriptionHistoryViewModel() { }
    }

    public partial class PrescriptionHistoryViewModel : BaseViewModel<PrescriptionHistoryView>
    {
        private readonly IPatientVisitPrescriptionRepository _patientVisitPrescriptionRepository;
        private readonly IMapper _mapper;
        private readonly IOldMedicineRepository _oldMedicineRepository;

        public PrescriptionHistoryViewModel(
            IPatientVisitPrescriptionRepository patientVisitPrescriptionRepository, IMapper mapper,
            IOldMedicineRepository oldMedicineRepository) {
            _patientVisitPrescriptionRepository = patientVisitPrescriptionRepository;
            _mapper = mapper;
            _oldMedicineRepository = oldMedicineRepository;
        }

        public async Task Initialize(int patientId) {
            PatientId = patientId;

            var prescription = await _oldMedicineRepository
                    .GetByPatientId(patientId);

            var prescriptionList = (await _patientVisitPrescriptionRepository               
                    .GetByKey(e => e.PatientVisit.PatientId == patientId))
                    .Where(e=>e.PatientVisit.DeletedDate==null)
                .OrderByDescending(e => e.PatientVisit.VisitDate)
                .ThenBy(e => e.RowIndex);

            foreach (var item in prescriptionList) {
                prescription.Add(new OldMedicineViewTableDto {
                    MedicineName = item.Medicine.MedicineName,
                    MedicineType = item.Medicine.MedicineType.EnName,
                    PatientVisitId = item.PatientVisitId,
                    VisitDate = item.PatientVisit.VisitDate,
                    Index = PrescriptionHistory.Count
                });
                prescription.Add(new OldMedicineViewTableDto {
                    MedicineName = item.MedicineUsage.UsageName,
                    MedicineType = string.Empty,
                    PatientVisitId = item.PatientVisitId,
                    VisitDate = item.PatientVisit.VisitDate,
                    Index = PrescriptionHistory.Count
                });
            }

            PrescriptionHistory = prescription
                .GroupBy(e => e.PatientVisit.VisitDate,
                        (key, value) => new PrescriptionHistoryModel {
                            VisitDate = key,
                            Prescriptions = value.ToList()
                        })
                .OrderByDescending(e => e.VisitDate)
                .ToList();
        }

        public async Task InitializeByVisitId(int visitId) {
            var prescription = await _oldMedicineRepository
                .GetByPatientVisitId(visitId);

            var prescriptionList = (await _patientVisitPrescriptionRepository
                    .GetByKey(e => e.PatientVisit.Id == visitId))
                .OrderByDescending(e => e.PatientVisit.VisitDate)
                .ThenBy(e => e.RowIndex);

            foreach (var item in prescriptionList) {
                prescription.Add(new OldMedicineViewTableDto {
                    MedicineName = item.Medicine.MedicineName,
                    MedicineType = item.Medicine.MedicineType.EnName,
                    PatientVisitId = item.PatientVisitId,
                    VisitDate = item.PatientVisit.VisitDate,
                    Index = PrescriptionHistory.Count
                });
                prescription.Add(new OldMedicineViewTableDto {
                    MedicineName = item.MedicineUsage.UsageName,
                    MedicineType = string.Empty,
                    PatientVisitId = item.PatientVisitId,
                    VisitDate = item.PatientVisit.VisitDate,
                    Index = PrescriptionHistory.Count
                });
            }

            PrescriptionHistory = prescription
                .GroupBy(e => e.PatientVisit.VisitDate,
                    (key, value) => new PrescriptionHistoryModel {
                        VisitDate = key,
                        Prescriptions = value.ToList()
                    })
                .OrderByDescending(e => e.VisitDate)
                .ToList();
        }

        public List<PrescriptionHistoryModel> PrescriptionHistory { get; set; }
        public int PatientId { get; set; }
    }
}
