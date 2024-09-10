using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Dialogs.PasswordInput;
using EyeClinic.WPF.Components.Home.CartoonForm.ItemsEditer;
using EyeClinic.WPF.Components.Home.CartoonForm;
using EyeClinic.WPF.Components.Home.CartoonForm.ItemsEditer.ItemsEditerDialog;
using EyeClinic.WPF.Components.Home.ReadyItems.ReadyItemsProducting.ReadyItemsProductingEditer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Unity;
using operation = EyeClinic.Core.Enums.Operation;
using EyeClinic.Core;
using EyeClinic.WPF.Setup;
using Syncfusion.Data.Extensions;
using EyeClinic.WPF.Components.Home.ReadyItems.ReadyItemsProducting.AddDogmaEditer;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.DataAccess.Entities;





namespace EyeClinic.WPF.Components.Home.ReadyItems.ReadyItemsProducting
{
    public partial class ReadyItemsProductingViewModel
    {
        public ReadyItemsProductingViewModel()
        {
            if (IsDesignMode) { }
        }
    }
    public partial class ReadyItemsProductingViewModel : BaseViewModel<ReadyItemsProductingView>
    {
        private readonly IDialogService _dialogService;
        private readonly IUnityContainer _container;
        private readonly IReadyProductRepository _cartoonItemRepository;
        private readonly INotificationService _notificationService;
        private readonly IReadyProductRepository _readyProductRepository;
        public ReadyItemsProductingViewModel(IUnityContainer unityContainer, IReadyProductRepository readyProductRepository, INotificationService notificationService,
            IReadyProductRepository cartoonItemRepository, IDialogService dialogService)
        {
            _notificationService = notificationService;
            _container = unityContainer;

            _cartoonItemRepository = cartoonItemRepository;
            _dialogService = dialogService;
            _readyProductRepository = readyProductRepository;

            BackHomeCommand = new RelayCommand(BackHome);
            GoBackCommand = new RelayCommand(GoBack);

            AddCartoonItemCommand = new RelayCommand(AddItems);
            AddCartoonItemCommand2 = new RelayCommand(AddItems2);
            EditCartoonItemCommand = new RelayCommand(EditItems);
            DeleteCartoonItemCommand = new RelayCommand<ReadyProductDbo>(DeleteItems);
            ShowItemsCommand = new RelayCommand<ReadyProductDbo>(ShowCartoonList);
            SearchCommand = new RelayCommand(Search);
            SortByCommand = new RelayCommand(SortBy);

            SortItems = new List<string>{ "حسب الاسم", "حسب التاريخ", "حسب الرقم الخاص", "حسب الزيادة","افتراضي"};
        }

      

        public async Task Initialize(Model.TestDto testDto)
        {
            CartoonItemDataList = new ObservableCollection<ReadyProductDbo>(
                await _cartoonItemRepository.GetReadyProductAsyncById(testDto));
            //CartoonItemDataList2 = new ObservableCollection<ReadyProduct>(
            //   await _cartoonItemRepository.GetReadyProductAsyncById2(testDto));
            SelectedTest = testDto;
            GetTheValues(testDto);

            var LastId = CartoonItemDataList.Select(e => e.Id).LastOrDefault();
            foreach (var item in CartoonItemDataList)
            {
                item.ProductedValueUnit = testDto.Unit;
                item.increasevalue = Math.Abs(item.IsIncreaseDogmaValue - item.TotalResult);
                
                if (string.IsNullOrEmpty(item.Note))
                {
                    item.Note = "لا ملاحظة";
                }
                if (LastId == item.Id)
                {
                    item.IsLastValue = true;
                }
                else
                {
                    item.IsLastValue = false;
                }
            }

        }
        public List<string> SortItems { get; set; }
        public ObservableCollection<ReadyProductDbo> CartoonItemDataList { get; set; }
        public ObservableCollection<ReadyProduct> CartoonItemDataList2 { get; set; }
        public ReadyProductDbo SelectedCartoonItem { get; set; }
        public bool IsQuintity { get; set; }
        public string SearchText { get; set; }
        public TestDto SelectedTest { get; set; }
        public double Quintity { get; set; }
        public string SelectedItem { get; set; }
        public string ProductName => SelectedItem;
        public double TotalWight { get; set; }
        public double TotalWaste { get; set; }
        public double TotalExported { get; set; }
        public double TotalValue { get; set; }
        public double UnitValue { get; set; }
        public double res { get; set; }
        public double AddDogmaValue { get; set; }
        public double AddValue { get; set; }
        public string SelectedSortName { get; set; }

