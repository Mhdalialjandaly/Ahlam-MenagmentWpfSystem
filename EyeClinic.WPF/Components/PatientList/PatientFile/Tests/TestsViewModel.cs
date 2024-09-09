using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using EyeClinic.Core;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.AppServices.Print;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Base.Extends;
using EyeClinic.WPF.Components.Home.Setting.Tests.TestEditor;
using EyeClinic.WPF.Components.PatientList.PatientFile.Tests.TestHistory;
using EyeClinic.WPF.Components.PatientList.PatientFile.Tests.TestPicker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Unity;
using WIA;
using EyeClinic.Core.Enums;
using EyeClinic.DataAccess.Entities;
using EyeClinic.WPF.Setup;
using EyeClinic.WPF.Components.Dialogs.PasswordInput;
using EyeClinic.WPF.Components.PatientList.PatientVisitList.PatientVisitEditor;
using Org.BouncyCastle.Asn1.X509;
using Operation = EyeClinic.Core.Enums.Operation;
using AutoMapper;
using EyeClinic.WPF.AppServices.Localization;
using System.Security.AccessControl;
using Syncfusion.CompoundFile.XlsIO.Native;
using EyeClinic.WPF.Components.PatientList.PatientFile.SpecialNote;
using System.Text;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Graphics;
using EyeClinic.WPF.Components.PatientList.PatientFile.Tests.PdfFile;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf.IO;
using EyeClinic.WPF.Components.PatientList.PatientFile.PDFMerger;
namespace EyeClinic.WPF.Components.PatientList.PatientFile.Tests
{
    public partial class TestsViewModel
    {
        public TestsViewModel()
        {
            if (IsDesignMode) { }
        }

        public ObservableCollection<TestHistoryModel> PatientVisitsTest { get; internal set; }
    }

    public partial class TestsViewModel : BaseViewModel<TestsView>
    {
        private readonly IPatientVisitTestRepository _patientVisitTestRepository;
        private readonly ITestsRepository _testsRepository;
        private readonly IUnityContainer _container;
        private readonly IDialogService _dialogService;
        private readonly IPatientVisitRepository _patientVisitRepository;
        private readonly IMedicalCenterRepository _medicalCenterRepository;
        private readonly INotificationService _notificationService;
        private readonly IPatientRepository _patientRepository;

        public TestsViewModel(IPatientVisitTestRepository patientVisitTestRepository,IPatientRepository patientRepository, INotificationService notificationService, ITestsRepository testsRepository,
            IUnityContainer container, IDialogService dialogService,
            IPatientVisitRepository patientVisitRepository, IMedicalCenterRepository medicalCenterRepository)
        {
            _patientVisitTestRepository = patientVisitTestRepository;
            _testsRepository = testsRepository;
            _notificationService = notificationService;
            _container = container;
            _dialogService = dialogService;
            _patientVisitRepository = patientVisitRepository;
            _medicalCenterRepository = medicalCenterRepository;
            _patientRepository = patientRepository; 

            AddTestCommand = new RelayCommand(AddTest);
            AddNewTestCommand = new RelayCommand(AddNewTest);
            SaveRowCommand = new RelayCommand<PatientVisitsTestDto>(SaveRow);
            DeleteRowCommand = new RelayCommand<PatientVisitsTestDto>(DeleteRow);
            EditRowCommand = new RelayCommand<PatientVisitsTestDto>(EditeRow);
            TestHistoryCommand = new RelayCommand(TestHistory);
            SaveAllCommand = new RelayCommand(SaveAll);

            UploadImageCommand = new RelayCommand<PatientVisitsTestDto>(UploadImage);
            UploadImageCommand2 = new RelayCommand<PatientVisitsTestDto>(UploadImage2);
            UploadImageCommand3 = new RelayCommand<PatientVisitsTestDto>(UploadImage3);

            ImageScanCommand = new RelayCommand<PatientVisitsTestDto>(ImageScan);

            UploadOrderPdfSourceCommand = new RelayCommand<PatientVisitsTestDto>(UploadOrderPdfSource);
            OpenImageCommand2 = new RelayCommand<PatientVisitsTestDto>(OpenImage2);
            OpenImageCommand3 = new RelayCommand<PatientVisitsTestDto>(OpenImage3);

            PrintCommand = new RelayCommand(Print);
            DownloadPdfFileCommand = new RelayCommand<PatientVisitsTestDto>(DownloadPdfFile);
            DownloadImageFileCommand = new RelayCommand<PatientVisitsTestDto>(DownloadImageFile);
            ShowPdfFileCommand = new RelayCommand<PatientVisitsTestDto>(ShowPdfFile);
            ShowOrderPdfCommand = new RelayCommand<PatientVisitsTestDto>(ShowOrderPdfFile);
            MergeAllPDFWithImagesCommand = new RelayCommand(MergeAllPDFWithImages);
        }


