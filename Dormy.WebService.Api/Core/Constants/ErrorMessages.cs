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
        public const string TwoPropertiesMustBeUnique = "{0} and {1} must be unique.";
        public const string RequiredFieldErrorMessage = "{0} is required field.";
        public const string PropertyMustBeMoreThanOrEqual0 = "{0} must be more than or equal to 0.";
        public const string PropertyMustBeMoreThan0 = "{0} must be more than 0.";
        public const string SomeServicesAreNotExisted = "Some of room services are not found";
        public const string SomeRoomTypesAreNotExisted = "Some of room types are not found";
        public const string SomeTypeServicesAreNotExisted = "Some of room type services are not found";
        public const string ValueDoesNotExistInEnum = "{0} doesn't exist in {1}";
        public const string AccountDoesNotHavePermission = "Your account doesn’t have permission";
        public const string UpdateEntityConflict = "You only update {0} which has status 'SUBMITTED'";
        public const string ChangeRequestStatusSubmittedConflict = "{0} was SUBMITTED. You can change to other statuses except SUBMITTED.";
        public const string ChangeRequestStatusApproveConflict = "{0} was APPROVED. You can not change to other statuses except CANCELLED.";
        public const string ChangeRequestStatusCancelledConflict = "{0} was CANCELLED. You can not change to other statuses.";
        public const string ChangeRequestStatusRejectedConflict = "{0} was REJECTED. You can not change to other statuses.";
    }
}
