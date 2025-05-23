using DepartmentEmployeeApp.Data;
using DepartmentEmployeeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace DepartmentEmployeeApp.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeDAL _empDal;
        private readonly DepartmentDAL _deptDal;

        public EmployeeController(IConfiguration config)
        {
            _empDal = new EmployeeDAL(config);
            _deptDal = new DepartmentDAL(config);
        }

        public IActionResult Index()
        {
            var employees = _empDal.GetAll();
            return View(employees);
        }

        public IActionResult Create()
        {
            var model = new Employee
            {
                DateOfBirth = new DateTime(2000, 1, 1) // fallback safe default
            };
            ViewData["Title"] = "Create Employee";
            ViewBag.Departments = new SelectList(_deptDal.GetAll(), "DepartmentId", "DepartmentName");
            return View(new Employee());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee emp)
        {
            ModelState.Remove(nameof(emp.DepartmentName));

            if (_empDal.GetByEmail(emp.Email) != null)
            {
                ModelState.AddModelError("Email", "Email already exists for another employee.");
            }

            if (emp.DateOfBirth < new DateTime(1753, 1, 1))
            {
                ModelState.AddModelError("DateOfBirth", "DateOfBirth must be after 01/01/1753.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Departments = new SelectList(_deptDal.GetAll(), "DepartmentId", "DepartmentName");
                return View(emp);
            }

            _empDal.Insert(emp);
            TempData["Message"] = "Employee created successfully!";
            TempData["MessageType"] = "success";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var emp = _empDal.GetById(id);
            if (emp == null) return NotFound();

            ViewData["Title"] = "Edit Employee";
            ViewBag.Departments = new SelectList(_deptDal.GetAll(), "DepartmentId", "DepartmentName", emp.DepartmentId);
            return View("Create", emp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Employee emp)
        {
            ModelState.Remove(nameof(emp.DepartmentName));

            var existing = _empDal.GetByEmail(emp.Email);
            if (existing != null && existing.EmployeeId != emp.EmployeeId)
            {
                ModelState.AddModelError("Email", "Email already exists for another employee.");
            }

            if (emp.DateOfBirth < new DateTime(1753, 1, 1))
            {
                ModelState.AddModelError("DateOfBirth", "DateOfBirth must be after 01/01/1753.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Departments = new SelectList(_deptDal.GetAll(), "DepartmentId", "DepartmentName", emp.DepartmentId);
                return View("Create", emp);
            }

            _empDal.Update(emp);

            TempData["Message"] = "Employee updated successfully!";
            TempData["MessageType"] = "success";
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var emp = _empDal.GetById(id);
            if (emp == null) return NotFound();
            return View(emp);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _empDal.Delete(id);
            TempData["Message"] = "Employee deleted successfully!";
            TempData["MessageType"] = "success";
            return RedirectToAction("Index");
        }
    }
}