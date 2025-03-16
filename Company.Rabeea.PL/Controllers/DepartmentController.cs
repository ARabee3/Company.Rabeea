using Company.Rabeea.BLL.Interfaces;
using Company.Rabeea.BLL.Repositories;
using Company.Rabeea.DAL.Models;
using Company.Rabeea.PL.Dto;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

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
       
    }
}
