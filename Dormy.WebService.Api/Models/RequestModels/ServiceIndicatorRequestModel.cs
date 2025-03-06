namespace Dormy.WebService.Api.Models.RequestModels
{
    public class ServiceIndicatorRequestModel
    {
        public Guid RoomServiceId { get; set; }

        public Guid RoomId { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public decimal OldIndicator { get; set; }

        public decimal NewIndicator { get; set; }
    }

    public class ServiceIndicatorUpdationRequestModel : ServiceIndicatorRequestModel
    {
        public Guid Id { get; set; }
    }
}
