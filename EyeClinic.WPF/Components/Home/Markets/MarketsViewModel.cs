using EyeClinic.DataAccess.Entities;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Dialogs.PasswordInput;
using EyeClinic.WPF.Components.Home.Markets.MarketsProductEditer;
using EyeClinic.WPF.Components.Home.ReadyItems.ReadyItemsProducting.AddItemsViewModel;
using EyeClinic.WPF.Components.Home.ReadyItems.ReadyItemsProducting;
using EyeClinic.WPF.Components.Home.Setting.Diseases.DiseaseEditor;
using EyeClinic.WPF.Components.PatientList.PatientFile.Tests;
using EyeClinic.WPF.DataModel;
using EyeClinic.WPF.Setup;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unity;
using operation = EyeClinic.Core.Enums.Operation;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Core;
using EyeClinic.WPF.Components.Home.Markets.MarketsEntry;
using EyeClinic.WPF.Components.Home.Markets.MarketsExtry;
namespace EyeClinic.WPF.Components.Home.Markets
{
    public partial class MarketsViewModel 
    {
        public MarketsViewModel()
        { if (IsDesignMode) { } }
    }
  public partial class MarketsViewModel:BaseViewModel<MarketsView>
    {
        public readonly INotificationService _notificationService;
        public readonly IUnityContainer _container;
        public readonly IMarketsProductsRepository _marketsProductsRepository;
        public readonly IDialogService _dialogService;
        public  MarketsViewModel(INotificationService notificationService,IDialogService dialogService,IUnityContainer container,IMarketsProductsRepository marketsProductsRepository)
        {
            _notificationService = notificationService;
            _container = container;
            _marketsProductsRepository = marketsProductsRepository;
            _dialogService = dialogService;

            BackHomeCommand = new RelayCommand(BackHome);
            AddMarketsProductCommand = new RelayCommand(AddMarketsProduct);
            DeleteMarketsProductCommand = new RelayCommand<MarketsProductsDto>(DeleteMarketsProduct);
            SearchCommand=new RelayCommand(Search);
            EditeMarketsProductCommand = new RelayCommand(EditeMarketProduct);
            ShowEntryItemsCommand = new RelayCommand<MarketsProductsDto>(ShowEntryItems);
            ShowExtryItemsCommand = new RelayCommand<MarketsProductsDto>(ShowExtryItems);
        }
      
        public override async Task Initialize()
        {
            MarketProducts =new ObservableCollection<MarketsProductsDto>( await _marketsProductsRepository.GetAll());
        }

        public event EventHandler<MarketsProductsDto> OnSelectedContactChanged;
        private MarketsProductsDto _selectedContact;

        public MarketsProductsDto SelectedContact
        {
            get => _selectedContact;
            set
            {
                _selectedContact = value;
                OnSelectedContactChanged?.Invoke(this, value);
            }
        }

        public ObservableCollection<MarketsProductsDto> MarketProducts { get; set; }
        public MarketsProductsDto SelectedMarketProducts { get; set; }
      
        public string SearchText { get; set; }
        public double ProductedValue { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand AddMarketsProductCommand { get; set; }
        public ICommand EditeMarketsProductCommand { get; set; }
        public ICommand DeleteMarketsProductCommand { get; set; }
        public ICommand BackHomeCommand { get; set; }
        public ICommand ShowEntryItemsCommand { set; get; }
        public ICommand ShowExtryItemsCommand { set; get; }
        public ICommand ShowCustomerViewCommand { set; get; }
        public ICommand ShowProductsCounterCommand { set; get; }
        private void AddMarketsProduct()
        {
            BusyExecute(async () => {
                var editor = _container.Resolve<MarketsProductEditerViewModel>();
                await editor.Initialize();
                editor.Operation = operation.Add;

                _container.Resolve<IDialogService>()
                    .ShowEditorDialog(editor.GetView() as MarketsProductEditerView, async () => {
                        var item = await editor.Save();
                        if (item == null)
                            return false;

                        MarketProducts.Add(item);
                        return true;
                    });
            });

        }

        private void EditeMarketProduct()
        {          
            if (SelectedMarketProducts == null)
            { _notificationService.Warning("Select The Product"); return; }

            BusyExecute(async () =>
            {
                var editor = _container.Resolve<MarketsProductEditerViewModel>();
                await editor.Initialize();
                editor.Operation = operation.Edit;
                editor.BuildFromModel(SelectedMarketProducts);

                _container.Resolve<IDialogService>()
                    .ShowEditorDialog(editor.GetView() as MarketsProductEditerView, async () =>
                    {
                        var item = await editor.Save();
                        if (item == null)
                            return false;

                        MarketProducts.UpdateItem(SelectedMarketProducts, item);
                        return true;
                    });
            });
        }

        private void DeleteMarketsProduct(MarketsProductsDto marketsProductsDto)
        {
            var pwd = _container.Resolve<PasswordInputViewModel>();
            pwd.DisposeOnLogin = false;
            pwd.CustomPassword = "54425";
            pwd.OnSuccessLogin += () =>
            {
                _dialogService.DisposeLastDialog();
                BusyExecute(async () =>
                {
                    await _marketsProductsRepository.Delete(e => e.Id == marketsProductsDto.Id);
                    MarketProducts.Remove(marketsProductsDto);
                    //await _container.Resolve<ReadyItemsViewModel>().(disease);
                });
            };
            _container.Resolve<IDialogService>()
                   .ShowPopupContent(pwd.GetView() as PasswordInputView);

        }

        private void Search()
        {
            BusyExecute(async () =>
            {
                SearchText ??= "";

                MarketProducts = new ObservableCollection<MarketsProductsDto>(
                await _marketsProductsRepository.GetByKey(e=>e.MarketsProductName.ToLower().Contains(SearchText.ToLower())  && e.DeletedBy == null));
            });
        }
        private void BackHome()
        {
            BusyExecute(async()=>await _container.BackHome());
        }

        public void BackToQueue(QueueItem queue)
        {
            BusyExecute(async () => await _container.BackHome());
        }
        private void ShowCartoonList(TestDto dto)
        {
            //if (SelectedDisease == null)
            //    return;
            //BusyExecute(async () =>
            //{
            //    var cartoonItemsView = _container.Resolve<TestsViewModel>();
            //    await cartoonItemsView.Initialize(SelectedDisease.Id);
            //    _container.Navigate(cartoonItemsView.GetView() as TestsView);
            //});
        }
        public int ConvertToInt(string st)
        {
            int Result = Convert.ToInt32(st);
            return Result;
        }

        public void ShowEntryItems(MarketsProductsDto marketsProductsDto )
        {
            if (SelectedMarketProducts == null)
                return;
            BusyExecute(async () =>
            {
                var cartoonItemsView = _container.Resolve<MarketsEntryViewModel>();
                await cartoonItemsView.Initialize(marketsProductsDto);
                _container.Navigate(cartoonItemsView.GetView() as MarketsEntryView);
            });
        }
        public void ShowExtryItems(MarketsProductsDto marketsProductsDto)
        {
            if (SelectedMarketProducts == null)
                return;
            BusyExecute(async () =>
            {
                var cartoonItemsView = _container.Resolve<MarketsExtryViewModel>();
                await cartoonItemsView.Initialize(marketsProductsDto);
                _container.Navigate(cartoonItemsView.GetView() as MarketsExtryView);
            });
        }
    }
}
