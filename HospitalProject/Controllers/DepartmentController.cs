using HospitalProject.Data;
using HospitalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace HospitalProject.Controllers
{
    public class DepartmentController : Controller
    {
        AppdbContext db = new AppdbContext();
        public IActionResult AllDepartments()
        {
            var allDepartments = db.Departments.Include(d => d.Doctors).ToList();
            return View(allDepartments);
        }
        public IActionResult ViewDepartment(int id)
        {
            Department department = db.Departments.Include(d => d.Doctors).FirstOrDefault(d => d.DepartmentId == id);
            return View(department);
        }
        public IActionResult DeleteDepartment(int id)
        {
            var department = db.Departments.Include(d => d.Doctors).FirstOrDefault(d => d.DepartmentId == id);
            db.Departments.Remove(department);
            db.SaveChanges();
            return RedirectToAction("AllDepartments");
        }
        [HttpGet]
        public IActionResult AddDepartment()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SaveAdd(Department department)
        {
            if(String.IsNullOrWhiteSpace(department.Description))
            {
                department.Description = "No Description Found";
            }
            if (!ModelState.IsValid)
            {
                return View("AddDepartment", department);
            }
            db.Departments.Add(department);
            db.SaveChanges();
            return RedirectToAction("AllDepartments");
        }
        [HttpGet]
        public IActionResult EditDepartment(int id)
        {
            var department = db.Departments.FirstOrDefault(d => d.DepartmentId == id);
            return View(department);
        }
        [HttpPost]
        public IActionResult SaveEdit(Department department)
        {
            if (String.IsNullOrWhiteSpace(department.Description))
            {
                department.Description = "No Description Found";
            }
            if (!ModelState.IsValid)
            {
                return View("EditDepartment", department);
            }
            else{
                db.Departments.Update(department);
                db.SaveChanges();
                return RedirectToAction("AllDepartments");
            }
        }
    }
}
