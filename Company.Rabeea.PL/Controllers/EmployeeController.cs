using AspNetCoreGeneratedDocument;
using AutoMapper;
using Company.Rabeea.BLL.Interfaces;
using Company.Rabeea.DAL.Models;
using Company.Rabeea.PL.Dto;
using Company.Rabeea.PL.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Company.Rabeea.PL.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> Index(string SearchInput)
        {
            IEnumerable<Employee> employees;
            if(SearchInput is not null)
            {
                employees = await _unitOfWork.EmployeeRepository.GetByNameAsync(SearchInput);
            }
            else
            {
                employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
            }
            return View(employees);
        }

        //public async Task<IActionResult> Search(string SearchInput)
        //{
        //    IEnumerable<Employee> employees;
        //    employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
            
        //    return PartialView("EmployeePartialViews/EmployeeSearchPartialView", employees);
        //}
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["departments"] = departments;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeDto employee)
        {
            if (ModelState.IsValid)
            {
                if (employee.Image is not null) employee.ImageName = AttachmentSettings.UploadFile(employee.Image, "images");
                var emp = _mapper.Map<Employee>(employee);
                await _unitOfWork.EmployeeRepository.AddAsync(emp);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    TempData["Message"] = "Employee Created Successfully";
                    return RedirectToAction(nameof(Index));

                } 
            }
            return View(employee);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null) return BadRequest();
            var dept = await _unitOfWork.EmployeeRepository.GetAsync(id.Value);
            if (dept is null)
            {
                return NotFound();
            }
            return View(viewName, dept);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["departments"] = departments;
            if (id is null) return BadRequest();
            var employee = await _unitOfWork.EmployeeRepository.GetAsync(id.Value);
            if (employee is null)
            {
                return NotFound();
            }
            var emp = _mapper.Map<CreateEmployeeDto>(employee);
            return View(emp);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int id, CreateEmployeeDto employee)
        {
            if(employee.ImageName is not null && employee.Image is not null)
            {
                AttachmentSettings.DeleteFile(employee.ImageName , "images");
            }
            if(employee.Image is not null)
            {
                employee.ImageName = AttachmentSettings.UploadFile(employee.Image , "images");
            }

            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["departments"] = departments;
            if (ModelState.IsValid)
            {
                var emp = _mapper.Map <Employee>(employee);
                emp.Id = id;
                if (id != emp.Id) return BadRequest();
                _unitOfWork.EmployeeRepository.Update(emp);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0) return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();
            var emp = await _unitOfWork.EmployeeRepository.GetAsync(id.Value);
            if (emp is null)
            {
                return NotFound();
            }
            emp.IsDeleted = true;
            _unitOfWork.EmployeeRepository.Update(emp);
            var count = await _unitOfWork.CompleteAsync();
            if (count > 0)
            {
                if(emp.ImageName is not null)
                {
                    AttachmentSettings.DeleteFile(emp.ImageName, "images");
                    emp.ImageName = null;
                }
            }
            return RedirectToAction(nameof(Index));
            // Soft Delete
        }
    }
}
