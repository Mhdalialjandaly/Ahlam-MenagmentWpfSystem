using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Dialogs.PasswordInput;
using EyeClinic.WPF.Components.Home.ReadyItems.ReadyItemsProducting.AddItemsViewModel;
using EyeClinic.WPF.Components.Home.ReadyItems.ReadyItemsProducting;
using EyeClinic.WPF.Components.Home.ReadyItems.WoreHouse.WareHouseEditer;
using EyeClinic.WPF.Components.PatientList.PatientFile.Tests;
using EyeClinic.WPF.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EyeClinic.DataAccess.Repositories;
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel;
using Unity;
using Operation = EyeClinic.Core.Enums.Operation;
using EyeClinic.WPF.Setup;
using EyeClinic.Core;
using Syncfusion.UI.Xaml.Utility;
using EyeClinic.WPF.Components.Home.Clinic;
namespace EyeClinic.WPF.Components.Home.ReadyItems.WoreHouse
{
    public partial class WoreHouseViewModel 
    {
        public WoreHouseViewModel() { }
    }
    public partial class WoreHouseViewModel:BaseViewModel<WoreHouseView>
    {
        private IUnityContainer _container;
        private readonly IDiseasesRepository _diseasesRepository;
        private readonly ITestsRepository _tests;
        private readonly IDialogService _dialogservices;
        private readonly IReadyProductRepository _readyproductsRepository;
        private readonly INotificationService _NotificationService;
        private readonly IWareHouseReadyMaterialRepository _wareHouseReadyMaterialRepository;
        private readonly INotificationService _notificationService;
        public WoreHouseViewModel(IUnityContainer container,
            IDiseasesRepository diseasesRepository,
            ITestsRepository tests,
            IDialogService dialogservices,
            IReadyProductRepository readyproductsRepository, 
            IWareHouseReadyMaterialRepository wareHouseReadyMaterialRepository,
            INotificationService notificationService)
        {
            _notificationService = notificationService;
            _container = container;
            _diseasesRepository = diseasesRepository;
            _dialogservices = dialogservices;
            _tests = tests;
            _readyproductsRepository = readyproductsRepository;
            _NotificationService = notificationService;
            _wareHouseReadyMaterialRepository = wareHouseReadyMaterialRepository;

            AddDiseaseCommand = new RelayCommand(AddDisease);
            EditDiseaseCommand = new RelayCommand(EditDisease);
            DeleteDiseaseCommand = new RelayCommand<ReadyProductDbo>(DeleteDisease);
            ShowItemsCommand = new RelayCommand<ReadyProductDbo>(ShowCartoonList);
            SearchCommand = new RelayCommand(Search);
            BackHomeCommand = new RelayCommand<QueueItem>(BackToQueue);
            WoreHouseViewCommand = new RelayCommand(WoreHouseView);
            BackCommand = new RelayCommand(Back);
            ShowProductsCounterCommand = new RelayCommand<ReadyProductDbo>(ShowItemsProducting);
            SendToWareHouseCommand = new RelayCommand<ReadyProductDbo>(SendToWareHouse);

        }

      
        public override async Task Initialize()
        {
            ReadyProducts = new ObservableCollection<ReadyProductDbo>(await _readyproductsRepository.GetReadyProductWareHouse());
        }

        public event EventHandler<ReadyProductDbo> OnSelectedContactChanged;
        private ReadyProductDbo _selectedContact;

