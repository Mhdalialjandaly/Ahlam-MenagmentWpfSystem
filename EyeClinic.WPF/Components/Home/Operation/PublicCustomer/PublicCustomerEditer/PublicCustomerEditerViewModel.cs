using EyeClinic.Core.Common;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Home.Setting.Locations.LocationEditor;
using EyeClinic.WPF.Components.Home.Setting.MartialStatus.MartialStatusEditor;
using EyeClinic.WPF.Components.PatientList.PatientFile.NewGlass;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unity;
using operation = EyeClinic.Core.Enums.Operation;

namespace EyeClinic.WPF.Components.Home.Operation.PublicCustomer.PublicCustomerEditer
{
    public partial class PublicCustomerEditerViewModel 
    {
        public PublicCustomerEditerViewModel() { }
      
    }
    public partial class PublicCustomerEditerViewModel : BaseViewModel<PublicCustomerEditerView>
    { 
        private readonly IDialogService _dialogService;
        public readonly INotificationService _notificationService;
        public readonly ILocationRepository _locationRepository;
        public readonly IUnityContainer _UnityContainer;
        public readonly IPublicCustomerRepository _publicCustomerRepository;
        public PublicCustomerEditerViewModel(INotificationService notificationService
            ,IDialogService dialogService
            ,ILocationRepository locationRepository
             ,IUnityContainer unityContainer,IPublicCustomerRepository publicCustomerRepository)
        {
            _notificationService = notificationService;
            _locationRepository = locationRepository;
            _UnityContainer = unityContainer;
            _publicCustomerRepository = publicCustomerRepository;
            _dialogService = dialogService;
        }

        public override async Task Initialize()
        {
            Locations = new ObservableCollection<LocationDto>(
                await _locationRepository.GetAll());
        }

        public int Id { get; set; }
        public int Number { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string Type { get; set; }
        public string JobTitle { get; set; } 
        public string HomePhone { get; set; }
        public string WorkPhone { get; set; }
        public string Nationality { get; set; }
        public bool Referral { get; set; }

        public ObservableCollection<LocationDto> Locations { get; set; }
        public LocationDto SelectedLocation { get; set; }
      
        public string Notes { get; set; }
        public DateTime CreatedDate { get; set; }

        public ICommand GetPatientGlassCommand { get; set; }
        public ICommand AddDiseaseCommand { get; set; }
        public ICommand DeletePatientDiseaseCommand { get; set; }
        public ICommand AddMartialStatusCommand { get; set; }
        public ICommand AddLocationCommand { get; set; }

       

        public async Task<PublicCustomerDto> SaveAsync()
        {
            if (!ValidForSave())
                return null;

            var publiccustomer = BuildFromProperties();          
            if (Operation == operation.Add)
            {
                publiccustomer.CreatedDate = DateTime.Now;
                publiccustomer.LastModifiedDate = DateTime.Now;
                var item = await _publicCustomerRepository.Add(publiccustomer);
                return item;
            }

            publiccustomer.Id = Id;
            publiccustomer.LastModifiedDate = DateTime.Now;
            await _publicCustomerRepository.Update(publiccustomer);
            return publiccustomer;
        }

        private bool ValidForSave()
        {
            if (Number == 0)
            {
                _notificationService.Warning("الرجاء اعطاء رقم");
                return false;
            }
            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
            {
                _notificationService.Warning("الرجاء ادخال الاسم الاول والثاني");
                return false;
            }
            //if (SelectedGender == null) {
            //    _notificationService.Warning("Gender is required");
            //    return false;
            //}
            //if (SelectedAgeType == null) {
            //    _notificationService.Warning("Age type is required");
            //    return false;
            //}
            //if (SelectedMartialStatus == null) {
            //    _notificationService.Warning("Martial Status is required");
            //    return false;
            //}
            if (SelectedLocation == null)
            {
                _notificationService.Warning("الموقع غير محدد");
                return false;
            }
            if (string.IsNullOrWhiteSpace(HomePhone) && string.IsNullOrWhiteSpace(WorkPhone))
            {
                _notificationService.Warning("Home phone or Work phone are required");
                return false;
            }
            //if (SelectedGender.Name=="ذكر" && PregnantMonth !=0)
            //{
            //    PregnantMonth = 0;
            //    _notificationService.Warning("The Patient Male Cant Be Pregnant !");
            //    return false;
            //}
            //if (SelectedGender.Name == "انثى" && PregnantMonth > 9 && Age <= 50)
            //{             
            //    _notificationService.Warning("The Patient Female Pregnant Month Cant Be More Than 9 Months !");
            //    return false;
            //}
            return true;
        }

        private PublicCustomerDto BuildFromProperties()
        {

            var patient = new PublicCustomerDto
            {
                FirstName = FirstName,
                LastName = LastName,
                CompanyName = CompanyName,
                Type = Type,
                HomePhone = HomePhone,
                WorkPhone = WorkPhone,
                LocationId = SelectedLocation.Id,
                Notes = Notes,
                Nationality = Nationality,
                Referral = Referral,
                Number = Number,
                CreatedDate = CreatedDate
            };

            return patient;
        }

        public void BuildFromModel(PublicCustomerDto patient)
        {
            Id = patient.Id;
            FirstName = patient.FirstName;
            LastName = patient.LastName;
            CompanyName = patient.CompanyName;
            Type = patient.Type;
            HomePhone = patient.HomePhone;
            WorkPhone = patient.WorkPhone;
            SelectedLocation = Locations.FirstOrDefault(e => e.Id == patient.LocationId);
            Notes = patient.Notes;
            Nationality = patient.Nationality;
            Referral = patient.Referral;
            Number = patient.Number;
            CreatedDate = patient.CreatedDate;
        }
       
        private void AddLocation()
        {
            BusyExecute(async () =>
            {
                var locationEditor = _UnityContainer.Resolve<LocationEditorViewModel>();
                await locationEditor.Initialize();

                _dialogService.ShowEditorDialog(locationEditor.GetView() as LocationEditorView, async () =>
                {
                    var item = await locationEditor.Save();
                    if (item == null)
                        return false;

                    Locations ??= new ObservableCollection<LocationDto>();
                    Locations.Add(item);
                    return true;
                });
            });
        }
    }
}
