using EyeClinic.Core;
using EyeClinic.Core.Enums;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.DataAccess.Repositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Dialogs.PasswordInput;
using EyeClinic.WPF.Components.Home.CartoonForm.Customers;
using EyeClinic.WPF.Components.Home.CartoonForm.ItemsEditer;
using EyeClinic.WPF.Components.Home.Setting.Diseases.DiseaseEditor;
using EyeClinic.WPF.DataModel;
using EyeClinic.WPF.Setup;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unity;
using operation = EyeClinic.Core.Enums.Operation;

namespace EyeClinic.WPF.Components.Home.CartoonForm
{
    public partial class CartoonViewModel
    {
        public CartoonViewModel()
        {
            if (IsDesignMode) { }
        }
    }
    public partial class CartoonViewModel : BaseViewModel<CartoonView>
    {
        private readonly IDialogService _dialogService;
        private readonly IUnityContainer _container;
        private readonly IDiseasesRepository _diseasesRepository;
        private readonly ICartoonLabelsRepository _cartoonrepository;

        public CartoonViewModel(IUnityContainer container,
            IDiseasesRepository diseasesRepository,           
            ICartoonLabelsRepository cartoonItemRepository,IDialogService dialogService) 
        {
            _container = container;
            _diseasesRepository = diseasesRepository;
            _dialogService = dialogService;
            _cartoonrepository = cartoonItemRepository;

            AddDiseaseCommand = new RelayCommand(AddDisease);
            EditDiseaseCommand = new RelayCommand(EditDisease);
            DeleteDiseaseCommand = new RelayCommand<DiseaseDto>(DeleteDisease);
            ShowItemsCommand = new RelayCommand<DiseaseDto>(ShowCartoonList);            
            SearchCommand = new RelayCommand(Search);
            BackHomeCommand = new RelayCommand<QueueItem>(BackToQueue);
            ShowCustomerViewCommand=new RelayCommand(ShowCustomerView);
        }

       

        public override async Task Initialize()
        {           
         
          DiseasesList   = new ObservableCollection<DiseaseDto>(
                await _diseasesRepository.GetAll());

            foreach (var item in DiseasesList)
            {
                var Re = await _cartoonrepository.GetSumCartoonLabels(item.Id);
                item.CurrentValue = Re;
            }
        }

        public event EventHandler<CartoonLabelsDto> OnSelectedContactChanged;
        private CartoonLabelsDto _selectedContact;

        public CartoonLabelsDto SelectedContact
        {
            get => _selectedContact;
            set
            {
                _selectedContact = value;
                OnSelectedContactChanged?.Invoke(this, value);
            }
        }

        public ObservableCollection<DiseaseDto> DiseasesList { get; set; }
        public DiseaseDto SelectedDisease { get; set; }
        public double CurrentValue { get; set; }
        public string SearchText { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand AddDiseaseCommand { get; set; }
        public ICommand EditDiseaseCommand { get; set; }
        public ICommand DeleteDiseaseCommand { get; set; }
        public ICommand BackHomeCommand { get; set; }
        public ICommand ShowItemsCommand { set; get; }
        public ICommand ShowCustomerViewCommand { set; get; }
        private void AddDisease()
        {
            BusyExecute(async () => {
                var editor = _container.Resolve<DiseaseEditorViewModel>();
                await editor.Initialize();
                editor.Operation = operation.Add;

                _container.Resolve<IDialogService>()
                    .ShowEditorDialog(editor.GetView() as DiseaseEditorView, async () => {
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
                return;

            BusyExecute(async () => {
                var editor = _container.Resolve<DiseaseEditorViewModel>();
                await editor.Initialize();
                editor.Operation = operation.Edit;
                editor.BuildFromModel(SelectedDisease);

                _container.Resolve<IDialogService>()
                    .ShowEditorDialog(editor.GetView() as DiseaseEditorView, async () => {
                        var item = await editor.Save();
                        if (item == null)
                            return false;

                        DiseasesList.UpdateItem(SelectedDisease, item);
                        return true;
                    });
            });
        }

        private void DeleteDisease(DiseaseDto disease)
        {
            var pwd = _container.Resolve<PasswordInputViewModel>();
            pwd.DisposeOnLogin = false;
            pwd.CustomPassword = "54425";
            pwd.OnSuccessLogin += () =>
            {
                _dialogService.DisposeLastDialog();
                BusyExecute(async () =>
                {                    
                        await _diseasesRepository.Delete(e => e.Id == disease.Id);
                        DiseasesList.Remove(disease);                    
                });
            };
            _container.Resolve<IDialogService>()
                   .ShowPopupContent(pwd.GetView() as PasswordInputView);
           
        }

        private void Search()
        {           
            BusyExecute(async () =>
            {
                DiseasesList = new ObservableCollection<DiseaseDto>(
                   await _diseasesRepository.GetByKey(e=>
                   e.EnName.ToLower().Contains(SearchText.ToLower()) ||
                   e.ArName.ToLower().Contains(SearchText.ToLower())));

                foreach (var item in DiseasesList)
                {
                    var Re = await _cartoonrepository.GetSumCartoonLabels(item.Id);
                    item.CurrentValue = Re;
                }
            });
            OnPropertyChanged();
        }
        public void BackToQueue(QueueItem queue)
        {
            BusyExecute(async () => await _container.BackHome());
        }
        private void ShowCartoonList(DiseaseDto dto)
        {
            if (SelectedDisease == null)
                return;
            BusyExecute(async () =>
            {
                var cartoonItemsView = _container.Resolve<CartoonItemsViewModel>();
                await cartoonItemsView.Initialize(SelectedDisease);
                _container.Navigate(cartoonItemsView.GetView() as CartoonItemsView);
            });
        }
        public int ConvertToInt(string st)
        {
           int Result = Convert.ToInt32(st);
            return Result;
        }
        private void ShowCustomerView()
        {
            BusyExecute(async () =>
            {
                var cartoonItemsView = _container.Resolve<CustomerViewModel>();
                await cartoonItemsView.Initialize();
                _container.Navigate(cartoonItemsView.GetView() as CustomerView);
            });
        }
    }
}
