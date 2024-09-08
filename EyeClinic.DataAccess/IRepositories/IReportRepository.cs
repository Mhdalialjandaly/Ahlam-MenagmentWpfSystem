using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EyeClinic.Core.Interface;
using EyeClinic.Model;

namespace EyeClinic.DataAccess.IRepositories
{
    public interface IReportRepository : IInjectable
    {
        Task<List<PatientDto>> GetPatientNamesByDisease(DiseaseDto disease, DateTime fromDate, DateTime toDate);

        Task<List<PatientDto>> GetPatientNamesByOperationId(int selectedOperationId,DateTime fromDate,DateTime toDate);
        Task<List<PatientVisitDto>> GetTotalVisits(DateTime from, DateTime to);
    }
}
