using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Startup;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class ContractExtensionService: IContractExtensionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ContractExtensionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
