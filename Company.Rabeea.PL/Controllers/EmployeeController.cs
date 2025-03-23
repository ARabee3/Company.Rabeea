using Company.Rabeea.BLL.Interfaces;
using Company.Rabeea.DAL.Models;
using Company.Rabeea.PL.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Company.Rabeea.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            this._employeeRepository = employeeRepository;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var employees = _employeeRepository.GetAll();
            return View(employees);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateEmployeeDto employee)
        {

            if (ModelState.IsValid)
            {
                var emp = new Employee()
                {
                    Name = employee.Name,
                    Address = employee.Address,
                    Age = employee.Age,
                    Email = employee.Email,
                    CreateAt = employee.CreateAt,
                    HiringDate = employee.HiringDate,
                    IsActive = true,
                    Phone = employee.Phone,
                    Salary = employee.Salary
                };
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
            if (id is null) return BadRequest();
            var employee = _employeeRepository.Get(id.Value);
            if (employee is null)
            {
                return NotFound();
            }
            var emp = new CreateEmployeeDto()
            {
                Name = employee.Name,
                Address = employee.Address,
                Age = employee.Age,
                Email = employee.Email,
                CreateAt = employee.CreateAt,
                HiringDate = employee.HiringDate,
                IsActive = true,
                Phone = employee.Phone,
                Salary = employee.Salary
            };
            return View(emp);
        }

        [HttpPost]
        public IActionResult Edit([FromRoute] int id, CreateEmployeeDto employee)
        {
            if (ModelState.IsValid)
            {
                var emp = new Employee()
                {
                    Id = id,
                    Name = employee.Name,
                    Address = employee.Address,
                    Age = employee.Age,
                    Email = employee.Email,
                    CreateAt = employee.CreateAt,
                    HiringDate = employee.HiringDate,
                    IsActive = true,
                    Phone = employee.Phone,
                    Salary = employee.Salary
                };
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
