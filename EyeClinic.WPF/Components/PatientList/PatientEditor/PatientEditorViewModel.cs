using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using EyeClinic.Core.Common;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Home.Setting.Locations.LocationEditor;
using EyeClinic.WPF.Components.Home.Setting.MartialStatus.MartialStatusEditor;
using EyeClinic.WPF.Components.PatientList.PatientFile.NewGlass;
using Unity;
using Operation = EyeClinic.Core.Enums.Operation;

namespace EyeClinic.WPF.Components.PatientList.PatientEditor
{
    public partial class PatientEditorViewModel
    {
        public PatientEditorViewModel() {
            if (IsDesignMode) { }
        }
    }

    public partial class PatientEditorViewModel : BaseViewModel<PatientEditorView>
    {
        private readonly IUnityContainer _container;
        private readonly IPatientRepository _patientRepository;
        private readonly IMartialStatusRepository _martialStatusRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IDialogService _dialogService;
        private readonly INotificationService _notificationService;
        private readonly IGlassRepository _glassRepository;
        private readonly IDiseasesRepository _diseasesRepository;

        public PatientEditorViewModel(IUnityContainer container, IPatientRepository patientRepository,
            IMartialStatusRepository martialStatusRepository, ILocationRepository locationRepository,
            IDialogService dialogService, INotificationService notificationService,
            IGlassRepository glassRepository, IDiseasesRepository diseasesRepository) {
            _container = container;
            _patientRepository = patientRepository;
            _martialStatusRepository = martialStatusRepository;
            _locationRepository = locationRepository;
            _dialogService = dialogService;
            _notificationService = notificationService;
            _glassRepository = glassRepository;
            _diseasesRepository = diseasesRepository;

            GetPatientGlassCommand = new RelayCommand(GetPatientGlass);
            AddDiseaseCommand = new RelayCommand(AddDisease);
            DeletePatientDiseaseCommand = new RelayCommand<PatientDisease>(DeletePatientDisease);
            AddMartialStatusCommand = new RelayCommand(AddMartialStatus);
            AddLocationCommand = new RelayCommand(AddLocation);
        }

        public override async Task Initialize() {
            GenderTypes = GenderType.Create();
            AgeTypes = AgeTypeDto.Create();
            MartialStatusList = new ObservableCollection<MartialStatusDto>(
                await _martialStatusRepository.GetAll());
            Locations = new ObservableCollection<LocationDto>(
                await _locationRepository.GetAll());
            Glasses = await _glassRepository.GetAll();
            Diseases = await _diseasesRepository.GetAll();
        }

        public event EventHandler<PatientGlassDto> OnPatientGlassChanged;

        public ICommand GetPatientGlassCommand { get; set; }
        public ICommand AddDiseaseCommand { get; set; }
        public ICommand DeletePatientDiseaseCommand { get; set; }
        public ICommand AddMartialStatusCommand { get; set; }
        public ICommand AddLocationCommand { get; set; }


        public void GetPatientGlass() {
            BusyExecute(async () => {
                var glassViewModel = _container.Resolve<PatientGlassViewModel>();
                await glassViewModel.Initialize(Id);
                glassViewModel.OnSavePatientGlass += (_, patientGlass) => {
                    PatientGlass = patientGlass;
                    OnPatientGlassChanged?.Invoke(this, patientGlass);
                };

                _dialogService.ShowEditorDialog(glassViewModel.GetView() as NewGlassView, () => {
                    glassViewModel.SaveNewGlassCommand.Execute(null);
                    return Task.FromResult(true);
                });
            });
        }

        public async Task<PatientDto> SaveAsync(bool isoperation) {
            if (!ValidForSave())
                return null;

            var patient = BuildFromProperties(isoperation);
            patient.PatientGlass = null;
            if (Operation == Operation.Add) {
                patient.CreatedDate = DateTime.Now;
                patient.LastModifiedDate = DateTime.Now;
                var item = await _patientRepository.Add(patient);
                return item;
            }

            patient.Id = Id;
            patient.LastModifiedDate = DateTime.Now;
            await _patientRepository.Update(patient);
            return patient;
        }

