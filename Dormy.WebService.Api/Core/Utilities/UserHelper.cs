using Dormy.WebService.Api.Core.Entities;

namespace Dormy.WebService.Api.Core.Utilities
{
    public class UserHelper
    {
        public static string ConvertAdminIdToAdminFullname(AdminEntity? adminEntity)
        {
            string adminFullname = "";
            if (adminEntity?.FirstName != null || adminEntity?.LastName != null)
            {
                adminFullname = adminEntity.FirstName + " " + adminEntity.LastName;
            }

            return adminFullname;
        }

        public static string ConvertUserIdToUserFullname(UserEntity? userEntity)
        {
            string userFullname = "";
            if (userEntity?.FirstName != null || userEntity?.LastName != null)
            {
                userFullname = userEntity.FirstName + " " + userEntity.LastName;
            }

            return userFullname;
        }
    }
}
