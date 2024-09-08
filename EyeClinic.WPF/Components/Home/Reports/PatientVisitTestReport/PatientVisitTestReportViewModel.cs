using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.Base;

namespace EyeClinic.WPF.Components.Home.Reports.PatientVisitTestReport
{
    public partial class PatientVisitTestReportViewModel
    {
        public PatientVisitTestReportViewModel() { }
    }

    public partial class PatientVisitTestReportViewModel : BaseViewModel<PatientVisitTestReportView>
    {
        private readonly IPatientVisitTestRepository _patientVisitTestRepository;
        private readonly ITestsRepository _testsRepository;

        public PatientVisitTestReportViewModel(IPatientVisitTestRepository patientVisitTestRepository,
            ITestsRepository testsRepository)
        {
            _patientVisitTestRepository = patientVisitTestRepository;
            _testsRepository = testsRepository;

            FromDate = DateTime.Now.Date;
            FromDateD=DateTime.Now.Date;

            ToDate = DateTime.Now.Date;
            ToDateD = DateTime.Now.Date;

            GetReportCommand = new RelayCommand(GetReport);
            GetReportCommandD = new RelayCommand(GetReport2);


            GetReportRightEyeCommand = new RelayCommand(GetReportRightEye);
            GetReportRightEyeCommandD = new RelayCommand(GetReportRightEyeD);


            GetReportLeftEyeCommand = new RelayCommand(GetReportLeftEye);
            GetReportLeftEyeCommandD = new RelayCommand(GetReportLeftEyeD);
        }

        public override async Task Initialize()
        {
            Tests = await _testsRepository.GetAll();
        }

        public int TotalPatients { get; set; }
        public int TotalPatientsD { get; set; }
        public int TotalImages { get; set; }
        public int TotalImagesD { get; set; }
        public int TotalImagesLeftEye { set; get; }
        public int TotalImagesLeftEyeD { set; get; }

        public int TotalPatientsLeftEye { set; get; }
        public int TotalPatientsLeftEyeD { set; get; }

        public int TotalPatientsRightEye { get; set; }
        public int TotalPatientsRightEyeD { get; set; }

        public int TotalImagesRightEye { get; set; }
        public int TotalImagesRightEyeD { get; set; }

        public string NameImage { set; get; }
        public string NameImage2 { set; get; }


        public List<TestDto> Tests { get; set; }
        public TestDto SelectedTest { get; set; }
        public TestDto SelectedTest2 { get; set; }


        public DateTime FromDate { get; set; }
        public DateTime FromDateD { set; get; }

        public DateTime ToDate { get; set; }
        public DateTime ToDateD { set; get; }

        public ICommand GetReportCommand { get; set; }
        public ICommand GetReportCommandD { get; set; }
        public ICommand GetReportRightEyeCommand { set; get; }
        public ICommand GetReportRightEyeCommandD { set; get; }

        public ICommand GetReportLeftEyeCommand { set; get; }
        public ICommand GetReportLeftEyeCommandD { set; get; }

        public List<PatientVisitReportModel> ReportData { get; set; }
        public List<PatientVisitReportModel> ReportDataD { get; set; }


        public void GetReport()
        {
            if (SelectedTest2 == null)
                return;
            NameImage = "Both Eye Report";
            Task.Run(() => {
                BusyExecute(async () => {
                    var report = await _patientVisitTestRepository
                        .GetByDateAndTest(FromDate.Date, ToDate.Date, SelectedTest2.Id);

                    var patients = new List<int>();
                    var images = new List<string>();

                    var patients1 = new List<int>();
                    var images1 = new List<string>();

                    foreach (var reportItem in report)
                    {
                        if (!patients.Contains(reportItem.PatientVisit.PatientId))
                        {
                            patients.Add(reportItem.PatientVisit.PatientId);
                        }

                        if (reportItem.RightEyeNote != null || reportItem.LeftEyeNote != null ||
                            !images.Contains(reportItem.RightEyeNote + reportItem.LeftEyeNote))
                        {
                            images.Add(reportItem.RightEyeNote + reportItem.LeftEyeNote);
                        }
                    }



                    TotalPatients = patients.Distinct().Count();
                    TotalImages = images.Distinct().Count();


                    ReportData = report.Select(e => new PatientVisitReportModel()
                    {
                        VisitDate = e.PatientVisit.VisitDate,
                        ImageNumber = e.ImageNumber,
                        ImageName = e.Test.EnName,
                        LeftEyeNote = e.LeftEyeNote,
                        RightEyeNote = e.RightEyeNote,
                        PatientName = e.PatientVisit.Patient.FullName
                    }).Where(e => e.LeftEyeNote != null || e.RightEyeNote != null).ToList();
                });
            });
        }

