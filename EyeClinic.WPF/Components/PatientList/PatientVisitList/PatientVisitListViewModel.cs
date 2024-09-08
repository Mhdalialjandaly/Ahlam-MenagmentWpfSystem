using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using EyeClinic.Core;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.NotificationService;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.Dialogs.PasswordInput;
using EyeClinic.WPF.Components.PatientList.PatientVisitList.PatientVisitEditor;
using EyeClinic.WPF.Components.PatientList.PatientVisitList.SummeryVisits;
using EyeClinic.WPF.Components.PatientList.PatientVisitList.VisitNote;
using EyeClinic.WPF.Components.PatientList.PatientVisitList.VisitNote.VisitNoteList;
using EyeClinic.WPF.Setup;
using Microsoft.EntityFrameworkCore;
using Syncfusion.Data.Extensions;
using Syncfusion.Windows.Controls.Input;
using Unity;

namespace EyeClinic.WPF.Components.PatientList.PatientVisitList
{
    public partial class PatientVisitListViewModel
    {
        public PatientVisitListViewModel()
        {
            if (IsDesignMode) { }



        }

        public PatientVisitListViewModel DataContext { get; internal set; }
    }

    public partial class PatientVisitListViewModel : BaseViewModel<PatientVisitListView>
    {
        private readonly IPatientVisitRepository _patientVisitRepository;
        private readonly IDialogService _dialogService;
        private readonly IUnityContainer _container;
        private readonly IPatientVisitTestRepository _patientVisitTestRepository;
        private readonly EyeClinicContext _context;



        public PatientVisitListViewModel(IPatientVisitRepository patientVisitRepository,
            IDialogService dialogService, IUnityContainer container,IPatientVisitTestRepository patientVisittest)
        {
            _patientVisitRepository = patientVisitRepository;
            _dialogService = dialogService;
            _container = container;
            _patientVisitTestRepository = patientVisittest;

            SummeryOfVisitCommand = new RelayCommand<int?>(SummeryOfVisit);
            ShowVisitCustomNoteCommand = new RelayCommand(ShowVisitCustomNote);
            ShowVisitNotesCommand = new RelayCommand(ShowVisitNotes);
            ViewAllVisitNotesCommand = new RelayCommand(ViewAllVisitNotes);

            AddVisitCommand = new RelayCommand(AddVisit);
            EditVisitCommand = new RelayCommand(EditVisit);
            //DeleteVisitCommand = new RelayCommand(DeleteVisit);


            DeleteVisitCommand = new RelayCommand(() => {
                if (!_container.CheckUserRoleSilent(UserRoles.Admin) )
                {
                    _container.Resolve<INotificationService>()
                    .Warning("You have no permission");
                    return;
                }
                var pwd = _container.Resolve<PasswordInputViewModel>();
                pwd.DisposeOnLogin = false;
                pwd.CustomPassword = "54425";
                pwd.OnSuccessLogin += () => 
                {
                    _dialogService.DisposeLastDialog();
                    DeleteVisit();
                };
                _container.Resolve<IDialogService>()
                    .ShowPopupContent(pwd.GetView() as PasswordInputView);
            });

            SortCommand = new RelayCommand(Sort);



            ChangeSelectedVisitCommand = new RelayCommand<PatientVisitDto>(visit => {
                SelectedPatientVisit = visit;
            });

            ShowManageVisitButtons = Global.DeviceName.ToLower() != "المسؤول";
            ShowSummaryOfVisitsCommand = new RelayCommand(ShowSummaryOfVisits);
        }

        private void Sort()
        {
            var visits = this.PatientVisitList;
            if (SortDesc)
                PatientVisitList = new ObservableCollection<PatientVisitDto>(visits
                    .OrderByDescending(e => e.VisitDate).ToList());
            else
                PatientVisitList = new ObservableCollection<PatientVisitDto>(visits
                    .OrderBy(e => e.VisitDate).ToList());
            SortDesc = !SortDesc;
            SortContent = SortDesc ? "Sort ↓" : "Sort ↑";
        }

