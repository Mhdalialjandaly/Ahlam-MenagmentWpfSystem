using System.Collections.Generic;
using EyeClinic.Core.Enums;

namespace EyeClinic.WPF.Components.Home.Reception.Queue
{
    public class QueueFilterType
    {
        public PatientVisitStatus? VisitStatus { get; set; }
        public string DisplayName { get; set; }

        public static List<QueueFilterType> GetAll() {
            return new List<QueueFilterType>()
            {   
                new() {DisplayName = "Started", VisitStatus = PatientVisitStatus.Started},
                new() {DisplayName = "Pending", VisitStatus = PatientVisitStatus.Created},
                new() {DisplayName = "Done", VisitStatus = PatientVisitStatus.Finished},
                new() {DisplayName = "All", VisitStatus = null},
            };
        }

        public override string ToString() => DisplayName;
    }
}
