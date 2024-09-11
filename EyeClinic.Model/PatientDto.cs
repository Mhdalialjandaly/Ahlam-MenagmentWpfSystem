using System;
using System.Collections.Generic;
using System.Linq;

namespace EyeClinic.Model
{
    public class PatientDto
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public int Age { get; set; }
        public string AgeTypeName { get; set; }
        public int PregnantMonth { get; set; }
        public DateTime? StartPregnantDate { get; set; }
        public DateTime? EndPregnantDate { get; set; }
        public string JobTitle { get; set; }
        public bool Gender { get; set; }
        public int MartialStatusId { get; set; }
        public string HomePhone { get; set; }
        public string WorkPhone { get; set; }
        public string Nationality { get; set; }
        public int LocationId { get; set; }
        public bool IsSmoking { get; set; }
        public bool IsDrinking { get; set; }
        public bool IsDrawing { get; set; }
        public bool Referral { get; set; }
        public int GlassId { get; set; }
        public string Notes { get; set; }
        public string Why { get; set; }


        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }


        public string HighLightDiseases => string.Join(" ", PatientDiseases
            .Where(e => e.DiseaseId == 1)
            .Select(e => e.Disease.EnName));

        public string HighLightDiseases2 => string.Join(" ", PatientDiseases
           .Where(e => e.DiseaseId == 2)
           .Select(e => e.Disease.ArName));

        public string HighLightDiseases3 => string.Join(" ", PatientDiseases
          .Where(e => e.DiseaseId == 13)
          .Select(e => e.Disease.EnName));

        public string ReferralPatient => Referral == true ? "(زبون داخلي)" : " (زبون خارجي)" ;

        public string IOPPatient => IsDrawing == true ? "(زبون خارجي)" : "" ;
    

        public int Remaining => PatientVisits?
                                    .Where(e => e.DeletedDate == null && e.DeletedDate == null)
                                    .Sum(x => x.Remaining) ?? 0 +
                                PatientOperations?
                                    .Where(e=>e.MedicalCenterReserved || e.IsFinish == true)
                                    .Sum(e => e.Remaining) ?? 0;
        public bool IfRemaining { get; set; }
        public string FullName => $"{FirstName} - {LastName} - {FatherName} {MotherName}";
        public string GenderDisplayName => Gender ? "Male" : "Female";
        public string AgeDisplayName => $"{Age} {AgeTypeName}";
        public string LastModifiedDateDisplayName {
            get {
                var lastDate = LastModifiedDate ?? CreatedDate;
                if ((DateTime.Now.Date - lastDate.Date).TotalDays > 1)
                    return $"Modified : {lastDate:dd/MM/yyyy}";

                return null;
            }
        }


        public GlassDto Glass { get; set; }
        public LocationDto Location { get; set; }
        public MartialStatusDto MartialStatus { get; set; }
        public PatientGlassDto PatientGlass { get; set; }
        public PatientSickStoryDto PatientSickStory { get; set; }
        public List<PatientDisease> PatientDiseases { get; set; }
        public List<PatientVisitDto> PatientVisits { get; set; }
        public List<PatientOperationDto> PatientOperations { get; set; }
    }
}
