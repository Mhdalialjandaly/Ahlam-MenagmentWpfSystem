using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using Syncfusion.Data.Extensions;
using Syncfusion.Windows.Shared;
using Unity;

namespace EyeClinic.WPF.Components.Home.Reception.Review.Reservation
{
    public partial class ReservationViewModel
    {
        public ReservationViewModel() { }
    }

    public partial class ReservationViewModel : BaseViewModel<ReservationView>
    {
        private readonly IUnityContainer _container;
        private readonly IReservationRepository _reservationRepository;
        private readonly INotificationService _notificationService;
       

        public ReservationViewModel(IUnityContainer container, IReservationRepository reservationRepository,
                INotificationService notificationService)
        {
            _container = container;
            _reservationRepository = reservationRepository;
            _notificationService = notificationService;

            ReservationDate = DateTime.Now.Date;
            PatientVisitType = new List<string>()
            {
                "مستعجلة","اول مرة","خارجية","داخلية"
            };

            PatinetReviewTime = new List<string>()
            {
                 "2:00"
                ,"2:15"
                ,"2:30"
                ,"2:45"
                ,"3:00"
                ,"3:15"
                ,"3:30"
                ,"3:45"
                ,"4:00"
                ,"4:15"
                ,"4:30"
                ,"4:45"
                ,"5:00"
                ,"5:15"
                ,"5:30"
                ,"5:45"
                ,"6:00"
                ,"6:15"
                ,"6:30"
                ,"6:45"
                ,"7:00"
                ,"7:15"
                ,"7:30"
                ,"7:45"
                ,"8:00"
                ,"8:15"
                ,"8:30"
                ,"8:45"
                ,"9:00"
            };
        }
        public List<string> PatientVisitType { set; get; }
        public string SelectedPatientVisitType { set; get; }
        public List<string> PatinetReviewTime { set; get; }
        public string SelectedPatinetReviewTime { set; get; }
        public string PatientName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime ReservationDate { get; set; }

        public async Task<bool> Save()
        {
            if (ValidForSave())
            {
                var review = BuildFromProperties();
                await _reservationRepository.Add(review);
                return true;
            }

            return false;
        }

        private Model.ReservationDto BuildFromProperties()
        {
            return new()
            {
                ReservationTime = SelectedPatinetReviewTime,
                PatientName = PatientName,
                PhoneNumber = PhoneNumber,
                PatientVisitType = SelectedPatientVisitType,
                ReservationDate = ReservationDate,
                CreatedDate = DateTime.Now
            };
        }

        private bool ValidForSave()
        {
            if (SelectedPatinetReviewTime == null)
            {
                _notificationService.Warning("Please Add Times For This Review");
                return false;
            }
            if (string.IsNullOrWhiteSpace(PatientName))
            {
                _notificationService.Warning("Patient Name is required");
                return false;
            }
            if (string.IsNullOrWhiteSpace(PhoneNumber))
            {
                _notificationService.Warning("PhoneNumber is required");
                return false;
            }

            if (ReservationDate < DateTime.Now.Date)
            {
                _notificationService.Warning("Review Date must be bigger than today");
                return false;
            }

            return true;
        }

    }
}
