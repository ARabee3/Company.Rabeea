using Company.Rabeea.DAL.Models;
using Company.Rabeea.PL.Authentication;
using Company.Rabeea.PL.Dto;
using Company.Rabeea.PL.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Company.Rabeea.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMailService _mailService;
        private readonly ITwilioService _twilioService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,IMailService mailService,ITwilioService twilioService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mailService = mailService;
            _twilioService = twilioService;
        }
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpDto signUpDto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(signUpDto.UserName);
                if (user is null)
                {
                    user = await _userManager.FindByEmailAsync(signUpDto.Email);
                    if (user is null)
                    {
                        user = new AppUser()
                        {
                            UserName = signUpDto.UserName,
                            FirstName = signUpDto.FirstName,
                            LastName = signUpDto.LastName,
                            Email = signUpDto.Email,
                            IsAgree = signUpDto.IsAgree
                        };
                        var result = await _userManager.CreateAsync(user, signUpDto.Password);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("SignIn");
                        }
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }

                ModelState.AddModelError("", "Invalid Sign Up !!!!");

            }
            return View();
        }
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var flag = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (flag)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                        if (result.Succeeded)
                        {
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                        }
                    }
                }
                ModelState.AddModelError("", "Invalid Login !!");
            }
            return View();
        }
        [HttpGet]
        public new async Task<IActionResult> SignOut() // new because we want to hide the existing SignOut();
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SendResetPasswordUrl(ForgetPasswordDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var url = Url.Action("ResetPassword", "Account", new { email = model.Email, token }, Request.Scheme);
                    var email = new Email
                    {
                        To = model.Email,
                        Subject = "Password Reset",
                        Body = url!
                    };
                    _mailService.SendEmail(email);
                    return RedirectToAction("CheckYourInbox");

                    
                }
            }
            ModelState.AddModelError("", "Invalid Data");
            return View("ForgetPassword");
        }
        [HttpPost]
        public async Task<IActionResult> SendResetPasswordUrlSms(ForgetPasswordDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var url = Url.Action("ResetPassword", "Account", new { email = model.Email, token }, Request.Scheme);
                    var sms = new Sms
                    {
                        To = user.PhoneNumber!,
                        Body = url!
                    };
                    _twilioService.SendSms(sms);
                    return RedirectToAction("CheckYourPhone");

                    
                }
            }
            ModelState.AddModelError("", "Invalid Data");
            return View("ForgetPassword");
        }
        [HttpGet]
        public IActionResult CheckYourInbox()
        {
            return View();
        } 
        [HttpGet]
        public IActionResult CheckYourPhone()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            if (ModelState.IsValid)
            {
                var email = TempData["email"] as string;
                var token = TempData["token"] as string;
                if (email is null || token is null) return BadRequest("Invalid Operation");
                var user = await _userManager.FindByEmailAsync(email);
                if(user is not null)
                {
                   var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                    if (result.Succeeded)
                    {
                        TempData["SuccessMessage"] = "Successfully Reset Password";
                        return RedirectToAction("SignIn");
                    }
                }
                ModelState.AddModelError("", "Invalid Reset Password");
            }
            return View();
        }

        public IActionResult GoogleLogin(string returnUrl = "/") // Pass the desired returnUrl
        {
            // Configure the redirect URL back to your app after Google auth
            string redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(GoogleDefaults.AuthenticationScheme, redirectUrl);
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        // [AllowAnonymous] // Optional
        public IActionResult FacebookLogin(string returnUrl = "/") // Pass the desired returnUrl
        {
            string redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(FacebookDefaults.AuthenticationScheme, redirectUrl);
            return Challenge(properties, FacebookDefaults.AuthenticationScheme);
        }


        // [AllowAnonymous] // Optional
        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl ??= Url.Content("~/"); // Default to home page if returnUrl is null

            if (remoteError != null)
            {
                // _logger?.LogError("Error from external provider: {Error}", remoteError); // Log error
                TempData["ErrorMessage"] = $"Error from external provider: {remoteError}";
                return RedirectToAction(nameof(SignIn)); // Redirect to SignIn page with error
            }

            // Get the login information from the external provider (e.g., Google)
            // This reads the temporary external cookie created by the middleware.
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                // _logger?.LogWarning("Error loading external login information during callback."); // Log error
                TempData["ErrorMessage"] = "Error loading external login information.";
                return RedirectToAction(nameof(SignIn)); // Redirect if info is missing
            }

            // _logger?.LogInformation("External login info retrieved for {Provider} - {ProviderKey}", info.LoginProvider, info.ProviderKey); // Log success

            // Try to sign in the user with this external login provider (checks AspNetUserLogins table)
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (result.Succeeded)
            {
                // _logger?.LogInformation("User {UserId} logged in with {Provider} provider.", info.ProviderKey, info.LoginProvider); // Log success
                // Existing user found and linked! Sign-in successful.
                // It's important to clear the external cookie now
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
                return LocalRedirect(returnUrl); // Redirect to the original target URL
            }
            if (result.IsLockedOut)
            {
                // _logger?.LogWarning("User account locked out."); // Log warning
                return RedirectToAction("Lockout"); // Handle account lockout if you have a view for it
            }
            else // User is not linked or doesn't exist locally
            {
                // _logger?.LogInformation("External login user not found locally or not linked. Attempting to find by email or create."); // Log info

                // Get the email claim from the external provider
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                if (email != null)
                {
                    // Attempt to find the user by email
                    var user = await _userManager.FindByEmailAsync(email);

                    if (user == null) // User doesn't exist, create a new one
                    {
                        // _logger?.LogInformation("No user found with email {Email}. Creating new user.", email); // Log info
                        user = new AppUser
                        {
                            UserName = email, // Often use email as username for external logins
                            Email = email,
                            // Extract other details if available and desired
                            FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName),
                            LastName = info.Principal.FindFirstValue(ClaimTypes.Surname),
                            EmailConfirmed = true // Assume email is confirmed by external provider
                        };
                        var createUserResult = await _userManager.CreateAsync(user);
                        if (!createUserResult.Succeeded)
                        {
                            // _logger?.LogError("Failed to create new user: {Errors}", string.Join(", ", createUserResult.Errors.Select(e => e.Description))); // Log error
                            TempData["ErrorMessage"] = "Error creating user account: " + string.Join(", ", createUserResult.Errors.Select(e => e.Description));
                            return RedirectToAction(nameof(SignIn)); // Redirect on creation failure
                        }
                        // _logger?.LogInformation("Successfully created new user with ID {UserId} for email {Email}.", user.Id, email); // Log info
                    }

                    // User exists (or was just created), now link the external login
                    // _logger?.LogInformation("Attempting to add login {Provider}-{ProviderKey} for user {UserId}.", info.LoginProvider, info.ProviderKey, user.Id); // Log info
                    var addLoginResult = await _userManager.AddLoginAsync(user, info);
                    if (addLoginResult.Succeeded)
                    {
                        // _logger?.LogInformation("Successfully added login {Provider}-{ProviderKey} for user {UserId}.", info.LoginProvider, info.ProviderKey, user.Id); // Log info
                        // Link successful! Now sign the user in locally.
                        // It's important to clear the external cookie now
                        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

                        // Sign in the newly linked/created user
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        // _logger?.LogInformation("User {UserId} signed in after external login linking/creation.", user.Id); // Log info
                        return LocalRedirect(returnUrl); // Redirect to target
                    }
                    else
                    {
                        // _logger?.LogError("Failed to add login for user {UserId}: {Errors}", user.Id, string.Join(", ", addLoginResult.Errors.Select(e => e.Description))); // Log error
                        TempData["ErrorMessage"] = "Error linking external account: " + string.Join(", ", addLoginResult.Errors.Select(e => e.Description));
                        return RedirectToAction(nameof(SignIn)); // Redirect on linking failure
                    }
                }

                // If email is null, cannot automatically link/create.
                // You might redirect to a registration page asking for more details.
                // _logger?.LogWarning("Email claim not found from external provider {Provider}.", info.LoginProvider); // Log warning
                TempData["ErrorMessage"] = $"Could not retrieve email from {info.ProviderDisplayName}. Cannot automatically create account.";
                return RedirectToAction(nameof(SignIn));
            }
        }

        // You might need a Lockout view/action
        public IActionResult Lockout()
        {
            return View(); // Create a simple Lockout.cshtml view
        }

        // ... Make sure you have SignIn, SignUp views and actions ...
        // ... Remember to remove GoogleResponse and FacebookResponse if using this pattern ...

    }

    // Do the same simplification for FacebookResponse


}

