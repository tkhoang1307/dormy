using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Presentation.Validations
{
    public class OvernightAbsenceStatusChangeValidator
    {
        public static (bool, string) VerifyOvernightAbsenceStatusChangeValidator(OvernightAbsenceStatusEnum currentStatus, OvernightAbsenceStatusEnum changedStatus)
        {
            switch (currentStatus)
            {
                case OvernightAbsenceStatusEnum.SUBMITTED: //SUBMITTED => all statuses
                    if (changedStatus == OvernightAbsenceStatusEnum.SUBMITTED)
                    {
                        return (true, ErrorMessages.ChangeRequestStatusSubmittedConflict);
                        // {0} was SUBMITTED. You can change to other statuses except SUBMITTED.
                    }
                    break;
                case OvernightAbsenceStatusEnum.APPROVED: //APPROVED => CANCELLED
                    if (changedStatus != OvernightAbsenceStatusEnum.CANCELLED)
                    {
                        return (true, ErrorMessages.ChangeRequestStatusApproveConflict);
                        // {0} was APPROVED. You can not change to other statuses except CANCELLED.
                    }
                    break;
                case OvernightAbsenceStatusEnum.REJECTED: //REJECTED => x
                    if (changedStatus != null)
                    {
                        return (true, ErrorMessages.ChangeRequestStatusRejectedConflict);
                        // {0} was REJECTED. You can not change to other statuses.
                    }
                    break;
                case OvernightAbsenceStatusEnum.CANCELLED: //CANCELLED => x
                    if (changedStatus != null)
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
