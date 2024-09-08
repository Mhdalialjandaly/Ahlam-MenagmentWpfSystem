using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.Localization;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.PatientList.PatientEditor;
using Unity;
using Operation = EyeClinic.Core.Enums.Operation;

namespace EyeClinic.WPF.Components.Shell.Header
{
    public class HeaderViewModel : BaseViewModel<HeaderView>
    {
        private readonly IDialogService _dialogService;
        private readonly ILocalizationService _localizationService;
        private readonly IUnityContainer _container;

        public HeaderViewModel(IDialogService dialogService, ILocalizationService localizationService,
            IUnityContainer container) {
            _dialogService = dialogService;
            _localizationService = localizationService;
            _container = container;
            CloseCommand = new RelayCommand(Close);
            MinimizeCommand = new RelayCommand(Minimize);
            MaximizeCommand = new RelayCommand(Maximize);
            EditPatientCommand = new RelayCommand(EditPatient);
            SmallMinimizeCommand = new RelayCommand(SmallMinimize);
        }

        

        public override Task Initialize() {
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += TimerOnTick;
            timer.Start();

            return base.Initialize();
        }
        public string CurrentTime { get; set; }
        public PatientDto CurrentPatient { get; set; }
        public bool IsViewOnly { get; set; }
        public List<string> ImageNumbers { get; set; }
        public ICommand CloseCommand { get; set; }

        public ICommand SmallMinimizeCommand { set; get;}
        public ICommand MinimizeCommand { get; set; }
        public ICommand MaximizeCommand { get; set; }

        public ICommand EditPatientCommand { get; set; }

        private void TimerOnTick(object sender, EventArgs e) {
            CurrentTime = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
        }

        public void SetCurrentPatient(PatientDto patient, bool isViewOnly) {
            BusyExecute(async () => {
                CurrentPatient = patient;
                IsViewOnly = isViewOnly;
                if (patient == null)
                    ImageNumbers = new List<string>();
                else
                    ImageNumbers = await _container.Resolve<IPatientVisitTestRepository>()
                        .GetImagesByPatientId(patient.Id);
            });
        }

        public void Close() {
            _dialogService.ShowConfirmationMessage(_localizationService.Localize("AreYouSureExit"), () => {
                Application.Current.Shutdown();
                return Task.FromResult(true);
            });
        }
        public void SmallMinimize()
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }
        public void Minimize() {
            if (Application.Current.MainWindow != null)
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            Application.Current.MainWindow.WindowStyle = WindowStyle.ToolWindow;
        }
        private void Maximize()
        {
            if (Application.Current.MainWindow != null)
                Application.Current.MainWindow.WindowStyle = WindowStyle.None;
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
           
        }


        public bool PatientInfoAlreadyVisible { get; set; }

        private void EditPatient() {
            BusyExecute(async () => {
                if (PatientInfoAlreadyVisible)
                    return;

                var editor = _container.Resolve<PatientEditorViewModel>();
                await editor.Initialize();
                editor.BuildFromModel(CurrentPatient);
                editor.Operation = Operation.Edit;

                _dialogService.ShowEditorDialog(editor.GetView() as PatientEditorView, async () => {
                    var item = await editor.SaveAsync();
                    if (item == null)
                        return false;

                    CurrentPatient = await _container.Resolve<IPatientRepository>()
                        .GetById(CurrentPatient.Id);
                    PatientInfoAlreadyVisible = false;
                    return true;
                }, () => {
                    PatientInfoAlreadyVisible = false;
                });
                PatientInfoAlreadyVisible = true;
            });
        }
    }
}
