using System;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using AutoMapper;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.Localization;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.AppServices.Print;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Base.Extends;
using EyeClinic.WPF.Components.PatientList.PatientFile.Tests.PdfFile;
using Syncfusion.Pdf.Graphics;
using Unity;

namespace EyeClinic.WPF.Components.PatientList.PatientVisitList.VisitNote
{
    public partial class VisitNoteViewModel
    {
        public VisitNoteViewModel() {
            if (IsDesignMode) { }
        }

        public Action<object, object ,object> OnSave { get; internal set; }
    }

    public partial class VisitNoteViewModel : BaseViewModel<VisitNoteView>
    {
        private readonly IUnityContainer _container;
        private readonly IPatientVisitRepository _patientVisitRepository;
        private readonly IPrintService _printService;
        private readonly INotificationService _notificationService;

        public VisitNoteViewModel(IUnityContainer container,INotificationService notificationService, IPatientVisitRepository patientVisitRepository,
                IPrintService printService) {
            _container = container;
            _patientVisitRepository = patientVisitRepository;
            _printService = printService;
            _notificationService = notificationService;

            PrintNoteCommand = new RelayCommand(PrintNote);
            UploadPdfFileCommand = new RelayCommand(UploadPdfFile);
            DownloadPdfFileCommand = new RelayCommand(DownloadPdfFile);
        }

       

        public async void Initialize(int patientVisitId, string note,string pdfPath) {
            PatientVisitId = patientVisitId;
            Note = note;
            BaseNote = note;
            path=pdfPath;
                   

            PatientVisitId = patientVisitId;
            VisitDate = await _patientVisitRepository
              .GetVisitDateById(PatientVisitId);
        }

        public int PatientVisitId { get; set; }
        public DateTime? VisitDate { get; set; }

        public string Note { get; set; }
        private string BaseNote { get; set; }
        public byte[] PdfFile { get; set; }
        public string path { get; set; }
        public string LoadedPdf { get; set; }
        public ICommand UploadPdfFileCommand { set; get; }
        public ICommand DownloadPdfFileCommand { set; get; }
        public ICommand PrintNoteCommand { get; set; }
        public ICommand BackCommand => new RelayCommand(() => {
            ContainerHandler.Container.Resolve<IDialogService>().DisposeLastDialog();
        });

        public async Task<bool> Save() {
            try {
                BusyStatus = "Please wait";
                IsBusy = true;
                if (BaseNote != null)
                    await _patientVisitRepository.UpdateVisitNote(PatientVisitId, Note,path);
                OnSave?.Invoke(this, Note,path); 
                return true;
            } catch (Exception ex) {
                LogError(ex.Message, ex);
                return false;
            } finally {
                IsBusy = false;
            }
        }
        public void GetImageFromPath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PDF Files (*.pdf)|*.pdf;";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog.FileName;
                // Use 'selectedFilePath' as needed (e.g., load the image)
                path = selectedFilePath;
                //image1 = Image.FromFile(selectedFilePath);
                _notificationService.Success("This Pdf  Has Added");
            }

        }
        private  void UploadPdfFile()
        {
            GetImageFromPath();
            if (string.IsNullOrWhiteSpace(path))
                return;            
         
             DownloadPdfFile();
        }
     
        public void PrintNote() {
            BusyExecute(async () => {
                var patient = await _container.Resolve<IPatientRepository>()
                    .GetByPatientVisitId(PatientVisitId);
                _printService.PrintNote(patient, Note);
            });
        }
        private void DownloadPdfFile()
        {
            if (!Directory.Exists("PDFLotFolder"))
            {
                Directory.CreateDirectory("PDFLotFolder");
            }
           
            try
            {
                //string noSpaces = Path.GetFileName(PatientVisitId.ToString()).Replace(" ", "");
                //using (var pdfFileStream = new FileStream("PDFLotFolder\\" + noSpaces + ".pdf", FileMode.Create))
                //{
                //    pdfFileStream.Write(PdfFile, 0, PdfFile.Length);
                //    _notificationService.Success("تم");
                //}
                PdfViewer pdfViewer = new PdfViewer();
                pdfViewer.PdfViewerView.LoadFile(path);
                pdfViewer.ProductNameLabel.Text = Path.GetFileName(path);
                pdfViewer.CodeLabel.Text = PatientVisitId.ToString();
                pdfViewer.ShowDialog();
            }
            catch (Exception ex)
            {
                _notificationService.Warning(
                _container.Resolve<ILocalizationService>()
               .Localize("الرجاء التاكد من تحميل الملف"));
            }
            
        }

        public async Task<bool> SaveCustomNote()
        {
            try {
                BusyStatus = "Please wait";
                IsBusy = true;
                if (BaseNote != Note )
                    await _patientVisitRepository.UpdateVisitMedicalReport(PatientVisitId, Note);
                return true;
            } catch (Exception ex) {
                LogError(ex.Message, ex);
                return false;
            } finally {
                IsBusy = false;
            }
        }
    }
}
