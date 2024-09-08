using System;
using System.Threading.Tasks;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.Base;
using operation = EyeClinic.Core.Enums.Operation;

namespace EyeClinic.WPF.Components.Home.Reception.TodayPayout.TodayPayoutEditor
{
    public partial class TodayPayoutEditorViewModel
    {
        public TodayPayoutEditorViewModel() {
            if (IsDesignMode) { }
        }
    }

    public partial class TodayPayoutEditorViewModel : BaseViewModel<TodayPayoutEditorView>
    {
        private readonly IPayoutRepository _payoutRepository;

        public TodayPayoutEditorViewModel(IPayoutRepository payoutRepository) {
            _payoutRepository = payoutRepository;
        }


        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public int Amount { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedDate { get; set; }


        public PayoutDto BuildFromProperties() {
            return new PayoutDto() {
                Id = Id,
                CreatedDate = CreatedDate,
                Amount = Amount,
                DateTime = DateTime,
                Reason = Reason
            };
        }

        public void BuildFromModel(PayoutDto payout) {
            Id = payout.Id;
            Amount = payout.Amount;
            DateTime = payout.DateTime;
            Reason = payout.Reason;
            CreatedDate = payout.CreatedDate;
        }

        public async Task<PayoutDto> SaveAsync() {
            var payout = BuildFromProperties();

            if (Operation == operation.Add) {
                var payoutItem = await _payoutRepository.Add(payout);
                return payoutItem;
            }

            if (Operation == operation.Edit && Id > 0) {
                payout.Id = Id;
                payout.LastModifiedDate = DateTime.Now;
                await _payoutRepository.Update(payout);
                return payout;
            }

            return null;
        }
    }
}
