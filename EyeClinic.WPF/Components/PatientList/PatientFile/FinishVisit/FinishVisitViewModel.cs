using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Base.Extends;
using EyeClinic.WPF.Components.Shell.QueueWindow;
using Unity;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.FinishVisit
{
    public partial class FinishVisitViewModel
    {
        public FinishVisitViewModel() {
            if (IsDesignMode) { }
        }
    }

    public partial class FinishVisitViewModel : BaseViewModel<FinishVisitView>
    {
        private readonly IPatientVisitRepository _patientVisitRepository;

        public FinishVisitViewModel(IPatientVisitRepository patientVisitRepository) {
            _patientVisitRepository = patientVisitRepository;

            DayTimeTypes = new List<string>
            {
                "AM", "PM"
            };
        }

        public void Initialize(int patientVisitId) {
            PatientVisitId = patientVisitId;
        }

        public int PatientVisitId { get; set; }

        private DateTime? _reviewDate;
        public DateTime? ReviewDate {
            get => _reviewDate;
            set {
                _reviewDate = value;
                if (value != null) {
                    EnableTime = true;
                    if (Hour == 0)
                        Hour = 9;

                    SelectedDayTimeType ??= DayTimeTypes.FirstOrDefault();
                }
            }
        }


        public bool EnableTime { get; set; }

        public int Hour { get; set; }
        public int Minute { get; set; }
        public List<string> DayTimeTypes { get; set; }
        public string SelectedDayTimeType { get; set; }

        public async Task<bool> Save() {
            DateTime? reviewDate = null;
            if (ReviewDate != null) {
                reviewDate = ReviewDate.Value;
                reviewDate = new DateTime(reviewDate.Value.Year, reviewDate.Value.Month, reviewDate.Value.Day,
                    Hour == 0 ? DateTime.Now.Hour : Hour + (SelectedDayTimeType == "PM" ? 12 : 0),
                    Minute, 0);
            }

            await _patientVisitRepository.FinishPatientVisit(PatientVisitId, reviewDate);
            await ContainerHandler.Container.Resolve<QueueWindowViewModel>().Initialize();
            return true;
        }
    }
}
