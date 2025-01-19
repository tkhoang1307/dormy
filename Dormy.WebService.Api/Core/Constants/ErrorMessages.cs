namespace Dormy.WebService.Api.Core.Constants
{
    public static class ErrorMessages
    {
        public const string DuplicatedErrorMessage = "New password must be different with current password.";
        public const string UsernameIsExisted = "Username is existed.";
        public const string CanNotDeleteNotEmptyEntity = "Can not delete data, related data existed";
        public const string BedIsOccupiedErrorMessage = "Can not delete data, bed is current in used";
        public const string RoomCapacityIsSmallerThanCurrentErrorMessage = "Can not set room capicity smnaller than actual value";
    }
}
