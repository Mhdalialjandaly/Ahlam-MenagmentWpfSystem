using System.Linq;
using AutoMapper;
using EyeClinic.DataAccess.Entities;

namespace EyeClinic.DataAccess
{
    public class EyeClinicMappingProfile : Profile
    {
        public EyeClinicMappingProfile() {
            CreateMap<Model.WareHouseReadyMaterialDto, WareHouseReadyMaterial>().PreserveReferences();
            CreateMap<WareHouseReadyMaterial, Model.WareHouseReadyMaterialDto>();

            CreateMap<Model.MarketsProductsDto, MarketsProducts>().PreserveReferences();
            CreateMap<MarketsProducts, Model.MarketsProductsDto>();

            CreateMap<Model.MarketsEntryDto, MarketsEntry>().PreserveReferences();
            CreateMap<MarketsEntry, Model.MarketsEntryDto>();

            CreateMap<Model.MarketsExtryDto, MarketsExtry>().PreserveReferences();
            CreateMap<MarketsExtry, Model.MarketsExtryDto>();

            CreateMap<Model.ReadyItemProductingDto, ReadyItemProducting>().PreserveReferences();
            CreateMap<ReadyItemProducting, Model.ReadyItemProductingDto>();

            CreateMap<Model.ReadyProductDbo, ReadyProduct>().PreserveReferences();
            CreateMap<ReadyProduct, Model.ReadyProductDbo>();

            CreateMap<Model.DiseaseDto, Disease>().PreserveReferences();
            CreateMap<Model.CustomerDto, Customer>().PreserveReferences();
            CreateMap<Customer, Model.CustomerDto>();
            CreateMap<Disease, Model.DiseaseDto>();
            CreateMap<CartoonLabels, Model.CartoonLabelsDto>().ReverseMap();
         

            CreateMap<Model.QueueDto, Queue>().PreserveReferences();
            CreateMap<Queue, Model.QueueDto>();            

            CreateMap<Model.QueueDto, Model.PatientVisitDto>().ReverseMap();
            CreateMap<Queue, Model.PatientVisitDto>().ReverseMap();

            CreateMap<Model.DiagnosisDto, Diagnosis>().PreserveReferences();
            CreateMap<Diagnosis, Model.DiagnosisDto>();

            CreateMap<Model.EyeTestDto, EyeTest>().PreserveReferences();
            CreateMap<EyeTest, Model.EyeTestDto>();

            CreateMap<Model.GlassDto, Glass>().PreserveReferences();
            CreateMap<Glass, Model.GlassDto>();

            CreateMap<Model.LocationDto, Location>().PreserveReferences();
            CreateMap<Location, Model.LocationDto>();

            CreateMap<Model.MartialStatusDto, MartialStatus>().PreserveReferences();
            CreateMap<MartialStatus, Model.MartialStatusDto>();

            CreateMap<Model.MedicineTypeDto, MedicineType>().PreserveReferences();
            CreateMap<MedicineType, Model.MedicineTypeDto>();

            CreateMap<Model.MedicineDto, Medicine>().PreserveReferences();
            CreateMap<Medicine, Model.MedicineDto>();

            CreateMap<Model.MedicineUsageDto, MedicineUsage>().PreserveReferences();
            CreateMap<MedicineUsage, Model.MedicineUsageDto>();

            CreateMap<Model.PatientDto, Patient>()
                .PreserveReferences();
            CreateMap<Patient, Model.PatientDto>();

            

            CreateMap<Model.PatientGlassDto, PatientGlass>().PreserveReferences();
            CreateMap<PatientGlass, Model.PatientGlassDto>();

            CreateMap<Model.PatientDisease, PatientDisease>()
                .ForMember(e => e.Disease, e => e.Ignore())
                .PreserveReferences();
            CreateMap<PatientDisease, Model.PatientDisease>();

            CreateMap<Model.PatientVisitDto, PatientVisit>().PreserveReferences();
            CreateMap<PatientVisit, Model.PatientVisitDto>();

          

            CreateMap<Model.PatientVisitGlassDto, PatientVisitGlass>().PreserveReferences();
            CreateMap<PatientVisitGlass, Model.PatientVisitGlassDto>();

            CreateMap<Model.PatientSickStoryDto, PatientSickStory>().PreserveReferences();
            CreateMap<PatientSickStory, Model.PatientSickStoryDto>();

      

            CreateMap<Model.PatientVisitsTestDto, PatientVisitsTest>()
                .ForMember(e => e.PatientVisit, e => e.Ignore())
                .PreserveReferences();
            CreateMap<PatientVisitsTest, Model.PatientVisitsTestDto>();

            CreateMap<Model.PatientVisitEyeTestDto, PatientVisitEyeTest>()
                .ForMember(e => e.PatientVisit, e => e.Ignore())
                .ForMember(e => e.EyeTest, e => e.Ignore())
                .PreserveReferences();
            CreateMap<PatientVisitEyeTest, Model.PatientVisitEyeTestDto>();

            CreateMap<Model.PatientVisitPrescriptionDto, PatientVisitPrescription>()
                .ForMember(e => e.Medicine, e => e.Ignore())
                .ForMember(e => e.MedicineUsage, e => e.Ignore())
                .ForMember(e => e.PatientVisit, e => e.Ignore());

            CreateMap<PatientVisitPrescription, Model.PatientVisitPrescriptionDto>();

            CreateMap<Model.ReadyPrescriptionDto, ReadyPrescription>().PreserveReferences();
            CreateMap<ReadyPrescription, Model.ReadyPrescriptionDto>();

            CreateMap<Model.ReadyPrescriptionMedicineDto, ReadyPrescriptionMedicine>().PreserveReferences();
            CreateMap<ReadyPrescriptionMedicine, Model.ReadyPrescriptionMedicineDto>();

            CreateMap<Model.PatientVisitLabTestDto, PatientVisitLabTest>()
                .ForMember(e => e.PatientVisit, e => e.Ignore())
                .PreserveReferences();
            CreateMap<PatientVisitLabTest, Model.PatientVisitLabTestDto>()
                .ForMember(e => e.RequestDate, e => e.MapFrom(x => x.PatientVisit.VisitDate));

            CreateMap<Model.PatientVisitDiagnosis, PatientVisitDiagnosis>().PreserveReferences();
            CreateMap<PatientVisitDiagnosis, Model.PatientVisitDiagnosis>();

            CreateMap<Model.PaymentDto, Payment>().PreserveReferences();
            CreateMap<Payment, Model.PaymentDto>();

            CreateMap<Model.PaymentTypeDto, PaymentType>().PreserveReferences();
            CreateMap<PaymentType, Model.PaymentTypeDto>();

            CreateMap<Model.LabTestDto, LabTest>().PreserveReferences();
            CreateMap<LabTest, Model.LabTestDto>();

            CreateMap<Model.TestDto, Test>().PreserveReferences();
            CreateMap<Test, Model.TestDto>();

            CreateMap<Model.MedicalCenterDto, MedicalCenter>().PreserveReferences();
            CreateMap<MedicalCenter, Model.MedicalCenterDto>();

            CreateMap<Model.OperationDto, Operation>().PreserveReferences();
            CreateMap<Operation, Model.OperationDto>();

            CreateMap<Model.PatientOperationDto, PatientOperation>().PreserveReferences();
            CreateMap<PatientOperation, Model.PatientOperationDto>();

            CreateMap<Model.PatientOperationSessionDto, PatientOperationSession>().PreserveReferences();
            CreateMap<PatientOperationSession, Model.PatientOperationSessionDto>();


            CreateMap<Model.UserDto, User>().ReverseMap();

            CreateMap<Model.RoleDto, Role>().ReverseMap();

            CreateMap<Model.PayoutDto, Payout>().ReverseMap();

            CreateMap<Model.NotPayReasonDto, NotPayReason>().ReverseMap();

            CreateMap<Model.ContactDto, Contact>().PreserveReferences();
            CreateMap<Contact, Model.ContactDto>();

            CreateMap<Model.ContactAccountDto, ContactAccount>().PreserveReferences();
            CreateMap<ContactAccount, Model.ContactAccountDto>();

            CreateMap<Model.ContactAccountPaymentDto, ContactAccountPayment>().PreserveReferences();
            CreateMap<ContactAccountPayment, Model.ContactAccountPaymentDto>();

            CreateMap<Model.ReminderDto, Reminder>().PreserveReferences();
            CreateMap<Reminder, Model.ReminderDto>();

            CreateMap<Model.VisitTypeDto, VisitType>().PreserveReferences();
            CreateMap<VisitType, Model.VisitTypeDto>();

            CreateMap<Model.ReservationDto, Reservation>().ReverseMap();
            CreateMap<Model.OldMedicineViewTableDto, OldMedicineViewTable>().ReverseMap();

            CreateMap<Model.OldPatientEyeImageDto, OldPatientEyeImage>().ReverseMap();

            CreateMap<Model.AccountPaymentCategoryDto, AccountPaymentCategory>().ReverseMap();

        }
    }
}
