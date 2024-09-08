using System;
using System.IO;
using Microsoft.EntityFrameworkCore;

// ReSharper disable once CheckNamespace
namespace EyeClinic.DataAccess.Entities
{
    public partial class EyeClinicContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder) {
            modelBuilder.SeedData();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            if (!optionsBuilder.IsConfigured) {
                if (string.IsNullOrWhiteSpace(ConnectionHandler.ConnectionString))
                    ConnectionHandler.ConnectionString =
                //@"Server=asrvdc\amn;Database=AhlamCoDb;User Id=ah;Password=Asd123zxc;";
                File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "//conn.txt");

                optionsBuilder.UseSqlServer(ConnectionHandler.ConnectionString);
            }
        }
    }

    public static class ConnectionHandler
    {
        public static string ConnectionString { get; set; }
    }
}
