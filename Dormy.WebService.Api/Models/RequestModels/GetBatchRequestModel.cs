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
}
