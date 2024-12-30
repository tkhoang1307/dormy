namespace Dormy.WebService.Api.Core.Entities
{
    public class SettingEntity: BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;

        public bool ParameterBool { get; set; }

        public DateTime ParameterDate { get; set; } = DateTime.Now;

        public Guid AdminId { get; set; }

        public AdminEntity Admin { get; set; } = null!;
    }
}
