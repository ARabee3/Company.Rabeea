using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Company.Rabeea.PL.Dto;

public class CreateDepartmentDto
{
    [Required(ErrorMessage = "Code is Required!")]
    public string Code { get; set; }
    [Required(ErrorMessage = "Name is Required!")]
    public string Name { get; set; }
    [Required(ErrorMessage = "CreateAt is Required!")]
    public DateTime CreateAt { get; set; }
}
