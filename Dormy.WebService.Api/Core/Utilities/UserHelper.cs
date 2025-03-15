using Dormy.WebService.Api.Core.Entities;
using System.Globalization;

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

        public static string BuildPasswordForUser(DateTime date)
        {
            string day = date.Day.ToString("D2");

            string month = date.ToString("MMM", CultureInfo.InvariantCulture);

            string year = date.Year.ToString("D4");

            return day + month + year;
        }
    }
}
