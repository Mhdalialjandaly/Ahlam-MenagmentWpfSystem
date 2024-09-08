using System;
using System.Collections.Generic;

#nullable disable

namespace EyeClinic.DataAccess.Entities
{
    public partial class Patient : IEntityModel
    {
        public Patient() {
            PatientDiseases = new HashSet<PatientDisease>();
            PatientOperations = new HashSet<PatientOperation>();
            PatientVisits = new HashSet<PatientVisit>();
            Queues = new HashSet<Queue>();
        }

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
        public int GlassId { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public bool Referral { get; set; }
        public int? TempId { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        public virtual Glass Glass { get; set; }
        public virtual Location Location { get; set; }
        public virtual MartialStatus MartialStatus { get; set; }
        public virtual PatientGlass PatientGlass { get; set; }
        public virtual PatientSickStory PatientSickStory { get; set; }
        public virtual ICollection<PatientDisease> PatientDiseases { get; set; }
        public virtual ICollection<PatientOperation> PatientOperations { get; set; }
        public virtual ICollection<PatientVisit> PatientVisits { get; set; }
        public virtual ICollection<Queue> Queues { get; set; }
    }
}
