namespace Dormy.WebService.Api.Models.RequestModels
{
    public class GetBatchRequestModel
    {
        public bool IsGetAll { get; set; } = false;
        public List<Guid> Ids { get; set; } = [];
    }
}
