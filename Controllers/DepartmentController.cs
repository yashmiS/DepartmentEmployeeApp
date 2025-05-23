using System.Data.SqlClient;
using DepartmentEmployeeApp.Data;
using DepartmentEmployeeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DepartmentEmployeeApp.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly DepartmentDAL _dal;

        public DepartmentController(IConfiguration config)
        {
            _dal = new DepartmentDAL(config);
        }

        public IActionResult Index()
        {
            var departments = _dal.GetAll();
            return View(departments);
        }

        public IActionResult Create()
        {
            var department = new Department();
            return View(department);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Department dept)
        {
            if (ModelState.IsValid)
            {
                var existingDept = _dal.GetByCode(dept.DepartmentCode);
                if (existingDept != null)
                {
                    TempData["Message"] = "A department with this code already exists.";
                    TempData["MessageType"] = "warning";
                    return View(dept);
                }
                _dal.Insert(dept);
                TempData["Message"] = "Employee created successfully!";
                TempData["MessageType"] = "success";
                return RedirectToAction("Index");
            }
            return View(dept);
        }

        public IActionResult Edit(int id)

        {
            var dept = _dal.GetById(id);
            if (dept == null) return NotFound();

            return View("Create", dept);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Department dept)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", dept);;
            }

            var existingDept = _dal.GetByCode(dept.DepartmentCode);

            // Check if another department with the same code exists
            if (existingDept != null && existingDept.DepartmentId != dept.DepartmentId)
            {
                TempData["Message"] = "A department with this code already exists.";
                TempData["MessageType"] = "warning";
                return View("Create", dept);;
            }

            _dal.Update(dept);
            TempData["Message"] = "Department updated successfully!";
            TempData["MessageType"] = "success";
            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            var dept = _dal.GetById(id);
            if (dept == null) return NotFound();
            return View(dept);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _dal.Delete(id);
                TempData["Message"] = "Department deleted successfully.";
                TempData["MessageType"] = "success";
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    TempData["Message"] = "Cannot delete this department because it has employees assigned.";
                    TempData["MessageType"] = "warning";
                }
                else
                {
                    TempData["Message"] = "An error occurred while deleting the department.";
                    TempData["MessageType"] = "danger";
                }
            }

            return RedirectToAction("Index");
        }


    }
}
