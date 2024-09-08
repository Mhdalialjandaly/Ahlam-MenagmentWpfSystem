using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EyeClinic.Core;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Base.Extends;
using Microsoft.EntityFrameworkCore;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.Operations.OperationList.DropOperationFile
{
    public partial class DropOperationFileViewModel
    {
        public DropOperationFileViewModel() {
            if (IsDesignMode) { }
        }
    }

    public partial class DropOperationFileViewModel : BaseViewModel<DropOperationFileView>
    {
        private readonly IMapper _mapper;
        private readonly ITestsRepository _testsRepository;
        private readonly IPatientVisitTestRepository _patientVisitTestRepository;

        public DropOperationFileViewModel(IMapper mapper, ITestsRepository testsRepository,
                IPatientVisitTestRepository patientVisitTestRepository) {
            _mapper = mapper;
            _testsRepository = testsRepository;
            _patientVisitTestRepository = patientVisitTestRepository;
        }

        public List<Checkable<PatientVisitsTestDto>> VisitsTests { get; set; }

        public async Task Initialize(int patientId) {
            var lastVisitHasTests = await _patientVisitTestRepository
                .Get().Include(e => e.PatientVisit)
                .AsNoTracking().Where(e => e.PatientVisit.PatientId == patientId)
                .OrderByDescending(e => e.PatientVisit.VisitDate)
                .FirstOrDefaultAsync();

            var visitEyeTests = _mapper.Map<List<PatientVisitsTestDto>>(
                await _patientVisitTestRepository
                        .Get().AsNoTracking()
                        .Include(e => e.Test)
                        .Where(e => e.PatientVisitId == lastVisitHasTests.PatientVisitId)
                        .ToListAsync());

            VisitsTests = visitEyeTests.Select(e => new Checkable<PatientVisitsTestDto> { Item = e }).ToList();
        }

        public async Task DropSelectedItems() {
            var selectedItems = VisitsTests
                .Where(e => e.IsChecked)
                .ToList();

            if (selectedItems.IsNullOrEmpty())
                return;

            await _testsRepository.DropAll(selectedItems
                .Select(e => e.Item).ToList());
        }
    }
}
