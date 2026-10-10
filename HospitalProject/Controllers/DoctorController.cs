using HospitalProject.Data;
using HospitalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalProject.Controllers
{
    public class DoctorController : Controller
    {
        AppdbContext db =new AppdbContext();
        public IActionResult AllDoctors()
        {
            var allDoctors = db.Doctors.Include(d => d.Department).Include(p=>p.Patients).ToList();
            return View(allDoctors);
        }
        public IActionResult ViewDoctor(int id)
        {
            Doctor doctor = db.Doctors.Include(d => d.Department).Include(p=>p.Patients).Include(t=>t.Treatments).FirstOrDefault(d => d.DoctorId == id);
            return View(doctor);
        }
        public IActionResult DeleteDoctor(int id)
        {
            var doctor = db.Doctors.Include(d => d.Department).Include(p=>p.Patients).FirstOrDefault(d => d.DoctorId == id);
            db.Doctors.Remove(doctor);
            db.SaveChanges();
            return RedirectToAction("AllDoctors");
        }
        [HttpGet]
        public IActionResult AddDoctor()
        {
            ViewBag.Departments = db.Departments.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult SaveAdd(Doctor doctor)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.Departments = db.Departments.ToList();
                return View("AddDoctor", doctor);
            }
            db.Doctors.Add(doctor);
            db.SaveChanges();
            return RedirectToAction("AllDoctors");
        }
        [HttpGet]
        public IActionResult EditDoctor(int id)
        {
            var doctor = db.Doctors.FirstOrDefault(d => d.DoctorId == id);
            var Departments = db.Departments.ToList();
            ViewBag.Departments = Departments;
            return View(doctor);
        }
        [HttpPost]
        public IActionResult SaveEdit(Doctor doctor)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = db.Departments.ToList();
                return View("EditDoctor", doctor);
            }
            db.Doctors.Update(doctor);
            db.SaveChanges();
            return RedirectToAction("AllDoctors");
        }
    }
}
