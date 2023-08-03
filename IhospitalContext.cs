using System;
using hospital.Models;
using Microsoft.EntityFrameworkCore;
namespace hospital;

public interface IhospitalContext
{
    DbSet<Doctor> Doctor { get; set; }
    DbSet<Secretary> Secretary { get; set; }
    DbSet<Appointment> Appointment { get; set; }
    DbSet<Building> Building { get; set; }
    DbSet<Patient> Patient { get; set; }
    DbSet<Department> Department { get; set; }

    int SaveChanges();
}