        public void GetReport2()
        {
            if (SelectedTest2 == null)
                return;
            NameImage2 = "Both Eye Report";
            Task.Run(() => {
                BusyExecute(async () => {
                    var report2 = await _patientVisitTestRepository
                        .GetByDateAndTestD(FromDateD.Date, ToDateD.Date, SelectedTest2.Id);

                    var patientsD = new List<int>();
                    var imagesD = new List<string>();

                    var patients1D = new List<int>();
                    var images1D = new List<string>();

                    foreach (var reportItem in report2)
                    {
                        if (!patientsD.Contains(reportItem.PatientVisit.PatientId))
                        {
                            patientsD.Add(reportItem.PatientVisit.PatientId);
                        }

                        if (reportItem.RightEyeNote == null || reportItem.LeftEyeNote == null ||
                            imagesD.Contains(reportItem.RightEyeNote + reportItem.LeftEyeNote))
                        {
                            imagesD.Add(reportItem.RightEyeNote + reportItem.LeftEyeNote);
                        }
                    }



                    TotalPatientsD = patientsD.Distinct().Count();
                    TotalImagesD = imagesD.Distinct().Count();


                    ReportDataD = report2.Select(e => new PatientVisitReportModel()
                    {
                        VisitDate = e.PatientVisit.VisitDate,
                        ImageNumber = e.ImageNumber,
                        ImageName = e.Test.EnName,
                        LeftEyeNote = e.LeftEyeNote,
                        RightEyeNote = e.RightEyeNote,
                        PatientName = e.PatientVisit.Patient.FullName
                    }).ToList();
                });
            });
        }

        public void GetReportRightEye()
        {
            if (SelectedTest == null)
                return;
            NameImage = "Right Eye Report";
            Task.Run(() =>
            {
                BusyExecute(async () =>
                {
                    var report = await _patientVisitTestRepository
                        .GetByDateAndTestRightEye(FromDate.Date, ToDate.Date, SelectedTest.Id);

                    var patients1 = new List<int>();
                    var images1 = new List<string>();
                    foreach (var reportItem in report)
                    {
                        if (!patients1.Contains(reportItem.PatientVisit.PatientId))
                        {
                            patients1.Add(reportItem.PatientVisit.PatientId);
                        }

                        if (reportItem.RightEyeNote != null ||
                            !images1.Contains(reportItem.RightEyeNote))
                        {
                            images1.Add(reportItem.RightEyeNote);
                        }
                    }

                    TotalPatientsRightEye = patients1.Distinct().Count();
                    TotalImagesRightEye = images1.Distinct().Count();

                    ReportData = report.Select(e => new PatientVisitReportModel()
                    {
                        VisitDate = e.PatientVisit.VisitDate,
                        ImageNumber = e.ImageNumber,
                        ImageName = e.Test.EnName,
                        LeftEyeNote = e.LeftEyeNote,
                        RightEyeNote = e.RightEyeNote,
                        PatientName = e.PatientVisit.Patient.FullName
                    }).ToList();
                });
            });    
        }
        public void GetReportRightEyeD()
        {
            if (SelectedTest2 == null)
                return;
            NameImage2 = "Right Eye Report";
            Task.Run(() =>
            {
                BusyExecute(async () =>
                {
                    var report2 = await _patientVisitTestRepository
                        .GetByDateAndTestRightEyeD( FromDateD.Date, ToDateD.Date, SelectedTest2.Id);

                    var patients1D = new List<int>();
                    var images1D = new List<string>();
                    foreach (var reportItem in report2)
                    {
                        if (!patients1D.Contains(reportItem.PatientVisit.PatientId))
                        {
                            patients1D.Add(reportItem.PatientVisit.PatientId);
                        }

                        if (reportItem.RightEyeNote == "" ||
                            images1D.Contains(reportItem.RightEyeNote))
                        {
                            images1D.Add(reportItem.RightEyeNote);
                        }
                    }

                    TotalPatientsRightEyeD = patients1D.Distinct().Count();
                    TotalImagesRightEyeD = images1D.Distinct().Count();

                    ReportDataD = report2.Select(e => new PatientVisitReportModel()
                    {
                        VisitDate = e.PatientVisit.VisitDate,
                        ImageNumber = e.ImageNumber,
                        ImageName = e.Test.EnName,
                        LeftEyeNote = e.LeftEyeNote,
                        RightEyeNote = e.RightEyeNote,
                        PatientName = e.PatientVisit.Patient.FullName
                    }).ToList();
                });
            });
          
        }