        public ICommand SortByCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand AddCartoonItemCommand { get; set; }
        public ICommand AddCartoonItemCommand2 { get; set; }
        public ICommand EditCartoonItemCommand { get; set; }
        public ICommand DeleteCartoonItemCommand { get; set; }
        public ICommand BackHomeCommand { get; set; }
        public ICommand GoBackCommand { get; set; }
        public ICommand ShowItemsCommand { set; get; }

        public event Action OnSave;


        private void AddItems()
        {
            BusyExecute(async () => {
                var editor = _container.Resolve<ReadyItemsProductingEditerViewModel>();
                await editor.Initialize();
                editor.Operation = operation.Add;
                editor.res = Quintity;               
                if (string.IsNullOrEmpty(editor.SelectedCreatedValueUnit))
                {
                    editor.SelectedCreatedValueUnit = SelectedTest.Unit;
                }
                if (string.IsNullOrEmpty(editor.SelectedWasteValueUnit))
                {
                    editor.SelectedWasteValueUnit = SelectedTest.Unit;
                }

                _container.Resolve<IDialogService>()
                    .ShowEditorDialog(editor.GetView() as ReadyItemsProductingEditerView, async () => {
                        var item = await editor.Save(SelectedTest, editor.res);
                        OnSave?.Invoke();
                        OnPropertyChanged();
                        if (item == null)
                            return false;
                        CartoonItemDataList.Add(item);
                        await Initialize(SelectedTest);
                        return true;
                    });

            });
        }
        private void AddItems2()
        {
            BusyExecute(async () => {
                var editor = _container.Resolve<AddDogmaEditerViewModel>();
                await editor.Initialize();
                editor.Operation = operation.Add;
                editor.SelectedCreatedValueUnit = SelectedTest.Unit;
                editor.res = Quintity;
                if (string.IsNullOrEmpty(editor.SelectedCreatedValueUnit))
                {
                    editor.SelectedCreatedValueUnit = SelectedTest.Unit;
                }
                if (string.IsNullOrEmpty(editor.SelectedWasteValueUnit))
                {
                    editor.SelectedWasteValueUnit = SelectedTest.Unit;
                }
                _container.Resolve<IDialogService>()
                    .ShowEditorDialog(editor.GetView() as AddDogmaEditerView, async () => {
                        var item = await editor.Save(SelectedTest, editor.res);
                        OnSave?.Invoke();
                        OnPropertyChanged();
                        if (item == null)
                            return false;
                        CartoonItemDataList.Add(item);

                        await Initialize(SelectedTest);
                        return true;
                    });

            });
        }

        private void EditItems()
        {
            if (SelectedCartoonItem == null)
                return;

            BusyExecute(async () => {
                var editor = _container.Resolve<ReadyItemsProductingEditerViewModel>();
                await editor.Initialize();
                editor.Operation = operation.Edit;
                editor.BuildFromModel(SelectedCartoonItem);
                //res =  await editor.GetRest(SelectedTest);               
                _container.Resolve<IDialogService>()
                    .ShowEditorDialog(editor.GetView() as ReadyItemsProductingEditerView, async () => {
                        var item = await editor.Save(SelectedTest,res);
                        this.OnSave?.Invoke();
                        if (item == null)
                            return false;

                        CartoonItemDataList.UpdateItem(SelectedCartoonItem, item);
                        await Initialize(SelectedTest);
                        return true;
                    });
                await Initialize(SelectedTest);
                OnPropertyChanged();
            });

        }

