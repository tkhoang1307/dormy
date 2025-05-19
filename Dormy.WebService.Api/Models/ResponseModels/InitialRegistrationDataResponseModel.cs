using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class InitialRegistrationDataResponseModel
    {
        public List<EnumResponseModel>? GenderEnums { get; set; } = [];

        public List<EnumResponseModel>? RelationshipEnums { get; set; } = [];

        public List<InitialDataWorkplaceResponseModel>? ListWorkplaces { get; set; } = [];

        public List<InitialDataRoomTypeResponseModel>? ListRoomTypes { get; set; } = [];
    }

    public class InitialDataWorkplaceResponseModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Abbrevation { get; set; } = string.Empty;
    }

    public class InitialDataRoomTypeResponseModel
    {
        public Guid Id { get; set; }

        public string RoomTypeName { get; set; } = string.Empty;

        public int Capacity { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; } = string.Empty;
    }

    public class SearchBuildingAndRoomRequestModel
    {
        public Guid RoomTypeId { get; set; }

        public string Gender { get; set; } = string.Empty;
    }

    public class SearchBuildingsAndRoomsResponseModel
    {
        public Guid BuildingId { get; set; }

        public string BuildingName { get; set; } = string.Empty;

        public List<SearchRoomsResponseModel>? ListRooms { get; set; } = [];
    }

    public class SearchRoomsResponseModel
    {
        public Guid RoomId { get; set; }

        public int RoomNumber { get; set; }

        public int FloorNumber { get; set; }

        public int TotalUsedBed { get; set; }

        public int TotalAvailableBed { get; set; }

        public string Status { get; set; } = string.Empty;
    }

    public class InformationOfTheLatestContract
    {
        public Guid ContractId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public int NumberExtension { get; set; }
        public Guid RoomId { get; set; }
        public int RoomNumber { get; set; }
        public Guid BuildingId { get; set; }
        public string BuildingName { get; set;} = string.Empty;
        public Guid RoomTypeId { get; set; }
        public string RoomTypeName { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }

    public class UserInformation
    {
        public string Gender { get; set; } = string.Empty;
    }

    public class InitialCreateEntendContractDataResponseModel
    {
        public UserInformation UserInformation { get; set; } = null!;
        public InformationOfTheLatestContract? ContractInformation { get; set; } = null!;
        public List<InitialDataRoomTypeResponseModel>? ListRoomTypes { get; set; } = [];
    }
}
