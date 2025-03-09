namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class WorkplaceResponseModel: BaseResponseModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string Abbrevation { get; set; } = string.Empty;
    }
}
