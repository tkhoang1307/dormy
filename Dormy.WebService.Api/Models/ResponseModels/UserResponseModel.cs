namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class UserResponseModel
    {
        public Guid UserId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; } = DateTime.Now;

        public string PhoneNumber { get; set; } = string.Empty;

        public string NationalIdNumber { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;
    }

    public class UserProfileResponseModel : UserResponseModel
    {
        public List<GuardiansResponseModel> Guardians { get; set; } = [];

        public List<VehicleResponseModel> Vehicles { get; set; } = [];

        public WorkplaceResponseModel? Workplace { get; set; }

        public HealthInsuranceResponseModel? HealthInsurance { get; set; }
    }

    public class GuardiansResponseModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
