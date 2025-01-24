namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class RoomServiceResponseModel : BaseResponseModel
    {
        public Guid Id { get; set; }

        public string RoomServiceName { get; set; } = string.Empty;

        public string Unit { get; set; } = string.Empty;

        public decimal Cost { get; set; }
    }
}
