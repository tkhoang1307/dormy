namespace Dormy.WebService.Api.Core.Entities
{
    public class RoomUtilityEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public decimal ElectricityUsage { get; set; }

        public decimal ElectricityFee { get; set; }

        public decimal WaterUsage { get; set; }

        public decimal WaterFee { get; set; }

        // Need to check again
        public DateTime Month { get; set; } = DateTime.Now;
    }
}