        private void DeleteItems(ReadyProductDbo Item)
        {
            if (Item.IsIncreaseDogma)
            {
                var pwd = _container.Resolve<PasswordInputViewModel>();
                pwd.DisposeOnLogin = false;
                pwd.CustomPassword = "54425";
                pwd.OnSuccessLogin += () =>
                {
                    _dialogService.DisposeLastDialog();
                    BusyExecute(async () =>
                    {
                        await _cartoonItemRepository.Delete(e => e.Id == Item.Id);
                       
                        CartoonItemDataList.Remove(Item);
                        await Initialize(SelectedTest);
                       

                    });

                };
                _container.Resolve<IDialogService>()
                       .ShowPopupContent(pwd.GetView() as PasswordInputView);
            }
            else
            {
                var pwd = _container.Resolve<PasswordInputViewModel>();
                pwd.DisposeOnLogin = false;
                pwd.CustomPassword = "54425";
                pwd.OnSuccessLogin += () =>
                {
                    _dialogService.DisposeLastDialog();
                    BusyExecute(async () =>
                    {
                        await _cartoonItemRepository.Delete(e => e.Id == Item.Id);
                        CartoonItemDataList.Remove(Item);
                        await Initialize(SelectedTest);
                    });

                };
                _container.Resolve<IDialogService>()
                       .ShowPopupContent(pwd.GetView() as PasswordInputView);
            }
        }

        private void Search()
        {
            try
            {
                BusyExecute(async () =>
                {
                    CartoonItemDataList =
                    new ObservableCollection<ReadyProductDbo>(
                    await _cartoonItemRepository.GetReadyProductAsyncByName(SelectedTest, SearchText));
                    GetTheValues(SelectedTest);

                    var LastId = CartoonItemDataList.Select(e => e.Id).LastOrDefault();
                    foreach (var item in CartoonItemDataList)
                    {
                        item.CreatedValueUnit = "كغ";
                        if (string.IsNullOrEmpty(item.Note))
                        {
                            item.Note = "لا ملاحظة";
                        }
                        if (LastId == item.Id)
                        {
                            item.IsLastValue = true;
                        }
                        else
                        {
                            item.IsLastValue = false;
                        }
                    }
                });
            }
            catch (Exception e)
            {
                _notificationService.Warning(e.Message);
            }           
          
        }
        public void BackHome()
        {
            BusyExecute(async () => await _container.BackHome());
        }
        public void GoBack()
        {
            BusyExecute(async () =>
            {
                var cartoonView = _container.Resolve<ReadyItemsViewModel>();
                await cartoonView.Initialize();
                _container.Navigate(cartoonView.GetView() as ReadyItemsView);
            });
        }
        private void ShowCartoonList(ReadyProductDbo dto)
        {
           
            if (SelectedCartoonItem == null)
                return;

            BusyExecute(async () => {
                var editor = _container.Resolve<ReadyItemsProductingEditerViewModel>();
                await editor.Initialize();
                //editor.Operation = Operation.Edit;
                editor.BuildFromModel(SelectedCartoonItem);

                _container.Resolve<IDialogService>()
                    .ShowEditorDialog(editor.GetView() as ReadyItemsProductingEditerView, async () => {
                        //var item = await editor.Save(SelectedTest);
                        //if (item == null)
                        //    return false;
                        //CartoonItemDataList.UpdateItem(SelectedCartoonItem, item);
                        await Initialize(SelectedTest);
                        return true;
                    });
            });
            BusyExecute(async () =>
            {
                var cartoonItemsView = _container.Resolve<ReadyItemsProductingViewModel>();
                await cartoonItemsView.Initialize(SelectedTest);
                _container.Navigate(cartoonItemsView.GetView() as ReadyItemsProductingView);
            });
        }
        public  void GetTheValues(TestDto testDto)
        {
            SelectedTest = testDto;
            
            AddValue = CartoonItemDataList.Where(e => e.DeletedDate == null && e.IsIncreaseDogma == true && e.ProductId == testDto.Id).Sum(e => e.IsIncreaseDogmaValue);
            AddDogmaValue = AddValue;
            UnitValue = testDto.TotalResult + AddValue;
            SelectedItem = testDto.ArName;
            TotalWaste = CartoonItemDataList.Sum(e => e.TotalWaste);
            TotalWight = CartoonItemDataList.Sum(e => e.TotalValue);
            TotalExported = CartoonItemDataList.Sum(e => e.ExportedValue);
            
            double result = TotalWaste + TotalWight + TotalExported;
            Quintity = UnitValue  - result ;
          
            if (Quintity <= 0)
                IsQuintity = false;
            else
                IsQuintity = true;
            
        }

