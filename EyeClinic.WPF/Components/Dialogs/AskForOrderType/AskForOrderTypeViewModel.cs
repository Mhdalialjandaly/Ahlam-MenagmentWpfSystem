using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.Localization;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Dialogs.AskForCost;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace EyeClinic.WPF.Components.Dialogs.AskForOrderType
{
  
        public partial class AskForOrderTypeViewModel
        {
            public AskForOrderTypeViewModel()
            {
                if (IsDesignMode) { }
            }
        }

        public partial class AskForOrderTypeViewModel : BaseViewModel<AskForOrderTypeView>
        {
            private readonly IUnityContainer _container;
            private readonly IPublicCustomerOrderRepository _publicCustomerOrder;
            private readonly INotPayReasonRepository _notPayReasonRepository;
            private readonly IVisitTypeRepository _visitTypeRepository;
            private readonly INotificationService _notificationService;

            public AskForOrderTypeViewModel(IUnityContainer container, IPublicCustomerOrderRepository publicCustomerOrder,
                INotPayReasonRepository notPayReasonRepository, IVisitTypeRepository visitTypeRepository,
                    INotificationService notificationService)
            {
                _container = container;
                _publicCustomerOrder = publicCustomerOrder;
                _notPayReasonRepository = notPayReasonRepository;
                _visitTypeRepository = visitTypeRepository;
                _notificationService = notificationService;
            }

            public async Task Initialize(PublicCustomerDto publicCustomer)
            {
                GetView();

                OrderDate = DateTime.Now.Date;
                NotPayReasons = await _notPayReasonRepository.GetAll();
                VisitTypes = await _visitTypeRepository.GetAll();
            }

            public int Id { get; set; }
            public int Cost { get; set; }
            public DateTime OrderDate { get; set; }
            public int Payment { get; set; }
            public string Note { get; set; }
            public DateTime? UpComingDate { get; set; }
            public DateTime CreatedDate { get; set; }

            public List<NotPayReasonDto> NotPayReasons { get; set; }
            public NotPayReasonDto SelectedNotPayReason { get; set; }
            public List<VisitTypeDto> VisitTypes { get; set; }

            private VisitTypeDto _selectedVisitType;
            public VisitTypeDto SelectedVisitType
            {
                get => _selectedVisitType;
                set
                {
                    _selectedVisitType = value;
                    Cost = SelectedVisitType.Cost + (IncludeRetinalImageService ? 75000 : 0);
                }
            }


            private bool _includeRetinalImageService;

            public bool IncludeRetinalImageService
            {
                get => _includeRetinalImageService;
                set
                {
                    _includeRetinalImageService = value;
                    if (value)
                        Cost += 30000;
                    else
                        Cost -= 30000;
                }
            }


            public async Task<PublicCustomerOrderDto> Save(int publicCustomerid)
            {
                try
                {
                    if (!await IsValidForSave(publicCustomerid))
                        return null;

                    var publicCustomerOrder = BuildFromProperties(publicCustomerid);

                    var order = await _publicCustomerOrder.Add(publicCustomerOrder);
                 
                    return order;
                }
                catch (Exception ex)
                {
                    _container.Resolve<INotificationService>()
                        .Error("This Oreder already in the Queue");
                    LogError(ex.Message, ex);
                    return null;
                }
            }

            private PublicCustomerOrderDto BuildFromProperties(int publiccustomerid)
            {
                var orderDate = OrderDate != DateTime.MinValue ? OrderDate : DateTime.Now;
                return new()
                {

                    PublicCustomerOrderType = SelectedVisitType.ArName,
                    PublicCustomerId = publiccustomerid,
                    OrderDate = orderDate,
                    Notes = Note,
                    UpComingDate = UpComingDate,
                    CreatedDate = CreatedDate,
                };
            }

            private async Task<bool> IsValidForSave(int publiccustomerid)
            {
                var exist = await _publicCustomerOrder.Get().AsNoTracking()
                    .FirstOrDefaultAsync(e => e.PublicCustomerId == publiccustomerid &&
                                              OrderDate == e.OrderDate &&
                                              e.DeletedDate == null);
                if (exist != null)
                {
                    _notificationService.Warning(
                        _container.Resolve<ILocalizationService>()
                                .Localize("OrderAlreadyExistInQueue"));
                    return false;
                }

                if (SelectedVisitType == null)
                {
                    _notificationService.Warning(
                        _container.Resolve<ILocalizationService>()
                            .Localize("InvalidOrderType"));
                    return false;
                }

                if (Payment < 0)
                {
                    _notificationService.Warning(
                        _container.Resolve<ILocalizationService>()
                            .Localize("PaymentMustBeBiggerThanZero"));
                    return false;
                }
                if (Payment > Cost)
                {
                    _notificationService.Warning(
                        _container.Resolve<ILocalizationService>()
                            .Localize("PaymentMustBeLessOrEqualToCost"));
                    return false;
                }

                if (Payment < Cost && SelectedNotPayReason == null)
                {
                    _notificationService.Warning(
                        _container.Resolve<ILocalizationService>()
                            .Localize("YouHaveToChooseTheReasonOfNotPayment"));
                    return false;
                }

                return true;
            }
        }
    
}
