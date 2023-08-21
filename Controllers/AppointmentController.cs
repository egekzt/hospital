using System;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using hospital.Models;
using hospital;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using PagedList;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
//pagination,filter sorting
//manage deletion problem,manage not displaying correctly
//check creation
//user roles, authentication, login landing
//set up users first, and then set up permissions after, give every user required permissions
namespace hospital.Controllers;

public class AppointmentController : Controller
{
    private readonly IhospitalContext _dbContext = new hospitalContext();

    public AppointmentController()
    {

    }



    public IActionResult Index(string sortOrder, string searchString, int? page)
    {
        int pageSize = 10; // Set the number of items you want to display per page
        int pageNumber = page ?? 1; // If the page parameter is null, default to page 1

        // Retrieve the list of appointments from the database
        List<Appointment> appointments = _dbContext.Appointment.ToList();

        // Sorting logic based on the sortOrder parameter
        ViewBag.IdSort = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
        ViewBag.DateSort = sortOrder == "date" ? "date_desc" : "date";
        ViewBag.PatientSsnSort = sortOrder == "patientSsn" ? "patientSsn_desc" : "patientSsn";
        ViewBag.DoctorSsnSort = sortOrder == "doctorSsn" ? "doctorSsn_desc" : "doctorSsn";
        ViewBag.AdressOfBuildingSort = sortOrder == "adressOfBuilding" ? "adressOfBuilding_desc" : "adressOfBuilding";
        ViewBag.RoomNumberSort = sortOrder == "roomNumber" ? "roomNumber_desc" : "roomNumber";
        if (!String.IsNullOrEmpty(searchString))
        {
            searchString = searchString.ToLower();
            appointments = appointments.Select(a =>
            {
                a.Patient = _dbContext.Patient.FirstOrDefault(p => p.ssn == a.patientSsn);
                a.Doctor = _dbContext.Doctor.FirstOrDefault(d => d.ssn == a.doctorSsn);
                return a;
            }).Where(a =>
                a.date.ToString().Contains(searchString) ||
                (a.Patient?.fullName?.ToLower() ?? "").Contains(searchString) ||
                (a.Doctor?.fullName?.ToLower() ?? "").Contains(searchString) ||
                a.adressOfBuilding.ToLower().Contains(searchString) ||
                a.roomNumber.ToString().Contains(searchString)
            ).ToList();
        }

        
        appointments = appointments.Select(a =>
        {
            a.Patient = _dbContext.Patient.FirstOrDefault(p => p.ssn == a.patientSsn);
            a.Doctor = _dbContext.Doctor.FirstOrDefault(d => d.ssn == a.doctorSsn);
            return a;
        }).ToList();

        switch (sortOrder)
        {
            case "id_desc":
                appointments = appointments.OrderByDescending(a => a.id).ToList();
                break;
            case "date":
                appointments = appointments.OrderBy(a => a.date).ToList();
                break;
            case "date_desc":
                appointments = appointments.OrderByDescending(a => a.date).ToList();
                break;
            case "patientSsn":
                appointments = appointments.OrderBy(a => a.patientSsn).ToList();
                break;
            case "patientSsn_desc":
                appointments = appointments.OrderByDescending(a => a.patientSsn).ToList();
                break;
            case "doctorSsn":
                appointments = appointments.OrderBy(a => a.doctorSsn).ToList();
                break;
            case "doctorSsn_desc":
                appointments = appointments.OrderByDescending(a => a.doctorSsn).ToList();
                break;
            case "adressOfBuilding":
                appointments = appointments.OrderBy(a => a.adressOfBuilding).ToList();
                break;
            case "adressOfBuilding_desc":
                appointments = appointments.OrderByDescending(a => a.adressOfBuilding).ToList();
                break;
            case "roomNumber":
                appointments = appointments.OrderBy(a => a.roomNumber).ToList();
                break;
            case "roomNumber_desc":
                appointments = appointments.OrderByDescending(a => a.roomNumber).ToList();
                break;
            default:
                appointments = appointments.OrderBy(a => a.id).ToList(); // Default sorting by ID
                break;
        }

        // Use the PaginatedList class to create a paginated list
        PaginatedList<Appointment> pagedAppointments = PaginatedList<Appointment>.Create(appointments, pageNumber, pageSize);

        return View(pagedAppointments);
    }



    public IActionResult ListAppointments()
    {
        List<Appointment> appointments = _dbContext.Appointment.ToList();
        return View(appointments);
    }


    public IActionResult Delete(int id)
    {
        // Find the appointment with the given id
        var appointment = _dbContext.Appointment.Find(id);

        if (appointment != null)
        {
            // Remove the appointment from the database and save changes
            _dbContext.Appointment.Remove(appointment);
            _dbContext.SaveChanges();
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult SaveChanges(Appointment model)
    {
        if (ModelState.IsValid)
        {
            // Find the appointment in the database by its ID
            var appointment = _dbContext.Appointment.Find(model.id);

            if (appointment == null)
            {
                var appointment2 = new Appointment();
                if (model.date.HasValue)
                {
                    DateTimeOffset dateTimeOffset = new DateTimeOffset(model.date.Value, TimeSpan.Zero);
                    appointment2.date = dateTimeOffset.UtcDateTime;
                }
                appointment2.patientSsn = model.patientSsn;
                appointment2.doctorSsn = model.doctorSsn;
                appointment2.adressOfBuilding = model.adressOfBuilding;
                appointment2.roomNumber = model.roomNumber;

                _dbContext.Appointment.Add(appointment2);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            // Update the appointment properties with the values from the form
            appointment.id = model.id;
            if (model.date.HasValue)
            {
                DateTimeOffset dateTimeOffset = new DateTimeOffset(model.date.Value, TimeSpan.Zero);
                appointment.date = dateTimeOffset.UtcDateTime;
            }
            appointment.patientSsn = model.patientSsn;
            appointment.doctorSsn = model.doctorSsn;
            appointment.adressOfBuilding = model.adressOfBuilding;
            appointment.roomNumber = model.roomNumber;
            _dbContext.Appointment.Update(appointment);
            _dbContext.SaveChanges();

            // Save the changes to the database
            

            return RedirectToAction("Index");
        }
        
        return View("AppointmentManage", model); // If the model is not valid, return to the Manage view with validation errors
    }

    [HttpGet]
    public IActionResult AppointmentManage(int? id)
    {
        var patients = _dbContext.Patient.ToList();
        var doctors = _dbContext.Doctor.ToList();
        var addresses = _dbContext.Building.Select(b => b.adress).Distinct().ToList();

        ViewBag.Patients = new SelectList(patients, "ssn", "fullName");
        ViewBag.Doctors = new SelectList(doctors, "ssn", "fullName");
        ViewBag.Addresses = addresses;

       
        if (id == null)
        {
             // Create new appointment
             Appointment x = new Appointment();
                return View("AppointmentManage",x);
        }
        else
        {
            // Update existing appointment
            var appointment = _dbContext.Appointment
                .Include(a => a.Patient) // Include the Patient related entity
                .Include(a => a.Doctor) // Include the Doctor related entity
                .FirstOrDefault(a => a.id == id);

            if (appointment == null)
            {
                return NotFound();
            }

            return View("AppointmentManage", appointment);
        }





        return View("Index");
    }





}


