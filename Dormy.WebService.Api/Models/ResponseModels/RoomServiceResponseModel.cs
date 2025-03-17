namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class RoomServiceResponseModel : BaseResponseModel
    {
        public Guid Id { get; set; }

        public string RoomServiceName { get; set; } = string.Empty;

        public string Unit { get; set; } = string.Empty;

        public decimal Cost { get; set; }

        public string RoomServiceType { get; set; } = string.Empty;

        public bool IsServiceIndicatorUsed { get; set; }
    }

    public class EnumResponseModel
    {
        public string EnumValue { get; set; } = string.Empty;

        public string VietnameseEnumDescription { get; set; } = string.Empty;

        public string EnglishEnumDescription { get; set; } = string.Empty;
    }

    public class RoomServiceRegistrationResponseModel
    {
        public Guid Id { get; set; }
        public string RoomServiceName { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public decimal Cost { get; set; }
    }
}
