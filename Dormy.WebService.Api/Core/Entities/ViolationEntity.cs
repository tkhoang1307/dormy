namespace Dormy.WebService.Api.Core.Entities
{
    public class ViolationEntity : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Description { get; set; } = string.Empty;

        public DateTime ViolationDate { get; set; } = DateTime.Now;

        public decimal Penalty { get; set; }

        public Guid UserId { get; set; }

        public UserEntity User { get; set; } = null!;
    }
}
