using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.Base;
using Microsoft.EntityFrameworkCore;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.Prescriptions.OldPrescription
{
    public class OldPrescriptionViewModel : BaseViewModel<OldPrescriptionView>
    {
        private readonly IMapper _mapper;
        private readonly IOldMedicineRepository _oldMedicineRepository;

        public OldPrescriptionViewModel(IMapper mapper, IOldMedicineRepository oldMedicineRepository) {
            _mapper = mapper;
            _oldMedicineRepository = oldMedicineRepository;
        }

        public async Task Initialize(int patientId) {
            PatientId = patientId;

            var requiredTests = await _oldMedicineRepository.Get()
                .AsNoTracking()
                .Include(e => e.PatientVisit)
                .Where(e => e.PatientVisit.PatientId == patientId)
                .OrderByDescending(e => e.PatientVisit.VisitDate)
                .ToListAsync();

            if (requiredTests == null)
                return;

            PatientVisitPrescriptions = new ObservableCollection<OldMedicineViewTableDto>(
                _mapper.Map<List<OldMedicineViewTableDto>>(requiredTests));
        }

        public ObservableCollection<OldMedicineViewTableDto> PatientVisitPrescriptions { get; set; }
        public int PatientId { get; set; }
    }
}
