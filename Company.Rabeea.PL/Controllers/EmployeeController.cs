using AutoMapper;
using Company.Rabeea.BLL.Interfaces;
using Company.Rabeea.DAL.Models;
using Company.Rabeea.PL.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Company.Rabeea.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository, IMapper mapper)
        {
            this._employeeRepository = employeeRepository;
            this._departmentRepository = departmentRepository;
            this._mapper = mapper;
        }
        [HttpGet]
        public IActionResult Index(string SearchInput)
        {
            IEnumerable<Employee> employees;
            if(SearchInput is not null)
            {
                employees = _employeeRepository.GetByName(SearchInput);
            }
            else
            {
                employees = _employeeRepository.GetAll();
            }
            return View(employees);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var departments = _departmentRepository.GetAll();
            ViewData["departments"] = departments;
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateEmployeeDto employee)
        {

            if (ModelState.IsValid)
            {
                var emp = _mapper.Map<Employee>(employee);
                var count = _employeeRepository.Add(emp);
                if (count > 0)
                {
                    TempData["Message"] = "Employee Created Successfully";
                    return RedirectToAction(nameof(Index));

                } 
            }
            return View(employee);
        }

        [HttpGet]
        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (id is null) return BadRequest();
            var dept = _employeeRepository.Get(id.Value);
            if (dept is null)
            {
                return NotFound();
            }
            return View(viewName, dept);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var departments = _departmentRepository.GetAll();
            ViewData["departments"] = departments;
            if (id is null) return BadRequest();
            var employee = _employeeRepository.Get(id.Value);
            if (employee is null)
            {
                return NotFound();
            }
            var emp = _mapper.Map<CreateEmployeeDto>(employee);
            return View(emp);
        }

        [HttpPost]
        public IActionResult Edit([FromRoute] int id, CreateEmployeeDto employee)
        {
            var departments = _departmentRepository.GetAll();
            ViewData["departments"] = departments;
            if (ModelState.IsValid)
            {
                var emp = _mapper.Map <Employee>(employee);
                emp.Id = id;
                if (id != emp.Id) return BadRequest();
                var count = _employeeRepository.Update(emp);
                if (count > 0) return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        public IActionResult Delete(int? id)
        {
            if (id is null) return BadRequest();
            var emp = _employeeRepository.Get(id.Value);
            if (emp is null)
            {
                return NotFound();
            }
            emp.IsDeleted = true;
            _employeeRepository.Update(emp);
            return RedirectToAction(nameof(Index));
            // Soft Delete
        }
    }
}
