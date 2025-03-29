using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Company.Rabeea.PL.Dto;

public class CreateEmployeeDto
{
    [Required(ErrorMessage = "Name is Required!")]
    public string Name { get; set; }


    [Range(22,60,ErrorMessage ="Age Must Be Between 22 & 60")]
    public int? Age { get; set; }


    [Required(ErrorMessage = "Email is Required!")]
    [DataType(DataType.EmailAddress , ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; }


    [Required(ErrorMessage = "Address is Required!")]
    [RegularExpression(@"[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$",
        ErrorMessage ="Address Format is '123-Street-city-country'")]
    public string Address { get; set; }


    [Phone]
    public string Phone { get; set; }
    

    [Required(ErrorMessage = "Salary is Required!")]
    [DataType(DataType.Currency)]
    public decimal Salary { get; set; }

    public bool IsActive { get; set; }

    [Required(ErrorMessage = "HiringDate is Required!")]
    [DisplayName("Hiring Date")]
    public DateTime HiringDate { get; set; }
    [Required(ErrorMessage = "CreateAt is Required!")]
    [DisplayName("Date Of Creation")]
    public DateTime CreateAt { get; set; }

    [DisplayName("Department")]
    public int? DepartmentId { get; set; }
    public string? ImageName { get; set; }
    public IFormFile? Image { get; set; }
}
