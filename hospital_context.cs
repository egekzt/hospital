using System;
using hospital.Models;
using Microsoft.EntityFrameworkCore;

namespace hospital
{
    public class hospitalContext : DbContext, IhospitalContext
    {

        public hospitalContext()
        {
        }

        public hospitalContext(DbContextOptions<hospitalContext> options)
            : base(options)
        {
        }
        public DbSet<Doctor>Doctor { get; set; }
        public DbSet<Secretary> Secretary { get; set; }
        public DbSet<Appointment> Appointment { get; set; }
        public DbSet<Building> Building { get; set; }
        public DbSet<Patient> Patient { get; set; }
        public DbSet<Department> Department { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("User ID =kiziltan;Password=123456;Host=127.0.0.1;Port=5432;Database=kiziltan;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.ToTable("doctor");

                entity.HasKey(e => e.ssn);
                entity.Property(e => e.fullName).HasColumnName("full_name");
                entity.Property(e => e.ssn).HasColumnName("ssn");
                entity.Property(e => e.departmentId).HasColumnName("department_id");
                entity.Property(e => e.phoneNumer).HasColumnName("phone_number");
                entity.Property(e => e.email).HasColumnName("email");
                entity.Property(e => e.password).HasColumnName("password");
            });

            modelBuilder.Entity<Secretary>(entity =>
            {
                entity.ToTable("secretary");

                entity.HasKey(e => e.ssn);
                entity.Property(e => e.ssn).HasColumnName("ssn");
                entity.Property(e => e.name).HasColumnName("name");
                entity.Property(e => e.phoneNumber).HasColumnName("phone_number");
                entity.Property(e => e.email).HasColumnName("email");
                entity.Property(e => e.password).HasColumnName("password");

            });

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.ToTable("appointment");

                entity.HasKey(e => e.id);
                entity.Property(e => e.id).HasColumnName("id");
                entity.Property(e => e.doctorSsn).HasColumnName("doctor_ssn");
                entity.Property(e => e.patientSsn).HasColumnName("patient_ssn");
                entity.Property(e => e.adressOfBuilding).HasColumnName("adress_of_building");
                entity.Property(e => e.date).HasColumnName("date");
                entity.Property(e => e.roomNumber).HasColumnName("room_number");
            });

            modelBuilder.Entity<Building>(entity =>
            {
                entity.ToTable("building");

                entity.HasKey(e => e.adress);
                entity.Property(e => e.numberOfRooms).HasColumnName("number_of_rooms");
                entity.Property(e => e.departmentId).HasColumnName("department_id");
                entity.Property(e => e.adress).HasColumnName("adress");
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.ToTable("patient");

                entity.HasKey(e => e.ssn);
                entity.Property(e => e.fullName).HasColumnName("full_name");
                entity.Property(e => e.ssn).HasColumnName("ssn");
                entity.Property(e => e.phoneNumber).HasColumnName("phone_number");
                entity.Property(e => e.email).HasColumnName("email");
                entity.Property(e => e.password).HasColumnName("password");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("department");

                entity.HasKey(e => e.id);
                entity.Property(e => e.id).HasColumnName("id");
                entity.Property(e => e.name).HasColumnName("name");
                entity.Property(e => e.headOfDepartment).HasColumnName("head_of_department");
            });
        }
    }
}

