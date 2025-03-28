using AspNetCoreGeneratedDocument;
using AutoMapper;
using Company.Rabeea.BLL.Interfaces;
using Company.Rabeea.DAL.Models;
using Company.Rabeea.PL.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Company.Rabeea.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult Index(string SearchInput)
        {
            IEnumerable<Employee> employees;
            if(SearchInput is not null)
            {
                employees = _unitOfWork.EmployeeRepository.GetByName(SearchInput);
            }
            else
            {
                employees = _unitOfWork.EmployeeRepository.GetAll();
            }
            return View(employees);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var departments = _unitOfWork.DepartmentRepository.GetAll();
            ViewData["departments"] = departments;
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateEmployeeDto employee)
        {

            if (ModelState.IsValid)
            {
                var emp = _mapper.Map<Employee>(employee);
                _unitOfWork.EmployeeRepository.Add(emp);
                var count = _unitOfWork.Complete();
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
            var dept = _unitOfWork.EmployeeRepository.Get(id.Value);
            if (dept is null)
            {
                return NotFound();
            }
            return View(viewName, dept);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var departments = _unitOfWork.DepartmentRepository.GetAll();
            ViewData["departments"] = departments;
            if (id is null) return BadRequest();
            var employee = _unitOfWork.EmployeeRepository.Get(id.Value);
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
            var departments = _unitOfWork.DepartmentRepository.GetAll();
            ViewData["departments"] = departments;
            if (ModelState.IsValid)
            {
                var emp = _mapper.Map <Employee>(employee);
                emp.Id = id;
                if (id != emp.Id) return BadRequest();
                _unitOfWork.EmployeeRepository.Update(emp);
                var count = _unitOfWork.Complete();
                if (count > 0) return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        public IActionResult Delete(int? id)
        {
            if (id is null) return BadRequest();
            var emp = _unitOfWork.EmployeeRepository.Get(id.Value);
            if (emp is null)
            {
                return NotFound();
            }
            emp.IsDeleted = true;
            _unitOfWork.EmployeeRepository.Update(emp);
            _unitOfWork.Complete();
            return RedirectToAction(nameof(Index));
            // Soft Delete
        }
    }
}