        private void SortBy()
        {
            switch (SelectedSortName)
            {
                default:
                    BusyExecute(async () =>
                    {
                        var x = new ObservableCollection<ReadyProductDbo>(
                        await _cartoonItemRepository.GetReadyProductAsyncById(SelectedTest));
                        CartoonItemDataList = x.ToObservableCollection();
                    });
                    break;
                case "افتراضي":
                    BusyExecute(async () =>
                    {
                        var x = new ObservableCollection<ReadyProductDbo>(
                        await _cartoonItemRepository.GetReadyProductAsyncById(SelectedTest));
                        CartoonItemDataList = x.ToObservableCollection();
                    });
                    break;
                case "حسب الاسم":
                    BusyExecute(async () =>
                    {
                        var x = new ObservableCollection<ReadyProductDbo>(
                        await _cartoonItemRepository.GetReadyProductAsyncById(SelectedTest)).OrderBy(e => e.ArName);
                        CartoonItemDataList = x.ToObservableCollection();
                    });
                    break;
                case "حسب التاريخ":
                    BusyExecute(async () =>
                    {
                        var x = new ObservableCollection<ReadyProductDbo>(
                        await _cartoonItemRepository.GetReadyProductAsyncById(SelectedTest)).OrderBy(e => e.CreatedDate);
                        CartoonItemDataList = x.ToObservableCollection();
                    });
                    break;
                case "حسب الرقم الخاص":
                    BusyExecute(async () =>
                    {
                        var x = new ObservableCollection<ReadyProductDbo>(
                        await _cartoonItemRepository.GetReadyProductAsyncById(SelectedTest)).OrderBy(e => e.Id);
                        CartoonItemDataList = x.ToObservableCollection();
                    });
                    break;
                case "حسب الزيادة":
                    BusyExecute(async () =>
                    {
                        var x = new ObservableCollection<ReadyProductDbo>(
                        await _cartoonItemRepository.GetReadyProductAsyncById(SelectedTest)).OrderBy(e => e.IsIncreaseDogmaValue);
                        CartoonItemDataList = x.ToObservableCollection();
                    });
                    break;
            }
           
            //CartoonItemDataList2 = new ObservableCollection<ReadyProduct>(
            //   await _cartoonItemRepository.GetReadyProductAsyncById2(testDto));
           
            GetTheValues(SelectedTest);

            var LastId = CartoonItemDataList.Select(e => e.Id).LastOrDefault();
            foreach (var item in CartoonItemDataList)
            {
                item.ProductedValueUnit = SelectedTest.Unit;
                item.increasevalue = Math.Abs(item.IsIncreaseDogmaValue - item.TotalResult);

                if (string.IsNullOrEmpty(item.Note))
                {
                    item.Note = "لا ملاحظة";
                }
                if (LastId == item.Id)
                {
                    item.IsLastValue = true;
                }
                else
                {
                    item.IsLastValue = false;
                }
            }
        }



    }
}

