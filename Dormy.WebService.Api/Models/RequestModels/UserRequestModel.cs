namespace Dormy.WebService.Api.Models.RequestModels
{
    public class UserRequestModel
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; } = DateTime.Now;

        public string PhoneNumber { get; set; } = string.Empty;

        public string NationalIdNumber { get; set; } = string.Empty;

        // GenderEnum
        public string Gender { get; set; } = string.Empty;
    }

    public class UserUpdateRequestModel
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; } = DateTime.Now;

        public string PhoneNumber { get; set; } = string.Empty;

        public string NationalIdNumber { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;
    }

    public class UserRegisterRequestModel
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;
        
        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; } = DateTime.Now;

        public string PhoneNumber { get; set; } = string.Empty;

        public string NationalIdNumber { get; set; } = string.Empty;

        // GenderEnum MALE|FEMALE|OTHER
        public string Gender { get; set; } = string.Empty;
    }
}