        public int NumberBinding { get; set; }
        public string PdfSource { get; set; }
        public int PatientVisitId { get; set; }
        public int PatientId { get; set; }
        public Model.TestDto SelectedTests { get; set; }
        public DateTime? VisitDate { get; set; }
        public PatientFileViewModel PatientFileViewModel { get; set; }

        public ObservableCollection<MedicalCenterDto> MedicalCenters { get; set; }
        public ObservableCollection<MedicalCenterDto> SelectedMedicalCenters { get; set; }
        public ObservableCollection<Model.TestDto> PatientItems { get; set; }

        public List<TestDto> Tests { get; set; }
        public TestDto SelectedTest { get; set; }
        public bool LeftEyeRequired { get; set; }
        public bool RightEyeRequired { get; set; }
        public int PatientVisitIdin { get; set; }
        public string OrderName { get; set; }
        public string IndexN { get; set; }
        public ObservableCollection<PatientVisitsTestDto> PatientVisitTests { get; set; }

        public ICommand AddNewTestCommand { get; set; }
        public ICommand AddTestCommand { get; set; }
        public ICommand SaveRowCommand { get; set; }
        public ICommand DeleteRowCommand { get; set; }
        public ICommand EditRowCommand { get; set; }
        public ICommand TestHistoryCommand { get; set; }
        public ICommand SaveAllCommand { get; set; }
        public ICommand UploadImageCommand { get; set; }
        public ICommand UploadImageCommand2 { get; set; }
        public ICommand UploadImageCommand3 { get; set; }
        public ICommand PrintCommand { get; set; }
        public ICommand ShowOrderPdfCommand { get; set; }
        public ICommand UploadOrderPdfSourceCommand { get; set; }
        public ICommand MergeAllPDFWithImagesCommand { get; set; }

        public ICommand OpenImageCommand2 { get; set; }
        public ICommand OpenImageCommand3 { get; set; }
        public ICommand ImageScanCommand { get; set; }
        public ICommand DownloadPdfFileCommand { get; set; }
        public ICommand DownloadImageFileCommand { get; set; }
        public ICommand ShowPdfFileCommand { get; set; }
        public ICommand CancelCommand => new RelayCommand(() =>
        {
            ContainerHandler.Container.Resolve<IDialogService>().DisposeLastDialog();
        });

        public async Task Initialize(int patientVisitId)
        {
            PatientVisitIdin = patientVisitId;
            PatientVisitId = patientVisitId;
            Tests = await _testsRepository.GetAll();


            var item = await _patientVisitRepository.Get().AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == patientVisitId);
           
            VisitDate = await _patientVisitRepository
              .GetVisitDateById(PatientVisitId);

            //VisitDate = await _patientVisittestRepository
            //  .GetVisitDateById(PatientVisitId);

            if (item != null)
                PatientId = item.PatientId;


            var pa = await _patientRepository.GetById(PatientId);
            OrderName = pa.FullName;

            var requiredTests = await _patientVisitTestRepository
                .GetByKey(e => e.PatientVisitId == patientVisitId);
            if (requiredTests == null)
                return;

            NumberBinding = requiredTests.Count();
           
            MedicalCenters = new ObservableCollection<MedicalCenterDto>(
                await _medicalCenterRepository.GetAll());
            int index = 0;
            PatientVisitTests = new ObservableCollection<PatientVisitsTestDto>(requiredTests);
            foreach (var itempvs in PatientVisitTests)
            {
                itempvs.Test.IndexN = index + 1;
                index++;
            }

        }