        public ReadyProductDbo SelectedContact
        {
            get => _selectedContact;
            set
            {
                _selectedContact = value;
                OnSelectedContactChanged?.Invoke(this, value);
            }
        }


      
        public TestDto SelectedDisease { get; set; }
        public ObservableCollection<ReadyProductDbo> ReadyProducts { set; get; }
        public WareHouseReadyMaterialDto SelectedMaterial { get; set; }
        public string SearchText { get; set; }
        public double ProductedValue { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand AddDiseaseCommand { get; set; }
        public ICommand EditDiseaseCommand { get; set; }
        public ICommand DeleteDiseaseCommand { get; set; }
        public ICommand BackHomeCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand ShowItemsCommand { set; get; }
        public ICommand ShowCustomerViewCommand { set; get; }
        public ICommand ShowProductsCounterCommand { set; get; }
        public ICommand SendToWareHouseCommand { get; set; }
        public ICommand WoreHouseViewCommand { set; get; }
        private void AddDisease()
        {
            //BusyExecute(async () => {
            //    var editor = _container.Resolve<AddItemsViewModel>();
            //    await editor.Initialize();
            //    editor.Operation = Operation.Add;

            //    _container.Resolve<IDialogService>()
            //        .ShowEditorDialog(editor.GetView() as AddItemsView, async () => {
            //            var item = await editor.Save();
            //            if (item == null)
            //                return false;

            //            DiseasesList.Add(item);
            //            return true;
            //        });
            //});

        }

        private void EditDisease()
        {
            //if (!_container.CheckUserRole(UserRoles.Admin, UserRoles.Administrative, UserRoles.Designer))
            //    return;
            //if (SelectedDisease == null)
            //{ _NotificationService.Warning("الرجاء اختيار المنتج"); return; }



            //BusyExecute(async () => {
            //    var editor = _container.Resolve<AddItemsViewModel>();
            //    await editor.Initialize();
            //    editor.Operation = Operation.Edit;
            //    editor.BuildFromModel(SelectedDisease);

            //    _container.Resolve<IDialogService>()
            //        .ShowEditorDialog(editor.GetView() as AddItemsView, async () => {
            //            var item = await editor.Save();
            //            if (item == null)
            //                return false;

            //            DiseasesList.UpdateItem(SelectedDisease, item);
            //            return true;
            //        });
            //});
        }

        private void DeleteDisease(ReadyProductDbo readyProductDbo)
        {
            //var pwd = _container.Resolve<PasswordInputViewModel>();
            //pwd.DisposeOnLogin = false;
            //pwd.CustomPassword = "54425";
            //pwd.OnSuccessLogin += () =>
            //{
            //    _dialogservices.DisposeLastDialog();
            //    BusyExecute(async () => {
            //        await _tests.Delete(e => e.Id == disease.Id);
            //        DiseasesList.Remove(disease);
            //        //await _container.Resolve<ReadyItemsViewModel>().(disease);
            //    });
            //};
            //_container.Resolve<IDialogService>()
            //       .ShowPopupContent(pwd.GetView() as PasswordInputView);

        }

        private void Search()
        {
            BusyExecute(async () => {
                ReadyProducts = new ObservableCollection<ReadyProductDbo>( await _readyproductsRepository.GetReadyProductWareHouseByName(SearchText));        
            });
            
        }
        private  void Back()
        {
            _container.SetCurrentPatient(null);
            if (_dialogservices.IsPopupActivated())
            {
                _dialogservices.DisposeLastDialog();
                return;
            }

            BusyExecute(async () => {
                var clinic = _container.Resolve<ReadyItemsViewModel>();
                await clinic.Initialize();
                _container.Navigate(clinic.GetView() as ReadyItemsView);
            });
        }

        public void BackToQueue(QueueItem queue)
        {
            BusyExecute(async () => await _container.BackHome());
        }
        private void ShowCartoonList(ReadyProductDbo readyProductDbo)
        {
            if (SelectedDisease == null)
                return;
            BusyExecute(async () =>
            {
                var cartoonItemsView = _container.Resolve<TestsViewModel>();
                await cartoonItemsView.Initialize(SelectedDisease.Id);
                _container.Navigate(cartoonItemsView.GetView() as TestsView);
            });
        }
        public int ConvertToInt(string st)
        {
            int Result = Convert.ToInt32(st);
            return Result;
        }
        private void WoreHouseView()
        {
            BusyExecute(async () =>
            {
                //    var cartoonItemsView = _container.Resolve<ReadyItemsProductingViewModel>();
                //    await cartoonItemsView.Initialize(testDto);
                //    _container.Navigate(cartoonItemsView.GetView() as ReadyItemsProductingView);
            });
        }
        public void ShowItemsProducting(ReadyProductDbo readyProductDbo)
        {
            //if (SelectedDisease == null)
            //    return;
            //BusyExecute(async () =>
            //{
            //    var cartoonItemsView = _container.Resolve<ReadyItemsProductingViewModel>();
            //    await cartoonItemsView.Initialize(testDto);
            //    _container.Navigate(cartoonItemsView.GetView() as ReadyItemsProductingView);
            //});
        }
        private void SendToWareHouse(ReadyProductDbo readyProductDbo)
        {
            //    if (!_container.CheckUserRole(UserRoles.Admin, UserRoles.Administrative, UserRoles.Designer))
            //        return;
            //    if (SelectedDisease == null)
            //    {
            //        _NotificationService.Warning("الرجاء اختيار المنتج");
            //        return;
            //    }

            //BusyExecute(async () =>
            //{
            //    if (SelectedMaterial != null)
            //    {
            //        var editor = _container.Resolve<WareHouseEditerViewModel>();
            //        await editor.Initialize();
            //        editor.Operation = Operation.Edit;
            //        editor.BuildFromModel(SelectedDisease);

            //        _container.Resolve<IDialogService>()
            //            .ShowEditorDialog(editor.GetView() as WareHouseEditerView, async () =>
            //            {
            //                var item = await editor.Save();
            //                if (item == null)
            //                    return false;

            //                return true;
            //            });
            //    }
            //    else
            //    {
            //        var editor = _container.Resolve<WareHouseEditerViewModel>();
            //        await editor.Initialize();
            //        editor.Operation = Operation.Edit;
            //        editor.BuildFromTestModel(SelectedDisease);

            //        _container.Resolve<IDialogService>()
            //            .ShowEditorDialog(editor.GetView() as WareHouseEditerView, async () =>
            //            {
            //                var item = await editor.Save();
            //                if (item == null)
            //                    return false;

            //                return true;
            //            });
            //    }

            //});
        
        }

    }
}

