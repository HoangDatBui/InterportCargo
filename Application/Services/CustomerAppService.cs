using InterportCargo.Application.Interfaces;
using InterportCargo.BusinessLogic.Entities;
using InterportCargo.BusinessLogic.Interfaces;

namespace InterportCargo.Application.Services
{
    /// <summary>
    /// Application service implementation for customer operations
    /// </summary>
    public class CustomerAppService : ICustomerAppService
    {
        private readonly ICustomerService _customerService;

        /// <summary>
        /// Initializes a new instance of the CustomerAppService
        /// </summary>
        /// <param name="customerService">Customer service dependency</param>
        public CustomerAppService(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// Retrieves all customers
        /// </summary>
        /// <returns>List of all customers</returns>
        public List<Customer> GetAllCustomers()
        {
            return _customerService.GetAll();
        }

        /// <summary>
        /// Retrieves a customer by their unique identifier
        /// </summary>
        /// <param name="id">Customer ID</param>
        /// <returns>Customer entity or null if not found</returns>
        public Customer? GetCustomer(int id)
        {
            return _customerService.GetById(id);
        }

        /// <summary>
        /// Retrieves a customer by their email address
        /// </summary>
        /// <param name="email">Customer email address</param>
        /// <returns>Customer entity or null if not found</returns>
        public Customer? GetCustomerByEmail(string email)
        {
            return _customerService.GetByEmail(email);
        }

        /// <summary>
        /// Registers a new customer
        /// </summary>
        /// <param name="firstName">Customer's first name</param>
        /// <param name="familyName">Customer's family name</param>
        /// <param name="email">Customer's email address</param>
        /// <param name="phoneNumber">Customer's phone number</param>
        /// <param name="companyName">Customer's company name (optional)</param>
        /// <param name="address">Customer's address</param>
        /// <param name="password">Customer's password</param>
        /// <returns>Registration result with success status and any error messages</returns>
        public async Task<RegistrationResult> RegisterCustomerAsync(string firstName, string familyName, string email, 
            string phoneNumber, string? companyName, string address, string password)
        {
            // Create customer entity
            var customer = new Customer
            {
                FirstName = firstName?.Trim() ?? string.Empty,
                FamilyName = familyName?.Trim() ?? string.Empty,
                Email = email?.Trim() ?? string.Empty,
                PhoneNumber = phoneNumber?.Trim() ?? string.Empty,
                CompanyName = companyName?.Trim(),
                Address = address?.Trim() ?? string.Empty
            };

            // Validate required fields
            if (string.IsNullOrWhiteSpace(customer.FirstName))
                return RegistrationResult.Failure("First name is required.");

            if (string.IsNullOrWhiteSpace(customer.FamilyName))
                return RegistrationResult.Failure("Family name is required.");

            if (string.IsNullOrWhiteSpace(customer.Email))
                return RegistrationResult.Failure("Email address is required.");

            if (string.IsNullOrWhiteSpace(customer.PhoneNumber))
                return RegistrationResult.Failure("Phone number is required.");

            if (string.IsNullOrWhiteSpace(customer.Address))
                return RegistrationResult.Failure("Address is required.");

            if (string.IsNullOrWhiteSpace(password))
                return RegistrationResult.Failure("Password is required.");

            // Validate email format
            if (!IsValidEmail(customer.Email))
                return RegistrationResult.Failure("Invalid email format.");

            // Validate password strength
            if (password.Length < 6)
                return RegistrationResult.Failure("Password must be at least 6 characters long.");

            // Register customer through business service
            return await _customerService.RegisterCustomerAsync(customer, password);
        }

        /// <summary>
        /// Updates an existing customer
        /// </summary>
        /// <param name="customer">Customer entity with updated information</param>
        public void UpdateCustomer(Customer customer)
        {
            _customerService.Update(customer);
        }

        /// <summary>
        /// Deletes a customer
        /// </summary>
        /// <param name="id">Customer ID to delete</param>
        public void DeleteCustomer(int id)
        {
            _customerService.Delete(id);
        }

        /// <summary>
        /// Validates customer credentials for login
        /// </summary>
        /// <param name="email">Customer email</param>
        /// <param name="password">Plain text password</param>
        /// <returns>True if credentials are valid, false otherwise</returns>
        public bool ValidateCustomerCredentials(string email, string password)
        {
            return _customerService.ValidateCredentials(email, password);
        }

        /// <summary>
        /// Validates email format
        /// </summary>
        /// <param name="email">Email address to validate</param>
        /// <returns>True if email format is valid, false otherwise</returns>
        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
