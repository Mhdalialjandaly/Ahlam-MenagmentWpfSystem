using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EyeClinic.Core.Enums;
using EyeClinic.Core.Interface;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;

namespace EyeClinic.DataAccess.IRepositories
{
    public interface IPatientVisitRepository : IBaseRepository<Model.PatientVisitDto, PatientVisit>, IInjectable
    {
        Task<List<Model.PatientVisitDto>> GetByPatientId(int patientId, DateTime visitDate);
        Task<List<Model.PatientVisitDto>> GetVisitsByDate(DateTime date, bool includeStarted);
        Task<List<Model.PatientVisitDto>> GetVisits(bool includeStarted);
        Task ChangeVisitStatus(int patientVisitId, PatientVisitStatus visitStatus = PatientVisitStatus.Started);
        Task FinishPatientVisit(int patientVisitId, DateTime? reviewDate);
        Task UpdateVisitNote(int patientVisitId, string note, string pdffile);
        Task UpdateVisitWhyNote(int patientVisitId, string why);

        Task UpdateVisitMedicalReport(int patientVisitId, string note);
        Task<List<PatientService>> GetNavigationIncludedData(int patientId, int patientVisitId);
        Task<List<Model.PatientVisitDto>> GetReviews(DateTime reviewDate);
        Task<List<Model.PatientVisitDto>> GetByDate(DateTime reviewDate);
        Task<List<Model.PatientVisitDto>> GetByFakeDate(DateTime reviewDate,int fakevalue);
        Task<Model.PatientVisitDto> GetLastVisitByPatientId(int patientId);
        Task<DateTime?> GetVisitDateById(int patientVisitId);
        Task RemoveFromQueue(int id);
        Task AddPayment(int patientId, int payment);
        Task AddPaymentForToday(int patientId, int payment);
    }
}
