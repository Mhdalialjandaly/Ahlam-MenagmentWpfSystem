using EyeClinic.Model;
using EyeClinic.WPF.Base;

namespace EyeClinic.WPF.DataModel
{
    public class QueueItem : Bindable
    {
        public PatientVisitDto PatientVisit { get; set; }
        public int QueueIndex { get; set; }
        public string LastPatientName{get;set;}
        public string WaitingTimeh { get; set; }
        public string WaitingTimem { get; set; }
        public string WaitingTimed { get; set; }
    }
}
