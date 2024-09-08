using System;
using System.Threading.Tasks;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.Base;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.EyeDiagnosis.EyeDiagnosisEditor
{
    public partial class EyeDiagnosisEditorViewModel
    {
        public EyeDiagnosisEditorViewModel() { if (IsDesignMode) { } }
    }

    public partial class EyeDiagnosisEditorViewModel : BaseViewModel<EyeDiagnosisEditorView>
    {
        private readonly IDiagnosisRepository _diagnosisRepository;

        public EyeDiagnosisEditorViewModel(IDiagnosisRepository diagnosisRepository) {
            _diagnosisRepository = diagnosisRepository;
        }

        public string Code { get; set; }
        public string EnName { get; set; }
        public string ArName { get; set; }

        public async Task<DiagnosisDto> Save() {
            if (string.IsNullOrWhiteSpace(Code))
                return null;

            if (string.IsNullOrWhiteSpace(EnName) && string.IsNullOrWhiteSpace(ArName))
                return null;

            if (string.IsNullOrWhiteSpace(EnName))
                EnName = ArName;
            if (string.IsNullOrWhiteSpace(ArName))
                ArName = EnName;

            return await _diagnosisRepository.Add(new DiagnosisDto {
                Code = Code,
                ArName = ArName,
                EnName = EnName,
                CreatedDate = DateTime.Now
            });
        }
    }
}