        public void GetReportLeftEye()
        {
            if (SelectedTest == null)
                return;
            NameImage = "Left Eye Report";
            Task.Run(() =>
            {
                BusyExecute(async () =>
                {
                    var report = await _patientVisitTestRepository
                      .GetByDateAndTestLeftEye(FromDate.Date, ToDate.Date, SelectedTest.Id);

                    var patients2 = new List<int>();
                    var images2 = new List<string>();
                    foreach (var reportItem in report)
                    {
                        if (!patients2.Contains(reportItem.PatientVisit.PatientId))
                        {
                            patients2.Add(reportItem.PatientVisit.PatientId);
                        }

                        if (reportItem.LeftEyeNote != null ||
                            !images2.Contains(reportItem.LeftEyeNote))
                        {
                            images2.Add(reportItem.LeftEyeNote);
                        }
                    }

                    TotalPatientsLeftEye = patients2.Distinct().Count();
                    TotalImagesLeftEye = images2.Distinct().Count();

                    ReportData = report.Select(e => new PatientVisitReportModel()
                    {
                        VisitDate = e.PatientVisit.VisitDate,
                        ImageNumber = e.ImageNumber,
                        ImageName = e.Test.EnName,
                        LeftEyeNote = e.LeftEyeNote,
                        RightEyeNote = e.RightEyeNote,
                        PatientName = e.PatientVisit.Patient.FullName
                    }).ToList();
                });
            });
        }
        public void GetReportLeftEyeD()
        {
            if (SelectedTest2 == null)
                return;
            NameImage2 = "Left Eye Report";
            Task.Run(() =>
            {
                BusyExecute(async () =>
                {
                    var report2 = await _patientVisitTestRepository
                        .GetByDateAndTestLeftEyeD(FromDateD.Date, ToDateD.Date, SelectedTest2.Id);

                    var patients1D = new List<int>();
                    var images1D = new List<string>();
                    foreach (var reportItem in report2)
                    {
                        if (!patients1D.Contains(reportItem.PatientVisit.PatientId))
                        {
                            patients1D.Add(reportItem.PatientVisit.PatientId);
                        }

                        if (reportItem.LeftEyeNote == "" ||
                            images1D.Contains(reportItem.LeftEyeNote))
                        {
                            images1D.Add(reportItem.LeftEyeNote);
                        }
                    }

                    TotalPatientsLeftEyeD = patients1D.Distinct().Count();
                    TotalImagesLeftEyeD = images1D.Distinct().Count();

                    ReportDataD = report2.Select(e => new PatientVisitReportModel()
                    {
                        VisitDate = e.PatientVisit.VisitDate,
                        ImageNumber = e.ImageNumber,
                        ImageName = e.Test.EnName,
                        LeftEyeNote = e.LeftEyeNote,
                        RightEyeNote = e.RightEyeNote,
                        PatientName = e.PatientVisit.Patient.FullName
                    }).ToList();
                });
            });

        }

    }

    public class PatientVisitReportModel
    {
        public string PatientName { get; set; }
        public DateTime VisitDate { get; set; }
        public string ImageName { get; set; }
        public string LeftEyeNote { get; set; }
        public string RightEyeNote { get; set; }
        public string ImageNumber { get; set; }
    }
}
