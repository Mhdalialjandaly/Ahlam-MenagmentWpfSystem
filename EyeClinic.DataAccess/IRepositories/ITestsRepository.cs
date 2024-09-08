using System.Collections.Generic;
using System.Threading.Tasks;
using EyeClinic.Core.Interface;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;

namespace EyeClinic.DataAccess.IRepositories
{
    public interface ITestsRepository : IBaseRepository<Model.TestDto, Test>, IInjectable
    {
        Task DropAll(List<Model.PatientVisitsTestDto> patientVisitsTests);
        Task<List<Model.TestDto>> GetByCode(string code);
        Task<List<Model.TestDto>> GetByIsProduct(bool isproduct);
        Task<List<Model.TestDto>> GetByIsProductWithName(bool isproduct,string arname);
    }
}
