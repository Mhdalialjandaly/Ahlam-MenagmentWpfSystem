using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace EyeClinic.DataAccess.Entities
{
    public partial class EyeClinicContext : DbContext
    {
        public EyeClinicContext() {
        }

        public EyeClinicContext(DbContextOptions<EyeClinicContext> options)
            : base(options) {
        }
        public virtual DbSet<WareHouseReadyMaterial> WareHouseReadyMaterials { get; set; }

        public virtual DbSet<MarketsEntry> MarketsEntries { get; set; }
        public virtual DbSet<MarketsExtry> MarketsExtries { get; set; }
        public virtual DbSet<MarketsProducts>  MarketsProducts { get; set; }
       

        public virtual DbSet<PublicCustomer> PublicCustomers { get; set; }
        public virtual DbSet<PublicCustomerOrder> PublicCustomerOrders { get; set; }
        public virtual DbSet<CustomerNoteStory> CustomerNoteStories { get; set; }
        public virtual DbSet<ReadyProduct> ReadyProducts { get; set; }
        public virtual DbSet<ReadyItemProducting> Producting { get; set; }
        public virtual DbSet<Customer> Customers { set; get; }
        public virtual DbSet<CartoonLabels> CartoonLabels { get; set; }
        public virtual DbSet<DeniedUser> DeniedUsers { get; set; }
        public virtual DbSet<AccountPaymentCategory> AccountPaymentCategories { get; set; }
        public virtual DbSet<AppLanguage> AppLanguages { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<ContactAccount> ContactAccounts { get; set; }
        public virtual DbSet<ContactAccountPayment> ContactAccountPayments { get; set; }
        public virtual DbSet<Diagnosis> Diagnoses { get; set; }
        public virtual DbSet<Disease> Diseases { get; set; }
        public virtual DbSet<EyeTest> EyeTests { get; set; }
        public virtual DbSet<Glass> Glasses { get; set; }
        public virtual DbSet<LabTest> LabTests { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<MartialStatus> MartialStatuses { get; set; }
        public virtual DbSet<MedicalCenter> MedicalCenters { get; set; }
        public virtual DbSet<Medicine> Medicines { get; set; }
        public virtual DbSet<MedicineType> MedicineTypes { get; set; }
        public virtual DbSet<MedicineUsage> MedicineUsages { get; set; }
        public virtual DbSet<NotPayReason> NotPayReasons { get; set; }
        public virtual DbSet<OldMedicineViewTable> OldMedicineViewTables { get; set; }
        public virtual DbSet<OldPatientEyeImage> OldPatientEyeImages { get; set; }
        public virtual DbSet<Operation> Operations { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<PatientDisease> PatientDiseases { get; set; }
        public virtual DbSet<PatientGlass> PatientGlasses { get; set; }
        public virtual DbSet<PatientOperation> PatientOperations { get; set; }
        public virtual DbSet<PatientOperationSession> PatientOperationSessions { get; set; }
        public virtual DbSet<PatientSickStory> PatientSickStories { get; set; }
        public virtual DbSet<PatientVisit> PatientVisits { get; set; }
        public virtual DbSet<Queue> Queues { get; set; }
        public virtual DbSet<OrderQueue> OrderQueues { get; set; }
        public virtual DbSet<PatientVisitDiagnosis> PatientVisitDiagnoses { get; set; }
        public virtual DbSet<PatientVisitEyeTest> PatientVisitEyeTests { get; set; }
        public virtual DbSet<PatientVisitEyeTestHistory> PatientVisitEyeTestHistories { get; set; }
        public virtual DbSet<PatientVisitGlass> PatientVisitGlasses { get; set; }
        public virtual DbSet<PatientVisitLabTest> PatientVisitLabTests { get; set; }
        public virtual DbSet<PatientVisitPrescription> PatientVisitPrescriptions { get; set; }
        public virtual DbSet<PatientVisitsTest> PatientVisitsTests { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<PaymentType> PaymentTypes { get; set; }
        public virtual DbSet<Payout> Payouts { get; set; }
        public virtual DbSet<ReadyPrescription> ReadyPrescriptions { get; set; }
        public virtual DbSet<ReadyPrescriptionMedicine> ReadyPrescriptionMedicines { get; set; }
        public virtual DbSet<Reminder> Reminders { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Test> Tests { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<VisitType> VisitTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AccountPaymentCategory>(entity => {
                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<AppLanguage>(entity => {
                entity.ToTable("AppLanguage");

                entity.Property(e => e.CreatedDate).HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");

                entity.Property(e => e.PrinterName).HasMaxLength(200);
            });

            modelBuilder.Entity<Contact>(entity => {
                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.ContactName)
                    .IsRequired()
                    .HasMaxLength(75);

                entity.Property(e => e.ContactPhones).HasMaxLength(400);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<ContactAccount>(entity => {
                entity.HasIndex(e => e.ContactId, "IX_ContactAccounts_ContactId");

                entity.Property(e => e.AccountName)
                    .IsRequired()
                    .HasMaxLength(75);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Notes).HasMaxLength(400);

                entity.Property(e => e.Remaining).HasComputedColumnSql("([TotalCost]-isnull([dbo].[GetTotalContactAccountPayments]([Id],[PayOutAccount]),(0)))", false);

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.ContactAccounts)
                    .HasForeignKey(d => d.ContactId)
                    .HasConstraintName("FK_ContactAccounts_ContactId");
            });

            modelBuilder.Entity<ContactAccountPayment>(entity => {
                entity.ToTable("ContactAccountPayment");

                entity.HasIndex(e => e.ContactAccountId, "IX_ContactAccountPayment_ContactAccountId");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Notes).HasMaxLength(400);

                entity.Property(e => e.PaymentDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.ContactAccountPayments)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ContactAccountPayment_ContactAccountPaymentCategories_CategoryId");

                entity.HasOne(d => d.ContactAccount)
                    .WithMany(p => p.ContactAccountPayments)
                    .HasForeignKey(d => d.ContactAccountId);

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.ContactAccountPayments)
                    .HasForeignKey(d => d.ContactId);
            });

            modelBuilder.Entity<Diagnosis>(entity => {
                entity.ToTable("Diagnosis");

                entity.Property(e => e.ArName)
                    .IsRequired()
                    .HasMaxLength(75);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EnName)
                    .IsRequired()
                    .HasMaxLength(75);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Disease>(entity => {
                entity.Property(e => e.ArName)
                    .IsRequired()
                    .HasMaxLength(75);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EnName).HasMaxLength(200);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<EyeTest>(entity => {
                entity.Property(e => e.ArName)
                    .IsRequired()
                    .HasMaxLength(75);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EnName)
                    .IsRequired()
                    .HasMaxLength(75);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Glass>(entity => {
                entity.Property(e => e.ArName)
                    .IsRequired()
                    .HasMaxLength(75);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EnName).HasMaxLength(200);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<LabTest>(entity => {
                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LabTestName)
                    .IsRequired()
                    .HasMaxLength(75);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Location>(entity => {
                entity.Property(e => e.ArName)
                    .IsRequired()
                    .HasMaxLength(75);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EnName).HasMaxLength(200);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<MartialStatus>(entity => {
                entity.ToTable("MartialStatus");

                entity.Property(e => e.ArName)
                    .IsRequired()
                    .HasMaxLength(75);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EnName).HasMaxLength(200);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<MedicalCenter>(entity => {
                entity.Property(e => e.ArName)
                    .IsRequired()
                    .HasMaxLength(75);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EnName)
                    .IsRequired()
                    .HasMaxLength(75);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Medicine>(entity => {
                entity.HasIndex(e => e.MedicineTypeId, "IX_Medicines_MedicineTypeId");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.MedicineName).IsRequired();

                entity.HasOne(d => d.MedicineType)
                    .WithMany(p => p.Medicines)
                    .HasForeignKey(d => d.MedicineTypeId);
            });

            modelBuilder.Entity<MedicineType>(entity => {
                entity.Property(e => e.ArName)
                    .IsRequired()
                    .HasMaxLength(75);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EnName)
                    .IsRequired()
                    .HasMaxLength(75);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<MedicineUsage>(entity => {
                entity.HasIndex(e => e.UsageMedicineTypeId, "IX_MedicineUsages_UsageMedicineTypeId");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.UsageName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.UsageMedicineType)
                    .WithMany(p => p.MedicineUsages)
                    .HasForeignKey(d => d.UsageMedicineTypeId);
            });

            modelBuilder.Entity<NotPayReason>(entity => {
                entity.Property(e => e.ArName).HasMaxLength(200);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EnName).HasMaxLength(200);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<OldMedicineViewTable>(entity => {
                entity.ToTable("OldMedicineViewTable");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Index).HasColumnName("_index");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.MedicineName).IsRequired();

                entity.Property(e => e.MedicineType).IsRequired();

                entity.Property(e => e.VisitDate).HasColumnType("date");

                entity.HasOne(d => d.PatientVisit)
                    .WithMany(p => p.OldMedicineViewTables)
                    .HasForeignKey(d => d.PatientVisitId)
                    .HasConstraintName("FK_OldMedicineViewTable_PatientVisit_PatientVisitId");
            });

            modelBuilder.Entity<OldPatientEyeImage>(entity => {
                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Operation>(entity => {
                entity.Property(e => e.ArName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(75);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EnName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Patient>(entity => {
                entity.HasIndex(e => e.GlassId, "IX_Patients_GlassId");

                entity.HasIndex(e => e.LocationId, "IX_Patients_LocationId");

                entity.HasIndex(e => e.MartialStatusId, "IX_Patients_MartialStatusId");

                entity.Property(e => e.AgeTypeName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FatherName).HasMaxLength(200);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.HomePhone).HasMaxLength(200);

                entity.Property(e => e.JobTitle).HasMaxLength(200);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.MotherName).HasMaxLength(200);

                entity.Property(e => e.Nationality).HasMaxLength(200);

                entity.Property(e => e.WorkPhone).HasMaxLength(200);

                entity.HasOne(d => d.Glass)
                    .WithMany(p => p.Patients)
                    .HasForeignKey(d => d.GlassId);

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Patients)
                    .HasForeignKey(d => d.LocationId);

                entity.HasOne(d => d.MartialStatus)
                    .WithMany(p => p.Patients)
                    .HasForeignKey(d => d.MartialStatusId);
            });

            modelBuilder.Entity<PatientDisease>(entity => {
                entity.HasIndex(e => e.DiseaseId, "IX_PatientDiseases_DiseaseId");

                entity.HasIndex(e => e.PatientId, "IX_PatientDiseases_PatientId");

                entity.Property(e => e.AgeOfInjury).HasMaxLength(200);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastCheckMeasure).HasMaxLength(200);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.MaxMeasure).HasMaxLength(200);

                entity.HasOne(d => d.Disease)
                    .WithMany(p => p.PatientDiseases)
                    .HasForeignKey(d => d.DiseaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.PatientDiseases)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<PatientGlass>(entity => {
                entity.ToTable("PatientGlass");

                entity.HasIndex(e => e.PatientId, "Unique_PatientGlass_PatientId")
                    .IsUnique();

                entity.Property(e => e.AddVision).HasColumnName("Add_Vision");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LAxis)
                    .HasMaxLength(100)
                    .HasColumnName("L_Axis");

                entity.Property(e => e.LAxis2)
                    .HasMaxLength(100)
                    .HasColumnName("L_Axis2");

                entity.Property(e => e.LBase)
                    .HasMaxLength(5)
                    .HasColumnName("L_Base");

                entity.Property(e => e.LBase2)
                    .HasMaxLength(5)
                    .HasColumnName("L_Base2");

                entity.Property(e => e.LCyl)
                    .HasMaxLength(100)
                    .HasColumnName("L_Cyl");

                entity.Property(e => e.LCyl2)
                    .HasMaxLength(100)
                    .HasColumnName("L_Cyl2");

                entity.Property(e => e.LIpd)
                    .HasMaxLength(100)
                    .HasColumnName("L_IPD");

                entity.Property(e => e.LIpd2)
                    .HasMaxLength(100)
                    .HasColumnName("L_IPD2");

                entity.Property(e => e.LPrism)
                    .HasMaxLength(100)
                    .HasColumnName("L_Prism");

                entity.Property(e => e.LPrism2)
                    .HasMaxLength(100)
                    .HasColumnName("L_Prism2");

                entity.Property(e => e.LSph)
                    .HasMaxLength(100)
                    .HasColumnName("L_Sph");

                entity.Property(e => e.LSph2)
                    .HasMaxLength(100)
                    .HasColumnName("L_Sph2");

                entity.Property(e => e.LVa)
                    .HasMaxLength(100)
                    .HasColumnName("L_VA");

                entity.Property(e => e.LVa2)
                    .HasMaxLength(100)
                    .HasColumnName("L_VA2");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RAxis)
                    .HasMaxLength(100)
                    .HasColumnName("R_Axis");

                entity.Property(e => e.RAxis2)
                    .HasMaxLength(100)
                    .HasColumnName("R_Axis2");

                entity.Property(e => e.RBase)
                    .HasMaxLength(5)
                    .HasColumnName("R_Base");

                entity.Property(e => e.RBase2)
                    .HasMaxLength(5)
                    .HasColumnName("R_Base2");

                entity.Property(e => e.RCyl)
                    .HasMaxLength(100)
                    .HasColumnName("R_Cyl");

                entity.Property(e => e.RCyl2)
                    .HasMaxLength(100)
                    .HasColumnName("R_Cyl2");

                entity.Property(e => e.RIpd)
                    .HasMaxLength(100)
                    .HasColumnName("R_IPD");

                entity.Property(e => e.RIpd2)
                    .HasMaxLength(100)
                    .HasColumnName("R_IPD2");

                entity.Property(e => e.RPrism)
                    .HasMaxLength(100)
                    .HasColumnName("R_Prism");

                entity.Property(e => e.RPrism2)
                    .HasMaxLength(100)
                    .HasColumnName("R_Prism2");

                entity.Property(e => e.RSph)
                    .HasMaxLength(100)
                    .HasColumnName("R_Sph");

                entity.Property(e => e.RSph2)
                    .HasMaxLength(100)
                    .HasColumnName("R_Sph2");

                entity.Property(e => e.RVa)
                    .HasMaxLength(100)
                    .HasColumnName("R_VA");

                entity.Property(e => e.RVa2)
                    .HasMaxLength(100)
                    .HasColumnName("R_VA2");

                entity.HasOne(d => d.Patient)
                    .WithOne(p => p.PatientGlass)
                    .HasForeignKey<PatientGlass>(d => d.PatientId)
                    .HasConstraintName("FK_PatientGlass_PatientId");
            });

            modelBuilder.Entity<PatientOperation>(entity => {
                entity.HasIndex(e => e.LeftEyeOperationId, "IX_PatientOperations_LeftEyeOperationId");

                entity.HasIndex(e => e.MedicalCenterId, "IX_PatientOperations_MedicalCenterId");

                entity.HasIndex(e => e.PatientId, "IX_PatientOperations_PatientId");

                entity.HasIndex(e => e.RightEyeOperationId, "IX_PatientOperations_RightEyeOperationId");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DownPayment).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.OperationDate).HasColumnType("date");

                entity.Property(e => e.PaymentLocation).HasMaxLength(200);

                entity.Property(e => e.PhotoSource).HasMaxLength(200);

                entity.Property(e => e.Remaining).HasComputedColumnSql("(isnull(([TotalCost]-[DownPayment])-[dbo].[GetTotalPaymentsByPatientOperationId]([Id]),(0)))", false);

                entity.HasOne(d => d.LeftEyeOperation)
                    .WithMany(p => p.PatientOperationLeftEyeOperations)
                    .HasForeignKey(d => d.LeftEyeOperationId);

                entity.HasOne(d => d.MedicalCenter)
                    .WithMany(p => p.PatientOperations)
                    .HasForeignKey(d => d.MedicalCenterId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.PatientOperations)
                    .HasForeignKey(d => d.PatientId);

                entity.HasOne(d => d.RightEyeOperation)
                    .WithMany(p => p.PatientOperationRightEyeOperations)
                    .HasForeignKey(d => d.RightEyeOperationId);
            });

            modelBuilder.Entity<PatientOperationSession>(entity => {
                entity.HasIndex(e => e.PatientOperationId, "IX_PatientOperationSessions_PatientOperationId");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.SessionDate).HasColumnType("datetime");

                entity.HasOne(d => d.PatientOperation)
                    .WithMany(p => p.PatientOperationSessions)
                    .HasForeignKey(d => d.PatientOperationId);
            });

            modelBuilder.Entity<PatientSickStory>(entity => {
                entity.HasKey(e => e.PatientId)
                    .HasName("PK_PatientSickStory_PatientId");

                entity.ToTable("PatientSickStory");

                entity.Property(e => e.PatientId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Patient)
                    .WithOne(p => p.PatientSickStory)
                    .HasForeignKey<PatientSickStory>(d => d.PatientId);
            });

            modelBuilder.Entity<PatientVisit>(entity => {
                entity.HasIndex(e => e.NotPaymentReasonId, "IX_PatientVisits_NotPaymentReasonId");

                entity.HasIndex(e => e.VisitDate, "IX_PatientVisits_PatientId_VisitDate");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Remaining).HasComputedColumnSql("(case when [NotPaymentReasonId] IS NULL then isnull([Cost]-[Payment],(0)) else (0) end)", false);

                entity.Property(e => e.ReviewDate).HasColumnType("datetime");

                entity.Property(e => e.VisitDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.NotPaymentReason)
                    .WithMany(p => p.PatientVisits)
                    .HasForeignKey(d => d.NotPaymentReasonId)
                    .HasConstraintName("FK_PatientVisits_NotPayReasons_Id");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.PatientVisits)
                    .HasForeignKey(d => d.PatientId);
            });

            modelBuilder.Entity<Queue>(entity => {
                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Remaining).HasComputedColumnSql("(case when [NotPaymentReasonId] IS NULL then isnull([Cost]-[Payment],(0)) else (0) end)", false);

                entity.Property(e => e.ReviewDate).HasColumnType("datetime");

                entity.Property(e => e.VisitDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.Queues)
                    .HasForeignKey(d => d.PatientId);
            });

            modelBuilder.Entity<PatientVisitDiagnosis>(entity => {
                entity.ToTable("PatientVisitDiagnosis");

                entity.HasIndex(e => e.DiagnosisId, "IX_PatientVisitDiagnosis_DiagnosisId");

                entity.HasIndex(e => e.PatientVisitId, "IX_PatientVisitDiagnosis_PatientVisitId");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Diagnosis)
                    .WithMany(p => p.PatientVisitDiagnoses)
                    .HasForeignKey(d => d.DiagnosisId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.PatientVisit)
                    .WithMany(p => p.PatientVisitDiagnoses)
                    .HasForeignKey(d => d.PatientVisitId);
            });

            modelBuilder.Entity<PatientVisitEyeTest>(entity => {
                entity.HasIndex(e => e.EyeTestId, "IX_PatientVisitEyeTests_EyeTestId");

                entity.HasIndex(e => e.PatientVisitId, "IX_PatientVisitEyeTests_PatientVisitId");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.LeftEyeResult).HasMaxLength(200);

                entity.Property(e => e.RightEyeResult).HasMaxLength(200);

                entity.HasOne(d => d.EyeTest)
                    .WithMany(p => p.PatientVisitEyeTests)
                    .HasForeignKey(d => d.EyeTestId);

                entity.HasOne(d => d.PatientVisit)
                    .WithMany(p => p.PatientVisitEyeTests)
                    .HasForeignKey(d => d.PatientVisitId);
            });

            modelBuilder.Entity<PatientVisitEyeTestHistory>(entity => {
                entity.HasNoKey();

                entity.ToTable("PatientVisitEyeTestHistory");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.LeftEyeResult).HasMaxLength(200);

                entity.Property(e => e.RightEyeResult).HasMaxLength(200);
            });

            modelBuilder.Entity<PatientVisitGlass>(entity => {
                entity.ToTable("PatientVisitGlass");

                entity.HasIndex(e => e.PatientVisitId, "IX_PatientVisitGlass_PatientVisitId");

                entity.Property(e => e.AddVision).HasColumnName("Add_Vision");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LAxis)
                    .HasMaxLength(100)
                    .HasColumnName("L_Axis");

                entity.Property(e => e.LAxis2)
                    .HasMaxLength(100)
                    .HasColumnName("L_Axis2");

                entity.Property(e => e.LBase)
                    .HasMaxLength(5)
                    .HasColumnName("L_Base");

                entity.Property(e => e.LBase2)
                    .HasMaxLength(5)
                    .HasColumnName("L_Base2");

                entity.Property(e => e.LCyl)
                    .HasMaxLength(100)
                    .HasColumnName("L_Cyl");

                entity.Property(e => e.LCyl2)
                    .HasMaxLength(100)
                    .HasColumnName("L_Cyl2");

                entity.Property(e => e.LIpd)
                    .HasMaxLength(100)
                    .HasColumnName("L_IPD");

                entity.Property(e => e.LIpd2)
                    .HasMaxLength(100)
                    .HasColumnName("L_IPD2");

                entity.Property(e => e.LPrism)
                    .HasMaxLength(100)
                    .HasColumnName("L_Prism");

                entity.Property(e => e.LPrism2)
                    .HasMaxLength(100)
                    .HasColumnName("L_Prism2");

                entity.Property(e => e.LSph)
                    .HasMaxLength(100)
                    .HasColumnName("L_Sph");

                entity.Property(e => e.LSph2)
                    .HasMaxLength(100)
                    .HasColumnName("L_Sph2");

                entity.Property(e => e.LVa)
                    .HasMaxLength(100)
                    .HasColumnName("L_VA");

                entity.Property(e => e.LVa2)
                    .HasMaxLength(100)
                    .HasColumnName("L_VA2");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RAxis)
                    .HasMaxLength(100)
                    .HasColumnName("R_Axis");

                entity.Property(e => e.RAxis2)
                    .HasMaxLength(100)
                    .HasColumnName("R_Axis2");

                entity.Property(e => e.RBase)
                    .HasMaxLength(5)
                    .HasColumnName("R_Base");

                entity.Property(e => e.RBase2)
                    .HasMaxLength(5)
                    .HasColumnName("R_Base2");

                entity.Property(e => e.RCyl)
                    .HasMaxLength(100)
                    .HasColumnName("R_Cyl");

                entity.Property(e => e.RCyl2)
                    .HasMaxLength(100)
                    .HasColumnName("R_Cyl2");

                entity.Property(e => e.RIpd)
                    .HasMaxLength(100)
                    .HasColumnName("R_IPD");

                entity.Property(e => e.RIpd2)
                    .HasMaxLength(100)
                    .HasColumnName("R_IPD2");

                entity.Property(e => e.RPrism)
                    .HasMaxLength(100)
                    .HasColumnName("R_Prism");

                entity.Property(e => e.RPrism2)
                    .HasMaxLength(100)
                    .HasColumnName("R_Prism2");

                entity.Property(e => e.RSph)
                    .HasMaxLength(100)
                    .HasColumnName("R_Sph");

                entity.Property(e => e.RSph2)
                    .HasMaxLength(100)
                    .HasColumnName("R_Sph2");

                entity.Property(e => e.RVa)
                    .HasMaxLength(100)
                    .HasColumnName("R_VA");

                entity.Property(e => e.RVa2)
                    .HasMaxLength(100)
                    .HasColumnName("R_VA2");

                entity.HasOne(d => d.PatientVisit)
                    .WithMany(p => p.PatientVisitGlasses)
                    .HasForeignKey(d => d.PatientVisitId)
                    .HasConstraintName("FK_PatientVisitGlass_PatientVisitId");
            });

            modelBuilder.Entity<PatientVisitLabTest>(entity => {
                entity.HasIndex(e => e.LabTestId, "IX_PatientVisitLabTests_LabTestId");

                entity.HasIndex(e => e.PatientVisitId, "IX_PatientVisitLabTests_PatientVisitId");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Result).HasMaxLength(400);

                entity.HasOne(d => d.LabTest)
                    .WithMany(p => p.PatientVisitLabTests)
                    .HasForeignKey(d => d.LabTestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PatientVisitLabTests_LabTestId");

                entity.HasOne(d => d.PatientVisit)
                    .WithMany(p => p.PatientVisitLabTests)
                    .HasForeignKey(d => d.PatientVisitId)
                    .HasConstraintName("FK_PatientVisitLabTests_PatientVisitId");
            });

            modelBuilder.Entity<PatientVisitPrescription>(entity => {
                entity.HasIndex(e => e.MedicineId, "IX_PatientVisitPrescriptions_MedicineId");

                entity.HasIndex(e => e.MedicineUsageId, "IX_PatientVisitPrescriptions_MedicineUsageId");

                entity.HasIndex(e => e.PatientVisitId, "IX_PatientVisitPrescriptions_PatientVisitId");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Medicine)
                    .WithMany(p => p.PatientVisitPrescriptions)
                    .HasForeignKey(d => d.MedicineId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.MedicineUsage)
                    .WithMany(p => p.PatientVisitPrescriptions)
                    .HasForeignKey(d => d.MedicineUsageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PatientVisitPrescriptions_MedicineUsages_UsageId");

                entity.HasOne(d => d.PatientVisit)
                    .WithMany(p => p.PatientVisitPrescriptions)
                    .HasForeignKey(d => d.PatientVisitId);
            });

            modelBuilder.Entity<PatientVisitsTest>(entity => {
                entity.HasIndex(e => e.PatientVisitId, "IX_PatientVisitsTests_PatientVisitId");

                entity.HasIndex(e => e.TestId, "IX_PatientVisitsTests_TestId");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Dropped)
                    .IsRequired()
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.ImageNumber).HasMaxLength(300);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.PatientVisit)
                    .WithMany(p => p.PatientVisitsTests)
                    .HasForeignKey(d => d.PatientVisitId);

                entity.HasOne(d => d.Test)
                    .WithMany(p => p.PatientVisitsTests)
                    .HasForeignKey(d => d.TestId);
            });

            modelBuilder.Entity<Payment>(entity => {
                entity.HasIndex(e => e.PaymentTypeId, "IX_Payments_PaymentTypeId");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Paid)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.PaymentDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ReminderDate).HasColumnType("datetime");

                entity.HasOne(d => d.PaymentType)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.PaymentTypeId);
            });

            modelBuilder.Entity<PaymentType>(entity => {
                entity.Property(e => e.BeneficiaryName).HasMaxLength(200);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Debt)
                    .IsRequired()
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Remaining).HasComputedColumnSql("(isnull([TotalCost]-[dbo].[GetTotalPaymentByPaymentTypeId]([Id]),(0)))", false);

                entity.Property(e => e.TotalPayments).HasComputedColumnSql("([dbo].[GetTotalPaymentByPaymentTypeId]([Id]))", false);

                entity.Property(e => e.TypeName)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<Payout>(entity => {
                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DateTime).HasColumnType("date");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<ReadyPrescription>(entity => {
                entity.Property(e => e.ArName)
                    .IsRequired()
                    .HasMaxLength(75);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EnName)
                    .IsRequired()
                    .HasMaxLength(75);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<ReadyPrescriptionMedicine>(entity => {
                entity.HasIndex(e => e.MedicineId, "IX_ReadyPrescriptionMedicines_MedicineId");

                entity.HasIndex(e => e.MedicineUsageId, "IX_ReadyPrescriptionMedicines_MedicineUsageId");

                entity.HasIndex(e => e.ReadyPrescriptionId, "IX_ReadyPrescriptionMedicines_ReadyPrescriptionId");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Medicine)
                    .WithMany(p => p.ReadyPrescriptionMedicines)
                    .HasForeignKey(d => d.MedicineId);

                entity.HasOne(d => d.MedicineUsage)
                    .WithMany(p => p.ReadyPrescriptionMedicines)
                    .HasForeignKey(d => d.MedicineUsageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReadyPrescriptionMedicines_MedicineUsages");

                entity.HasOne(d => d.ReadyPrescription)
                    .WithMany(p => p.ReadyPrescriptionMedicines)
                    .HasForeignKey(d => d.ReadyPrescriptionId);
            });

            modelBuilder.Entity<Reminder>(entity => {
                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ReminderDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ReminderText)
                    .IsRequired()
                    .HasMaxLength(400);
            });

            modelBuilder.Entity<Reservation>(entity => {
                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PatientName)
                    .IsRequired()
                    .HasMaxLength(75);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(75);

                entity.Property(e => e.ReservationDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Role>(entity => {
                entity.Property(e => e.ArName)
                    .IsRequired()
                    .HasMaxLength(75);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EnName)
                    .IsRequired()
                    .HasMaxLength(75);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Test>(entity => {
                entity.Property(e => e.ArName)
                    .IsRequired()
                    .HasMaxLength(75);

                entity.Property(e => e.Code).HasMaxLength(200);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EnName)
                    .IsRequired()
                    .HasMaxLength(75);

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<User>(entity => {
                entity.HasIndex(e => e.RoleId, "IX_Users_RoleId");

                entity.HasIndex(e => e.UserName, "Unique_Users_UserName")
                    .IsUnique();

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<VisitType>(entity => {
                entity.ToTable("VisitType");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