        public async void AddTest()
        {
            var request = _container.Resolve<TestPickerViewModel>();
            await request.Initialize();

            _dialogService.ShowEditorDialog(request.GetView() as TestPickerView, async () =>
            {
                var selectedTests = request.Tests
                    .Where(e => e.IsChecked)
                    .ToList();

                if (selectedTests.IsNullOrEmpty())
                    return false;

                PatientVisitTests ??= new ObservableCollection<PatientVisitsTestDto>();
                var filteredTests = new List<Checkable<TestDto>>();

                foreach (var selectedTest in selectedTests)
                {
                    if (PatientVisitTests.All(e => e.Test.Code != selectedTest.Item.Code))
                    {
                        filteredTests.Add(selectedTest);
                    }
                }

                await _patientVisitTestRepository.AddRange(
                    filteredTests.Select(e => new PatientVisitsTestDto
                    {
                        TestId = e.Item.Id,
                        PatientVisitId = PatientVisitId,
                        RightEye = e.RightEye,
                        LeftEye = e.LeftEye
                    }).ToList());
                await Initialize(PatientVisitId);
                return true;
            });
        }



        private void AddNewTest()
        {
            BusyExecute(async () => {
                var editor = _container.Resolve<TestEditorViewModel>();
                await editor.Initialize();
                editor.Operation = Core.Enums.Operation.Add;

                _container.Resolve<IDialogService>()
                    .ShowEditorDialog(editor.GetView() as TestEditorView, async () => {
                        
                        var items = await editor.Save();
                        if (items == null)
                            return false;
                        //PatientItems.Add(items);
                        return true;
                    });
            });
        }


