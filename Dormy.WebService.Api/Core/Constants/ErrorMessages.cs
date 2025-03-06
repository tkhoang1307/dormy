using MimeKit.Cryptography;

namespace Dormy.WebService.Api.Core.Constants
{
    public static class ErrorMessages
    {
        public const string DuplicatedErrorMessage = "New password must be different with current password.";
        public const string PasswordDoesNotMatchErrorMessage = "Password is incorrect.";
        public const string UsernameIsExisted = "Username is existed.";
        public const string CanNotDeleteNotEmptyEntity = "Can not delete data, related data existed";
        public const string RoomIsOccupiedErrorMessage = "Can not delete data, room is current in used";
        public const string RoomCapacityIsSmallerThanCurrentErrorMessage = "Can not set room capicity smnaller than actual value";
        public const string PropertyDoesNotExist = "{0} does not exist.";
        public const string InvalidMonth = "{0} is in range [1, 12]";
        public const string TwoPropertiesMustBeUnique = "{0} and {1} must be unique.";
        public const string RequiredFieldErrorMessage = "{0} is required field.";
        public const string PropertyMustBeMoreThanOrEqual0 = "{0} must be more than or equal to 0.";
        public const string PropertyMustBeMoreThan0 = "{0} must be more than 0.";
        public const string PropertyAMustBeLessThanOrEqualToPropertyB = "{0} must be less than or equal to {1}.";
        public const string SomeServicesAreNotExisted = "Some of room services are not found";
        public const string SomeRoomTypesAreNotExisted = "Some of room types are not found";
        public const string SomeTypeServicesAreNotExisted = "Some of room type services are not found";
        public const string ValueDoesNotExistInEnum = "{0} doesn't exist in {1}";
        public const string AccountDoesNotHavePermission = "Your account doesn’t have permission";
        public const string UpdateEntityConflict = "You only update {0} which has status 'SUBMITTED'";
        public const string CreateInvoiceConflict = "Invoice for month {0}/{1} was created.";
        public const string UpdateInvoiceConflict = "Invoice was only updated when status is DRAFT.";
        public const string DeleteInvoiceConflict = "Invoice was only deleted when status is DRAFT.";
        public const string AccountDoesNotHavePermissionInvoice = "Your account doesn’t have permission to access this invoice";
        public const string ChangeRequestStatusSubmittedConflict = "{0} was SUBMITTED. You can change to other statuses except SUBMITTED.";
        public const string ChangeRequestStatusApproveConflict = "{0} was APPROVED. You can not change to other statuses except CANCELLED.";
        public const string ChangeRequestStatusCancelledConflict = "{0} was CANCELLED. You can not change to other statuses.";
        public const string ChangeRequestStatusRejectedConflict = "{0} was REJECTED. You can not change to other statuses.";
        public const string ChangeInvoiceStatusDraftConflict = "{0} is DRAFT version. You only can change to UNPAID or CANCELLED.";
        public const string ChangeInvoiceStatusUnpaidConflict = "{0} is still UNPAID. You only can change to PAID or OVERDUE or CANCELLED.";
        public const string ChangeInvoiceStatusPaidConflict = "{0} was PAID. You can not change to other statuses.";
        public const string ChangeInvoiceStatusOverdueConflict = "{0} was OVERDUE. You only can change to CANCELLED.";
        public const string ChangeInvoiceStatusCancelledConflict = "{0} was CANCELLED. You can not change to other statuses.";
        public const string ChangeContractStatusPendingConflict = "{0} is PENDING. You only can change to WAITING_PAYMENT or REJECTED.";
        public const string ChangeContractStatusWaitingPaymentConflict = "{0} is still WAITING_PAYMENT. You only can change to ACTIVE.";
        public const string ChangeContractStatusActiveConflict = "{0} is being ACTIVE. You only can change to EXTENDED or EXPIRED or TERMINATED.";
        public const string ChangeContractStatusExtendedConflict = "{0} was EXTENDED. You only can keep this status.";
        public const string ChangeContractStatusExpiredConflict = "{0} was EXPIRED. You only can change to EXTENDED.";
        public const string ChangeContractStatusTerminatedConflict = "{0} was TERMINATED. You can not change to other statuses.";
        public const string ChangeContractStatusRejectedConflict = "{0} was REJECTED. You can not change to other statuses.";
        public const string ChangeContractExtensionStatusActiveConflict = "{0} is being ACTIVE. You only can change to EXPIRED or TERMINATED.";
        public const string CreateServiceIndicatorConflict = "{0} indicator for month {1}/{2} existed.";
    }
}
