using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using EyeClinic.Core.Interface;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;

namespace EyeClinic.DataAccess.IRepositories
{
    public interface IOldMedicineRepository : IBaseRepository<Model.OldMedicineViewTableDto, OldMedicineViewTable>, IInjectable
    {
        Task<List<Model.OldMedicineViewTableDto>> GetByPatientVisitId(int patientVisitId);
        Task<List<Model.OldMedicineViewTableDto>> GetByPatientId(int patientId);
        Task<List<Model.OldMedicineViewTableDto>> GetByDateAndMedicine(DateTime fromDate, DateTime toDate, string medicineName);
    }
}
