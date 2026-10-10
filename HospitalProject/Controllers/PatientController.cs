using HospitalProject.Data;
using HospitalProject.Migrations;
using HospitalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalProject.Controllers
{
    public class PatientController : Controller
    {
        AppdbContext db = new AppdbContext();
        public IActionResult AllPatients()
        {
            var allPatients = db.Patients.Include(d => d.Doctors).Include(p => p.Treatments).ToList();
            return View(allPatients);
        }
        public IActionResult ViewPatient(int id)
        {
            Patient patient = db.Patients.Include(p => p.Treatments).Include(p => p.Doctors).ThenInclude(d => d.Department).FirstOrDefault(p => p.ID == id);
            return View(patient);
        }
        public IActionResult DeletePatient(int id)
        {
            var patient = db.Patients.FirstOrDefault(p => p.ID == id);
            db.Patients.Remove(patient);
            db.SaveChanges();
            return RedirectToAction("AllPatients");
        }
        [HttpGet]
        public IActionResult AddPatient()
        {
            ViewBag.Genders = new List<Gender>() { Gender.Male, Gender.Female };
            ViewBag.Doctors = db.Doctors.Include(d => d.Department).Include(p => p.Patients).ToList();
            return View(new Patient());
        }
        [HttpPost]
        public IActionResult SaveAdd(Patient patient, int[] DoctorIDs)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.Genders = new List<Gender>()
                    {
                        Gender.Male,
                        Gender.Female
                    };

                ViewBag.Doctors = db.Doctors.Include(p => p.Patients).ToList();
                return View("AddPatient", patient);
            }
            patient.Doctors = new List<Doctor>();
            foreach (var doctor in db.Doctors.ToList())
            {
                for (int i = 0; i < DoctorIDs.Length; i++)
                {
                    if (doctor.DoctorId == DoctorIDs[i])
                    {
                        patient.Doctors.Add(doctor);
                    }
                }
            }
            db.Patients.Add(patient);
            db.SaveChanges();
            return RedirectToAction("AllPatients");
        }
        [HttpGet]
        public IActionResult EditPatient(int id)
        {
            var patient = db.Patients.Include(p => p.Doctors).Include(p => p.Treatments).FirstOrDefault(p => p.ID == id);
            ViewBag.Doctors = db.Doctors.Include(d => d.Department).Include(p => p.Patients).Include(t=>t.Treatments).ToList();
            ViewBag.Genders = new List<Gender>() { Gender.Male, Gender.Female };
            return View(patient);
        }
        [HttpPost]
        public IActionResult SaveEdit(Patient patient, int[] DoctorIDs)
        {
            var oldPatient = db.Patients.Include(p => p.Doctors).FirstOrDefault(p => p.ID == patient.ID);

            foreach (var doctor in oldPatient.Doctors)
            {
                if (!DoctorIDs.Contains(doctor.DoctorId))
                {
                    var treatment = db.Treatments.FirstOrDefault(t => t.PatientId == patient.ID && t.DoctorId == doctor.DoctorId);

                    if (treatment != null)
                        db.Treatments.Remove(treatment);
                }
            }

            oldPatient.Doctors.Clear();

            foreach (var id in DoctorIDs)
            {
                var doctor = db.Doctors.Find(id);
                if (doctor != null)
                    oldPatient.Doctors.Add(doctor);
            }

            db.SaveChanges();

            return RedirectToAction("AllPatients");
        }
    }
}
