using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using EyeClinic.Core.Enums;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.AppServices.DialogService;
using EyeClinic.WPF.AppServices.Localization;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Components.PatientList.PatientFile.Operations.OperationList.OperationEditor;
using EyeClinic.WPF.Components.PatientList.PatientFile.Operations.OperationList.OperationSessionList.OperationSessionEditor;
using EyeClinic.WPF.Setup;
using Microsoft.EntityFrameworkCore;
using Syncfusion.XlsIO;
using Syncfusion.XlsIO.Implementation;
using Syncfusion.XlsIO.Implementation.Collections.Grouping;
using Unity;
using operationCore = EyeClinic.Core.Enums.Operation;
namespace EyeClinic.WPF.Components.Home.OperationSchedule
{
    public partial class OperationScheduleViewModel
    {
        public OperationScheduleViewModel() { }
    }

    public partial class OperationScheduleViewModel : BaseViewModel<OperationScheduleView>
    {
        private readonly IPatientOperationRepository _operationRepository;
        private readonly IUnityContainer _container;
        private readonly IDialogService _dialogService;

        public OperationScheduleViewModel(IPatientOperationRepository operationRepository, IUnityContainer container,
            IDialogService dialogService) {
            _operationRepository = operationRepository;
            _container = container;
            _dialogService = dialogService;

            TargetDate = DateTime.Now.Date;
            FilterCommand = new RelayCommand(Filter);

            EditOperationCommand = new RelayCommand<PatientOperationDto>(EditOperation);
            BackHomeCommand = new RelayCommand(() => {
                BusyExecute(async () => await container.BackHome());
            });
            PrintScheduleCommand = new RelayCommand(PrintSchedule);
        }

        public override async Task Initialize() {
            var operations = await _operationRepository.GetOperationSchedule(TargetDate.Date);

            operations=operations.OrderBy(e=>e.EyeOperationDisplayName).ToList();
            Operations = new ObservableCollection<PatientOperationDto>(operations);
         
            var sessions = await _operationRepository.GetSessionSchedule(TargetDate.Date);
            foreach (var session in sessions) {
                var operation = session.PatientOperation;
                operation.Id = session.Id;
                operation.OperationDate = session.SessionDate;
                operation.SessionNumber = session.SessionNumber;
                Operations.Add(session.PatientOperation);
            }
        }


        public ICommand EditOperationCommand { get; set; }
        public ICommand BackHomeCommand { get; set; }
        public ICommand PrintScheduleCommand { get; set; }


        public ObservableCollection<PatientOperationDto> Operations { get; set; }

        public DateTime TargetDate { get; set; }

        public ICommand FilterCommand { get; set; }

        private void Filter() {
            BusyExecute(async () => {
                await Initialize();
            });
        }

        private void EditOperation(PatientOperationDto operation) {
            BusyExecute(async () => {
                if (operation.SessionNumber != null) {
                    var sessionEditor = _container.Resolve<OperationSessionEditorViewModel>();
                    await sessionEditor.Initialize();
                    sessionEditor.Operation = operationCore.Edit;
                    var item = await _container.Resolve<IPatientOperationSessionRepository>()
                        .GetById(operation.Id);

                    sessionEditor.PatientOperationId = operation.Id;
                    sessionEditor.BuildFromModel(item);

                    _dialogService.ShowEditorDialog(sessionEditor.GetView() as OperationSessionEditorView,
                        async () => {
                            var operationSession = await sessionEditor.SaveAsync();
                            if (operationSession == null)
                                return false;

                            await Initialize();
                            return true;
                        });
                    return;
                }

                var editor = _container.Resolve<OperationEditorViewModel>();
                await editor.Initialize(operation.PatientId);
                editor.Operation = Core.Enums.Operation.Edit;
                editor.Id = operation.Id;
                editor.BuildFromModel(operation);

                _dialogService.ShowEditorDialog(editor.GetView() as OperationEditorView,
                    async () => {
                        var patientOperation = await editor.SaveAsync();
                        if (patientOperation == null)
                            return false;

                        await Initialize();
                        return true;
                    });
            });
        }

