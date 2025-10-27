using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using InterportCargo.Application.Interfaces;

namespace InterportCargo.Pages.Account
{
    public class RegisterEmployeeModel : PageModel
    {
        private readonly IEmployeeAppService _employeeAppService;

        public RegisterEmployeeModel(IEmployeeAppService employeeAppService)
        {
            _employeeAppService = employeeAppService;
        }

        [BindProperty]
        public EmployeeRegistrationModel Employee { get; set; } = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Register employee using the application service
            var result = await _employeeAppService.RegisterEmployeeAsync(
                Employee.FirstName,
                Employee.FamilyName,
                Employee.Email,
                Employee.PhoneNumber,
                Employee.EmployeeType,
                Employee.Address,
                Employee.Password
            );

            if (result.IsSuccess)
            {
                // Registration successful - redirect to login page with success message
                TempData["SuccessMessage"] = "Registration successful! Please log in with your credentials.";
                return RedirectToPage("/Account/Login");
            }
            else
            {
                // Registration failed - add error to model state
                ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Registration failed. Please try again.");
                return Page();
            }
        }
    }

    public class EmployeeRegistrationModel
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Family name is required")]
        [StringLength(50, ErrorMessage = "Family name cannot exceed 50 characters")]
        public string FamilyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Employee type is required")]
        [StringLength(50, ErrorMessage = "Employee type cannot exceed 50 characters")]
        public string EmployeeType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required")]
        [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirm password do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
