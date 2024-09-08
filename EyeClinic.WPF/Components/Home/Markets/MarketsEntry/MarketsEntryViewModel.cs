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
using EyeClinic.WPF.Components.Home.Markets.MarketsEntry.MarketEntryEditer;
using operation = EyeClinic.Core.Enums.Operation;
using EyeClinic.WPF.Components.Home.Clinic;
using EyeClinic.WPF.Components.Home.Markets.MarketsProductEditer;
using EyeClinic.Core;
using EyeClinic.WPF.Components.Home.Markets.MarketsExtry;
using Syncfusion.XlsIO.Parser.Biff_Records;

namespace EyeClinic.WPF.Components.Home.Markets.MarketsEntry
{
    public partial class MarketsEntryViewModel
    {
        public MarketsEntryViewModel() { }
    }
    public partial class MarketsEntryViewModel : BaseViewModel<MarketsEntryView>
    {
        public readonly INotificationService _notificationService;
        public readonly IMarketsEntryRepository _marketsEntryRepository;
        public readonly IMarketsExtryRepository _marketsExtryRepository;
        public readonly IUnityContainer _unityContainer;
        public readonly IDialogService _dialogService;
        public readonly IMarketsProductsRepository _marketsProductsRepository;
        public MarketsEntryViewModel(INotificationService notificationService,IDialogService dialogService, 
            IMarketsEntryRepository marketsEntryRepository,
            IUnityContainer unityContainer,
            IMarketsProductsRepository marketsProductsRepository,
            IMarketsExtryRepository marketsExtryRepository)
        {
            _unityContainer = unityContainer;
            _notificationService = notificationService;
            _marketsEntryRepository = marketsEntryRepository;
            _dialogService = dialogService;
            _marketsProductsRepository = marketsProductsRepository;
            _marketsExtryRepository = marketsExtryRepository;   

            BackHomeCommand = new RelayCommand(BackHome);
            AddMarketEntryCommand = new RelayCommand(AddMarketEntry);
            GoBackCommand =new RelayCommand(GoBack);
            EditeMarketEntryCommand = new RelayCommand<MarketsEntryDto>(EditeMarketEntry);
            DeleteMarketEntryCommand = new RelayCommand<MarketsEntryDto>(DeleteMarketEntry);
            SearchCommand = new RelayCommand(Search);
        }
        public async Task Initialize(MarketsProductsDto marketsProductsDto)
        {
            MarketItemDataList = new ObservableCollection<MarketsEntryDto>(
                 await _marketsEntryRepository.GetMarketsEntriesById(marketsProductsDto));
            SelectedMarketProduct = marketsProductsDto;
            SelectedMarketProduct.RealValue = MarketItemDataList.Sum(e => e.Quinttity) + marketsProductsDto.FirstTermValue - marketsProductsDto.Extry;
            SelectedMarketProduct.Entry = MarketItemDataList.Sum(e => e.Quinttity);


        }
        public ObservableCollection<MarketsEntryDto> MarketItemDataList { get; set; }
        public MarketsEntryDto SelectedMarketItem { get; set; }
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
        public ICommand AddMarketEntryCommand { get; set; }
        public ICommand EditeMarketEntryCommand { get; set; }
        public ICommand DeleteMarketEntryCommand { get; set; }
        public ICommand BackHomeCommand { get; set; }
        public ICommand GoBackCommand { get; set; }

        public event Action OnSave;


        private void AddMarketEntry()
        {
            BusyExecute(async () =>
            {
                var editor = _unityContainer.Resolve<MarketEntryEditerViewModel>();
                await editor.Initialize();
                editor.Operation = operation.Add;

                _unityContainer.Resolve<IDialogService>()
                    .ShowEditorDialog(editor.GetView() as MarketEntryEditerView, async () =>
                    {
                        var item = await editor.Save(SelectedMarketProduct);
                        await  _marketsProductsRepository.UpdateMarketProductByItem(SelectedMarketProduct);
                        OnSave?.Invoke();

                        var cartoonItemsView = _unityContainer.Resolve<MarketsEntryViewModel>();
                        await cartoonItemsView.Initialize(SelectedMarketProduct);
                        _unityContainer.Navigate(cartoonItemsView.GetView() as MarketsEntryView);
                        if (item == null)
                            return false;                      
                        return true;
                    });             
            });          

        }
       

        private void EditeMarketEntry(MarketsEntryDto marketsEntryDto)
        {
            if (SelectedMarketItem == null)
                return;
            BusyExecute(async () =>
            {
                var editor = _unityContainer.Resolve<MarketEntryEditerViewModel>();
                await editor.Initialize();
                editor.Operation = operation.Edit;
                editor.BuildFromModel(marketsEntryDto);

                _unityContainer.Resolve<IDialogService>()
                    .ShowEditorDialog(editor.GetView() as MarketEntryEditerView, async () =>
                    {
                        var item = await editor.Save(SelectedMarketProduct);
                        await _marketsProductsRepository.UpdateMarketProductByItem(SelectedMarketProduct);
                        OnSave?.Invoke();


                        var cartoonItemsView = _unityContainer.Resolve<MarketsEntryViewModel>();
                        await cartoonItemsView.Initialize(SelectedMarketProduct);
                        _unityContainer.Navigate(cartoonItemsView.GetView() as MarketsEntryView);
                        if (item == null)
                            return false;                        
                        return true;
                    });                
            });           
           

        }

        private void DeleteMarketEntry(MarketsEntryDto marketsEntry)
        {
            var pwd = _unityContainer.Resolve<PasswordInputViewModel>();
            pwd.DisposeOnLogin = false;
            pwd.CustomPassword = "54425";
            pwd.OnSuccessLogin += () =>
            {
                _dialogService.DisposeLastDialog();
                BusyExecute(async () =>
                {
                    await _marketsEntryRepository.Delete(e => e.Id == marketsEntry.Id);
                    MarketItemDataList.Remove(marketsEntry);
                    await _marketsProductsRepository.UpdateMarketProductByItem(SelectedMarketProduct);


                    var cartoonItemsView = _unityContainer.Resolve<MarketsEntryViewModel>();
                    await cartoonItemsView.Initialize(SelectedMarketProduct);
                    _unityContainer.Navigate(cartoonItemsView.GetView() as MarketsEntryView);
                });               
            };
            _unityContainer.Resolve<IDialogService>()
                   .ShowPopupContent(pwd.GetView() as PasswordInputView);
        }

        private void Search()
        {
            BusyExecute(async () => {
                MarketItemDataList = new ObservableCollection<MarketsEntryDto>(
                 (await _marketsEntryRepository.GetByKey(e=>e.CodeNumber.ToString().Contains(SearchText))).Where(e=>e.DeletedDate == null));                
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
