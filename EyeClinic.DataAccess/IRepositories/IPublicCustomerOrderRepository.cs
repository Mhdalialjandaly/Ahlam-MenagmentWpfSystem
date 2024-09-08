using EyeClinic.Core.Enums;
using EyeClinic.Core.Interface;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeClinic.DataAccess.IRepositories
{
    public interface IPublicCustomerOrderRepository : IBaseRepository<Model.PublicCustomerOrderDto, PublicCustomerOrder>, IInjectable
    {
        //Task<List<Model.PatientVisitDto>> GetByPatientId(int patientId, DateTime visitDate);
        Task<List<Model.PublicCustomerOrderDto>> GetOrdersByDate(DateTime date, bool includeStarted);
        Task<List<Model.PublicCustomerOrderDto>> GetOrders(bool includeStarted);
        Task ChangeOrderStatus(int PublicCustomerOrderId, PublicCustomerOrderStatus orderStatus = PublicCustomerOrderStatus.Started);
        //Task FinishPatientVisit(int patientVisitId, DateTime? reviewDate);
        //Task UpdateVisitNote(int patientVisitId, string note);
        //Task UpdateVisitWhyNote(int patientVisitId, string why);

        //Task UpdateVisitMedicalReport(int patientVisitId, string note);
        //Task<List<PatientService>> GetNavigationIncludedData(int patientId, int patientVisitId);
        //Task<List<Model.PatientVisitDto>> GetReviews(DateTime reviewDate);
        //Task<List<Model.PatientVisitDto>> GetByDate(DateTime reviewDate);
        //Task<List<Model.PatientVisitDto>> GetByFakeDate(DateTime reviewDate, int fakevalue);
        Task<Model.PublicCustomerOrderDto> GetLastOrderByPublicCustomerId(int publicCustomerId);
        //Task<DateTime?> GetVisitDateById(int patientVisitId);
        Task RemoveFromOrderQueue(int id);
        //Task AddPayment(int patientId, int payment);
        //Task AddPaymentForToday(int patientId, int payment);
    }
}
