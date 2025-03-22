namespace Dormy.WebService.Api.Models.RequestModels
{
    public class GetBatchRequestModel
    {
        public bool IsGetAll { get; set; } = false;
        public List<Guid> Ids { get; set; } = [];
    }

    public class GetBatchGuardianRequestModel : GetBatchRequestModel
    {
        public Guid? UserId { get; set; }
    }

    public class GetBatchVehicleRequestModel : GetBatchRequestModel
    {
        public Guid? UserId { get; set; }
    }

    public class GetBatchInvoiceRequestModel : GetBatchRequestModel
    {
        public Guid? RoomId { get; set; }

        public string InvoiceType { get; set; } = string.Empty;
    }

    public class GetBatchServiceIndicatorRequestModel
    {
        public Guid? RoomServiceId { get; set; }
        public Guid? RoomId { get; set; }
        public List<Guid> Ids { get; set; } = [];
    }
}
