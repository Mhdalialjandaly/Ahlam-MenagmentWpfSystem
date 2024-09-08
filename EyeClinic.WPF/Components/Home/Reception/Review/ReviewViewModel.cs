using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Home.Reception.Review.Reservation;
using Unity;

namespace EyeClinic.WPF.Components.Home.Reception.Review
{
    public partial class ReviewViewModel
    {
        public ReviewViewModel() {
            if (IsDesignMode) { }
        }
    }

    public partial class ReviewViewModel : BaseViewModel<ReviewView>
    {
        private readonly IUnityContainer _container;
        private readonly IReservationRepository _reservationRepository;
        private readonly IPatientVisitRepository _patientVisitRepository;

        public ReviewViewModel(IUnityContainer container, IReservationRepository reservationRepository,
            IPatientVisitRepository patientVisitRepository) {
            _container = container;
            _reservationRepository = reservationRepository;
            _patientVisitRepository = patientVisitRepository;

            TargetDate = DateTime.Now.Date;
            FilterCommand = new RelayCommand(Filter);
            ReserveCommand = new RelayCommand(Reserve);

            RemoveCommand = new RelayCommand(Remove);

        }

        private void Remove() {
            if (MessageBox.Show("Do You Want To Delete ??", "Conform", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                BusyExecute(async () =>
                {
                    if (SelectedReview.ReservationId.HasValue)
                    { // Delete reservation
                        await _reservationRepository.Delete(e => e.Id == SelectedReview.ReservationId.Value);
                        var index = Reviews.IndexOf(SelectedReview);
                        if (index > -1)
                            Reviews.RemoveAt(index);
                    }
                    else
                    {
                        _container.Resolve<INotificationService>().Error("You can't delete visit");
                    }
                });
            }
        }



        public override async Task Initialize() {
            var reviews = (await _patientVisitRepository
                .GetReviews(TargetDate.Date))              
                .Select(e => new ReviewDataModel() {
                    PatientVisitId = e.Id,
                    ReservationId = null,
                    DateTime = e.ReviewDate.Value,
                    PatientName = e.Patient.FullName,
                    Time = e.ReviewDate?.ToString("hh:mm tt"),
                    PhoneNumber = e.Patient.HomePhone,
                    PatientVisitType = e.PatientVisitType,
                    Type = "Review"
                }).OrderBy(e => e.Time)
                .ToList();

            if (PatientNameReview == "" || PatientNameReview == null) {
                var reservation = (await _reservationRepository
                .GetByKey(e => e.ReservationDate.Date == TargetDate.Date))
                .Select(e => new ReviewDataModel() {
                    ReservationId = e.Id,
                    PatientVisitId = null,
                    DateTime = e.ReservationDate,
                    PatientName = e.PatientName,
                    Time = e.ReservationTime,
                    PhoneNumber = e.PhoneNumber,
                    PatientVisitType = e.PatientVisitType,
                    Type = "Reservation"
                }).OrderBy(e=>e.Time)
                .ToList();

                reviews.AddRange(reservation);
                Reviews = new ObservableCollection<ReviewDataModel>(reviews);
            } 
            else {
                var reservation = (await _reservationRepository
                .GetByKey(e => e.PatientName == PatientNameReview))
                .Select(e => new ReviewDataModel() {
                    DateTime = e.ReservationDate,
                    PatientName = e.PatientName,
                    Time = e.ReservationTime,
                    PhoneNumber = e.PhoneNumber,
                    PatientVisitType = e.PatientVisitType,
                    Type = "Reservation"
                })
                .ToList();

                reviews.AddRange(reservation);
                Reviews = new ObservableCollection<ReviewDataModel>(reviews);
            }



        }

        public ObservableCollection<ReviewDataModel> Reviews { get; set; }
        public ReviewDataModel SelectedReview { get; set; }

        public DateTime TargetDate { get; set; }
        public string PatientNameReview { set; get; }
        public ICommand GetByNameCommand { set; get; }
        public ICommand FilterCommand { get; set; }
        public ICommand ReserveCommand { get; set; }
        public ICommand RemoveCommand { set; get; }

        private void Reserve() {
            var reservationEditor = _container.Resolve<ReservationViewModel>();
            _container.Resolve<IDialogService>()
                .ShowEditorDialog(reservationEditor.GetView() as ReservationView, async () => {
                    if (await reservationEditor.Save()) {
                        FilterCommand?.Execute(null);
                        return true;
                    }

                    return false;
                });


        }
        private void Filter() {
            BusyExecute(async () => await Initialize());
        }
    }
}
