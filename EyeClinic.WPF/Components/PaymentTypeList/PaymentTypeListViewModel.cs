using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using EyeClinic.Core;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.PaymentTypeList.PaymentTypeEditor;
using EyeClinic.WPF.Setup;
using Unity;
using Operation = EyeClinic.Core.Enums.Operation;

namespace EyeClinic.WPF.Components.PaymentTypeList
{
    public partial class PaymentTypeListViewModel
    {
        public PaymentTypeListViewModel() {
            if (IsDesignMode) { }
        }
    }

    public partial class PaymentTypeListViewModel : BaseViewModel<PaymentTypeListView>
    {
        private readonly IPaymentTypeRepository _paymentTypeRepository;
        private readonly IDialogService _dialogService;
        private readonly IUnityContainer _container;

        public PaymentTypeListViewModel(IPaymentTypeRepository paymentTypeRepository, IDialogService dialogService,
            IUnityContainer container) {
            _paymentTypeRepository = paymentTypeRepository;
            _dialogService = dialogService;
            _container = container;

            AddPaymentTypeCommand = new RelayCommand(AddPaymentType);
            EditPaymentTypeCommand = new RelayCommand(EditPaymentType);
            DeletePaymentTypeCommand = new RelayCommand<PaymentTypeDto>(DeletePaymentType);
            ApplyPaymentTypeListDateFilterCommand = new RelayCommand(
                () => {
                    BusyExecute(async () => await Initialize());
                });

            BackCommand = new RelayCommand(Back);
        }


        public override async Task Initialize() {
            var view = GetView() as PaymentTypeListView;
            if (view == null)
                throw new Exception("Invalid View for PaymentTypeList");

            var paymentTypes = await _paymentTypeRepository
                .GetByKey(e => string.IsNullOrWhiteSpace(SearchText) ||
                               e.BeneficiaryName.ToLower().Contains(SearchText) ||
                               e.TypeName.ToLower().Contains(SearchText) ||
                               e.Note.ToLower().Contains(SearchText) &&
                               (e.Remaining > 0 || ShowCompleted));

            PaymentTypes = new ObservableCollection<PaymentTypeDto>(
                paymentTypes.OrderByDescending(e => e.CreatedDate).ToList());
        }

        public event EventHandler<PaymentTypeDto> OnSelectedPaymentTypeChanged;



        public ObservableCollection<PaymentTypeDto> PaymentTypes { get; set; }

        private PaymentTypeDto _selectedPaymentType;
        public PaymentTypeDto SelectedPaymentType {
            get => _selectedPaymentType;
            set {
                _selectedPaymentType = value;
                OnSelectedPaymentTypeChanged?.Invoke(this, value);
            }
        }

        public string SearchText { get; set; }
        public bool ShowCompleted { get; set; }



        public ICommand AddPaymentTypeCommand { get; set; }
        public ICommand EditPaymentTypeCommand { get; set; }
        public ICommand DeletePaymentTypeCommand { get; set; }
        public ICommand ApplyPaymentTypeListDateFilterCommand { get; set; }
        public ICommand BackCommand { get; set; }


        private void SearchPaymentTypes(string value) {
            BusyExecute(async () => {
                var items = await _paymentTypeRepository
                    .GetByKey(e => e.TypeName != null && e.TypeName.ToLower().Contains(value) ||
                                   e.Note != null && e.Note.ToLower().Contains(value) ||
                                   e.BeneficiaryName != null && e.BeneficiaryName.ToString().ToLower().Contains(value));


                PaymentTypes = new ObservableCollection<PaymentTypeDto>(items);
            });
        }

        private void AddPaymentType() {
            BusyExecute(async () => {
                var editor = _container.Resolve<PaymentTypeEditorViewModel>();
                await editor.Initialize();
                editor.Operation = Operation.Add;
                _dialogService.ShowEditorDialog(editor.GetView() as PaymentTypeEditorView,
                    async () => {
                        var paymentItem = await editor.SaveAsync();
                        if (paymentItem == null)
                            return false;

                        PaymentTypes.Insert(0, paymentItem);
                        return true;
                    });
            });
        }
        private void EditPaymentType() {
            if (SelectedPaymentType == null)
                return;

            BusyExecute(async () => {
                var editor = _container.Resolve<PaymentTypeEditorViewModel>();
                await editor.Initialize();
                editor.Operation = Operation.Edit;
                editor.Id = SelectedPaymentType.Id;
                editor.BuildFromModel(SelectedPaymentType);
                _dialogService.ShowEditorDialog(editor.GetView() as PaymentTypeEditorView,
                    async () => {
                        var paymentItem = await editor.SaveAsync();
                        if (paymentItem != null) {
                            paymentItem.Remaining = SelectedPaymentType.Remaining;
                            PaymentTypes.UpdateItem(SelectedPaymentType, paymentItem);
                            return true;
                        }

                        return false;
                    });
            });
        }

        private void DeletePaymentType(PaymentTypeDto paymentType) {
            if (paymentType == null)
                return;

            BusyExecute(async () => {
                await _paymentTypeRepository.Delete(paymentType.Id);
                PaymentTypes.Remove(paymentType);
            });
        }

        private void Back() {
            BusyExecute(async () => {
                await _container.BackHome();
            });
        }
    }
}
