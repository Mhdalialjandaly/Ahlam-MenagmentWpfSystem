using EyeClinic.Core;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Dialogs.PasswordInput;
using EyeClinic.WPF.Components.Home.CartoonForm.ItemsEditer.ItemsEditerDialog;
using EyeClinic.WPF.Components.Home.Setting.Diseases.DiseaseEditor;
using EyeClinic.WPF.DataModel;
using EyeClinic.WPF.Setup;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unity;
using operation = EyeClinic.Core.Enums.Operation;

namespace EyeClinic.WPF.Components.Home.CartoonForm.ItemsEditer
{
    public partial class CartoonItemsViewModel 
    {
        public CartoonItemsViewModel()
        {
            if (IsDesignMode) { }
        }
    }
    public partial class CartoonItemsViewModel : BaseViewModel<CartoonItemsView>
    {
        private readonly IDialogService _dialogService;
        private readonly IUnityContainer _container;
        private readonly IDiseasesRepository _diseasesRepository;
        private readonly ICartoonLabelsRepository _cartoonItemRepository;
        private readonly ICartoonLabelsRepository _cartoonrepository;
        public CartoonItemsViewModel(IUnityContainer unityContainer, IDiseasesRepository diagnosisRepository,
            ICartoonLabelsRepository cartoonItemRepository, IDialogService dialogService, ICartoonLabelsRepository cartoonLabelsRepository)
        {
            _container = unityContainer;
            _diseasesRepository = diagnosisRepository;
            _cartoonItemRepository = cartoonItemRepository;
            _dialogService = dialogService;
            _cartoonItemRepository = cartoonLabelsRepository;

            BackHomeCommand = new RelayCommand(BackHome);
            GoBackCommand = new RelayCommand(GoBack);

            AddCartoonItemCommand = new RelayCommand(AddItems);
            EditCartoonItemCommand = new RelayCommand(EditItems);
            DeleteCartoonItemCommand = new RelayCommand<CartoonLabelsDto>(DeleteItems);
            ShowItemsCommand = new RelayCommand<CartoonLabelsDto>(ShowCartoonList);
            SearchCommand = new RelayCommand(Search);
        }


        public async Task Initialize(DiseaseDto diseaseDto)
        {
            CartoonItemDataList = new ObservableCollection<CartoonLabelsDto>(
                await _cartoonItemRepository.GetCartoonItemAsyncById(diseaseDto.Id));
            FirstTermValue = diseaseDto.FirstValue;
            TotalDEntry = CartoonItemDataList.Select(e => e.DEntry).Sum();
            TotalDExtry = CartoonItemDataList.Select(e => e.DExtry).Sum();
            TotalResult = FirstTermValue + TotalDEntry - TotalDExtry;
            SelectedItem = diseaseDto.ArName;
            disease = diseaseDto;
        }

        public ObservableCollection<CartoonLabelsDto> CartoonItemDataList { get; set; }
        public CartoonLabelsDto SelectedCartoonItem { get; set; }
        public DiseaseDto disease  {set;get;}
        public string SearchText { get; set; }
     
        public string SelectedItem{ get; set; }
        public double   CurrentValue { get; set; }
        public double FirstTermValue { get; set; }
        public double TotalDEntry { get; set; }
        public double TotalDExtry { get; set; }
        public double TotalResult { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand AddCartoonItemCommand { get; set; }
        public ICommand EditCartoonItemCommand { get; set; }
        public ICommand DeleteCartoonItemCommand { get; set; }
        public ICommand BackHomeCommand { get; set; }
        public ICommand GoBackCommand { get; set; }
        public ICommand ShowItemsCommand { set; get; }
      
        

    
        private void AddItems()
        {
            BusyExecute(async () => {
                var editor = _container.Resolve<ItemsEditerViewModel>();
                await editor.Initialize();
                editor.Operation = operation.Add;

                _container.Resolve<IDialogService>()
                    .ShowEditorDialog(editor.GetView() as ItemsEditerView, async () => {
                        var item = await editor.Save(disease);
                        if (item == null)
                            return false;

                        CartoonItemDataList.Add(item);
                        return true;
                    });
            });

        }

        private void EditItems()
        {
            if (SelectedCartoonItem == null)
                return;

            BusyExecute(async () => {
                var editor = _container.Resolve<ItemsEditerViewModel>();
                await editor.Initialize();
                editor.Operation = operation.Edit;
                editor.BuildFromModel(SelectedCartoonItem);

                _container.Resolve<IDialogService>()
                    .ShowEditorDialog(editor.GetView() as ItemsEditerView, async () => {
                        var item = await editor.Save(disease);
                        if (item == null)
                            return false;

                        CartoonItemDataList.UpdateItem(SelectedCartoonItem, item);
                        return true;
                    });
            });
        }

        private void DeleteItems(CartoonLabelsDto Item)
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
                  
                });
            };
            _container.Resolve<IDialogService>()
                   .ShowPopupContent(pwd.GetView() as PasswordInputView);
           
        }

        private void Search()
        {
        }
        public void BackHome()
        {
            BusyExecute(async () => await _container.BackHome());
        }
        public void GoBack()
        {
            BusyExecute(async () =>
            {
                var cartoonView = _container.Resolve<CartoonViewModel>();
                await cartoonView.Initialize();
                _container.Navigate(cartoonView.GetView() as CartoonView);
            });
        }
        private void ShowCartoonList(CartoonLabelsDto dto)
        {
            if (SelectedCartoonItem == null)
                return;

            BusyExecute(async () => {
                var editor = _container.Resolve<ItemsEditerViewModel>();
                await editor.Initialize();
                editor.Operation = operation.Edit;
                editor.BuildFromModel(SelectedCartoonItem);

                _container.Resolve<IDialogService>()
                    .ShowEditorDialog(editor.GetView() as ItemsEditerView, async () => {
                        var item = await editor.Save(disease);
                        if (item == null)
                            return false;

                        CartoonItemDataList.UpdateItem(SelectedCartoonItem, item);
                        return true;
                    });
            });
        }

    }
}
