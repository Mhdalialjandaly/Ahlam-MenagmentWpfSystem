using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EyeClinic.Core.Interface;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.Model.Custom;

namespace EyeClinic.DataAccess.IRepositories
{
    public interface IPatientRepository : IBaseRepository<Model.PatientDto, Patient>, IInjectable
    {
        Task<Model.PatientDto> GetByPatientVisitId(int patientVisitId);
        Task<List<Model.PatientDto>> Search(string code, string firstName, string lastName,bool isoperationdepartment, bool includeExcelFile);
        Task UpdateNote(int patientId, string specialNote);
        Task<List<Model.PatientDto>> SearchByImageNumber(string imageNumber);
    }
}
