namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class RoomTypeBatchResponseModel : BaseResponseModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string RoomTypeName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int Capacity { get; set; }

        public decimal Price { get; set; }
    }

    public class RoomTypeResponseModel : RoomTypeBatchResponseModel
    {
        public List<RoomServiceResponseModel> RoomServices { get; set; } = [];
    }


    public class RoomTypeRegistrationResponseModel
    {
        public Guid Id { get; set; }
        public string RoomTypeName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public decimal Price { get; set; }
        public List<RoomServiceRegistrationResponseModel> RoomServices { get; set; } = [];
    }
}
