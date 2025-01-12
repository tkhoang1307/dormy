namespace Dormy.WebService.Api.Core.Entities
{
    public class InvoiceUserEntity : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid InvoiceId { get; set; }

        public InvoiceEntity Invoice { get; set; } = null!;

        public Guid UserId { get; set; }

        public UserEntity User { get; set; } = null!;
    }
}
