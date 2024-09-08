using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EyeClinic.Core.Interface;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;

namespace EyeClinic.DataAccess.IRepositories
{
    public interface IPatientOperationRepository : IBaseRepository<Model.PatientOperationDto, PatientOperation>, IInjectable
    {
        Task UpdateOperationStatus(int patientOperationId, bool finish);
        Task<List<Model.PatientOperationDto>> GetOperationSchedule(DateTime operationDate);
        Task<List<Model.PatientOperationSessionDto>> GetSessionSchedule(DateTime targetDateDate);
        Task<int> GetRemainingByPatientId(int patientId);
        Task AddPayment(int patientId, int paymentPayment);
    }
}
