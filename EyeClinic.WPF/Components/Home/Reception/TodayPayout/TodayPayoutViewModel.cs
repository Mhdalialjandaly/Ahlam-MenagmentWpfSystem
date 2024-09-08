using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using EyeClinic.Core;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Home.Reception.TodayPayout.TodayPayoutEditor;
using Unity;
using operation = EyeClinic.Core.Enums.Operation;

namespace EyeClinic.WPF.Components.Home.Reception.TodayPayout
{
    public partial class TodayPayoutViewModel
    {
        public TodayPayoutViewModel() {
            if (IsDesignMode) { }
        }
    }

    public partial class TodayPayoutViewModel : BaseViewModel<TodayPayoutView>
    {
        private readonly IPayoutRepository _payoutRepository;
        private readonly IUnityContainer _container;

        public TodayPayoutViewModel(IPayoutRepository payoutRepository, IUnityContainer container) {
            _payoutRepository = payoutRepository;
            _container = container;

            AddPayoutCommand = new RelayCommand(AddPayout);
            EditPayoutCommand = new RelayCommand(EditPayout);
            RemovePayoutCommand = new RelayCommand<PayoutDto>(RemovePayout);
            FilterCommand = new RelayCommand(Filter);

            TargetDate = DateTime.Now;
        }


        public override async Task Initialize() {
            var payouts = await _payoutRepository.GetByKey(e => e.DateTime.Date == DateTime.Now.Date);
            Payouts = new ObservableCollection<PayoutDto>(payouts);
        }

        public ObservableCollection<PayoutDto> Payouts { get; set; }
        public PayoutDto SelectedPayout { get; set; }
        public DateTime TargetDate { get; set; }

        public ICommand AddPayoutCommand { get; set; }
        public ICommand EditPayoutCommand { get; set; }
        public ICommand RemovePayoutCommand { get; set; }
        public ICommand FilterCommand { get; set; }


        private void AddPayout() {
            var payoutEditor = _container.Resolve<TodayPayoutEditorViewModel>();
            payoutEditor.Operation = operation.Add;
            payoutEditor.DateTime = TargetDate;
            _container.Resolve<IDialogService>()
                .ShowEditorDialog(payoutEditor.GetView() as TodayPayoutEditorView, async () => {
                    var item = await payoutEditor.SaveAsync();
                    if (item == null)
                        return false;

                    Payouts.Add(item);
                    return true;
                });
        }

        private void EditPayout() {
            if (SelectedPayout == null)
                return;

            var payoutEditor = _container.Resolve<TodayPayoutEditorViewModel>();
            payoutEditor.Operation = operation.Edit;
            payoutEditor.BuildFromModel(SelectedPayout);

            _container.Resolve<IDialogService>()
                .ShowEditorDialog(payoutEditor.GetView() as TodayPayoutEditorView, async () => {
                    var item = await payoutEditor.SaveAsync();
                    if (item == null)
                        return false;

                    Payouts.UpdateItem(SelectedPayout, item);
                    return true;
                });
        }

        private void RemovePayout(PayoutDto payout) {
            if (payout == null)
                return;

            BusyExecute(async () => {
                await _payoutRepository.Delete(payout.Id);
                Payouts.Remove(payout);
            });
        }

        private void Filter() {
            if (TargetDate == DateTime.MinValue)
                return;

            BusyExecute(async () => {
                var payouts = await _payoutRepository.GetByKey(e => e.DateTime.Date == TargetDate.Date);
                Payouts = new ObservableCollection<PayoutDto>(payouts);
            });
        }
    }
}
