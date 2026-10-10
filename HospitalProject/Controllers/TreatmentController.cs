using HospitalProject.Data;
using HospitalProject.Migrations;
using HospitalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalProject.Controllers
{
    public class TreatmentController : Controller
    {
        AppdbContext db = new AppdbContext();
        public IActionResult AllTreatmentsD(int doctorid)
        {
            var treatment = db.Treatments.Include(p => p.Patient).Include(d => d.Doctor).ThenInclude(d=>d.Department).Where(t => t.DoctorId == doctorid).ToList();
            ViewBag.Doctor = db.Doctors.FirstOrDefault(d => d.DoctorId == doctorid);
            return View(treatment);
        }

        /*public IActionResult AllTreatmentsP(int Patientid)
        {
            var treatment = db.Treatments.Include(p => p.Patient).Include(d => d.Doctor).ThenInclude(d=>d.Department).Where(t => t.PatientId == Patientid).ToList();
            return View(treatment);
        }*/

        [HttpGet]
        public IActionResult AddTreatment(int doctorId)
        {
            ViewBag.DoctorId = doctorId;

            ViewBag.Doctor = db.Doctors
                .Include(d => d.Patients)
                .FirstOrDefault(d => d.DoctorId == doctorId);

            ViewBag.Patients = db.Patients
                .Where(p => p.Doctors.Any(d => d.DoctorId == doctorId))
                .Where(p => !db.Treatments.Any(t =>
                    t.PatientId == p.ID &&
                    t.DoctorId == doctorId))
                .ToList();

            return View();
        }
        [HttpPost]
        public IActionResult SaveAdd(Treatment treatment)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.DoctorId = treatment.DoctorId;

                ViewBag.Doctor = db.Doctors
                    .Include(d => d.Patients)
                    .FirstOrDefault(d => d.DoctorId == treatment.DoctorId);

                ViewBag.Patients = db.Patients
                    .Where(p => p.Doctors.Any(d => d.DoctorId == treatment.DoctorId))
                    .Where(p => !db.Treatments.Any(t =>
                        t.PatientId == p.ID &&
                        t.DoctorId == treatment.DoctorId))
                    .ToList();

                return View("AddTreatment", treatment);
            }

            db.Treatments.Add(treatment);
            db.SaveChanges();

            return RedirectToAction("ViewDoctor", "Doctor",
                new { id = treatment.DoctorId });
        }
        [HttpGet]
        public IActionResult EditTreatment(int id)
        {
            var treatment = db.Treatments.Include(t => t.Patient).Include(t => t.Doctor).FirstOrDefault(t => t.Id == id);
            ViewBag.Patient = db.Patients.Include(p => p.Treatments).Include(d=>d.Doctors).FirstOrDefault(p => p.ID == treatment.PatientId);
            ViewBag.Doctor = db.Doctors.Include(d => d.Department).Include(p=>p.Treatments).Include(p=>p.Patients).FirstOrDefault(d => d.DoctorId == treatment.DoctorId);
            return View(treatment);
        }
        [HttpPost]
        public IActionResult SaveEdit(Treatment treatment)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Doctor = db.Doctors
                    .FirstOrDefault(d => d.DoctorId == treatment.DoctorId);

                ViewBag.Patient = db.Patients
                    .FirstOrDefault(p => p.ID == treatment.PatientId);

                return View("EditTreatment", treatment);
            }

            db.Treatments.Update(treatment);
            db.SaveChanges();

            return RedirectToAction("ViewDoctor", "Doctor",
                new { id = treatment.DoctorId });
        }
        public IActionResult DeleteTreatment(int id)
        {
            var treatment = db.Treatments.FirstOrDefault(t => t.Id == id);
            db.Treatments.Remove(treatment);
            db.SaveChanges();
            return RedirectToAction("AllTreatmentsD", new { doctorId = treatment.DoctorId });
        }
    }
}
