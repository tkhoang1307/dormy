namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class ServiceIndicatorResponseModel : BaseResponseModel
    {
        public Guid Id { get; set; }

        public Guid RoomServiceId {  get; set; }

        public string RoomServiceName { get; set; } = string.Empty;

        public Guid RoomId { get; set; }

        public string RoomName { get; set;} = string.Empty;

        public int Month { get; set; }

        public int Year { get; set; }

        public decimal OldIndicator { get; set; }

        public decimal NewIndicator { get; set; }
    }
}
