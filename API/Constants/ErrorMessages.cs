using Notebook.Models;

namespace Notebook.Constants
{
    public static class ErrorMessages
    {
        public const string BookNameRequired = "Book name is required";
        public const string BookNameAlreadyExists = "Book with the same name already exists";
        public const string UserNotFound = "User not found";
        public const string UserNameRequired = "User name is required";
        public const string UserNameAlreadyExists = "User with same user name already exists";
        public const string ErrorOccurredDuringRegistration = "An error occurred during registration";
        public const string EmailAlreadyExists = "User with same email already exists";
        public const string UserLockedOut = "User is locked out";
        public const string UserNotAllowed = "User is not allowed to login";
        public const string TwoFactorRequired = "Two-factor authentication is required";
        public const string InvalidLoginAttempt = "Invalid login attempt";
    }
}
