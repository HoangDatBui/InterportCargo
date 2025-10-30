using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using InterportCargo.BusinessLogic.Interfaces;

namespace InterportCargo.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IAuthenticationService _authenticationService;

        public LoginModel(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [BindProperty]
        public LoginRequestModel LoginRequest { get; set; } = new();

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Authenticate user
            var result = _authenticationService.AuthenticateUser(LoginRequest.Email, LoginRequest.Password);

            if (result.IsSuccess)
            {
                // Store user information in session
                HttpContext.Session.SetString("IsAuthenticated", "true");
                HttpContext.Session.SetString("UserEmail", result.Email ?? string.Empty);
                HttpContext.Session.SetString("UserName", result.UserName ?? string.Empty);
                HttpContext.Session.SetString("UserType", result.UserType ?? string.Empty);
                HttpContext.Session.SetInt32("UserId", result.UserId ?? 0);

                // Redirect to Quotations page
                return RedirectToPage("/Quotations/Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Invalid email or password.");
                return Page();
            }
        }
    }

    public class LoginRequestModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }
}
