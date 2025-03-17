using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Core.Entities
{
    public class SettingEntity: BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string KeyName { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;

        public SettingDataTypeEnum DataType {  get; set; }

        public bool IsApplied { get; set; } = false;
    }
}
