using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using EyeClinic.Core.Interface;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;

namespace EyeClinic.DataAccess.IRepositories
{
    public interface IPatientVisitPrescriptionRepository : IBaseRepository<Model.PatientVisitPrescriptionDto, PatientVisitPrescription>, IInjectable
    {
        Task AddOrUpdate(List<Model.PatientVisitPrescriptionDto> patientVisitPrescription);
    }
}
