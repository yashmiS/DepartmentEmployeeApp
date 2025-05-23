using DepartmentEmployeeApp.Data;
using DepartmentEmployeeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        // GET: /Employee
        public IActionResult Index()
        {
            var employees = _empDal.GetAll();
            return View(employees);
        }


        // GET: /Employee/Create
        public IActionResult Create()
        {
            ViewData["Title"] = "Create Employee";  // <-- Must set this
            ViewBag.Departments = new SelectList(_deptDal.GetAll(), "DepartmentId", "DepartmentName");
            return View(new Employee());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee emp)
        {
            ModelState.Remove(nameof(emp.DepartmentName));

            if (!_deptDal.GetAll().Any(d => d.DepartmentId == emp.DepartmentId))
            {
                ModelState.AddModelError("DepartmentId", "Invalid department selected.");
            }

            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "Create Employee";
                ViewBag.Departments = new SelectList(_deptDal.GetAll(), "DepartmentId", "DepartmentName");
                return View(emp);
            }

            try
            {
                _empDal.Insert(emp);
                TempData["Message"] = "Employee created successfully!";
                TempData["MessageType"] = "success";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to save employee: " + ex.Message);
                ViewBag.Departments = new SelectList(_deptDal.GetAll(), "DepartmentId", "DepartmentName");
                return View(emp);
            }
        }



        // GET: /Employee/Edit/5
        public IActionResult Edit(int id)
        {
            var emp = _empDal.GetById(id);
            if (emp == null) return NotFound();

            ViewData["Title"] = "Edit Employee";  // <-- Set here too
            ViewBag.Departments = new SelectList(_deptDal.GetAll(), "DepartmentId", "DepartmentName", emp.DepartmentId);
            return View("Create", emp);

        }


        // POST: /Employee/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Employee emp)
        {
            ModelState.Remove(nameof(emp.DepartmentName));

            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "Edit Employee";
                ViewBag.Departments = new SelectList(_deptDal.GetAll(), "DepartmentId", "DepartmentName", emp.DepartmentId);
                return View("Create", emp);
            }

            _empDal.Update(emp);
            TempData["Message"] = "Employee Updated successfully!";
            TempData["MessageType"] = "success";
            return RedirectToAction("Index");
        }


        // GET: /Employee/Delete/5
        public IActionResult Delete(int id)
        {
            var emp = _empDal.GetById(id);
            if (emp == null) return NotFound();
            return View(emp);
        }

        // POST: /Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _empDal.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
