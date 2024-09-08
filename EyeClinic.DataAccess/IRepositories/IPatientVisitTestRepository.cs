using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EyeClinic.Core.Interface;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;

namespace EyeClinic.DataAccess.IRepositories
{
    public interface IPatientVisitTestRepository : IBaseRepository<Model.PatientVisitsTestDto, PatientVisitsTest>, IInjectable
    {
        Task<int> GetLastImageNumber();
        Task<int> GetLastScanNumber();
        Task<int> GetTotalEyeMuscles();
        Task<DateTime?> GetVisitDateById(int patientVisitId);
        Task ClearImageNumber(int id);
        Task<List<string>> GetImagesByPatientId(int patientId);
        Task<List<Model.PatientVisitsTestDto>> GetByDateAndTest(DateTime fromDate, DateTime toDate, int testId);
        Task<List<Model.PatientVisitsTestDto>> GetByDateAndTestD(DateTime fromDate, DateTime toDate, int testId);

        Task<List<Model.PatientVisitsTestDto>> GetByDateAndTestLeftEye(DateTime fromDate, DateTime toDate, int testId);
        Task<List<Model.PatientVisitsTestDto>> GetByDateAndTestLeftEyeD(DateTime fromDate, DateTime toDate, int testId);


        Task<List<Model.PatientVisitsTestDto>> GetByDateAndTestRightEye(DateTime fromDate, DateTime toDate, int testId);
        Task<List<Model.PatientVisitsTestDto>> GetByDateAndTestRightEyeD(DateTime fromDate, DateTime toDate, int testId);

        Task<int> GetByVisitIdImagesHadScanned(int id);

    }
}
