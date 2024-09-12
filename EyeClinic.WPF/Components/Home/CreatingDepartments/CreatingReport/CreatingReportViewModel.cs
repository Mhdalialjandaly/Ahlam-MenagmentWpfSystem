using EyeClinic.Model;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.DataModel;
using System.Threading.Tasks;
using System;
using EyeClinic.DataAccess.IRepositories;
using Unity;
using EyeClinic.WPF.Setup;
using EyeClinic.Core.Enums;
using System.Linq;
using System.Windows.Input;
using EyeClinic.WPF.AppServices.DialogService;

namespace EyeClinic.WPF.Components.Home.CreatingDepartments.CreatingReport
{
   public partial class CreatingReportViewModel
    {
        public CreatingReportViewModel() { }
    }
    public partial class CreatingReportViewModel : BaseViewModel<CreatingReportView> 
    {
        private readonly INotificationService _notificationService;
        private readonly IOldMedicineRepository _oldMedicineRepository; 
        private readonly IUnityContainer _container;
        private readonly IDialogService _dialogService;
        public CreatingReportViewModel(INotificationService notificationService,
            IUnityContainer unityContainer,
            IOldMedicineRepository oldMedicineRepository,
            IDialogService diagnosisRepository)
        {
            _notificationService = notificationService;
            _container = unityContainer;
            _oldMedicineRepository = oldMedicineRepository;
            _dialogService = diagnosisRepository;

            BackHomeCommand = new RelayCommand(BackHome);
            BackCommand = new RelayCommand(Back);
        }
        public  async Task Initialize(string department)
        {
          var orders = await  _oldMedicineRepository.GetByKey(e => e.PatientVisit.VisitStatus == (int)PatientVisitStatus.Started);
            OrderItemsCount =  orders.Where(e => e.MedicineType == department).Count();
        }
        public int OrderItemsCount { get; set; }
        public event EventHandler<MedicineDto> OnSelectedProductChanged;
        private MedicineDto _selectedProduct;

        public MedicineDto SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnSelectedProductChanged?.Invoke(this, value);
            }
        }
        public ICommand BackHomeCommand { get; set; }
        public ICommand BackCommand { get; set; }

        private void BackHome()
        {
            BusyExecute(async () => await _container.BackHome());
        }
        public void Back()
        {
            _dialogService.DisposeLastDialog();
        }

    }
}