        private bool ValidForSave() {
            if (Number==0) 
            {
                _notificationService.Warning("the code is required more than 0");
                return false;
            }
            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName)) {
                _notificationService.Warning("First and last name is required");
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
            if (SelectedLocation == null) {
                _notificationService.Warning("Location is required");
                return false;
            }
            if (string.IsNullOrWhiteSpace(HomePhone) && string.IsNullOrWhiteSpace(WorkPhone)) {
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

        private PatientDto BuildFromProperties(bool isoperation) {
           
                var patient = new PatientDto
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    MotherName = MotherName,
                    FatherName = FatherName,
                    Age = 0,                  
                    PregnantMonth = 0,
                    AgeTypeName = "يوم",
                    Gender = true,
                    GlassId = SelectedGlass?.Id ?? 1,
                    HomePhone = HomePhone,
                    WorkPhone = WorkPhone,
                    IsDrawing = IsDrawing,
                    IsDrinking = IsDrinking,
                    IsSmoking = IsSmoking,
                    JobTitle = JobTitle,
                    LocationId = SelectedLocation.Id,
                    Notes = Notes,
                    Nationality = Nationality,
                    Referral = Referral,
                    MartialStatusId = 1,
                    PatientDiseases = PatientDiseases == null ? new List<PatientDisease>() : PatientDiseases.ToList(),
                    PatientGlass = PatientGlass,
                    Number = Number,
                    CreatedDate = CreatedDate,
                    IfRemaining = isoperation
                };

                return patient;
           
           
        }

        public void BuildFromModel(PatientDto patient) {
            Id = patient.Id;
            FirstName = patient.FirstName;
            LastName = patient.LastName;
            MotherName = patient.MotherName;
            FatherName = patient.FatherName;
            Age = patient.Age;
                StartPregnantDate = null;
                EndPregnantDate = null;
                PregnantMonth = 0;
            SelectedAgeType = AgeTypes.FirstOrDefault(e => e.EnName == patient.AgeTypeName);
            SelectedGender = GenderTypes.FirstOrDefault(e => e.Type == patient.Gender);
            SelectedGlass = Glasses.FirstOrDefault(e => e.Id == patient.GlassId);
            HomePhone = patient.HomePhone;
            WorkPhone = patient.WorkPhone;
            IsDrawing = patient.IsDrawing;
            IsDrinking = patient.IsDrinking;
            IsSmoking = patient.IsSmoking;
            JobTitle = patient.JobTitle;
            SelectedLocation = Locations.FirstOrDefault(e => e.Id == patient.LocationId);
            Notes = patient.Remaining.ToString();
            Nationality = patient.Nationality;
            Referral = patient.Referral;
            SelectedMartialStatus = MartialStatusList.FirstOrDefault(e => e.Id == patient.MartialStatusId);
            PatientDiseases = new ObservableCollection<PatientDisease>(patient.PatientDiseases);
            PatientGlass = patient.PatientGlass;
            Number = patient.Number;
            CreatedDate = patient.CreatedDate;
            isoperationDepartment = patient.IfRemaining;
        }

        private void AddDisease() {
            if (SelectedDisease == null ||
               string.IsNullOrWhiteSpace(AgeOfInjury) ||
               string.IsNullOrWhiteSpace(MaxMeasure) ||
               string.IsNullOrWhiteSpace(LastMeasure))
                return;

            PatientDiseases ??= new ObservableCollection<PatientDisease>();
            if (PatientDiseases.Any(e => e.DiseaseId == SelectedDisease.Id))
                return;

            PatientDiseases.Add(new PatientDisease() {
                AgeOfInjury = AgeOfInjury,
                MaxMeasure = MaxMeasure,
                DiseaseId = SelectedDisease.Id,
                Disease = SelectedDisease,
                LastCheckMeasure = LastMeasure,
                PatientId = Id,
                CreatedDate = DateTime.Now
            });

            SelectedDisease = null;
            AgeOfInjury = string.Empty;
            MaxMeasure = string.Empty;
            LastMeasure = string.Empty;
        }

        private void DeletePatientDisease(PatientDisease patientDisease) {
            PatientDiseases.Remove(patientDisease);
        }

        private void AddMartialStatus() {
            BusyExecute(async () => {
                var martialEditor = _container.Resolve<MartialStatusEditorViewModel>();
                await martialEditor.Initialize();

                _dialogService.ShowEditorDialog(martialEditor.GetView() as MartialStatusEditorView, async () => {
                    var item = await martialEditor.Save();
                    if (item == null)
                        return false;

                    MartialStatusList ??= new ObservableCollection<MartialStatusDto>();
                    MartialStatusList.Add(item);
                    return true;
                });
            });
        }

        private void AddLocation() {
            BusyExecute(async () => {
                var locationEditor = _container.Resolve<LocationEditorViewModel>();
                await locationEditor.Initialize();

                _dialogService.ShowEditorDialog(locationEditor.GetView() as LocationEditorView, async () => {
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
