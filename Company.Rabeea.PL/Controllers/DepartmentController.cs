using Company.Rabeea.BLL.Interfaces;
using Company.Rabeea.BLL.Repositories;
using Company.Rabeea.DAL.Models;
using Company.Rabeea.PL.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Company.Rabeea.PL.Controllers
{
    public class DepartmentController : Controller
    {
        // Ask CLR To Create Object of DepartmentRepository
        private readonly IDepartmentRepository _departmentRepository;
        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        public IActionResult Index()
        {
            var departments = _departmentRepository.GetAll();
            return View(departments);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateDepartmentDto department)
        {
            if (ModelState.IsValid) // Server Side Validaton
            {
                var dept = new Department
                {
                    Code = department.Code,
                    Name = department.Name,
                    CreateAt = department.CreateAt
                };
                var count = _departmentRepository.Add(dept);
                if(count > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(department);
        }

        [HttpGet]
        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (id is null) return BadRequest();
            var dept = _departmentRepository.Get(id.Value);
            if(dept is null)
            {
                return NotFound();
            }
            return View(viewName,dept);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id is null) return BadRequest();
            var dept = _departmentRepository.Get(id.Value);
            if (dept is null)
            {
                return NotFound();
            }
            var department = new CreateDepartmentDto
            {
                Code = dept.Code,
                Name = dept.Name,
                CreateAt = dept.CreateAt
            };
            return View(department);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken] allows only the app to use the POST Endpoint
        public IActionResult Edit([FromRoute]int id,CreateDepartmentDto department)
        { 
            if (ModelState.IsValid)
            {
                var dept = new Department()
                {
                    Id = id,
                    Code = department.Code,
                    Name = department.Name,
                    CreateAt = department.CreateAt
                };
                if (id != dept.Id) return BadRequest();
                var count = _departmentRepository.Update(dept);
                if (count > 0) return RedirectToAction("Index");
            }
            
            return View(department);
        }
        public IActionResult Delete(int? id)
        {
            if (id is null) return BadRequest();
            var dept = _departmentRepository.Get(id.Value);
            if (dept is null)
            {
                return NotFound();
            }
            _departmentRepository.Delete(dept);
            return RedirectToAction(nameof(Index));

        }

    }
}
