using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Startup;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class RoomTypeServiceService : IRoomTypeServiceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoomTypeServiceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
