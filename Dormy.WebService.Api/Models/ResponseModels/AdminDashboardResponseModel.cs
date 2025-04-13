namespace Dormy.WebService.Api.Models.ResponseModels
{
    public class AdminDashboardResponseModel
    {
        public int TotalRegistrations { get; set; }
        public int TotalBeds { get; set; }
        public int TotalUsedBeds { get; set; }
        public int TotalEmptyBeds { get; set; }
        public int TotalUsers { get; set; }
        public int TotalCurrentUsers { get; set; }
        public int TotalRequests { get; set; }
        public int TotalParkingRequests { get; set; }
        public int TotalUnResovledRequests { get; set; }
        public int TotalUnResovledParkingRequests { get; set; }
        public int TotalMaleUsers { get; set; }
        public int TotalFemaleUsers { get; set; }
    }
}
