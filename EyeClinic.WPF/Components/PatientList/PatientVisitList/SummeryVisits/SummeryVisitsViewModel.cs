using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Base.Extends;
using EyeClinic.WPF.Components.PatientList.PatientFile.EyeDiagnosis.EyeDiagnosisHistory;
using EyeClinic.WPF.Components.PatientList.PatientFile.EyeTests.EyeTestHistory;
using EyeClinic.WPF.Components.PatientList.PatientFile.LabTests.LabTestHistory;
using EyeClinic.WPF.Components.PatientList.PatientFile.NewGlass.NewGlassHistory;
using EyeClinic.WPF.Components.PatientList.PatientFile.Operations.OperationList.OperationHistory;
using EyeClinic.WPF.Components.PatientList.PatientFile.Prescriptions.PrescriptionHistory;
using EyeClinic.WPF.Components.PatientList.PatientFile.Tests.TestHistory;
using Microsoft.EntityFrameworkCore;
using Unity;

namespace EyeClinic.WPF.Components.PatientList.PatientVisitList.SummeryVisits
{
    public partial class SummeryVisitsViewModel
    {
        public SummeryVisitsViewModel() { }
    }

    public partial class SummeryVisitsViewModel : BaseViewModel<SummeryVisitsView>
    {
        private readonly IUnityContainer _container;

        public SummeryVisitsViewModel(IUnityContainer container) {
            _container = container;

            SortCommand = new RelayCommand(() => {
                if (SummeryDetail != null) {
                    if (SortDesc)
                        SummeryDetail = SummeryDetail.OrderByDescending(e => e.VisitDate).ToList();
                    else
                        SummeryDetail = SummeryDetail.OrderBy(e => e.VisitDate).ToList();

                    SortDesc = !SortDesc;
                    SortContent = SortDesc ? "Sort ↓" : "Sort ↑";


                }
            });
        }

        public string SortContent { get; set; } = "Sort ↑";
        public bool SortDesc { get; set; }
        public string ForeGroundColor { get; set; }

        public ICommand SortCommand { get; set; }
        public ICommand CancelCommand => new RelayCommand(() => {
            ContainerHandler.Container.Resolve<IDialogService>().DisposeLastDialog();
        });

