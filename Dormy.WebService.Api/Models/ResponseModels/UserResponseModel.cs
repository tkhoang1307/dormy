﻿namespace Dormy.WebService.Api.Models.ResponseModels
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

        // Contract
        public Guid? ContractId { get; set; } = null;
        public string? ContractStatus { get; set; } = string.Empty;
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public Guid? RoomId { get; set; }
    }

    public class UserCreationResponseModel
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
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
