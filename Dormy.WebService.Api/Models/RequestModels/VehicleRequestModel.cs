namespace Dormy.WebService.Api.Models.RequestModels
{
    public class VehicleRequestModel
    {
        public string LicensePlate { get; set; } = string.Empty;

        public string VehicleType { get; set; } = string.Empty;
    }

    public class VehicleUpdationRequestModel: VehicleRequestModel
    {
        public Guid Id { get; set; }
    }
}
