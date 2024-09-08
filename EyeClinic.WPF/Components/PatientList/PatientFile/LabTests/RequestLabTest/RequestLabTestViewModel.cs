using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using EyeClinic.WPF.Base;
using EyeClinic.WPF.Base.Extends;

namespace EyeClinic.WPF.Components.PatientList.PatientFile.LabTests.RequestLabTest
{
    public class RequestLabTestViewModel : BaseViewModel<RequestLabTestView>
    {
        private readonly ILabTestRepository _labTestRepository;

        public RequestLabTestViewModel(ILabTestRepository labTestRepository) {
            _labTestRepository = labTestRepository;
        }

        public override async Task Initialize() {
            LabTests = (await _labTestRepository.GetAll())
                .Select(e => new Checkable<LabTestDto>() {
                Item = e
                }).OrderByDescending(e=>e.Item.LastModifiedDate).ToList();
        }

        public List<Checkable<LabTestDto>> LabTests { get; set; }
    }
}
