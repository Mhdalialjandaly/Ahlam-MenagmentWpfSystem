using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Dialogs.PasswordInput;
using EyeClinic.WPF.Components.Home.ReadyItems.ReadyItemsProducting.AddDogmaEditer;
using EyeClinic.WPF.Components.Home.ReadyItems.ReadyItemsProducting.ReadyItemsProductingEditer;
using EyeClinic.WPF.Components.Home.ReadyItems.ReadyItemsProducting;
using EyeClinic.WPF.Components.Home.ReadyItems;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unity;
using EyeClinic.WPF.Setup;
using EyeClinic.WPF.Components.Home.Markets.MarketsExtry.MarketExtryEditer;
using operation = EyeClinic.Core.Enums.Operation;
using EyeClinic.WPF.Components.Home.Clinic;
using EyeClinic.WPF.Components.Home.Markets.MarketsProductEditer;
using EyeClinic.Core;
using EyeClinic.DataAccess.Repositories;
using EyeClinic.WPF.Components.Home.Markets.MarketsEntry;

namespace EyeClinic.WPF.Components.Home.Markets.MarketsExtry
{
    public partial class MarketsExtryViewModel
    {
        public MarketsExtryViewModel() { }
    }
    public partial class MarketsExtryViewModel : BaseViewModel<MarketsExtryView>
    {
        public readonly INotificationService _notificationService;
        public readonly IMarketsExtryRepository _marketsExtryRepository;
        public readonly IUnityContainer _unityContainer;
        public readonly IDialogService _dialogService;
        public readonly IMarketsProductsRepository _marketsProductsRepository;
        public readonly IMarketsEntryRepository _marketsEntryRepository;
        public MarketsExtryViewModel(INotificationService notificationService, IDialogService dialogService, 
            IMarketsExtryRepository marketsExtryRepository,
            IUnityContainer unityContainer, IMarketsProductsRepository marketsProductsRepository, IMarketsEntryRepository marketsEntryRepository)
        {
            _unityContainer = unityContainer;
            _notificationService = notificationService;
            _marketsExtryRepository = marketsExtryRepository;
            _dialogService = dialogService;
            _marketsProductsRepository = marketsProductsRepository;
            _marketsEntryRepository = marketsEntryRepository;

            BackHomeCommand = new RelayCommand(BackHome);
            AddMarketExtryCommand = new RelayCommand(AddMarketExtry);
            GoBackCommand = new RelayCommand(GoBack);
            EditeMarketExtryCommand = new RelayCommand<MarketsExtryDto>(EditeMarketExtry);
            DeleteMarketExtryCommand = new RelayCommand<MarketsExtryDto>(DeleteMarketExtry);
            SearchCommand = new RelayCommand(Search);
           
        }
        public async Task Initialize(MarketsProductsDto marketsProductsDto)
        {
            MarketItemDataList = new ObservableCollection<MarketsExtryDto>(
                 await _marketsExtryRepository.GetMarketsExtriesById(marketsProductsDto));

            SelectedMarketProduct = marketsProductsDto;
            SelectedMarketProduct.RealValue = marketsProductsDto.FirstTermValue + marketsProductsDto.Entry - MarketItemDataList.Sum(e=>e.Quinttity) ;
            SelectedMarketProduct.Extry = MarketItemDataList.Sum(e => e.Quinttity);
        }
        public ObservableCollection<MarketsExtryDto> MarketItemDataList { get; set; }
        public MarketsExtryDto SelectedMarketItem { get; set; }
        public MarketsProductsDto SelectedMarketProduct { get; set; }
        public int Id { get; set; }
        public double Quinttity { get; set; }
        public string UnitName { get; set; }
        public int CodeNumber { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string SearchText { get; set; }

        public ICommand SearchCommand { get; set; }
        public ICommand AddMarketExtryCommand { get; set; }
        public ICommand EditeMarketExtryCommand { get; set; }
        public ICommand DeleteMarketExtryCommand { get; set; }
        public ICommand BackHomeCommand { get; set; }
        public ICommand GoBackCommand { get; set; }

        public event Action OnSave;


        private void AddMarketExtry()
        {
            BusyExecute(async () =>
            {
                var editor = _unityContainer.Resolve<MarketExtryEditerViewModel>();
                await editor.Initialize();
                editor.Operation = operation.Add;

                _unityContainer.Resolve<IDialogService>()
                    .ShowEditorDialog(editor.GetView() as MarketExtryEditerView, async () =>
                    {
                        var item = await editor.Save(SelectedMarketProduct);
                        await _marketsProductsRepository.UpdateMarketProductByItem(SelectedMarketProduct);
                        OnSave?.Invoke();

                        var cartoonItemsView = _unityContainer.Resolve<MarketsExtryViewModel>();
                        await cartoonItemsView.Initialize(SelectedMarketProduct);
                        _unityContainer.Navigate(cartoonItemsView.GetView() as MarketsExtryView);
                        if (item == null)
                            return false;
                        MarketItemDataList.Add(item);                       
                        return true;
                    });
                
            });
        }


        private void EditeMarketExtry(MarketsExtryDto marketsExtryDto)
        {
            if (SelectedMarketItem == null)
                return;
            BusyExecute(async () =>
            {
                var editor = _unityContainer.Resolve<MarketExtryEditerViewModel>();
                await editor.Initialize();
                editor.Operation = operation.Edit;
                editor.BuildFromModel(marketsExtryDto);

                _unityContainer.Resolve<IDialogService>()
                    .ShowEditorDialog(editor.GetView() as MarketExtryEditerView, async () =>
                    {
                        var item = await editor.Save(SelectedMarketProduct);
                        await _marketsProductsRepository.UpdateMarketProductByItem(SelectedMarketProduct);
                        this.OnSave?.Invoke();

                        var cartoonItemsView = _unityContainer.Resolve<MarketsExtryViewModel>();
                        await cartoonItemsView.Initialize(SelectedMarketProduct);
                        _unityContainer.Navigate(cartoonItemsView.GetView() as MarketsExtryView);
                        if (item == null)
                            return false;

                        MarketItemDataList.UpdateItem(SelectedMarketItem, item);                       
                        return true;
                    });
            });

        }

        private void DeleteMarketExtry(MarketsExtryDto marketsExtry)
        {
            var pwd = _unityContainer.Resolve<PasswordInputViewModel>();
            pwd.DisposeOnLogin = false;
            pwd.CustomPassword = "54425";
            pwd.OnSuccessLogin += () =>
            {
                _dialogService.DisposeLastDialog();
                BusyExecute(async () =>
                {
                    await _marketsExtryRepository.Delete(e => e.Id == marketsExtry.Id);
                    MarketItemDataList.Remove(marketsExtry);
                    await _marketsProductsRepository.UpdateMarketProductByItem(SelectedMarketProduct);

                    var cartoonItemsView = _unityContainer.Resolve<MarketsExtryViewModel>();
                    await cartoonItemsView.Initialize(SelectedMarketProduct);
                    _unityContainer.Navigate(cartoonItemsView.GetView() as MarketsExtryView);
                });                
            };
            _unityContainer.Resolve<IDialogService>()
                   .ShowPopupContent(pwd.GetView() as PasswordInputView);
        }

        private void Search()
        {
            BusyExecute(async () => {
                MarketItemDataList = new ObservableCollection<MarketsExtryDto>
                ((await _marketsExtryRepository.GetByKey(e => e.CodeNumber.ToString().Contains(SearchText))).Where(e=>e.DeletedDate == null));
            });
        }
        public void BackHome()
        {
            BusyExecute(async () => await _unityContainer.BackHome());
        }
        public void GoBack()
        {
            _unityContainer.SetCurrentPatient(null);
            if (_dialogService.IsPopupActivated())
            {
                _dialogService.DisposeLastDialog();
                return;
            }

            BusyExecute(async () => {
                var clinic = _unityContainer.Resolve<MarketsViewModel>();
                await clinic.Initialize();
                _unityContainer.Navigate(clinic.GetView() as MarketsView);
            });
        }

    }
}
