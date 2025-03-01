using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Presentation.Validations
{
    public class ContractExtensionStatusChangeValidator
    {
        public static (bool, string) VerifyContractExtensionStatusChangeValidator(ContractExtensionStatusEnum currentStatus, ContractExtensionStatusEnum changedStatus)
        {
            switch (currentStatus)
            {
                case ContractExtensionStatusEnum.PENDING: //PENDING => WAITING_PAYMENT, REJECTED
                    if (changedStatus != ContractExtensionStatusEnum.WAITING_PAYMENT &&
                        changedStatus != ContractExtensionStatusEnum.REJECTED)
                    {
                        return (true, ErrorMessages.ChangeContractStatusPendingConflict);
                        // {0} is PENDING. You only can change to WAITING_PAYMENT or REJECTED.
                    }
                    break;
                case ContractExtensionStatusEnum.WAITING_PAYMENT: //WAITING_PAYMENT => ACTIVE
                    if (changedStatus != ContractExtensionStatusEnum.ACTIVE)
                    {
                        return (true, ErrorMessages.ChangeContractStatusWaitingPaymentConflict);
                        // {0} is still WAITING_PAYMENT. You only can change to ACTIVE.
                    }
                    break;
                case ContractExtensionStatusEnum.ACTIVE: //ACTIVE => EXPIRED, TERMINATED
                    if (changedStatus != ContractExtensionStatusEnum.EXPIRED &&
                        changedStatus != ContractExtensionStatusEnum.TERMINATED)
                    {
                        return (true, ErrorMessages.ChangeContractExtensionStatusActiveConflict);
                        // {0} is being ACTIVE. You only can change to EXPIRED or TERMINATED.
                    }
                    break;
                case ContractExtensionStatusEnum.EXPIRED: //EXPIRED => x
                    if (changedStatus != null)
                    {
                        return (true, ErrorMessages.ChangeContractStatusExpiredConflict);
                        // {0} was EXPIRED. You only can change to EXTENDED.
                    }
                    break;
                case ContractExtensionStatusEnum.TERMINATED: //TERMINATED => x
                    if (changedStatus != null)
                    {
                        return (true, ErrorMessages.ChangeContractStatusTerminatedConflict);
                        // {0} was TERMINATED. You can not change to other statuses.
                    }
                    break;
                case ContractExtensionStatusEnum.REJECTED: //REJECTED => x
                    if (changedStatus != null)
                    {
                        return (true, ErrorMessages.ChangeContractStatusRejectedConflict);
                        // {0} was REJECTED. You can not change to other statuses.
                    }
                    break;
            }

            return (false, "");
        }
    }
}
