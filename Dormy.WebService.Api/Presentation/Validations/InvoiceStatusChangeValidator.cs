using Dormy.WebService.Api.Core.Constants;
using Dormy.WebService.Api.Models.Enums;

namespace Dormy.WebService.Api.Presentation.Validations
{
    public class InvoiceStatusChangeValidator
    {
        public static (bool, string) VerifyInvoiceStatusChangeValidator(InvoiceStatusEnum currentStatus, InvoiceStatusEnum changedStatus)
        {
            switch (currentStatus)
            {
                case InvoiceStatusEnum.DRAFT: //DRAFT => UNPAID, CANCELLED
                    if (changedStatus != InvoiceStatusEnum.UNPAID &&
                        changedStatus != InvoiceStatusEnum.CANCELLED)
                    {
                        return (true, ErrorMessages.ChangeInvoiceStatusDraftConflict);
                        // {0} is DRAFT version. You only can change to UNPAID or CANCELLED.
                    }
                    break;
                case InvoiceStatusEnum.UNPAID: //UNPAID => PAID, OVERDUE, CANCELLED
                    if (changedStatus == InvoiceStatusEnum.DRAFT ||
                        changedStatus == InvoiceStatusEnum.UNPAID)
                    {
                        return (true, ErrorMessages.ChangeInvoiceStatusUnpaidConflict);
                        // {0} is still UNPAID. You only can change to PAID or OVERDUE or CANCELLED.
                    }
                    break;
                case InvoiceStatusEnum.PAID: //PAID => x
                    if (changedStatus != null)
                    {
                        return (true, ErrorMessages.ChangeInvoiceStatusPaidConflict);
                        // {0} was PAID. You can not change to other statuses.
                    }
                    break;
                case InvoiceStatusEnum.OVERDUE: //OVERDUE => CANCELLED
                    if (changedStatus != InvoiceStatusEnum.CANCELLED)
                    {
                        return (true, ErrorMessages.ChangeInvoiceStatusOverdueConflict);
                        // {0} was OVERDUE. You only can change to CANCELLED.
                    }
                    break;
                case InvoiceStatusEnum.CANCELLED: //CANCELLED => x
                    if (changedStatus != null)
                    {
                        return (true, ErrorMessages.ChangeInvoiceStatusCancelledConflict);
                        // {0} was CANCELLED. You can not change to other statuses.
                    }
                    break;
            }

            return (false, "");
        }
    }
}
