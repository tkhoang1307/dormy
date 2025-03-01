using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Presentation.Validations
{
    public class ContractStatusChangeValidator
    {
        public static (bool, string) VerifyContractStatusChangeValidator(ContractStatusEnum currentStatus, ContractStatusEnum changedStatus)
        {
            switch (currentStatus)
            {
                case ContractStatusEnum.PENDING: //PENDING => WAITING_PAYMENT, REJECTED
                    if (changedStatus != ContractStatusEnum.WAITING_PAYMENT &&
                        changedStatus != ContractStatusEnum.REJECTED)
                    {
                        return (true, ErrorMessages.ChangeContractStatusPendingConflict);
                        // {0} is PENDING. You only can change to WAITING_PAYMENT or REJECTED.
                    }
                    break;
                case ContractStatusEnum.WAITING_PAYMENT: //WAITING_PAYMENT => ACTIVE
                    if (changedStatus != ContractStatusEnum.ACTIVE)
                    {
                        return (true, ErrorMessages.ChangeContractStatusWaitingPaymentConflict);
                        // {0} is still WAITING_PAYMENT. You only can change to ACTIVE.
                    }
                    break;
                case ContractStatusEnum.ACTIVE: //ACTIVE => EXTENDED, EXPIRED, TERMINATED
                    if (changedStatus != ContractStatusEnum.EXTENDED &&
                        changedStatus != ContractStatusEnum.EXPIRED &&
                        changedStatus != ContractStatusEnum.TERMINATED)
                    {
                        return (true, ErrorMessages.ChangeContractStatusActiveConflict);
                        // {0} is being ACTIVE. You only can change to EXTENDED or EXPIRED or TERMINATED.
                    }
                    break;
                case ContractStatusEnum.EXTENDED: //EXTENDED => EXTENDED
                    if (changedStatus != ContractStatusEnum.EXTENDED)
                    {
                        return (true, ErrorMessages.ChangeContractStatusExtendedConflict);
                        // {0} was EXTENDED. You only can keep this status.
                    }
                    break;
                case ContractStatusEnum.EXPIRED: //EXPIRED => EXTENDED
                    if (changedStatus != ContractStatusEnum.EXTENDED)
                    {
                        return (true, ErrorMessages.ChangeContractStatusExpiredConflict);
                        // {0} was EXPIRED. You only can change to EXTENDED.
                    }
                    break;
                case ContractStatusEnum.TERMINATED: //TERMINATED => x
                    if (changedStatus != null)
                    {
                        return (true, ErrorMessages.ChangeContractStatusTerminatedConflict);
                        // {0} was TERMINATED. You can not change to other statuses.
                    }
                    break;
                case ContractStatusEnum.REJECTED: //REJECTED => x
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
