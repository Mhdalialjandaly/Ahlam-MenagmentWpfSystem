using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using EyeClinic.Core;
using EyeClinic.WPF.Base;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.MedicalReport.MedicalReportTemplate
{
    public partial class MedicalReportTemplateViewModel
    {
        public MedicalReportTemplateViewModel() { }
    }

    public partial class MedicalReportTemplateViewModel : BaseViewModel<MedicalReportTemplateView>
    {
        public override Task Initialize() {
            Directory.CreateDirectory(Global.MedicalReportTemplatePath);
            ContactAccounts = new List<string>();
            string[] files = Directory.GetFiles(Global.MedicalReportTemplatePath);
            foreach (string file in files) {
                ContactAccounts.Add(Path.GetFileName(file));
            }

            SelectedContactAccount = ContactAccounts.FirstOrDefault();
            CurrentFileIndex = 1;
            TotalFiles = files.Length;
            if (SelectedContactAccount == null)
                return Task.CompletedTask;

            LoadFile(SelectedContactAccount);

            return Task.CompletedTask;
        }

        private void LoadFile(string fileName) {
            if (string.IsNullOrWhiteSpace(fileName)) {
                ContactAccounts.Add("");
                CurrentFileIndex = ContactAccounts.Count - 1;
                SelectedContactAccount = null;
                if (GetView() is MedicalReportTemplateView tempview) {
                    TextRange range;
                    
                    range = new TextRange(tempview.mainRTB.Document.ContentStart,
                        tempview.mainRTB.Document.ContentEnd);
                }
                return;
            }

            var medicalReportKey = fileName;
            var fileLocation = Path.Combine(Global.MedicalReportTemplatePath, medicalReportKey);
            Directory.CreateDirectory(Global.MedicalReportTemplatePath);

            if (GetView() is MedicalReportTemplateView view) {
                TextRange range;
                FileStream fStream;
                if (File.Exists(fileLocation)) {
                    range = new TextRange(view.mainRTB.Document.ContentStart,
                        view.mainRTB.Document.ContentEnd);
                    fStream = new FileStream(fileLocation, FileMode.OpenOrCreate);
                    range.Load(fStream, DataFormats.XamlPackage);
                    fStream.Close();
                }
            }
        }


        public int CurrentFileIndex { get; set; }
        public int TotalFiles { get; set; }

        public ICommand SaveCommand => new RelayCommand(Save);
        public ICommand PreviousCommand => new RelayCommand(() => {
            try {
                if (CurrentFileIndex > 1) {
                    CurrentFileIndex--;
                    SelectedContactAccount = ContactAccounts[CurrentFileIndex - 1];
                    LoadFile(SelectedContactAccount);
                }
            } catch (Exception e) {
                Console.WriteLine(e);
            }
        });
        public ICommand NextCommand => new RelayCommand(() => {
            try {
                if (CurrentFileIndex < TotalFiles) {
                    CurrentFileIndex++;
                    SelectedContactAccount = ContactAccounts[CurrentFileIndex - 1];
                    LoadFile(SelectedContactAccount);
                }
            } catch (Exception e) {
                Console.WriteLine(e);
            }
        });

        public ICommand NewMedicalReportCommand => new RelayCommand(() => {
            try {
                SelectedContactAccount = null;
                LoadFile(SelectedContactAccount);
            } catch (Exception e) {
                Console.WriteLine(e);
            }
        });

        private void Save() {
            if (string.IsNullOrWhiteSpace(SelectedContactAccount))
                return;

            var medicalReportKey = SelectedContactAccount;
            var fileLocation = Path.Combine(Global.MedicalReportTemplatePath, medicalReportKey);
            Directory.CreateDirectory(Global.MedicalReportTemplatePath);

            if (GetView() is MedicalReportTemplateView view) {
                TextRange range;
                FileStream fStream;
                range = new TextRange(view.mainRTB.Document.ContentStart,
                    view.mainRTB.Document.ContentEnd);
                fStream = new FileStream(fileLocation, FileMode.Create);
                range.Save(fStream, DataFormats.XamlPackage);
                fStream.Close();
            }
        }


        public List<string> ContactAccounts { get; set; }
        public string SelectedContactAccount { get; set; }
    }
}
