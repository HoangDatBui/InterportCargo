using InterportCargo.BusinessLogic.Entities;
using InterportCargo.DataAccess.Interfaces;

namespace InterportCargo.BusinessLogic.Interfaces
{
    /// <summary>
    /// Service interface for unified authentication operations
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Authenticates a user (customer or employee) and returns user information
        /// </summary>
        /// <param name="email">User email address</param>
        /// <param name="password">Plain text password</param>
        /// <returns>User authentication result with user information</returns>
        AuthenticationResult AuthenticateUser(string email, string password);
    }

    /// <summary>
    /// Result of authentication operation
    /// </summary>
    public class AuthenticationResult
    {
        /// <summary>
        /// Indicates whether the authentication was successful
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Error message if authentication failed
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// User email if authenticated
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// User name if authenticated
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// User type (Customer or Employee)
        /// </summary>
        public string? UserType { get; set; }

        /// <summary>
        /// User ID if authenticated
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Creates a successful authentication result
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="userName">User name</param>
        /// <param name="userType">User type</param>
        /// <param name="userId">User ID</param>
        /// <returns>Successful authentication result</returns>
        public static AuthenticationResult Success(string email, string userName, string userType, int userId)
        {
            return new AuthenticationResult
            {
                IsSuccess = true,
                Email = email,
                UserName = userName,
                UserType = userType,
                UserId = userId
            };
        }

        /// <summary>
        /// Creates a failed authentication result
        /// </summary>
        /// <param name="errorMessage">Error message</param>
        /// <returns>Failed authentication result</returns>
        public static AuthenticationResult Failure(string errorMessage)
        {
            return new AuthenticationResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage
            };
        }
    }
}