        public ICommand ShowSummaryOfVisitsCommand { get; set; }
        private void ShowSummaryOfVisits()
        {

            if (SelectedPatientVisit == null)
            {
                System.Windows.MessageBox.Show("");
            }
            BusyExecute(async () => {
                var summery = _container.Resolve<SummeryVisitsViewModel>();

                var patient = await _container.Resolve<IPatientRepository>().GetById(PatientId);
                await summery.Initialize(PatientId, patient?.Notes);
                _dialogService.ShowFullScreenPopupContent(
                    summery.GetView() as SummeryVisitsView);
            });           
        }

 

        public async Task Initialize(int patientId, bool updateSelectedVisit = true)
        {
            PatientId = patientId;
            int index = -1;
            if (PatientVisitList != null)
            {
                index = PatientVisitList.IndexOf(SelectedPatientVisit);
            }

            var visits = await _patientVisitRepository
                .GetByKey(e => e.PatientId == patientId);

            PatientVisitList = new ObservableCollection<PatientVisitDto>(visits
                .OrderByDescending (e => e.VisitDate).ToList());

            if (updateSelectedVisit)
            {
                SelectedPatientVisit = PatientVisitList.FirstOrDefault();
                if (SelectedPatientVisit != null)
                    SelectedPatientVisit.IsChecked = true;
            }
            else
            {
                if (index > -1)
                {
                    PatientVisitList[index].IsChecked = true;
                    SelectedPatientVisit = PatientVisitList[index];
                }
            }

          


            foreach (var item in visits)
            {
                  var requiredTests = await _patientVisitTestRepository.Get()
                     .AsNoTracking()
                     .Include(e => e.PatientVisit)
                     .Include(e => e.Test)
                     .Where(e => e.PatientVisit.PatientId == PatientId && e.DeletedDate == null && e.PatientVisit.DeletedDate == null
                     &&  !string.IsNullOrWhiteSpace(e.ImageNumber)
                     && e.PatientVisitId == item.Id)
                     .ToListAsync();

                foreach (var item2 in requiredTests)
                {

                    if (item2 != null)
                    {
                        item.ImageUploaded = "UP";
                    }
                    else
                    {
                        item.ImageUploaded = null;
                    }
                }

            }
            
        }

        private void SummeryOfVisit(int? id)
        {
            if (id == null)
                return;

            BusyExecute(async () => {
                var summery = _container.Resolve<SummeryVisitsViewModel>();

                // To View operations you have to send PatientId by using this variable
                // SelectedPatientVisit.PatientId

                var patient = await _container.Resolve<IPatientRepository>().GetById(PatientId);
                await summery.InitializeByVisitId(id.Value, patient?.Notes);
                _dialogService.ShowFullScreenPopupContent(
                    summery.GetView() as SummeryVisitsView);
            });
        }

        public List<SummeryDetailModel> SummeryDetail { get; set; }
        public bool ShowManageVisitButtons { get; set; }
        public ObservableCollection<PatientVisitDto> PatientVisitList { get; set; }

        private PatientVisitDto _selectedPatientVisit;
        public PatientVisitDto SelectedPatientVisit
        {
            get => _selectedPatientVisit;
            set
            {
                _selectedPatientVisit = value;
                if (value != null)
                    OnPatientVisitSelected(value);
            }
        }

        public int PatientId { get; set; }
        public string SortContent { get; set; } = "Sort ↑";
        public bool SortDesc { get; set; }
        public byte[] PdfFile { get; set; }

        public event EventHandler<PatientVisitDto> PatientVisitChanged;


        public ICommand SummeryOfVisitCommand { set; get; }
        public ICommand ShowVisitCustomNoteCommand { get; set; }
        public ICommand ShowVisitNotesCommand { get; set; }
        public ICommand ViewAllVisitNotesCommand { get; set; }
        public ICommand ChangeSelectedVisitCommand { get; set; }

        public ICommand SortCommand { get; set; }
        public ICommand AddVisitCommand { get; set; }
        public ICommand EditVisitCommand { get; set; }
        public ICommand DeleteVisitCommand { get; set; }


        private void OnPatientVisitSelected(PatientVisitDto patientVisit)
        {
            PatientVisitChanged?.Invoke(this, patientVisit);
            foreach (var patientVisitDto in PatientVisitList)
            {
                patientVisitDto.IsChecked = false;
            }

            patientVisit.IsChecked = true;
        }

