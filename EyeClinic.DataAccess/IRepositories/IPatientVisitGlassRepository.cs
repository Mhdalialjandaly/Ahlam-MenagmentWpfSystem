using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EyeClinic.Core.Interface;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;

namespace EyeClinic.DataAccess.IRepositories
{
    public interface IPatientVisitGlassRepository : IBaseRepository<Model.PatientVisitGlassDto, PatientVisitGlass>, IInjectable
    {
        Task<Model.PatientVisitGlassDto> AddOrUpdate(Model.PatientVisitGlassDto patientVisitGlass);
        Task<Model.PatientVisitGlassDto> GetLastGlassInfo(int patientId);
        Task<DateTime?> GetVisitDateById(int patientVisitId);
        Task<List<Model.PatientVisitGlassDto>> GetByDate(DateTime fromDateDate, DateTime toDateDate);

    }
}
