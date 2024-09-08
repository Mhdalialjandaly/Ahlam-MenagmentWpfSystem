using EyeClinic.DataAccess.IRepositories;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Home.Reports.ClinicIncomeReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EyeClinic.WPF.Components.Reports.NumberOfGlassesPrescribed
{
  
        public class NumberOfGlassesPrescribedViewModel : BaseViewModel<NumberOfGlassesPrescribedView>
        {
            private readonly IPatientVisitGlassRepository _patientVisitGlassRepository;

            public NumberOfGlassesPrescribedViewModel(IPatientVisitGlassRepository patientVisitGlassRepository)
            {
                _patientVisitGlassRepository = patientVisitGlassRepository;

                FromDate = DateTime.Now.Date;
                ToDate = DateTime.Now.Date;
            
              

                GetReportCommand = new RelayCommand(GetReport);
            }


            public DateTime FromDate { get; set; }
            public DateTime ToDate { get; set; }

            public int TotalPatient { get; set; }
            public int TotalGlasses { get; set; }

            public ICommand GetReportCommand { get; set; }

            public void GetReport()
            {
                Task.Run(() => {
                    BusyExecute(async () => {
                        var report = await _patientVisitGlassRepository
                            .GetByDate(FromDate.Date,ToDate.Date);

                    });
                });
            }
        }
    }

