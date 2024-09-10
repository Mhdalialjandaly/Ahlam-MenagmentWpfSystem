using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EyeClinic.Core.Common;
using EyeClinic.Model;

namespace EyeClinic.WPF.Components.PatientList.PatientEditor
{
    public partial class PatientEditorViewModel
    {
        public int Id { get; set; }
        public int Number { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }

        public List<GenderType> GenderTypes { get; set; }
        public GenderType SelectedGender { get; set; }

        public int Age { get; set; }
        public List<AgeTypeDto> AgeTypes { get; set; }
        public AgeTypeDto SelectedAgeType { get; set; }

        public int PregnantMonth { get; set; }
        public DateTime? StartPregnantDate { get; set; }
        public DateTime? EndPregnantDate { get; set; }

        public string JobTitle { get; set; }
        public ObservableCollection<MartialStatusDto> MartialStatusList { get; set; }
        public MartialStatusDto SelectedMartialStatus { get; set; }
        public string HomePhone { get; set; }
        public string WorkPhone { get; set; }
        public string Nationality { get; set; }
        public bool Referral { get; set; }

        public ObservableCollection<LocationDto> Locations { get; set; }
        public LocationDto SelectedLocation { get; set; }

        public List<GlassDto> Glasses { get; set; }
        public GlassDto SelectedGlass { get; set; }
        public bool isoperationDepartment { get; set; }
        public bool IsSmoking { get; set; }
        public bool IsDrinking { get; set; }
        public bool IsDrawing { get; set; }

        public List<DiseaseDto> Diseases { get; set; }
        public DiseaseDto SelectedDisease { get; set; }
        public string AgeOfInjury { get; set; }
        public string MaxMeasure { get; set; }
        public string LastMeasure { get; set; }

        public ObservableCollection<PatientDisease> PatientDiseases { get; set; }
        public PatientGlassDto PatientGlass { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
