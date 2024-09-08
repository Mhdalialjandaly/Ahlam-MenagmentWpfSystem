using AutoMapper;
using EyeClinic.Core.Enums;
using EyeClinic.Core;
using EyeClinic.DataAccess.Base;
using EyeClinic.DataAccess.Entities;
using EyeClinic.DataAccess.IRepositories;
using EyeClinic.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EyeClinic.DataAccess.Repositories
{
    public class PublicCustomerOrderRepository : BaseRepository<Model.PublicCustomerOrderDto, PublicCustomerOrder>, IPublicCustomerOrderRepository
    {
        private readonly IMapper _mapper;
        private readonly EyeClinicContext _context;

        public PublicCustomerOrderRepository(IMapper mapper, EyeClinicContext context)
            : base(mapper, context)
        {
            _mapper = mapper;
            _context = context;
        }

        public override async Task<Model.PublicCustomerOrderDto> Add(Model.PublicCustomerOrderDto modelToAdd)
        {
            var orderqueueIndex = await _context.PublicCustomerOrders.AsNoTracking()
                .Where(e => e.OrderDate.Date == modelToAdd.OrderDate.Date)
                .CountAsync();

            modelToAdd.OrderIndex = orderqueueIndex + 1;
            return await base.Add(modelToAdd);
        }

        public async Task<List<Model.PublicCustomerOrderDto>> GetOrdersByDate(DateTime date, bool includeStarted)
        {
            var items = await _context.PublicCustomerOrders
                    .Include(e => e.PublicCustomer)
                    .ThenInclude(e => e.CustomerNoteStory)                   
                   
                    .Where(e => e.DeletedDate == null
                    && e.OrderDate.Date == date.Date
                                && (includeStarted || e.OrderStatus == 0))
                    .AsNoTracking()
                    .OrderBy(e => e.CreatedDate)
                    .ToListAsync();

            return _mapper.Map<List<Model.PublicCustomerOrderDto>>(items);
        }
        public async Task<List<Model.PublicCustomerOrderDto>> GetOrders(bool includeStarted)
        {
            var items = await _context.PublicCustomerOrders
                    .Include(e => e.PublicCustomer)
                    .ThenInclude(e => e.CustomerNoteStory)
                    .Include(e => e.PublicCustomer)
                    .Where(e => e.DeletedDate == null
                                && (includeStarted || e.OrderStatus == 0))
                    .AsNoTracking()
                    .OrderBy(e => e.CreatedDate)
                    .ToListAsync();

            return _mapper.Map<List<Model.PublicCustomerOrderDto>>(items);
        }

        public async Task ChangeOrderStatus(int publicCustomerId, PublicCustomerOrderStatus orderStatus = PublicCustomerOrderStatus.Started)
        {
            var visit = await _context.PublicCustomerOrders.FindAsync(publicCustomerId);
            visit.OrderStatus = (byte)orderStatus;
            visit.LastModifiedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        //public async Task<List<PatientVisitDto>> GetByPatientId(int patientId, DateTime visitDate)
        //{
        //    var visits = await _context.PatientVisits
        //        .AsNoTracking()
        //        .Include(e => e.Patient)
        //        .ThenInclude(e => e.PatientSickStory)
        //        .Where(e => e.PatientId == patientId
        //        && e.VisitDate.Date == visitDate.Date)
        //        .ToListAsync();

        //    return _mapper.Map<List<PatientVisitDto>>(visits);
        //}

        //public async Task FinishPatientVisit(int patientVisitId, DateTime? reviewDate)
        //{
        //    var visit = await _context.PatientVisits
        //        .FirstOrDefaultAsync(e => e.Id == patientVisitId);

        //    if (visit == null)
        //        return;

        //    visit.VisitStatus = 2;
        //    visit.ReviewDate = reviewDate;
        //    visit.LastModifiedDate = DateTime.Now;

        //    var queue = await _context.Queues
        //        .FirstOrDefaultAsync(e => e.PatientId == visit.PatientId &&
        //                                  e.VisitDate.Date == visit.VisitDate.Date);
        //    if (queue != null)
        //    {
        //        queue.VisitStatus = 1;
        //    }
        //    await SaveChangesAsync();
        //}

        //public async Task UpdateVisitNote(int patientVisitId, string note)
        //{
        //    var dbItem = _context.PatientVisits.FirstOrDefault(e => e.Id == patientVisitId);
        //    if (dbItem == null)
        //        throw new Exception($"Patient visit not found ( ID : {patientVisitId} )");

        //    dbItem.Notes = note;
        //    dbItem.LastModifiedDate = DateTime.Now;
        //    await SaveChangesAsync();
        //}
        //public async Task UpdateVisitWhyNote(int patientVisitId, string why)
        //{
        //    var dbItem = _context.PatientVisits.FirstOrDefault(e => e.Id == patientVisitId);
        //    if (dbItem == null)
        //        throw new Exception($"Patient visit not found ( ID : {patientVisitId} )");

        //    dbItem.Why = why;
        //    dbItem.LastModifiedDate = DateTime.Now;
        //    await SaveChangesAsync();
        //}

        //public async Task UpdateVisitMedicalReport(int patientVisitId, string note)
        //{
        //    var dbItem = _context.PatientVisits.FirstOrDefault(e => e.Id == patientVisitId);
        //    if (dbItem == null)
        //        throw new Exception($"Patient Medical Report not found ( ID : {patientVisitId} )");

        //    dbItem.MedicalReport = note;
        //    dbItem.LastModifiedDate = DateTime.Now;
        //    await SaveChangesAsync();
        //}


        //public async Task<List<PatientService>> GetNavigationIncludedData(int patientId, int patientVisitId)
        //{
        //    var services = new List<PatientService>()
        //    {
        //        PatientService.PatientFile,
        //        PatientService.EyeTests,
        //        PatientService.Diagnosis,
        //        PatientService.Prescriptions,
        //        PatientService.Tests,
        //        PatientService.NewGlass,
        //        PatientService.LabTests,
        //        PatientService.Operations,
        //        PatientService.MedicalReports,
        //        PatientService.SpecialNote,
        //        PatientService.VisitNote,
        //        PatientService.ShowAllVisitsNotes,
        //        PatientService.CouseOfVisit
        //    };

        //    var visitSpecialNote = await _context.Patients
        //        .AnyAsync(e => e.Id == patientId && !string.IsNullOrWhiteSpace(e.Notes));

        //    var patientHasNote = (await _context.PatientVisits
        //        .AnyAsync(e => e.Id == patientVisitId && !string.IsNullOrWhiteSpace(e.Notes)));

        //    var patientANYHasNote = (await _context.PatientVisits
        //        .AnyAsync(e => e.PatientId == patientId && !string.IsNullOrWhiteSpace(e.Notes)));

        //    var visitWhyNote = (await _context.PatientVisits
        //        .AnyAsync(e => e.Id == patientVisitId && !string.IsNullOrWhiteSpace(e.Why)));

        //    var patientHasSickStory = await _context.PatientSickStories
        //        .AnyAsync(e => e.PatientId == patientId && !string.IsNullOrWhiteSpace(e.SickStory));

        //    if (visitSpecialNote == false && patientHasSickStory == false)
        //        services.Remove(PatientService.PatientFile);

        //    if (!visitSpecialNote)
        //        services.Remove(PatientService.SpecialNote);

        //    if (!patientHasNote)
        //        services.Remove(PatientService.VisitNote);

        //    if (!patientANYHasNote)
        //        services.Remove(PatientService.ShowAllVisitsNotes);

        //    if (visitWhyNote == false)
        //        services.Remove(PatientService.CouseOfVisit);

        //    if (!await _context.PatientOperations.AnyAsync(e => e.PatientId == patientId && e.DeletedDate == null))
        //        services.Remove(PatientService.Operations);

        //    if (!await _context.PatientVisitsTests.AnyAsync(e => e.PatientVisitId == patientVisitId && e.DeletedDate == null))
        //        services.Remove(PatientService.Tests);

        //    if (!await _context.PatientVisitLabTests.AnyAsync(e => e.PatientVisitId == patientVisitId && e.DeletedDate == null))
        //        services.Remove(PatientService.LabTests);

        //    if (!await _context.PatientVisitGlasses.AnyAsync(e => e.PatientVisitId == patientVisitId && e.DeletedDate == null))
        //        services.Remove(PatientService.NewGlass);

        //    var presc = await _context.PatientVisitPrescriptions.AnyAsync(
        //        e => e.PatientVisitId == patientVisitId && e.DeletedDate == null);
        //    var oldPresc = await _context.OldMedicineViewTables.AnyAsync(
        //        e => e.PatientVisitId == patientVisitId && e.DeletedDate == null);
        //    if (oldPresc == false && presc == false)
        //        services.Remove(PatientService.Prescriptions);

        //    if (await _context.PatientVisitEyeTests
        //        .AsNoTracking()
        //        .Where(e => e.PatientVisitId == patientVisitId && e.DeletedDate == null)
        //        .AllAsync(e => string.IsNullOrWhiteSpace(e.LeftEyeResult) &&
        //                       string.IsNullOrWhiteSpace(e.RightEyeResult)))
        //        services.Remove(PatientService.EyeTests);

        //    if (!await _context.PatientVisitDiagnoses.AnyAsync(e => e.PatientVisitId == patientVisitId && e.DeletedDate == null))
        //        services.Remove(PatientService.Diagnosis);


        //    var medicalReportKey = patientVisitId + "-" + patientId;
        //    var fileLocation = Path.Combine(Global.MedicalReportPath, medicalReportKey);
        //    if (!File.Exists(fileLocation))
        //        services.Remove(PatientService.MedicalReports);

        //    return services;
        //}

        //public async Task<List<Model.PatientVisitDto>> GetReviews(DateTime reviewDate)
        //{
        //    var items = await _context.PatientVisits
        //        .Include(e => e.Patient)
        //        .Where(e => e.ReviewDate.HasValue && e.DeletedDate == null &&
        //                    e.ReviewDate.Value.Date == reviewDate.Date)
        //        .OrderBy(e => e.ReviewDate)
        //        .ToListAsync();

        //    return _mapper.Map<List<Model.PatientVisitDto>>(items);
        //}

        //public async Task<List<Model.PatientVisitDto>> GetByDate(DateTime reviewDate)
        //{
        //    var items = await _context.PatientVisits
        //        .Include(e => e.Patient)
        //        .Include(e => e.NotPaymentReason)
        //        .Include(e => e.Patient.PatientVisits)
        //        .Where(e => e.VisitDate.Date == reviewDate.Date && e.DeletedDate == null)
        //        .OrderBy(e => e.VisitDate)
        //        .ToListAsync();

        //    return _mapper.Map<List<Model.PatientVisitDto>>(items);
        //}
        //public async Task<List<Model.PatientVisitDto>> GetByFakeDate(DateTime reviewDate, int f)
        //{


        //    var items = await _context.PatientVisits
        //        .Include(e => e.Patient)
        //        .Include(e => e.NotPaymentReason)
        //        .Include(e => e.Patient.PatientVisits)
        //        .Where(e => e.VisitDate.Date == reviewDate.Date && e.DeletedDate == null)
        //        .OrderBy(e => e.VisitDate)
        //        .Take(f)
        //        .ToListAsync();

        //    return _mapper.Map<List<Model.PatientVisitDto>>(items);
        //}

        public async Task<Model.PublicCustomerOrderDto> GetLastOrderByPublicCustomerId(int publicCustomerId)
        {
            var order = await _context.PublicCustomerOrders
                .AsNoTracking()
                .Where(e => e.PublicCustomerId == publicCustomerId)
                .OrderByDescending(e => e.OrderDate)
                .FirstOrDefaultAsync();

            return _mapper.Map<Model.PublicCustomerOrderDto>(order);
        }


        public async Task RemoveFromOrderQueue(int id)
        {
            var item = await _context.PublicCustomerOrders.FirstOrDefaultAsync(e => e.Id == id);
            item.DeletedDate = DateTime.Now;
            item.DeletedBy = Global.GetValue(GlobalKeys.LoggedUser).ToString();
            await _context.SaveChangesAsync();
        }

        //public async Task AddPayment(int patientId, int payment)
        //{
        //    var visitsHasPayments = await _context
        //        .PatientVisits
        //        .Where(e => e.PatientId == patientId)
        //        .Where(e => e.Remaining > 0)
        //        .Where(e => e.DeletedDate == null)
        //        .Where(e => e.DeletedBy == null)
        //        .ToListAsync();

        //    foreach (var visit in visitsHasPayments)
        //    {
        //        if (payment == 0)
        //            break;

        //        if (payment >= visit.Remaining)
        //        {
        //            visit.RemainPayValue = payment;
        //            visit.Payment += visit.Remaining;
        //            payment -= visit.Remaining;
        //            visit.RemainPayed = true;


        //        }
        //        else
        //        {
        //            visit.RemainPayValue = payment;
        //            visit.Payment += payment;
        //            visit.RemainPayed = true;

        //            payment = 0;

        //        }
        //    }

        //    await _context.SaveChangesAsync();
        //}
        //public async Task AddPaymentForToday(int patientId, int payment)
        //{
        //    var visitsHasPayments1 = await _context
        //.PatientVisits
        //.Where(e => e.PatientId == patientId)
        //.Where(e => e.VisitDate == DateTime.Today)
        //.ToListAsync();
        //    if (visitsHasPayments1 != null)
        //    {
        //        var visitsHasPayments = await _context
        //            .PatientVisits
        //            .Where(e => e.PatientId == patientId)
        //            .Where(e => e.DeletedDate == null)
        //            .Where(e => e.DeletedBy == null)
        //            .OrderBy(e => e.VisitDate)
        //            .ToListAsync();

        //        foreach (var visit in visitsHasPayments)
        //        {
        //            if (payment == 0)
        //                break;

        //            if (payment >= visit.Remaining)
        //            {
        //                visit.RemainPayValue += payment;
        //                visit.RemainPayed = true;
        //            }
        //            else
        //            {
        //                visit.RemainPayValue += payment;
        //                visit.RemainPayed = true;
        //            }
        //        }

        //        await _context.SaveChangesAsync();
        //    }
        //    else
        //    {

        //    }
        //}

        //public async Task<DateTime?> GetVisitDateById(int patientVisitId)
        //{
        //    var item = (await _context
        //            .PatientVisits
        //            .AsNoTracking()
        //            .FirstOrDefaultAsync(e => e.Id == patientVisitId))
        //        ?.VisitDate;

        //    return item;
        //}
    }
}