        public void SaveRow(PatientVisitsTestDto patientVisitsTest)
        {
            BusyExecute(async () =>
            {
                await _patientVisitTestRepository.Update(patientVisitsTest);
                OnSave?.Invoke();
            });
        }
        public void DeleteRow(PatientVisitsTestDto patientVisitsTest)
        {
            if (patientVisitsTest.Locked)
            {
                _notificationService.Warning("not for delete" + "is locked");
                return;
            }
            if (_container.CheckUserRole(UserRoles.Admin))
            {
                if (MessageBox.Show("Do You Want To Delete ??", "Conform", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    BusyExecute(async () =>
                    {
                        await _patientVisitTestRepository.Delete(patientVisitsTest.Id);
                        PatientVisitTests.Remove(patientVisitsTest);
                        OnSave?.Invoke();
                    });
                }
            }
            else
            {
                var pwd = _container.Resolve<PasswordInputViewModel>();
                pwd.DisposeOnLogin = false;
                pwd.CustomPassword = "54425";
                pwd.OnSuccessLogin += () => {
                    BusyExecute(async () => {
                        await _patientVisitTestRepository.Delete(patientVisitsTest.Id);
                        PatientVisitTests.Remove(patientVisitsTest);
                        OnSave?.Invoke();
                    });
                };
                _container.Resolve<IDialogService>()
                  .ShowPopupContent(pwd.GetView() as PasswordInputView);
            }
        }
        public void EditeRow(PatientVisitsTestDto patientVisitsTest)
        {
           
            EditeTheTests(patientVisitsTest);
            //var password = _container.Resolve<PasswordInputViewModel>();
            //password.OnSuccessLogin += () => EditeTheTests(patientVisitsTest);
            //_container.Resolve<IDialogService>().
            //    ShowPopupContent(password.GetView() as PasswordInputView);
            //return;

        }
        public void EditeTheTests(PatientVisitsTestDto patientVisitsTest)
        {
            SelectedTests = patientVisitsTest.Test;
            BusyExecute(async () => {
                var editor = _container.Resolve<TestEditorViewModel>();
                await editor.Initialize();
                editor.Operation = Operation.Edit;
                editor.BuildFromModel(SelectedTests);

                _container.Resolve<IDialogService>()
                    .ShowEditorDialog(editor.GetView() as TestEditorView, async () => {
                        var item = await editor.Save();
                        OnSave?.Invoke();
                        await Initialize(PatientVisitId);
                        if (item == null)
                            return false;

                        return true;
                    });

            });
        }

        private PatientVisitsTestDto BuildFromProperties()
        {
            if (SelectedTest == null)
                return null;

            return new PatientVisitsTestDto
            {
                LeftEye = LeftEyeRequired,
                RightEye = RightEyeRequired,
                PatientVisitId = PatientVisitId,
                TestId = SelectedTest.Id
            };
        }

        private void TestHistory()
        {
            BusyExecute(async () =>
            {
                var history = _container.Resolve<TestHistoryViewModel>();
                await history.Initialize(PatientId);
                _dialogService.ShowInformationDialog(history.GetView() as TestHistoryView);
            });
        }

        public Action OnSave;
        private void SaveAll()
        {
            BusyExecute(async () =>
            {
                await _patientVisitTestRepository.UpdateRange(PatientVisitTests.ToList());
                OnSave?.Invoke();
            });
        }


        private void UploadImage2(PatientVisitsTestDto patientVisitTest)
        {
            try
            {
                var openFileDialog2 = new OpenFileDialog()
                {
                    Filter = "Images|*.jpg;*.jpeg;*.png"
                };
                var imagesDirectory2 = Global.TestImageDirectory;
                if (openFileDialog2.ShowDialog() == true)
                {
                    var newFileName2 = DateTime.Now.ToString("yyyy-mm-ddTHH-mm-ss")
                                      + Path.GetExtension(openFileDialog2.SafeFileName);
                    Directory.CreateDirectory(imagesDirectory2);
                    File.Copy(openFileDialog2.FileName,
                        Path.Combine(imagesDirectory2, newFileName2), true);
                    patientVisitTest.ImageNameRight = newFileName2;
                    if (string.IsNullOrEmpty(patientVisitTest.ImageNumber))
                    {
                        patientVisitTest.ImageNumber = "0";
                    }
                    SaveRow(patientVisitTest);
                    OnPropertyChanged();
                }
            }
            catch (Exception ex)
            {
                _container.Resolve<INotificationService>()
                    .Error(ex.GetBaseException().Message);
            }
        }
        private void UploadImage3(PatientVisitsTestDto patientVisitTest)
        {
            try
            {
                var openFileDialog3 = new OpenFileDialog()
                {
                    Filter = "Images|*.jpg;*.jpeg;*.png"
                };

                var imagesDirectory3 = Global.TestImageDirectory;
                if (openFileDialog3.ShowDialog() == true)
                {
                    var newFileName3 = DateTime.Now.ToString("yyyy-mm-ddTHH-mm-ss")
                                      + Path.GetExtension(openFileDialog3.SafeFileName);
                    Directory.CreateDirectory(imagesDirectory3);
                    File.Copy(openFileDialog3.FileName,
                        Path.Combine(imagesDirectory3, newFileName3), true);
                    patientVisitTest.ImageNameBoth = newFileName3;
                    if (string.IsNullOrEmpty(patientVisitTest.ImageNumber))
                    {
                        patientVisitTest.ImageNumber = "0";
                    }
                    SaveRow(patientVisitTest);
                    OnPropertyChanged();
                }
            }
            catch (Exception ex)
            {
                _container.Resolve<INotificationService>()
                    .Error(ex.GetBaseException().Message);
            }
        }

        private void UploadImage(PatientVisitsTestDto patientVisitTest)
        {
            try
            {
                var openFileDialog = new OpenFileDialog()
                {
                    Filter = "Images|*.jpg;*.jpeg;*.png"
                };

                var imagesDirectory = Global.TestImageDirectory;
                if (openFileDialog.ShowDialog() == true)
                {
                    var newFileName = DateTime.Now.ToString("yyyy-mm-ddTHH-mm-ss")
                                      + Path.GetExtension(openFileDialog.SafeFileName);
                    Directory.CreateDirectory(imagesDirectory);
                    File.Copy(openFileDialog.FileName,
                        Path.Combine(imagesDirectory, newFileName), true);
                    patientVisitTest.ImageNameLeft = newFileName;
                    if (string.IsNullOrEmpty(patientVisitTest.ImageNumber))
                    {
                        patientVisitTest.ImageNumber = "0";
                    }

                    SaveRow(patientVisitTest);
                    OnPropertyChanged();
                }
            }
            catch (Exception ex)
            {
                _container.Resolve<INotificationService>()
                    .Error(ex.GetBaseException().Message);
            }

            //here
            //Scan();
        }
        //public void Scan()
        //{
        //    ImageFile image;

        //    try
        //    {
        //        var dialog = new WIA.CommonDialog();

        //        var device = dialog.ShowSelectDevice(DeviceTypes.Scanner, false, true);
        //        var ScanerItem = device.Items; // select the scanner.

        //        //var imgFile = (ImageFile)ScanerItem.Transfer(FormatID.wiaFormatJPEG); //Retrive an image in Jpg format and store it into a variable.

        //        var Path = @"E:\ScanImg.jpg"; // save the image in some path with filename.

        //        if (File.Exists(Path))
        //        {
        //            File.Delete(Path);
        //        }

        //        //imgFile.SaveFile(Path);

        //        //pictureBox1.ImageLocation = Path;
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        public void ImageScan(PatientVisitsTestDto patientVisitTest)
        {

            //try
            //{
            //    var dialog = new WIA.CommonDialog();
            //    ImageFile image = dialog.ShowAcquireImage(AlwaysSelectDevice: true);
            //    string path = @"E:\scanner\Images"+DateTime.Now.ToString("hh")+".jpg";
            //    var imagesDirectory = Global.TestImageDirectory;
            //    if (image != null)
            //    {
            //        image.SaveFile(path);
            //        Directory.CreateDirectory(imagesDirectory);
            //        File.Copy(path,
            //            Path.Combine(imagesDirectory, path), true);
            //        patientVisitTest.ImageNameLeft = path;
            //        SaveRow(patientVisitTest);
            //        OnPropertyChanged();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    _container.Resolve<INotificationService>()
            //        .Error(ex.GetBaseException().Message);
            //}


        }
        private void Print()
        {
            BusyExecute(async () =>
            {
                await _patientVisitTestRepository.UpdateRange(PatientVisitTests.ToList());

                var patient = await _container.Resolve<IPatientRepository>()
                    .GetById(PatientId);
                _container.Resolve<IPrintService>()
                    .PrintTests(patient, PatientVisitTests.ToList());
            });
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
        private  void DownloadPdfFile(PatientVisitsTestDto testDto)
        {
            if (testDto.Test.ImagePath != null)
            {
                PdfViewer pdfViewer = new PdfViewer();
                pdfViewer.PdfViewerView.LoadFile(testDto.Test.ImagePath);
                pdfViewer.ProductNameLabel.Text = testDto.Test.ArName;
                pdfViewer.CodeLabel.Text = testDto.Test.Code;
                pdfViewer.ShowDialog();
            }
            else
            {
                _notificationService.Warning(
                  _container.Resolve<ILocalizationService>()
                      .Localize("CheckPath"));
            }
        }

        private  void ShowPdfFile(PatientVisitsTestDto dto)
        {
            //try
            //{
            //    bool pathExists = File.Exists("ImageFolder\\" + dto.Test.Code.Trim() + ".jpg") 
            //        && Directory.Exists("ImageFolder")
            //        && File.Exists("PDFFolder\\" + dto.Test.Code.Trim() + ".Pdf")
            //        && Directory.Exists("PDFFolder");
            //    if (!pathExists)
            //    {
            //        _notificationService.Warning("الرجاء تحميل الصورة والملف الرقمي اولا");
            //        return;
            //    }
            //    else
            //    {
            //        PdfViewer pdfViewer = new PdfViewer();
            //        pdfViewer.PdfViewerView.LoadFile("PDFFolder\\" + dto.Test.Code.Trim() + ".Pdf");
            //        pdfViewer.axWebBrowser1.Navigate("PDFFolder\\" + dto.Test.Code.Trim() + ".Pdf");
            //        pdfViewer.ProductNameLabel.Text = dto.Test.ArName;
            //        pdfViewer.pictureBox1.Load("ImageFolder\\" + dto.Test.Code.Trim() + ".jpg");
            //        pdfViewer.CodeLabel.Text = dto.Test.Code;
            //        pdfViewer.ShowDialog();
            //    }
            //}
            //catch (Exception)
            //{

            //    throw;
            //}
            try
            {
                PdfViewer pdfViewer = new PdfViewer();
                pdfViewer.PdfViewerView.LoadFile(dto.PdfSource);
                pdfViewer.axWebBrowser1.Navigate(dto.PdfSource);
                pdfViewer.ProductNameLabel.Text = dto.Test.ArName;
                pdfViewer.pictureBox1.Load("ImageFolder\\" + dto.Test.Code.Trim() + ".jpg");
                pdfViewer.CodeLabel.Text = dto.Test.Code;
                pdfViewer.ShowDialog();
            }
            catch (Exception ex )
            {
                _notificationService.Warning(ex.Message);
            }
          

        }
      

        private void DownloadImageFile(PatientVisitsTestDto testDto)
        {
            
                    try
                    {                      
                        PdfViewer pdfViewer = new PdfViewer();
                        pdfViewer.pictureBox1.Load(testDto.Test.ImagePath2);
                        pdfViewer.ProductNameLabel.Text = testDto.Test.ArName;
                        pdfViewer.PdfViewerView.LoadFile(testDto.PdfSource);
                        pdfViewer.CodeLabel.Text = testDto.Test.Code;
                        pdfViewer.ShowDialog();
                    }
                    catch (Exception ex)
                    {
                        _notificationService.Warning(
                        _container.Resolve<ILocalizationService>()
                       .Localize(ex.Message));
                    }              
          
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
        private void ShowOrderPdfFile(PatientVisitsTestDto dto)
        {
            PdfViewer pdfViewer = new PdfViewer();
            pdfViewer.PdfViewerView.LoadFile(dto.PdfSource);
            pdfViewer.axWebBrowser1.Navigate(dto.PdfSource);
            pdfViewer.ProductNameLabel.Text = dto.PdfSource;
            pdfViewer.CodeLabel.Text = dto.Id.ToString();           
            pdfViewer.ShowDialog();
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
        private async void UploadOrderPdfSource(PatientVisitsTestDto dto)
        {
            try
            {
                var openFileDialog3 = new OpenFileDialog()
                {
                    Filter = "PDF|*.pdf;"
                };

                var pdffile = Global.TestImageDirectory;
                if (openFileDialog3.ShowDialog() == true)
                {
                    var newFileName3 = DateTime.Now.ToString("yyyy-mm-ddTHH-mm-ss")
                                      + Path.GetExtension(openFileDialog3.SafeFileName);
                    Directory.CreateDirectory(pdffile);
                    File.Copy(openFileDialog3.FileName,
                        Path.Combine(pdffile, newFileName3), true);
                    dto.PdfSource = pdffile+"\\"+newFileName3;                   
                    SaveRow(dto);
                    OnPropertyChanged();
                    await Initialize(PatientVisitId);
                }
            }
            catch (Exception ex)
            {
                _container.Resolve<INotificationService>()
                    .Error(ex.GetBaseException().Message);
            }
        }
        private string CompineThePath()
        {
          var x =  Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Orders", "PDFFiles");

            return x;
        }
        private void MergeAllPDFWithImages()
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            
            foreach (var item in PatientVisitTests)
            {
                if (!Directory.Exists(desktopPath + "\\" + item.Test.ArName))                
                    Directory.CreateDirectory(desktopPath + "\\" + OrderName);

                string savepath =  desktopPath + "\\" + OrderName ;
                try
                {
                    PDFMergerWindow pDF = new PDFMergerWindow();
                    pDF.TextPathFile1.Text = item.PdfSource;                    
                    ImageToPdf(item.Test.ImagePath2, "Output.pdf");
                    pDF.TextPathFile2.Text = "Output.pdf";
                    pDF.textTarget.Text = savepath;
                    pDF.MergPDFAction(item);
                    pDF.Close();
                }
                catch (Exception ex)
                {
                    _notificationService.Error(ex.Message);
                }
                 
            }
        }
        public static void ImageToPdf(string imagePath, string pdfPath)
        {
            using (var document = new PdfDocument())
            {
                var page = document.AddPage();
                var gfx = XGraphics.FromPdfPage(page);

                using (var image = XImage.FromFile(imagePath))
                {
                    gfx.DrawImage(image, 0, 0, page.Width, page.Height);
                }

                document.Save(pdfPath);
            }
        }
        public static void MergePDFs(string targetPath, params string[] pdfPaths)
        {
            using (var targetDoc = new PdfDocument())
            {
                foreach (var pdfPath in pdfPaths)
                {
                    using (var pdfDoc = PdfReader.Open(pdfPath, PdfDocumentOpenMode.Import))
                    {
                        for (var i = 0; i < pdfDoc.PageCount; i++)
                        {
                            targetDoc.AddPage(pdfDoc.Pages[i]);
                        }
                    }
                }
                targetDoc.Save(targetPath);
            }
        }
    }
    
}
