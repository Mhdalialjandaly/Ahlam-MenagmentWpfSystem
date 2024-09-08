using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EyeClinic.Core;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.Base;
using Microsoft.EntityFrameworkCore;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.Tests.TestHistory
{
    public partial class TestHistoryViewModel
    {
        public TestHistoryViewModel() { }
    }

    public partial class TestHistoryViewModel : BaseViewModel<TestHistoryView>
    {
        private readonly IPatientVisitTestRepository _patientVisitTestRepository;

        public TestHistoryViewModel(IPatientVisitTestRepository patientVisitTestRepository) {
            _patientVisitTestRepository = patientVisitTestRepository;
        }

        public async Task Initialize(int patientId) {
            PatientId = patientId;

            var requiredTests = await _patientVisitTestRepository.Get()
                .AsNoTracking()
                .Include(e => e.PatientVisit)
                .Include(e => e.Test)
                .Where(e => e.PatientVisit.PatientId == PatientId && e.DeletedDate == null && e.PatientVisit.DeletedDate == null)
                .OrderByDescending(e => e.PatientVisit.VisitDate)
                .ToListAsync();

            if (requiredTests == null)
                return;

            PatientVisitsTest = new ObservableCollection<TestHistoryModel>(
                requiredTests
                    .GroupBy(e => e.PatientVisit.VisitDate.Date,
                        (key, value) => new TestHistoryModel {
                            VisitDate = key,
                            PatientVisitLabTests = value.ToList()
                        })
                    .OrderByDescending(e => e.VisitDate)
                    .ToList());

        }

        public async Task InitializeByVisitId(int visitId) {
            var requiredTests = await _patientVisitTestRepository.Get()
                .AsNoTracking()
                .Include(e => e.PatientVisit)
                .Include(e => e.Test)
                .Where(e => e.PatientVisit.Id == visitId && e.DeletedDate == null && e.PatientVisit.DeletedDate==null)
                .OrderByDescending(e => e.PatientVisit.VisitDate)
                .ToListAsync();

            if (requiredTests == null)
                return;

            PatientVisitsTest = new ObservableCollection<TestHistoryModel>(
                requiredTests
                    .GroupBy(e => e.PatientVisit.VisitDate.Date,
                        (key, value) => new TestHistoryModel {
                            VisitDate = key,
                            PatientVisitLabTests = value.ToList()
                        })
                    .OrderByDescending(e => e.VisitDate)
                    .ToList());
        }
        private void OpenImage(PatientVisitsTestDto dto)
        {
            var imageSource2 = Path.Combine(Global.TestImageDirectory, dto.ImageSource2);
            var p = new Process();
            p.StartInfo = new ProcessStartInfo(imageSource2)
            {
                UseShellExecute = true
            };
            p.Start();
        }
        private void OpenImage2(PatientVisitsTestDto dto)
        {
            var imageSource1 = Path.Combine(Global.TestImageDirectory, dto.ImageSource1);
            var p = new Process();
            p.StartInfo = new ProcessStartInfo(imageSource1)
            {
                UseShellExecute = true
            };
            p.Start();

        }
        private void OpenImage3(PatientVisitsTestDto dto)
        {
            var imageSource3 = Path.Combine(Global.TestImageDirectory, dto.ImageSource3);
            var p = new Process();
            p.StartInfo = new ProcessStartInfo(imageSource3)
            {
                UseShellExecute = true
            };
            p.Start();

        }

        public ObservableCollection<TestHistoryModel> PatientVisitsTest { get; set; }
        public int PatientId { get; set; }
    }
}
