using EyeClinic.Core;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.DataAccess.Migrations;
using EyeClinic.DataAccess.Repositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Dialogs.PasswordInput;
using EyeClinic.WPF.Components.Home.CartoonForm.ItemsEditer;
using EyeClinic.WPF.Components.Home.ReadyItems.ReadyItemsProducting;
using EyeClinic.WPF.Components.Home.ReadyItems.ReadyItemsProducting.AddItemsViewModel;
using EyeClinic.WPF.Components.Home.ReadyItems.WoreHouse;
using EyeClinic.WPF.Components.Home.ReadyItems.WoreHouse.WareHouseEditer;
using EyeClinic.WPF.Components.Home.Setting.Tests.TestEditor;
using EyeClinic.WPF.Components.PatientList.PatientFile.Tests;
using EyeClinic.WPF.Components.Shell.QueueWindow;
using EyeClinic.WPF.DataModel;
using EyeClinic.WPF.Setup;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Unity;
using operation = EyeClinic.Core.Enums.Operation;

namespace EyeClinic.WPF.Components.Home.ReadyItems
{    
    public partial class ReadyItemsViewModel
    {
        public ReadyItemsViewModel()
        {
            if (IsDesignMode) { }
        }
    }
    public partial class ReadyItemsViewModel : BaseViewModel<ReadyItemsView>
    {
        private IUnityContainer _container;
        private readonly IDiseasesRepository _diseasesRepository;
        private readonly ITestsRepository _tests;
        private readonly IDialogService _dialogservices;
        private readonly IReadyProductRepository _readyproductsRepository;
        private readonly INotificationService _NotificationService;
        private readonly IWareHouseReadyMaterialRepository _wareHouseReadyMaterialRepository;
        public ReadyItemsViewModel(IUnityContainer container,
            IDiseasesRepository diseasesRepository,
            ITestsRepository tests,
            IDialogService dialogservices,
            IReadyProductRepository readyproductsRepository,IWareHouseReadyMaterialRepository wareHouseReadyMaterialRepository, INotificationService notificationService)
        {
            _container = container;
            _diseasesRepository = diseasesRepository;
            _dialogservices = dialogservices;
            _tests = tests;
            _readyproductsRepository = readyproductsRepository;
            _NotificationService = notificationService;
            _wareHouseReadyMaterialRepository = wareHouseReadyMaterialRepository;

            AddDiseaseCommand = new RelayCommand(AddDisease);
            EditDiseaseCommand = new RelayCommand(EditDisease);
            DeleteDiseaseCommand = new RelayCommand<TestDto>(DeleteDisease);
            ShowItemsCommand = new RelayCommand<TestDto>(ShowCartoonList);
            SearchCommand = new RelayCommand(Search);
            BackHomeCommand = new RelayCommand<QueueItem>(BackToQueue);
            WoreHouseViewCommand = new RelayCommand(WoreHouseView);

            ShowProductsCounterCommand = new RelayCommand<TestDto>(ShowItemsProducting);
        }


        public override async Task Initialize()
        {
            //int index;
            ReadyProducts = new ObservableCollection<ReadyProductDbo>(await _readyproductsRepository.GetAll());
            DiseasesList = new ObservableCollection<TestDto>(
                  await _tests.GetByIsProduct(true));
            foreach (var item in DiseasesList)
            {
                item.TotalWight = ReadyProducts.Where(x => x.ProductId == item.Id && x.DeletedDate == null).Sum(x => x.TotalValue);
                item.TotalWaste = ReadyProducts.Where(x => x.ProductId == item.Id && x.DeletedDate == null).Sum(x => x.WasteValue);
                item.AddDogmaValue = ReadyProducts.Where(x => x.ProductId == item.Id && x.DeletedDate == null).Sum(x => x.IsIncreaseDogmaValue);
                double totalExportedValue = ReadyProducts.Where(x => x.ProductId == item.Id && x.DeletedDate == null).Sum(x => x.ExportedValue);
                //double totalcreatedvalue = item.UnitValue + Convert.ToDouble(item.FirstTermBalance);

                //item.TotalValue = (totalcreatedvalue + item.AddDogmaValue) - (item.TotalWaste + item.TotalWight + totalExportedValue);
                //BusyExecute(async () => {await _tests.Update(item); });
            }

        }

        public event EventHandler<TestDto> OnSelectedContactChanged;
        private TestDto _selectedContact;

        public TestDto SelectedContact
        {
            get => _selectedContact;
            set
            {                
                _selectedContact = value;
                OnSelectedContactChanged?.Invoke(this, value);                
            }                  
        }

       
       
        public ObservableCollection<TestDto> DiseasesList { get; set; }
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
        public ICommand ShowItemsCommand { set; get; }
        public ICommand ShowCustomerViewCommand { set; get; }
        public ICommand ShowProductsCounterCommand { set; get; }
    