        private void PrintSchedule() {
            if (Operations.Any(e => e.LeftEyeOperationId == null && e.RightEyeOperationId == null)) {
                _container.Resolve<IDialogService>()
                    .ShowInformationDialog(_container.Resolve<ILocalizationService>()
                        .Localize("OneOrMoreOperationIsNull"));
                return;
            }

            Directory.CreateDirectory(Path.Combine("D:", "OperationList"));
            string filename = Path.Combine("D:", "OperationList",
                $"{DateTime.Now:dd-MM-yyyy hh-mm-ss}.xlsx");

            var excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            application.DefaultVersion = ExcelVersion.Excel2013;
            IWorkbook workbook = application.Workbooks.Create(1);
            IWorksheet worksheet = workbook.Worksheets[0];
            worksheet.IsRightToLeft = true;

            var table = new DataTable();
            table.Clear();
            table.Columns.Add("التاريخ"); // 0
            table.Columns.Add("الاسم"); // 1
            table.Columns.Add("نوع الاجراء"); // 2
            table.Columns.Add("العين"); // 3
            table.Columns.Add("العدسة"); // 4
            table.Columns.Add("الجلسة"); // 5
            table.Columns.Add("الصور"); // 6
            table.Columns.Add("المركز الطبي"); // 7
            table.Columns.Add("المبلغ المطلوب"); // 8
            table.Columns.Add("المدفوع"); // 9
            table.Columns.Add("الباقي"); // 10
            table.Columns.Add("ملاحظات"); // 11

            var tableList = new List<TableRowModel>();
            foreach (var operation in Operations) {
                if (operation.IsFinish)
                    continue;

                bool zeroCost = false;

                if (operation.LeftEyeOperationId == operation.RightEyeOperationId) {
                    tableList.Add(new TableRowModel() {
                        Date = operation.OperationDate.ToString("dd/MM/yyyy"),
                        Name = operation.Patient.FullName,
                        SurgeryType = operation.LeftEyeOperation.EnName,
                        Eye = "R + L",
                        Ling = "",
                        Session = operation.SessionNumber,
                        Image = operation.PhotoSource,
                        MedicalCenter = operation.MedicalCenter.ArName,
                        Cost = operation.TotalCost,
                        Paid = operation.TotalCost - operation.Remaining,
                        Remaining = operation.Remaining,
                        Note = ""
                    });
                    continue;
                }

                TableRowModel operationRow = null;
                if (operation.LeftEyeOperationId != null) {
                    operationRow = new TableRowModel {
                        Date = operation.OperationDate.ToString("dd/MM/yyyy"),
                        Name = operation.Patient.FullName,
                        SurgeryType = operation.LeftEyeOperation.EnName,
                        Eye = "L",
                        Ling = "",
                        Session = operation.SessionNumber,
                        Image = operation.PhotoSource,
                        MedicalCenter = operation.MedicalCenter.ArName,
                        Cost = operation.TotalCost,
                        Paid = operation.TotalCost - operation.Remaining,
                        Remaining = operation.Remaining,
                        Note = ""
                    };
                    tableList.Add(operationRow);
                    zeroCost = true;
                }


                if (operation.RightEyeOperationId != null) {
                    if (zeroCost) { // patient has another operation
                        if (operationRow != null) {
                            operationRow.SurgeryType += Environment.NewLine;
                            operationRow.SurgeryType += operation.RightEyeOperation.EnName;
                            operationRow.Eye += Environment.NewLine;
                            operationRow.Eye += "R";
                            continue;
                        }
                    }

                    tableList.Add(new TableRowModel() {
                        Date = operation.OperationDate.ToString("dd/MM/yyyy"),
                        Name = operation.Patient.FullName,
                        SurgeryType = operation.RightEyeOperation.EnName,
                        Eye = "R",
                        Ling = "",
                        Session = operation.SessionNumber,
                        Image = operation.PhotoSource,
                        MedicalCenter = operation.MedicalCenter.ArName,
                        Cost = zeroCost ? 0 : operation.TotalCost,
                        Paid = zeroCost ? 0 : operation.TotalCost - operation.Remaining,
                        Remaining = zeroCost ? 0 : operation.Remaining,
                        Note = ""
                    });
                }
            }

            var ordered = tableList.OrderBy(e => e.MedicalCenter)
                .ThenBy(e => e.SurgeryType);
            foreach (var tableRow in ordered) {
                table.Rows.Add(
                    tableRow.Date,
                    tableRow.Name,
                    tableRow.SurgeryType,
                    tableRow.Eye,
                    tableRow.Ling,
                    tableRow.Session,
                    tableRow.Image,
                    tableRow.MedicalCenter,
                    tableRow.Cost,
                    tableRow.Paid,
                    tableRow.Remaining,
                    tableRow.Note
                    );
            }


            var totalRows = table.Rows.Count;
            while (totalRows > 0) {
                totalRows -= 12; // Total rows per page
            }

            var rangeExtend = table.Rows.Count + Math.Abs(totalRows);

            var range = worksheet.Range[$"A1:L{rangeExtend}"];
            range.BorderInside(ExcelLineStyle.Thin, Color.Black);
            range.BorderAround(ExcelLineStyle.Thin, Color.Black);

            worksheet.Columns[0].ColumnWidth = 10;
            worksheet.Columns[1].ColumnWidth = 22;
            worksheet.Columns[2].ColumnWidth = 22;
            worksheet.Columns[3].ColumnWidth = 5;
            worksheet.Columns[4].ColumnWidth = 6;
            worksheet.Columns[5].ColumnWidth = 5;
            worksheet.Columns[6].ColumnWidth = 11;
            worksheet.Columns[7].ColumnWidth = 18;
            worksheet.Columns[8].ColumnWidth = 11;
            worksheet.Columns[9].ColumnWidth = 8;
            worksheet.Columns[10].ColumnWidth = 8;
            worksheet.Columns[11].ColumnWidth = 10;

            for (int i = 1; i < table.Rows.Count + 12; i++) {
                worksheet.SetRowHeight(i, 44);
                if (i <= 12) {
                    worksheet.Columns[i - 1].HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet.Columns[i - 1].VerticalAlignment = ExcelVAlign.VAlignCenter;
                }
            }
            worksheet.PageSetup.Orientation = ExcelPageOrientation.Landscape;
            worksheet.PageSetup.PaperSize = ExcelPaperSize.PaperA4;
            worksheet.PageSetup.FooterMargin = 0.1;
            worksheet.PageSetup.HeaderMargin = 0.1;
            worksheet.PageSetup.LeftMargin = 0.10;
            worksheet.PageSetup.RightMargin = 0.10;
            worksheet.PageSetup.TopMargin = 0.25;
            worksheet.PageSetup.BottomMargin = 0.25;

            worksheet.ImportDataTable(table, true, 1, 1);
            workbook.SaveAs(filename);

            var p = new Process {
                StartInfo = new ProcessStartInfo(filename) { UseShellExecute = true }
            };
            p.Start();
        }
    }
}
