using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Base.Extends;
using Unity;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.SpecialNote
{
    public partial class SpecialNoteViewModel
    {
        public SpecialNoteViewModel() 
        {

        }
    }

    public partial class SpecialNoteViewModel : BaseViewModel<SpecialNoteView>
    {

        private readonly IPatientRepository _patientRepository;
        private readonly IPatientOperationRepository _patientOperationRepository;

        public SpecialNoteViewModel(IPatientRepository patientRepository,
                IPatientOperationRepository patientOperationRepository) {
            _patientRepository = patientRepository;
            _patientOperationRepository = patientOperationRepository;

            SaveCommand = new RelayCommand(Save);
        }

        public void Initialize(int patientId, string specialNote) {
            PatientId = patientId;
            SpecialNote = specialNote;

            //BusyExecute(async () => {
            //    Remaining = (await _patientOperationRepository
            //        .GetRemainingByPatientId(patientId)).ToString();
            //});
        }

        public event EventHandler<string> OnSave;

        public string SpecialNote { get; set; }
        public string Remaining { get; set; }
        public int PatientId { get; set; }



        public ICommand SaveCommand { get; set; }

        public ICommand CancelCommand => new RelayCommand(() => {
            ContainerHandler.Container.Resolve<IDialogService>().DisposeLastDialog();
        });

        public void Save() {
            BusyExecute(async () => {
                await _patientRepository.UpdateNote(PatientId, SpecialNote);
                OnSave?.Invoke(this, SpecialNote);
            });
        }
    }
}
