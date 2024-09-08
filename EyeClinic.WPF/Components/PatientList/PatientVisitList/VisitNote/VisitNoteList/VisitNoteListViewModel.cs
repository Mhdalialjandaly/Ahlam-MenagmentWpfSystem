using System;
using System.Collections.Generic;
using System.Windows.Input;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Base.Extends;
using Unity;

namespace EyeClinic.WPF.Components.PatientList.PatientVisitList.VisitNote.VisitNoteList
{
    public partial class VisitNoteListViewModel
    {
        public VisitNoteListViewModel() {
            if (IsDesignMode) {
                PatientVisits = new List<PatientVisitDto>
                {
                    new() {VisitDate = DateTime.Now.Date, Notes = "Test note for design mode"},
                    new() {VisitDate = DateTime.Now.Date, Notes = "Very long test note for design mode in order to check if the client type long long long text in this note of visit so you have to be able to handle this long text with your design so please, check if this is good and handle multi line and this is new line for check \n this is the start \n this is the start"},
                    new() {VisitDate = DateTime.Now.Date, Notes = "Test note for design mode"},
                    new() {VisitDate = DateTime.Now.Date, Notes = "Test note for design mode"},
                };
            }
        }

        public ICommand CancelCommand => new RelayCommand(() => {
            ContainerHandler.Container.Resolve<IDialogService>().DisposeLastDialog();
        });
    }



    public partial class VisitNoteListViewModel : BaseViewModel<VisitNoteListView>
    {
        public void Initialize(List<PatientVisitDto> patientVisits) {
            PatientVisits = patientVisits;
        }

        public Action OnSave;

        public List<PatientVisitDto> PatientVisits { get; set; }
    }
}