        public async Task Initialize(int patientId, string specialNote) {
            var summery = new SummeryVisitModel();
            SpecialNote = specialNote;

            var eyeDiagnosis = _container.Resolve<EyeDiagnosisHistoryViewModel>();
            await eyeDiagnosis.Initialize(patientId);
            summery.PatientVisitDiagnoses = eyeDiagnosis.PatientVisitDiagnoses;

            var eyeTest = _container.Resolve<EyeTestHistoryViewModel>();
            await eyeTest.Initialize(patientId);
            summery.PatientVisitEyeTests = eyeTest.PatientVisitEyeTests;

            var labTestHistory = _container.Resolve<LabTestHistoryViewModel>();
            await labTestHistory.Initialize(patientId);
            summery.PatientVisitLabTests = labTestHistory.PatientVisitLabTests;

            var treatment = _container.Resolve<PrescriptionHistoryViewModel>();
            await treatment.Initialize(patientId);
            summery.PrescriptionHistory = treatment.PrescriptionHistory;

            var test = _container.Resolve<TestHistoryViewModel>();
            await test.Initialize(patientId);
            summery.PatientVisitsTest = test.PatientVisitsTest;

            var glassHistory = _container.Resolve<NewGlassHistoryViewModel>();
            await glassHistory.Initialize(patientId);
            summery.PatientVisitGlass = glassHistory.PatientVisitGlass;

            //var patientImagetest = _container.Resolve<TestHistoryViewModel>();
            //await patientImagetest.Initialize(patientId);
            //summery.PatientVisitsTest = patientImagetest.PatientVisitsTest;

            var operations = _container.Resolve<OperationHistoryViewModel>();
            await operations.Initialize(patientId);
            summery.Operations = operations.Operations;


            AvailableDates = GetAvailableDates(summery)
                .OrderByDescending(e => e).ToList();
            AvailableDates.AddRange(summery.Operations.Select(e => e.OperationDate.Date));


            AvailableDates2 = GetAvailableDates2(summery);

            var date2 = AvailableDates2;         
                var note2 = await _container.Resolve<IPatientVisitRepository>().GetByPatientId(patientId, date2.Date);

           

            //var historyimagetest = _container.Resolve<TestHistoryViewModel>();
            //await historyimagetest.Initialize(patientId);
            //summery.PatientVisitsTest = historyimagetest.PatientVisitsTest;

            SummeryDetail = new List<SummeryDetailModel>();
            foreach (var date in AvailableDates) {
                var note = (await _container.Resolve<IPatientVisitRepository>()
                        .GetByPatientId(patientId, date.Date))
                    .FirstOrDefault();

                var detail = new SummeryDetailModel {
                    VisitDate = date,
                    VisitNote = note?.Notes,
                    //SpecialNote = note?.Patient.Notes,
                    MedicalHistory = note?.Patient.PatientSickStory?.SickStory,
                    Operations = summery
                        .Operations
                        .FirstOrDefault(e => e.OperationDate.Date == date.Date)?
                        .Operations,
                    PatientVisitsTests = summery
                        .PatientVisitsTest
                        .FirstOrDefault(e => e.VisitDate.Date == date.Date)?
                        .PatientVisitLabTests,
                    PatientVisitEyeTests = summery
                        .PatientVisitEyeTests
                        .FirstOrDefault(e => e.VisitDate.Date == date.Date)?
                        .PatientVisitEyeTests,
                    LeftEyeDiagnoses = summery
                        .PatientVisitDiagnoses
                        .FirstOrDefault(e => e.VisitDate.Date == date.Date)?
                        .LeftEyeDiagnoses,
                    RightEyeDiagnoses = summery
                        .PatientVisitDiagnoses
                        .FirstOrDefault(e => e.VisitDate.Date == date.Date)?
                        .RightEyeDiagnoses,
                    PatientVisitLabTests = summery
                        .PatientVisitLabTests
                        .FirstOrDefault(e => e.VisitDate.Date == date.Date)?
                        .PatientVisitLabTests,
                    Prescriptions = summery
                        .PrescriptionHistory
                        .FirstOrDefault(e => e.VisitDate.Date == date.Date)?
                        .Prescriptions,
                    PatientVisitGlass = summery.PatientVisitGlass
                        .FirstOrDefault(e => e.PatientVisit.VisitDate.Date == date.Date)                      

                };

                SummeryDetail.Add(detail);
            }
        }
        public async Task InitializeByVisitId(int visitId, string specialNote) {
            SpecialNote = specialNote;

            var summery = new SummeryVisitModel();

            var eyeDiagnosis = _container.Resolve<EyeDiagnosisHistoryViewModel>();
            await eyeDiagnosis.InitializeByVisitId(visitId);
            summery.PatientVisitDiagnoses = eyeDiagnosis.PatientVisitDiagnoses;

            var eyeTest = _container.Resolve<EyeTestHistoryViewModel>();
            await eyeTest.InitializeByVisitId(visitId);
            summery.PatientVisitEyeTests = eyeTest.PatientVisitEyeTests;

            var labTestHistory = _container.Resolve<LabTestHistoryViewModel>();
            await labTestHistory.InitializeByVisitId(visitId);
            summery.PatientVisitLabTests = labTestHistory.PatientVisitLabTests;

            var treatment = _container.Resolve<PrescriptionHistoryViewModel>();
            await treatment.InitializeByVisitId(visitId);
            summery.PrescriptionHistory = treatment.PrescriptionHistory;

            var test = _container.Resolve<TestHistoryViewModel>();
            await test.InitializeByVisitId(visitId);
            summery.PatientVisitsTest = test.PatientVisitsTest;

            var glassHistory = _container.Resolve<NewGlassHistoryViewModel>();
            await glassHistory.InitializeByVisitId(visitId);
            summery.PatientVisitGlass = glassHistory.PatientVisitGlass;

            var operations = _container.Resolve<OperationHistoryViewModel>();
            await operations.Initialize(visitId);
            summery.Operations = operations.Operations;

            AvailableDates = GetAvailableDates(summery)
                .OrderByDescending(e => e).ToList();
            AvailableDates.AddRange(summery.Operations.Select(e => e.OperationDate.Date));

            SummeryDetail = new List<SummeryDetailModel>();
            foreach (var date in AvailableDates) {
                var note = await _container.Resolve<IPatientVisitRepository>()
                        .Get().AsNoTracking()
                        .Include(e => e.Patient)
                        .ThenInclude(e => e.PatientSickStory)
                        .FirstOrDefaultAsync(e => e.Id == visitId &&
                                                  e.VisitDate.Date == date.Date);

                var detail = new SummeryDetailModel() {
                    VisitDate = date,
                    VisitNote = note?.Notes,
                    MedicalHistory = note?.Patient?.PatientSickStory?.SickStory,
                    SpecialNote = note?.Patient?.Notes,

                    Operations = summery
                        .Operations
                        .FirstOrDefault(e => e.OperationDate.Date == date.Date)?
                        .Operations,
                    PatientVisitEyeTests = summery
                        .PatientVisitEyeTests
                        .FirstOrDefault(e => e.VisitDate.Date == date.Date)?
                        .PatientVisitEyeTests,
                    PatientVisitsTests = summery
                        .PatientVisitsTest
                        .FirstOrDefault(e => e.VisitDate.Date == date.Date)?
                        .PatientVisitLabTests,
                    LeftEyeDiagnoses = summery
                        .PatientVisitDiagnoses
                        .FirstOrDefault(e => e.VisitDate.Date == date.Date)?
                        .LeftEyeDiagnoses,
                    RightEyeDiagnoses = summery
                        .PatientVisitDiagnoses
                        .FirstOrDefault(e => e.VisitDate.Date == date.Date)?
                        .RightEyeDiagnoses,
                    PatientVisitLabTests = summery
                        .PatientVisitLabTests
                        .FirstOrDefault(e => e.VisitDate.Date == date.Date)?
                        .PatientVisitLabTests,
                    Prescriptions = summery
                        .PrescriptionHistory
                        .FirstOrDefault(e => e.VisitDate.Date == date.Date)?
                        .Prescriptions,
                    PatientVisitGlass = summery.PatientVisitGlass
                        .FirstOrDefault(e => e.PatientVisit.VisitDate.Date == date.Date),

                    //PatientImagesTest = summery.PatientVisitEyeTests.FirstOrDefault
                    //(e=>e.VisitDate==date.Date).PatientImagesTest


                };

                SummeryDetail.Add(detail);
            }
        }