        private void ShowVisitCustomNote()
        {
            if (SelectedPatientVisit == null)
                return;

            var visitNote = _container.Resolve<VisitNoteViewModel>();
            visitNote.Initialize(SelectedPatientVisit.Id, SelectedPatientVisit.MedicalReport,SelectedPatientVisit.PdfPath);

            _dialogService.ShowEditorDialog(visitNote.GetView() as VisitNoteView,
                async () => {
                    if (await visitNote.SaveCustomNote())
                    {
                        SelectedPatientVisit.MedicalReport = visitNote.Note;
                        await Initialize(PatientId, false);
                        return true;
                    }

                    return false;
                });
        }

        private void ShowVisitNotes()
        {
            if (SelectedPatientVisit == null)
                return;

            var visitNote = _container.Resolve<VisitNoteViewModel>();
            visitNote.Initialize(SelectedPatientVisit.Id, SelectedPatientVisit.Notes,SelectedPatientVisit.PdfPath);

            _dialogService.ShowEditorDialog(visitNote.GetView() as VisitNoteView,
                async () => {
                    if (await visitNote.Save())
                    {
                        SelectedPatientVisit.Notes = visitNote.Note;
                        await Initialize(PatientId, false);
                        return true;
                    }

                    return false;
                });
        }

        private void AddVisit()
        {
            if (!_container.CheckUserRoleSilent(UserRoles.Admin) )
            {
                _container.Resolve<INotificationService>()
                .Warning("You have no permission");
                return;
            }
            BusyExecute(async () => {
                var editor = _container.Resolve<PatientVisitEditorViewModel>();
                await editor.Initialize();
                _container.Resolve<IDialogService>()
                    .ShowEditorDialog(editor.GetView() as PatientVisitEditorView, async () => {
                        var visit = await editor.Save(PatientId);
                        if (visit == null)
                            return false;

                        await Initialize(PatientId);
                        return true;
                    });
            });
        }

        private void EditVisit()
        {
            if (!_container.CheckUserRoleSilent(UserRoles.Admin) )
            {
                _container.Resolve<INotificationService>()
                .Warning("You have no permission");
                return;
            }
            if (SelectedPatientVisit == null)
                return;            
                    var password = _container.Resolve<PasswordInputViewModel>();
                    password.CustomPassword = "01";

                    password.OnSuccessLogin += () => {
                        BusyExecute(async () => {
                            var editor = _container.Resolve<PatientVisitEditorViewModel>();
                            await editor.Initialize(SelectedPatientVisit);
                            _container.Resolve<IDialogService>()
                                .ShowEditorDialog(editor.GetView() as PatientVisitEditorView, async () => {
                                    var visit = await editor.Save(SelectedPatientVisit.PatientId);
                                    if (visit == null)
                                        return false;

                                    PatientVisitList.UpdateItem(SelectedPatientVisit, visit);
                                    return true;
                                });
                        });
                    };

                    _container.Resolve<IDialogService>().
                        ShowPopupContent(password.GetView() as PasswordInputView); 
        }

        private void DeleteVisit()
        {

            //var pwd = _container.Resolve<PasswordInputViewModel>();
            //pwd.DisposeOnLogin = false;
            //pwd.CustomPassword = "54425";
            //pwd.OnSuccessLogin += () => {
            //    _dialogService.DisposeLastDialog();
            if (SelectedPatientVisit == null)
                return;

            _dialogService.ShowConfirmationMessage("Are You Sure ?", async () => {
                try
                {
                    await _patientVisitRepository.Delete(SelectedPatientVisit.Id);
                    PatientVisitList.Remove(SelectedPatientVisit);
                    return true;
                }
                catch (Exception ex)
                {
                    _container.Resolve<INotificationService>().Error(ex.GetBaseException().Message);
                    return false;
                }
            });           
        }

        private void ViewAllVisitNotes()
        {
            var visitList = _container.Resolve<VisitNoteListViewModel>();
            visitList.Initialize(PatientVisitList
                .Where(e => !string.IsNullOrWhiteSpace(e.Notes))
                .ToList());

            _dialogService.ShowInformationDialog(visitList.GetView() as VisitNoteListView);
        }
    }
}
