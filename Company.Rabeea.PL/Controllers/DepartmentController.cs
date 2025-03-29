using AutoMapper;
using Company.Rabeea.BLL.Interfaces;
using Company.Rabeea.BLL.Repositories;
using Company.Rabeea.DAL.Models;
using Company.Rabeea.PL.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
namespace Company.Rabeea.PL.Controllers
{
    public class DepartmentController : Controller
    {
        // Ask CLR To Create Object of DepartmentRepository
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public DepartmentController(
            IMapper mapper,
            IUnitOfWork unitOfWork
            )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            return View(departments);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateDepartmentDto department)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                var dept = _mapper.Map<Department>(department);
                await _unitOfWork.DepartmentRepository.AddAsync(dept);
                var count = await _unitOfWork.CompleteAsync();
                if(count > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(department);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null) return BadRequest();
            var dept = await _unitOfWork.DepartmentRepository.GetAsync(id.Value);
            if(dept is null)
            {
                return NotFound();
            }
            return View(viewName,dept);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();
            var dept = await _unitOfWork.DepartmentRepository.GetAsync(id.Value);
            if (dept is null)
            {
                return NotFound();
            }
            var department = _mapper.Map<CreateDepartmentDto>(dept);
            return View(department);
        }
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Edit([FromRoute]int id,CreateDepartmentDto department)
        { 
            if (ModelState.IsValid)
            {
                var dept = _mapper.Map<Department>(department);
                dept.Id = id;
                if (id != dept.Id) return BadRequest();
                _unitOfWork.DepartmentRepository.Update(dept);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0) return RedirectToAction("Index");
            }
            
            return View(department);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();
            var dept = await _unitOfWork.DepartmentRepository.GetAsync(id.Value);
            if (dept is null)
            {
                return NotFound();
            }
            _unitOfWork.DepartmentRepository.Delete(dept);
            await _unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Index));

        }

    }
}
