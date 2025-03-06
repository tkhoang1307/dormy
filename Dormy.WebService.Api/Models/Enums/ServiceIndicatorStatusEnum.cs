using System.ComponentModel;

namespace Dormy.WebService.Api.Models.Enums
{
    public enum ServiceIndicatorStatusEnum
    {
        [Description("Temporary")]
        TEMPORARY,
        [Description("Verifying")]
        VERIFYING,
        [Description("Correct")]
        CORRECT,
    }
}