        public ICommand WoreHouseViewCommand { set; get; }
        private void AddDisease()
        {
            BusyExecute(async () => {
                var editor = _container.Resolve<AddItemsViewModel>();
                await editor.Initialize();
                editor.Operation = operation.Add;                
                _container.Resolve<IDialogService>()
                    .ShowEditorDialog(editor.GetView() as AddItemsView, async () => {
                        var item = await editor.Save();
                        if (item == null)
                            return false;

                        DiseasesList.Add(item);
                        return true;
                    });
            });

        }

        private void EditDisease()
        {
            
            if (SelectedDisease == null)
            {  _NotificationService.Warning("الرجاء اختيار المنتج"); return; }

            BusyExecute(async () => {
                var editor = _container.Resolve<AddItemsViewModel>();
                await editor.Initialize();
                editor.Operation = operation.Edit;
                editor.BuildFromModel(SelectedDisease);
                var enablebox = editor.SelectedUnit;
                _container.Resolve<IDialogService>()
                    .ShowEditorDialog(editor.GetView() as AddItemsView, async () => {
                        if (editor.Unit == enablebox)
                        {
                            var item = await editor.Save();
                            if (item == null)
                                return false;

                            DiseasesList.UpdateItem(SelectedDisease, item);
                            return true;
                        }
                        else
                        {
                            _NotificationService.Error("لا يمكن تغيير الوحدة");
                            return false;
                        }
                    });
            });
        }

        private void DeleteDisease(TestDto disease)
        {
            var pwd = _container.Resolve<PasswordInputViewModel>();
            pwd.DisposeOnLogin = false;
            pwd.CustomPassword = "54425";
            pwd.OnSuccessLogin += () =>
            {               
                _dialogservices.DisposeLastDialog();
                BusyExecute(async () => {
                    await _tests.Delete(e=>e.Id==disease.Id);
                    DiseasesList.Remove(disease);
                    await _readyproductsRepository.Delete(e=>e.ProductId == disease.Id);
                    //await _container.Resolve<ReadyItemsViewModel>().(disease);
                });
            };
            _container.Resolve<IDialogService>()
                   .ShowPopupContent(pwd.GetView() as PasswordInputView);

        }

        private void Search()
        {
            try
            {
                BusyExecute(async () =>
                {
                    SearchText ??= "";
                    try
                    {
                        DiseasesList = new ObservableCollection<TestDto>(
                       await _tests.GetByIsProductWithName(true, SearchText));
                    }
                    catch (Exception z)
                    {

                        _NotificationService.Error(z.Message);
                    }
                  

                    if (DiseasesList != null)
                    {
                        foreach (var item in DiseasesList)
                        {
                            item.TotalWight = ReadyProducts.Where(x => x.ProductId == item.Id && x.DeletedDate == null).Sum(x => x.TotalValue);
                            item.TotalWaste = ReadyProducts.Where(x => x.ProductId == item.Id && x.DeletedDate == null).Sum(x => x.WasteValue);
                            item.AddDogmaValue = ReadyProducts.Where(x => x.ProductId == item.Id && x.DeletedDate == null).Sum(x => x.IsIncreaseDogmaValue);
                            double totalExportedValue = ReadyProducts.Where(x => x.ProductId == item.Id && x.DeletedDate == null).Sum(x => x.ExportedValue);
                            double totalcreatedvalue = item.UnitValue + Convert.ToDouble(item.FirstTermBalance);

                            item.TotalValue = (totalcreatedvalue + item.AddDogmaValue) - (item.TotalWaste + item.TotalWight + totalExportedValue);
                            //BusyExecute(async () => {await _tests.Update(item); });
                        }
                    }

                });
            }
            catch (Exception ex)
            {
                _NotificationService.Error(ex.Message);
            }
               
        }
        public void BackToQueue(QueueItem queue)
        {
            BusyExecute(async () => await _container.BackHome());
        }
        private void ShowCartoonList(TestDto dto)
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
                var WareHouse = _container.Resolve<WoreHouseViewModel>();
                await WareHouse.Initialize();
                _container.Navigate(WareHouse.GetView() as WoreHouseView);
            });
        }
        public void ShowItemsProducting(TestDto testDto) 
        {
            if (SelectedDisease == null)
                return;
            BusyExecute(async () =>
            {
                var cartoonItemsView = _container.Resolve<ReadyItemsProductingViewModel>();
                await cartoonItemsView.Initialize(testDto);
                _container.Navigate(cartoonItemsView.GetView() as ReadyItemsProductingView);
            });
        } 
        
        public void GetMaterialValue()
        {
            BusyExecute(async () => {
                SelectedMaterial = await _wareHouseReadyMaterialRepository.GetByTestId(SelectedDisease);
            });
        }

    }
}
