using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.DataAccess.Repositories;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.Print;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Base.Extends;
using EyeClinic.WPF.Components.PatientList.PatientFile;
using EyeClinic.WPF.Setup;
using Syncfusion.Windows.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unity;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EyeClinic.WPF.Components.PatientList.PatientVisitList.Why
{
    public partial class VisitWhyViewModel
    {
        public VisitWhyViewModel()
        {
            if (IsDesignMode) { }
        }
        public Action<object, object> OnSave { get; internal set; }


    }

    public partial class VisitWhyViewModel : BaseViewModel<VisitWhyView>
    {

        private readonly IUnityContainer _container;
        private readonly IPatientVisitRepository _patientVisitRepository;
        private readonly IPrintService _printService;

        public VisitWhyViewModel(IUnityContainer container, IPatientVisitRepository patientVisitRepository,
                IPrintService printService)
        {
            _container = container;
            _patientVisitRepository = patientVisitRepository;
            _printService = printService;

            PrintWhyCommand = new RelayCommand(PrintWhy);
            //SavebtnCommand = new RelayCommand(Save1);

        }



        public async void Initialize(int patientVisitId, string why)
        {
            PatientVisitId = patientVisitId;
            Why = why;
            BaseWhy = why;
            PatientVisitId = patientVisitId;;
            VisitDate = await _patientVisitRepository
              .GetVisitDateById(PatientVisitId);
        }
        
        public int PatientVisitId { get; set; }
        public DateTime? VisitDate { get; set; }

        public string Why { get; set; }
        private string BaseWhy { get; set; }
        public byte[] pdf { get; set; }

        public ICommand PrintWhyCommand { get; set; }
        public ICommand CancelCommand => new RelayCommand(() => {
            ContainerHandler.Container.Resolve<IDialogService>().DisposeLastDialog();
        });
        public ICommand BackCommand => new RelayCommand(() => {
            ContainerHandler.Container.Resolve<IDialogService>().DisposeLastDialog();
        });
        public ICommand SavebtnCommand { get; set; }

        public async Task<bool> Save()
        {
            if (IsBusy)
                return false;

            try
            { 
                BusyStatus = "Please wait";
                IsBusy = true;                
                if (BaseWhy != Why)
                    await _patientVisitRepository.UpdateVisitWhyNote(PatientVisitId, Why);
                 OnSave?.Invoke(this, Why);
                return true;
            }
            catch (Exception ex)
            {
                LogError(ex.Message, ex);
                return false;
            }
            finally
            {
                IsBusy = false;
            }
            
        }
        //public void Save1()
        //{
        //    BusyExecute(async () => {
        //        await _patientVisitRepository.UpdateVisitWhyNote(PatientVisitId, Why);               
        //        OnSave?.Invoke(this, Why);
                
        //    });
           
        //}

        public void PrintWhy()
        {
            BusyExecute(async () => {
                var patient = await _container.Resolve<IPatientRepository>()
                    .GetByPatientVisitId(PatientVisitId);
              
            });
        }

        public async Task<bool> SaveCustomWhy()
        {
            try
            {
                BusyStatus = "Please wait";
                IsBusy = true;
                if (BaseWhy != Why)
                    await _patientVisitRepository.UpdateVisitMedicalReport(PatientVisitId, Why );
                return true;
            }
            catch (Exception ex)
            {
                LogError(ex.Message, ex);
                return false;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
