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
            var department = new Department(); // Always pass a non-null model
            return View(department);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Department dept)
        {
            if (ModelState.IsValid)
            {
                _dal.Insert(dept);
                return RedirectToAction("Index");
            }
            return View(dept);
        }

        public IActionResult Edit(int id)
        
        {
            var dept = _dal.GetById(id);
            if (dept == null) return NotFound();

            return View("Create", dept); // ✅ Explicitly load the Create.cshtml view
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Department dept)
        {
            if (ModelState.IsValid)
            {
                _dal.Update(dept);
                return RedirectToAction("Index");
            }
            return View(dept);
        }

        public IActionResult Delete(int id)
        {
            var dept = _dal.GetById(id);
            if (dept == null) return NotFound();
            return View(dept);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _dal.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
