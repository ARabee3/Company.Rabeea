using Company.Rabeea.BLL.Interfaces;
using Company.Rabeea.BLL.Repositories;
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
    }
}