        public string SpecialNote { get; set; }

        private List<DateTime> GetAvailableDates(SummeryVisitModel summery) {
            var dates = new List<DateTime>();

            dates.AddRange(summery.Operations.Select(e => e.OperationDate.Date));
            dates.AddRange(summery.PatientVisitDiagnoses.Select(e => e.VisitDate.Date));
            dates.AddRange(summery.PatientVisitGlass.Select(e => e.PatientVisit.VisitDate.Date));
            dates.AddRange(summery.PatientVisitLabTests.Select(e => e.VisitDate.Date));
            dates.AddRange(summery.PatientVisitsTest.Select(e => e.VisitDate.Date));
            dates.AddRange(summery.PrescriptionHistory.Select(e => e.VisitDate.Date));
            dates.AddRange(summery.PatientVisitEyeTests.Select(e => e.VisitDate.Date));

            return dates.Distinct().ToList();
        }

        private DateTime GetAvailableDates2(SummeryVisitModel summery)
        {
            var dates = new DateTime();


            return dates;
        }

        public SummeryVisitModel SummeryVisits { get; set; }
        public List<SummeryDetailModel> SummeryDetail { get; set; }

        public List<DateTime> AvailableDates { get; set; }
        public DateTime AvailableDates2 { get; set; }

    }
}
