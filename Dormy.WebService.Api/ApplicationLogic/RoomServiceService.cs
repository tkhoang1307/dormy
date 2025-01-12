using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Startup;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class RoomServiceService : IRoomServiceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoomServiceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
