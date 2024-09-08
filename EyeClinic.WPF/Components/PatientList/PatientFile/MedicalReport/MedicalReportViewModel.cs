using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using EyeClinic.Core;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.Print;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Base.Extends;
using EyeClinic.WPF.Components.PatientList.PatientFile.MedicalReport.MedicalReportTemplate;
using EyeClinic.WPF.Components.PatientList.PatientVisitList.VisitNote;
using Unity;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.MedicalReport
{
    public partial class MedicalReportViewModel
    {
        public MedicalReportViewModel() { }
        private readonly IDialogService _dialogService;
    }


    public partial class MedicalReportViewModel : BaseViewModel<MedicalReportView>
    {
        private readonly IUnityContainer _container;
        public MedicalReportViewModel(IUnityContainer container) {
            _container = container;
            ClearCommad = new RelayCommand(Clear);
            SaveCommand = new RelayCommand(Save);
            PrintCommand = new RelayCommand(Print);
        }

        public void Initialize(PatientVisitDto patientVisit, PatientDto patient) {
            try
            {
                PatientVisit = patientVisit;
                Patient = patient;

                var medicalReportKey = PatientVisit.Id + "-" + PatientVisit.PatientId;
                var fileLocation = Path.Combine(Global.MedicalReportPath, medicalReportKey);
                Directory.CreateDirectory(Global.MedicalReportPath);

                if (GetView() is MedicalReportView view)
                {
                    TextRange range;
                    FileStream fStream;
                    if (File.Exists(fileLocation))
                    {
                        range = new TextRange(view.mainRTB.Document.ContentStart,
                            view.mainRTB.Document.ContentEnd);
                        fStream = new FileStream(fileLocation, FileMode.OpenOrCreate);
                        range.Load(fStream, DataFormats.XamlPackage);
                        fStream.Close();
                    }
                }

                Directory.CreateDirectory(Global.MedicalReportTemplatePath);
                ReportTemplates = new ObservableCollection<string>();
                string[] files = Directory.GetFiles(Global.MedicalReportTemplatePath);
                foreach (string file in files)
                {
                    ReportTemplates.Add(Path.GetFileName(file));
                }
            }
            catch (Exception)
            {

                throw;
            }    
           
        }

        public ObservableCollection<string> ReportTemplates { get; set; }
        private string _selectedReportTemplate;

        public string SelectedReportTemplate {
            get { return _selectedReportTemplate; }
            set {
                _selectedReportTemplate = value;
                if (!string.IsNullOrWhiteSpace(value))
                    GetReportTemplate(value);
            }
        }



        public Action OnSave;

        public PatientVisitDto PatientVisit { get; set; }
        public PatientDto Patient { get; set; }

        public ICommand ClearCommad { set; get; }
        public ICommand PrintCommand { get; set; }
        public ICommand SaveCommand { get; set; }

        public ICommand CancelCommand => new RelayCommand(() => {
            ContainerHandler.Container.Resolve<IDialogService>().DisposeLastDialog();
        });
        public ICommand ManageTemplateCommand => new RelayCommand(ManageTemplate);


        private void ManageTemplate() {
            BusyExecute(async () => {
                var template = _container.Resolve<MedicalReportTemplateViewModel>();
                await template.Initialize();

                _container.Resolve<IDialogService>()
                    .ShowInformationDialog(template.GetView() as MedicalReportTemplateView,
                    () => {
                        Initialize(PatientVisit, Patient);
                    });
            });
        }

        private void Save() {
            var medicalReport = _container.Resolve<MedicalReportViewModel>();
            var medicalReportKey = PatientVisit.Id + "-" + PatientVisit.PatientId;
            var fileLocation = Path.Combine(Global.MedicalReportPath, medicalReportKey);
            Directory.CreateDirectory(Global.MedicalReportPath);
            if (GetView() is MedicalReportView view) {
                TextRange range;
                FileStream fStream;
                range = new TextRange(view.mainRTB.Document.ContentStart,
                view.mainRTB.Document.ContentEnd);
                fStream = new FileStream(fileLocation, FileMode.Create);
                range.Save(fStream, DataFormats.XamlPackage);
                fStream.Close();

            }
            OnSave?.Invoke();
          
        }
        private void Clear()
        {        
        }
      


        private void Print() {
            if (GetView() is MedicalReportView view) {
                _container.Resolve<IPrintService>().PrintNote(
                     Patient, new TextRange(view.mainRTB.Document.ContentStart,
                    view.mainRTB.Document.ContentEnd).Text);
            }
        }

        private void GetReportTemplate(string templateName) {
            var medicalReportKey = templateName;
            var fileLocation = Path.Combine(Global.MedicalReportTemplatePath, medicalReportKey);
            Directory.CreateDirectory(Global.MedicalReportTemplatePath);

            if (GetView() is MedicalReportView view) {
                TextRange range;
                FileStream fStream;
                if (File.Exists(fileLocation)) {
                    range = new TextRange(view.mainRTB.Document.ContentStart,
                        view.mainRTB.Document.ContentEnd);
                    fStream = new FileStream(fileLocation, FileMode.OpenOrCreate);
                    range.Load(fStream, DataFormats.XamlPackage);
                    fStream.Close();
                    
                }
            }
           
        }
    }
}
