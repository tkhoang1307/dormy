using Dormy.WebService.Api.Models.ResponseModels;

namespace Dormy.WebService.Api.Core.Interfaces
{
    public interface ITokenRetriever
    {
        string CreateToken(JwtResponseModel jwtReponseModel);
    }
}
