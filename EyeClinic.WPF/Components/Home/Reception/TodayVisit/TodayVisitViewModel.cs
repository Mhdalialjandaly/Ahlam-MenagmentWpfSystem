using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using EyeClinic.Core;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Dialogs.PasswordInput;
using EyeClinic.WPF.Components.PatientList.PatientFile;
using EyeClinic.WPF.Components.PatientList.PatientFile.FinishVisit;
using EyeClinic.WPF.Setup;
using Microsoft.EntityFrameworkCore;
using Unity;

namespace EyeClinic.WPF.Components.Home.Reception.TodayVisit
{
    public partial class TodayVisitViewModel
    {
        public TodayVisitViewModel() {
            if (IsDesignMode) { }
        }
    }

    public partial class TodayVisitViewModel : BaseViewModel<TodayVisitView>
    {
        private readonly IUnityContainer _container;
        private readonly IPatientVisitRepository _patientVisitRepository;

        public TodayVisitViewModel(IUnityContainer container, IPatientVisitRepository patientVisitRepository) {
            _container = container;
            _patientVisitRepository = patientVisitRepository;

            TargetDate = DateTime.Now.Date;
            FilterCommand = new RelayCommand(() => BusyExecute(async () => await Initialize()));
            GoPatientFileCommand = new RelayCommand<PatientVisitDto>(GoPatientFile);
            AddReviewCommand = new RelayCommand<PatientVisitDto>(AddReview);
            IsReception = Global.DeviceName.ToLower().Contains("reception");
            //IsAdmin = _container.CheckUserRole(UserRoles.Admin);
        }

        public override async Task Initialize() {
          

             var user = Global.GetValue(GlobalKeys.LoggedUser);
            if (user is Model.UserDto loggedInUser)
                LoggedInUser = loggedInUser.RoleId;
           
            if (LoggedInUser == 4)
            {
                var items = await _patientVisitRepository
                .GetByDate(TargetDate.Date);

                fakevalue = items.Count();
                int ff = Convert.ToInt32(Math.Round(fakevalue * 0.25));

                var FakeVal = await _patientVisitRepository
                .GetByFakeDate(TargetDate.Date,ff);                

               
                PatientVisits = new ObservableCollection<PatientVisitDto>(FakeVal);


                var payouts = await _container.Resolve<IPayoutRepository>()
                    .GetByKey(e => e.DateTime.Date == TargetDate.Date &&
                                   e.DeletedDate == null);

                TotalPayout = payouts.Sum(e => e.Amount);
            }
            else
            {
                var items = await _patientVisitRepository
                 .GetByDate(TargetDate.Date);
                PatientVisits = new ObservableCollection<PatientVisitDto>(items);

                var payouts = await _container.Resolve<IPayoutRepository>()
                    .GetByKey(e => e.DateTime.Date == TargetDate.Date &&
                                   e.DeletedDate == null);

                TotalPayout = payouts.Sum(e => e.Amount);
            }
        
            
        }

        public int LoggedInUser { get; set; }
        public ObservableCollection<PatientVisitDto> PatientVisits { get; set; }
        public int fakevalue { set; get; }
        public int fakevalue2 { set; get; }
        public int TotalIncome => PatientVisits.Sum(e => e.Payment) - TotalPayout;
        public int TotalIncomeWithout => PatientVisits.Sum(e => e.Payment);
        public int TotalCost => PatientVisits.Sum(e => e.Cost);
        public int TotalRemaining => PatientVisits.Sum(e => e.Remaining);
        public int TotalRemainingHAsPayed => PatientVisits.Sum(e => e.RemainPayValue);
        public int AllTotalRemainingHAsPayed => TotalIncome + TotalRemainingHAsPayed;
        public bool IfRemain { set; get; }
        public int TotalPayout { get; set; }
        public bool IsReception { get; set; }
        public bool IsAdmin { get; set; }

        public int TotalDiscount => PatientVisits.Sum(e => e.Cost) - TotalIncomeWithout - TotalRemaining;
        public string VisitType { set; get; }

        public DateTime TargetDate { get; set; }

        public ICommand FilterCommand { get; set; }
        public ICommand GoPatientFileCommand { get; set; }
        public ICommand AddReviewCommand { get; set; }


        private void GoPatientFile(PatientVisitDto patientIdObj) {
            var patientId = patientIdObj.PatientId;
            BusyExecute(async () => {
                var visits = await _patientVisitRepository.Get().AsNoTracking()
                    .Where(e => e.PatientId == patientId)
                    .ToListAsync();
                var lastVisit = visits.OrderByDescending(e => e.VisitDate)
                    .FirstOrDefault();

                if (lastVisit == null)
                    return;

                var patientFile = _container.Resolve<PatientFileViewModel>();
                await patientFile.Initialize(patientId, lastVisit.Id, true);
                _container.Resolve<IDialogService>()
                    .ShowFullScreenPopupContent(patientFile.GetView() as PatientFileView);
            });
        }

        private void AddReview(PatientVisitDto patientVisit) {
            var finishVisitControl = _container.Resolve<FinishVisitViewModel>();
            finishVisitControl.Initialize(patientVisit.Id);
            _container.Resolve<IDialogService>().ShowEditorDialog(finishVisitControl.GetView() as FinishVisitView,
                async () => {
                    if (await finishVisitControl.Save()) {
                        _container.Resolve<INotificationService>()
                            .Success("Review created");
                        return true;
                    }
                    return false;
                });
        }

    }
}
