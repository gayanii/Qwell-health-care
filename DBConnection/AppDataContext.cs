using Microsoft.EntityFrameworkCore;
using QWellApp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWellApp.DBConnection
{
    public class AppDataContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //const string DefaultConnectionString = @"Data Source=DESKTOP-KKFTKU6\MSSQLLOCALDB;Initial Catalog=Qwell;Integrated Security=True;TrustServerCertificate=True;Max Pool Size=1000;";
            const string DefaultConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Qwell;Integrated Security=True;TrustServerCertificate=True;Max Pool Size=1000;";
            //const string DefaultConnectionString = @"Data Source=tcp:qwellappdbserver.database.windows.net,1433;Initial Catalog=Qwell;User Id=CloudSAf50139bf@qwellappdbserver;Password=Gayani@321";
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(DefaultConnectionString);
        }

        public DbSet<User> Users {  get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductRecord> ProductRecords { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<LabTest> LabTests { get; set; }
        public DbSet<LabRecord> LabRecords { get; set; }
        public DbSet<LabRecordTest> LabRecordTests { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<ProductMedicalRecord> ProductMedicalRecords { get; set; }
        //public DbSet<Commission> Commissions { get; set; }
        public DbSet<ProcedureRecord> ProcedureRecords { get; set; }
        public DbSet<RecordType> Records { get; set; }
        public DbSet<ChannelRecord> ChannelRecords { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed data for RecordType
            modelBuilder.Entity<RecordType>().HasData(
                new RecordType { Id = 1, TypeName = "Medical" },
                new RecordType { Id = 2, TypeName = "Lab" },
                new RecordType { Id = 3, TypeName = "Procedure" },
                new RecordType { Id = 4, TypeName = "Channel" }
            );

            // Seed data for Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, RoleName = "Doctor" },
                new Role { Id = 2, RoleName = "Nursing Assistant" },
                new Role { Id = 3, RoleName = "Intern Nurse" },
                new Role { Id = 4, RoleName = "Junior Nurse" },
                new Role { Id = 5, RoleName = "Nursing Officer" },
                new Role { Id = 6, RoleName = "Senior Nurse" },
                new Role { Id = 7, RoleName = "Chief Nurse" },
                new Role { Id = 8, RoleName = "Nursing Supervisor" },
                new Role { Id = 9, RoleName = "Operation Manager" },
                new Role { Id = 10, RoleName = "Manager" },
                new Role { Id = 11, RoleName = "Director" }
            );

            modelBuilder.Entity<ProcedureRecord>()
                .HasOne(p => p.Nurse1)
                .WithMany()
                .HasForeignKey(p => p.Nurse1Id)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            modelBuilder.Entity<ProcedureRecord>()
                .HasOne(p => p.Nurse2)
                .WithMany()
                .HasForeignKey(p => p.Nurse2Id)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            modelBuilder.Entity<ProductMedicalRecord>(entity =>
            {
                entity.HasOne(e => e.MedicalRecord)
                    .WithMany()
                    .HasForeignKey(e => e.MedicalRecordId)
                    .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

                entity.HasOne(e => e.ProcedureRecord)
                    .WithMany()
                    .HasForeignKey(e => e.ProcedureRecordId)
                    .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

                entity.HasOne(e => e.LabRecord)
                    .WithMany()
                    .HasForeignKey(e => e.LabRecordId)
                    .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

                entity.HasOne(e => e.ChannelRecord)
                    .WithMany()
                    .HasForeignKey(e => e.ChannelRecordId)
                    .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete
            });
        }
    }
}
