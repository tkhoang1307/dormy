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

    public class UserProfileResponseModel
    {
        public UserProfileDetailInformationResponseModel User { get; set; } = null!;
        public List<UserProfileGuardianResponseModel> Guardians { get; set; } = [];
        public UserProfileWorkplaceResponseModel? Workplace { get; set; }
        public UserProfileHealthInsuranceResponseModel? HealthInsurance { get; set; }
        public UserProfileContractResponseModel Contract { get; set; } = null!;
    }

    public class UserProfileGuardianResponseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string RelationshipToUser { get; set; } = null!;
    }

    public class UserProfileWorkplaceResponseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Abbrevation { get; set; } = null!;
    }

    public class UserProfileHealthInsuranceResponseModel
    {
        public Guid Id { get; set; }
        public string InsuranceCardNumber { get; set; } = null!;
        public string RegisteredHospital { get; set; } = null!;
        public DateTime ExpirationDate { get; set; }
    }

    public class UserProfileDetailInformationResponseModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string NationalIdNumber { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string Gender { get; set; } = null!;
    }

    public class UserProfileContractResponseModel
    {
        public Guid Id { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = null!;
        public int NumberExtension { get; set; }
        public Guid? ApproverId { get; set; }
        public string ApproverFullName { get; set; } = null!;
        public Guid RoomId { get; set; }
        public int RoomNumber { get; set; }
        public Guid RoomTypeId { get; set; }
        public string RoomTypeName { get; set; } = null!;
        public decimal Price { get; set; }
        public Guid BuildingId { get; set; }
        public string BuildingName { get; set; } = null!;
    }


}
