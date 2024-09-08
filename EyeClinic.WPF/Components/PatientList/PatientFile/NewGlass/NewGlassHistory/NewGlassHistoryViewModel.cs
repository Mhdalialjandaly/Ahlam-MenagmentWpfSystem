using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.Base;
using Microsoft.EntityFrameworkCore;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.NewGlass.NewGlassHistory
{
    public partial class NewGlassHistoryViewModel
    {
        public NewGlassHistoryViewModel() { }
    }

    public partial class NewGlassHistoryViewModel : BaseViewModel<NewGlassHistoryView>
    {
        private readonly IPatientVisitGlassRepository _patientVisitGlassRepository;
        private readonly IMapper _mapper;

        public NewGlassHistoryViewModel(IPatientVisitGlassRepository patientVisitGlassRepository,
            IMapper mapper) {
            _patientVisitGlassRepository = patientVisitGlassRepository;
            _mapper = mapper;
        }

        public async Task Initialize(int patientId) {
            PatientId = patientId;

            var requiredTests = await _patientVisitGlassRepository.Get()
                .AsNoTracking()
                .Include(e => e.PatientVisit)
                .Where(e => e.PatientVisit.PatientId == patientId)
                .OrderByDescending(e => e.PatientVisit.VisitDate)
                .ToListAsync();

            if (requiredTests == null)
                return;

            PatientVisitGlass = new ObservableCollection<PatientVisitGlassDto>(
                _mapper.Map<List<PatientVisitGlassDto>>(requiredTests));
        }

        public async Task InitializeByVisitId(int visitId) {
            var requiredTests = await _patientVisitGlassRepository.Get()
                .AsNoTracking()
                .Include(e => e.PatientVisit)
                .Where(e => e.PatientVisit.Id == visitId)
                .OrderByDescending(e => e.PatientVisit.VisitDate)
                .ToListAsync();

            if (requiredTests == null)
                return;

            PatientVisitGlass = new ObservableCollection<PatientVisitGlassDto>(
                _mapper.Map<List<PatientVisitGlassDto>>(requiredTests));
        }

        public ObservableCollection<PatientVisitGlassDto> PatientVisitGlass { get; set; }
        public int PatientId { get; set; }
    }
}
