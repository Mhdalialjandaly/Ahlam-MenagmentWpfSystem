using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.Print;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Base.Extends;
using EyeClinic.WPF.Components.PatientList.PatientFile.NewGlass.NewGlassHistory;
using Microsoft.EntityFrameworkCore;
using Unity;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.NewGlass
{
    public partial class NewGlassViewModel
    {
        public NewGlassViewModel() {
            if (IsDesignMode) { }
        }
    }

    public partial class NewGlassViewModel : BaseViewModel<NewGlassView>
    {
        private readonly IUnityContainer _container;
        private readonly IDialogService _dialogService;
        private readonly IPatientVisitRepository _patientVisitRepository;
        private readonly IPatientVisitGlassRepository _patientVisitGlassRepository;
        private readonly IPrintService _printService;

        public NewGlassViewModel(IUnityContainer container,
            IDialogService dialogService, IPatientVisitRepository patientVisitRepository,
            IPatientVisitGlassRepository patientVisitGlassRepository, IPrintService printService) {
            _container = container;
            _dialogService = dialogService;
            _patientVisitRepository = patientVisitRepository;
            _patientVisitGlassRepository = patientVisitGlassRepository;
            _printService = printService;

            SaveNewGlassCommand = new RelayCommand(SaveNewGlass);
            PrintNewGlassCommand = new RelayCommand(PrintNewGlass);
            SavePrintNewGlassCommand = new RelayCommand(SavePrintNewGlass);
            CancelNewGlassCommand = new RelayCommand(CancelNewGlass);
            DeleteNewGlassCommand = new RelayCommand(DeleteNewGlass);
            PatientVisitGlassHistoryCommand = new RelayCommand(PatientVisitGlassHistory);
            CurrentGlassCommand = new RelayCommand(GetPatientGlass);
        }

        public void GetPatientGlass() {
            BusyExecute(async () => {
                var glassViewModel = _container.Resolve<PatientGlassViewModel>();
                await glassViewModel.Initialize(PatientId);

                _dialogService.ShowEditorDialog(glassViewModel.GetView() as NewGlassView, () => {
                    glassViewModel.SaveNewGlassCommand.Execute(null);
                    HasCurrentGlass = glassViewModel.HasData();
                    return Task.FromResult(true);
                });
                glassViewModel.OnDelete += () => {
                    HasCurrentGlass = glassViewModel.HasData();
                };
            });
        }

        public override Task Initialize() {
            throw new Exception("Use Initialize with param");
        }

        public async Task Initialize(int patientVisitId) {
            var visitGlass = await _patientVisitGlassRepository
                .GetByKey(e => e.PatientVisitId == patientVisitId);

            PatientVisitId = patientVisitId;
            FillProperties(visitGlass.FirstOrDefault());

            VisitDate = await _patientVisitGlassRepository
                .GetVisitDateById(PatientVisitId);

            var item = await _patientVisitRepository.Get().AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == patientVisitId);

            if (item != null)
                PatientId = item.PatientId;

            var glassViewModel = _container.Resolve<PatientGlassViewModel>();
            await glassViewModel.Initialize(PatientId);
            HasCurrentGlass = glassViewModel.HasData();
        }

        public bool HasCurrentGlass { get; set; }

        public async Task<DateTime?> InitializeByPatientId(int patientId) {
            var visitGlass = await _patientVisitGlassRepository
                .GetLastGlassInfo(patientId);
            if (visitGlass == null)
                return null;

            PatientVisitId = visitGlass.PatientVisitId;
            FillProperties(visitGlass);
            return visitGlass.PatientVisit.VisitDate;
        }

        public DateTime? VisitDate { get; set; }
        public bool IsPatientVisitGlass => true;

        #region Properties

        public int PatientVisitId { get; set; }
        public int PatientId { get; set; }

        public string RSph { get; set; }
        public string LSph { get; set; }
        public string RSph2 { get; set; }
        public string LSph2 { get; set; }
        public string RCyl { get; set; }
        public string LCyl { get; set; }
        public string RCyl2 { get; set; }
        public string LCyl2 { get; set; }
        public string RAxis { get; set; }
        public string LAxis { get; set; }
        public string RAxis2 { get; set; }
        public string LAxis2 { get; set; }
        public string RPrism { get; set; }
        public string LPrism { get; set; }
        public string RPrism2 { get; set; }
        public string LPrism2 { get; set; }
        public string RBase { get; set; }
        public string LBase { get; set; }
        public string RBase2 { get; set; }
        public string LBase2 { get; set; }
        public string RIpd { get; set; }
        public string LIpd { get; set; }
        public string RIpd2 { get; set; }
        public string LIpd2 { get; set; }
        public string RVa { get; set; }
        public string LVa { get; set; }
        public string RVa2 { get; set; }
        public string LVa2 { get; set; }
        public bool AddVision { get; set; }
        public bool ContactLenses { get; set; }
        public string Notes { get; set; }

        public bool PrintAfterSave { get; set; }

        #endregion


        public ICommand SaveNewGlassCommand { get; set; }
        public ICommand SavePrintNewGlassCommand { get; set; }
        public ICommand PrintNewGlassCommand { get; set; }
        public ICommand CancelNewGlassCommand { get; set; }
        public ICommand DeleteNewGlassCommand { get; set; }
        public ICommand PatientVisitGlassHistoryCommand { get; set; }
        public ICommand CurrentGlassCommand { get; set; }

        public ICommand CancelCommand => new RelayCommand(() => {
            ContainerHandler.Container.Resolve<IDialogService>().DisposeLastDialog();
        });
        public PatientFileViewModel PatientFileViewModel { get; set; }

        private void FillProperties(PatientVisitGlassDto visitGlass) {
            BusyExecute(() =>
            {
                if (visitGlass == null)
                    return Task.CompletedTask;
                RSph = visitGlass.RSph;
                LSph = visitGlass.LSph;
                RSph2 = visitGlass.RSph2;
                LSph2 = visitGlass.LSph2;
                RCyl = visitGlass.RCyl;
                LCyl = visitGlass.LCyl;
                RCyl2 = visitGlass.RCyl2;
                LCyl2 = visitGlass.LCyl2;
                RAxis = visitGlass.RAxis;
                LAxis = visitGlass.LAxis;
                RAxis2 = visitGlass.RAxis2;
                LAxis2 = visitGlass.LAxis2;
                RPrism = visitGlass.RPrism;
                LPrism = visitGlass.LPrism;
                RPrism2 = visitGlass.RPrism2;
                LPrism2 = visitGlass.LPrism2;
                RBase = visitGlass.RBase;
                LBase = visitGlass.LBase;
                RBase2 = visitGlass.RBase2;
                LBase2 = visitGlass.LBase2;
                RIpd = visitGlass.RIpd;
                LIpd = visitGlass.LIpd;
                RIpd2 = visitGlass.RIpd2;
                LIpd2 = visitGlass.LIpd2;
                RVa = visitGlass.RVa;
                LVa = visitGlass.LVa;
                RVa2 = visitGlass.RVa2;
                LVa2 = visitGlass.LVa2;
                AddVision = visitGlass.AddVision;
                ContactLenses = visitGlass.ContactLenses;
                Notes = visitGlass.Notes;
                return Task.CompletedTask;
            });
        }

        private PatientVisitGlassDto BuildFromProperties() {
            return new() {
                PatientVisitId = PatientVisitId,
                AddVision = AddVision,
                LAxis = LAxis,
                LAxis2 = LAxis2,
                LBase = LBase,
                LBase2 = LBase2,
                LCyl = LCyl,
                LCyl2 = LCyl2,
                LIpd = LIpd,
                LIpd2 = LIpd2,
                LPrism = LPrism,
                LPrism2 = LPrism2,
                LSph = LSph,
                LSph2 = LSph2,
                LVa = LVa,
                LVa2 = LVa2,
                RAxis = RAxis,
                RAxis2 = RAxis2,
                RBase = RBase,
                RBase2 = RBase2,
                RCyl = RCyl,
                RCyl2 = RCyl2,
                RIpd = RIpd,
                RIpd2 = RIpd2,
                RPrism = RPrism,
                RPrism2 = RPrism2,
                RSph = RSph,
                RSph2 = RSph2,
                RVa = RVa,
                RVa2 = RVa2,
                ContactLenses = ContactLenses,
                Notes = Notes
            };
        }

        private void SavePrintNewGlass() {
            PrintAfterSave = true;
            SaveNewGlass();
        }


        public Action OnSave;
        private void SaveNewGlass() {
            BusyExecute(async () => {
                await _patientVisitGlassRepository.AddOrUpdate(BuildFromProperties());
                OnSave?.Invoke();

                if (PrintAfterSave) {
                    IsBusy = false;
                    PrintNewGlass();
                }
            });
        }

        private void PrintNewGlass() {
            BusyExecute(async () => {
                var patient = await _container.Resolve<IPatientRepository>()
                    .GetByPatientVisitId(PatientVisitId);

                _printService.PrintGlass(patient, BuildFromProperties());
                PrintAfterSave = false;
            });
        }

        private void CancelNewGlass() {
            BusyExecute(async () => {
                await Initialize(PatientVisitId);
                OnSave?.Invoke();
            });
        }

        private void DeleteNewGlass() {
            BusyExecute(async () => {
                await _patientVisitGlassRepository.Delete(e => e.PatientVisitId == PatientVisitId);
                await Initialize(PatientVisitId);
                OnSave?.Invoke();
            });
        }

        private void PatientVisitGlassHistory() {
            BusyExecute(async () => {
                var history = _container.Resolve<NewGlassHistoryViewModel>();
                await history.Initialize(PatientId);

                _dialogService.ShowInformationDialog(history.GetView() as NewGlassHistoryView);
            });
        }
    }
}
