using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Models.Enums;
using Dormy.WebService.Api.Models.RequestModels;
using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Presentation.Validations
{
    public class RequestStatusChangeValidator
    {
        public static (bool, string) VerifyRequestStatusChangeValidator(RequestStatusEnum currentStatus, RequestStatusEnum changedStatus)
        {
            switch (currentStatus)
            {
                case RequestStatusEnum.SUBMITTED: //SUBMITTED => all statuses
                    if (changedStatus == RequestStatusEnum.SUBMITTED)
                    {
                        return (true, ErrorMessages.ChangeRequestStatusSubmittedConflict);
                        // {0} was SUBMITTED. You can change to other statuses except SUBMITTED.
                    }
                    break;
                case RequestStatusEnum.APPROVED: //APPROVED => CANCELLED
                    if (changedStatus == RequestStatusEnum.SUBMITTED ||
                        changedStatus == RequestStatusEnum.APPROVED ||
                        changedStatus == RequestStatusEnum.REJECTED)
                    {
                        return (true, ErrorMessages.ChangeRequestStatusApproveConflict);
                        // {0} was APPROVED. You can not change to other statuses except CANCELLED.
                    }
                    break;
                case RequestStatusEnum.REJECTED: //REJECTED => x
                    if (changedStatus == RequestStatusEnum.SUBMITTED ||
                        changedStatus == RequestStatusEnum.APPROVED ||
                        changedStatus == RequestStatusEnum.REJECTED ||
                        changedStatus == RequestStatusEnum.CANCELLED)
                    {
                        return (true, ErrorMessages.ChangeRequestStatusRejectedConflict);
                        // {0} was REJECTED. You can not change to other statuses.
                    }
                    break;
                case RequestStatusEnum.CANCELLED: //CANCELLED => x
                    if (changedStatus == RequestStatusEnum.SUBMITTED ||
                        changedStatus == RequestStatusEnum.APPROVED ||
                        changedStatus == RequestStatusEnum.REJECTED ||
                        changedStatus == RequestStatusEnum.CANCELLED)
                    {
                        return (true, ErrorMessages.ChangeRequestStatusCancelledConflict);
                        // {0} was CANCELLED. You can not change to other statuses.
                    }
                    break;
            }

            return (false, "");
        }
    }
}
