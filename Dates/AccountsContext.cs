using System;
using hospital_api.Modules;
using Microsoft.EntityFrameworkCore;


namespace hospital_api.Dates
{
    public class AccountsContext : DbContext
    {
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<SpecialityModel> Specialities { get; set; }
        public DbSet<BlackListTokens> BlackListTokens { get; set; }
        public DbSet<PatientModel> Patients { get; set; }
        public DbSet<Icd10Model> Icd { get; set; }
        
        public DbSet<Inspection> Inspections { get; set; }
        public DbSet<Diagnosis> Diagnosis { get; set; }
        public DbSet<Consultation> Consultations { get; set; }
        public DbSet<Comment> Comments { get; set; }
        
        public AccountsContext(DbContextOptions<AccountsContext> options) : base(options)
        {
            // Database.EnsureCreated();
        }
    }
}

