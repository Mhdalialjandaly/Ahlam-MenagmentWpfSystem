using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using EyeClinic.Core;
using EyeClinic.Core.Enums;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Base.Interfaces;

namespace EyeClinic.WPF.AppServices.DialogService.Wizard
{
    public class WizardViewModel : BaseViewModel<WizardView>, IWizardViewModel
    {
        private readonly INotificationService _notificationService;

        public WizardViewModel(INotificationService notificationService) {
            _notificationService = notificationService;
        }
        public override async Task Initialize() {
            await base.Initialize();

            if (Steps.IsNullOrEmpty())
                return;

            Steps.ForEach(e => e.Operation = Operation.Add);

            Step = Steps[0];
            await Step.Initialize();
        }


        public event Action OnSave;
        public event Action OnCancel;

        private IPatientWizardStep _step;
        public IPatientWizardStep Step {
            get => _step;
            set {
                _step = value;
                if (value != null)
                    SetContent();
            }
        }

        public UserControl Content { get; set; }
        public List<IPatientWizardStep> Steps { get; set; }


        public ICommand PreviousCommand => new RelayCommand(Previous);
        public ICommand NextCommand => new RelayCommand(Next);
        public ICommand SaveCommand => new RelayCommand(Save);
        public ICommand CancelCommand => new RelayCommand(Cancel);


        #region Methods

        private void SetContent() {
            Content = (UserControl)Step.GetView();
        }

        private void Save() {
            if (!Validate())
                return;

            OnSave?.Invoke();
        }

        private void Cancel() {
            OnCancel?.Invoke();
        }

        private bool Validate() {
            var errors = new List<string>();
            foreach (var step in Steps) {
                step.Validate(ref errors);
            }

            if (errors.Any()) {
                foreach (var error in errors) {
                    _notificationService.Error(error);
                }

                return false;
            }

            return true;
        }

        public void AppendData<T>(ref T model) {
            foreach (var step in Steps) {
                step.AppendData(ref model);
            }
        }

        public async void Next() {

            var errors = new List<string>();
            Step.Validate(ref errors);

            if (errors.Any()) {
                foreach (var error in errors) {
                    _notificationService.Error(error);
                }
                return;
            }

            if (Steps.Count == Step.Index)
                return;

            var index = Step.Index + 1;
            var step = Steps.Find(e => e.Index == index);
            if (step != null) {
                await step.Initialize();
                Step = step;
            }
        }

        public async void Previous() {
            if (Step.Index == 1)
                return;

            var index = Step.Index - 1;
            var step = Steps.Find(e => e.Index == index);
            if (step != null) {
                await step.Initialize();
                Step = step;
            }
        }

        #endregion
    }
}
