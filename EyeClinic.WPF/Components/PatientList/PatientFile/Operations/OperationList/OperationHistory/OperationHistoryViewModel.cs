using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.Base;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.Operations.OperationList.OperationHistory
{
    public partial class OperationHistoryViewModel { public OperationHistoryViewModel() { } }

    public partial class OperationHistoryViewModel : BaseViewModel<OperationHistoryView>
    {
        private readonly IPatientOperationRepository _operationRepository;

        public OperationHistoryViewModel(IPatientOperationRepository operationRepository) {
            _operationRepository = operationRepository;
        }

        public void InitializeList(List<PatientOperationDto> operations) {
            Operations = operations
                .GroupBy(e => e.OperationDate,
                    (key, value) => new OperationHistoryModel {
                        OperationDate = key,
                        Operations = value.ToList()
                    })
                .OrderByDescending(e => e.OperationDate)
                .ToList();
        }

        public async Task Initialize(int patientId) {
            var operations = await _operationRepository
                .GetByKey(e => e.PatientId == patientId);

            InitializeList(operations);
        }


        public List<OperationHistoryModel> Operations { get; set; }
    }
}
