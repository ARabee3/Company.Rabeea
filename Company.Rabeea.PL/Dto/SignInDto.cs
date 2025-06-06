﻿using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel.DataAnnotations;

namespace Company.Rabeea.PL.Dto;

public class SignInDto
{
    [Required(ErrorMessage = "Email is Required !!")]
    [EmailAddress]
    public string Email { get; set; }


    [Required(ErrorMessage = "Password is Required !!")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public bool RememberMe { get; set; }
}
