using System;
using System.Collections.Generic;
using EyeClinic.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace EyeClinic.DataAccess
{
    public static class ApplicationDbInitializer
    {
        public static void SeedData(this ModelBuilder modelBuilder) {
            modelBuilder.Entity<User>()
                .HasData(new User {
                    Id = 1,
                    UserName = "Admin",
                    Password = "1",
                    RoleId = 1,
                    IsActive = true,
                    CreatedDate = new DateTime(2022, 1, 14)
                });

            modelBuilder.Entity<Role>()
                .HasData(new List<Role>
                {
                    new () { Id = 1, EnName = "Admin", ArName = "مسؤول", CreatedDate = new DateTime(2022, 1, 14) },
                    new() { Id = 2, EnName = "Reception", ArName = "استقبال", CreatedDate = new DateTime(2022, 1, 14) },
                    new() { Id = 3, EnName = "Nurse", ArName = "ممرضة", CreatedDate = new DateTime(2022, 1, 14) },
                    new() { Id = 4, EnName = "Doctor", ArName = "طبيب", CreatedDate = new DateTime(2022, 1, 14) },
                });

            modelBuilder.Entity<VisitType>()
                .HasData(new List<VisitType>()
                {
                    new () {Id = 1, EnName = "Review", ArName = "مراجعة", Cost = 0, CreatedDate = new DateTime(2022, 3, 27)},
                    new () {Id = 2, EnName = "Payment Review", ArName = "مراجعة مدفوعة", Cost = 10000, CreatedDate = new DateTime(2022, 3, 27)},
                    new () {Id = 3, EnName = "First Time", ArName = "اول مرة", Cost = 15000, CreatedDate = new DateTime(2022, 3, 27)},
                    new () {Id = 4, EnName = "New Visit", ArName = "زيارة جديدة", Cost = 15000, CreatedDate = new DateTime(2022, 3, 27)},
                    new () {Id = 5, EnName = "IOP", ArName = "IOP", Cost = 10000, CreatedDate = new DateTime(2022, 3, 27)},
                });
        }
    }
}
