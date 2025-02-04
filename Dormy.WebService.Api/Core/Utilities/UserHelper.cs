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
    }
}
