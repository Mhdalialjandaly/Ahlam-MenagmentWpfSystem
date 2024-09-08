using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.Print;
using EyeClinic.WPF.Base;
using Unity;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.NewGlass
{
    public class PatientGlassViewModel : BaseViewModel<NewGlassView>
    {
        private readonly IUnityContainer _container;
        private readonly IPatientGlassRepository _patientGlassRepository;
        private readonly IPrintService _printService;
        private readonly IDialogService _dialogService;

        public PatientGlassViewModel(IUnityContainer container, IPatientGlassRepository patientGlassRepository,
                IPrintService printService, IDialogService dialogService) {
            _container = container;
            _patientGlassRepository = patientGlassRepository;
            _printService = printService;
            _dialogService = dialogService;

            SaveNewGlassCommand = new RelayCommand(SaveNewGlass);
            PrintNewGlassCommand = new RelayCommand(PrintNewGlass);
            SavePrintNewGlassCommand = new RelayCommand(SavePrintNewGlass);
            CancelNewGlassCommand = new RelayCommand(CancelNewGlass);
            DeleteNewGlassCommand = new RelayCommand(DeleteNewGlass);
        }

        public override Task Initialize() {
            throw new Exception("Use Initialize with param");
        }

        public async Task Initialize(int patientId) {
            PatientId = patientId;

            if (patientId == 0)
                return;

            var patientGlass = await _patientGlassRepository
                .GetByKey(e => e.PatientId == patientId);

            FillProperties(patientGlass.FirstOrDefault());
        }

        public event EventHandler<PatientGlassDto> OnSavePatientGlass;
        public bool IsPatientVisitGlass => false;

        #region Properties

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

        private void FillProperties(PatientGlassDto visitGlass) {
            if (visitGlass == null)
                return;

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
        }

        private PatientGlassDto BuildFromProperties() {
            return new() {
                PatientId = PatientId,
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

        public bool HasData() {
            var data = RSph + LSph + RSph2 + LSph2 + RCyl + LCyl +
            RCyl2 + LCyl2 + RAxis + LAxis + RAxis2 + LAxis2 + RPrism +
            LPrism + RPrism2 + LPrism2 + RBase + LBase + RBase2 + LBase2 + RIpd +
            LIpd + RIpd2 + LIpd2 + RVa + LVa + RVa2 + LVa2 + Notes;

            return !string.IsNullOrWhiteSpace(data);
        }
        private void SavePrintNewGlass() {
            PrintAfterSave = true;
            SaveNewGlass();
        }

        private void SaveNewGlass() {
            if (PatientId == 0) {
                OnSavePatientGlass?.Invoke(this, BuildFromProperties());
                return;
            }

            BusyExecute(async () => {
                if (ModuleIsEmpty())
                    return;

                await _patientGlassRepository.AddOrUpdate(BuildFromProperties());
                OnSavePatientGlass?.Invoke(this, BuildFromProperties());

                if (PrintAfterSave) {
                    IsBusy = false;
                    PrintNewGlass();

                    _dialogService.DisposeLastDialog();
                }
            });
        }

        private void PrintNewGlass() {
            BusyExecute(async () => {
                var patient = await _container.Resolve<IPatientRepository>()
                    .GetById(PatientId);

                _printService.PrintGlass(patient, BuildFromProperties());
            });
        }

        private void CancelNewGlass() => _dialogService.DisposeLastDialog();

        public Action OnDelete;
        private void DeleteNewGlass() {
            BusyExecute(async () => {
                await _patientGlassRepository.Delete(e => e.PatientId == PatientId);
                OnDelete?.Invoke();

                _dialogService.DisposeLastDialog();
                OnSavePatientGlass?.Invoke(this, null);
            });
        }

        private bool ModuleIsEmpty() {
            return
            string.IsNullOrWhiteSpace(RSph) &&
                string.IsNullOrWhiteSpace(LSph) &&
                string.IsNullOrWhiteSpace(RSph2) &&
                string.IsNullOrWhiteSpace(LSph2) &&
                string.IsNullOrWhiteSpace(RCyl) &&
                string.IsNullOrWhiteSpace(LCyl) &&
                string.IsNullOrWhiteSpace(RCyl2) &&
                string.IsNullOrWhiteSpace(LCyl2) &&
                string.IsNullOrWhiteSpace(RAxis) &&
                string.IsNullOrWhiteSpace(LAxis) &&
                string.IsNullOrWhiteSpace(RAxis2) &&
                string.IsNullOrWhiteSpace(LAxis2) &&
                string.IsNullOrWhiteSpace(RPrism) &&
                string.IsNullOrWhiteSpace(LPrism) &&
                string.IsNullOrWhiteSpace(RPrism2) &&
                string.IsNullOrWhiteSpace(LPrism2) &&
                string.IsNullOrWhiteSpace(RBase) &&
                string.IsNullOrWhiteSpace(LBase) &&
                string.IsNullOrWhiteSpace(RBase2) &&
                string.IsNullOrWhiteSpace(LBase2) &&
                string.IsNullOrWhiteSpace(RIpd) &&
                string.IsNullOrWhiteSpace(LIpd) &&
                string.IsNullOrWhiteSpace(RIpd2) &&
                string.IsNullOrWhiteSpace(LIpd2) &&
                string.IsNullOrWhiteSpace(RVa) &&
                string.IsNullOrWhiteSpace(LVa) &&
                string.IsNullOrWhiteSpace(RVa2) &&
                string.IsNullOrWhiteSpace(LVa2) &&
                string.IsNullOrWhiteSpace(Notes);
        }
    }
}
