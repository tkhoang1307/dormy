using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Core.Utilities;
using Dormy.WebService.Api.Models.ResponseModels;
using Dormy.WebService.Api.Presentation.Mappers;
using Dormy.WebService.Api.Startup;
using Microsoft.EntityFrameworkCore;

namespace Dormy.WebService.Api.ApplicationLogic
{
    public class DormyJobsService : IDormyJobsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly IContractService _contractService;
        private readonly IInvoiceService _invoiceService;
        private readonly IContractExtensionService _contractExtensionService;

        public DormyJobsService(IUnitOfWork unitOfWork, 
                                IUserContextService userContextService,
                                IContractService contractService,
                                IInvoiceService invoiceService,
                                IContractExtensionService contractExtensionService)
        {
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _contractService = contractService;
            _invoiceService = invoiceService;
            _contractExtensionService = contractExtensionService;
        }

        public async Task<ApiResponse> ContractJob()
        {
            var contractEntities = await _unitOfWork.ContractRepository.GetAllAsync(x => (x.Status == Models.Enums.ContractStatusEnum.ACTIVE ||
                                                                                          x.Status == Models.Enums.ContractStatusEnum.EXTENDED) && 
                                                                                          DateTime.Compare(x.EndDate.Date, DateTime.Now.Date) > 0);
            foreach(var contractEntity in contractEntities)
            {
                contractEntity.Status = Models.Enums.ContractStatusEnum.EXPIRED;
            }

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk();
        }

        public async Task<ApiResponse> ỊnvoiceJob()
        {
            var invoiceEntities = await _unitOfWork.InvoiceRepository.GetAllAsync(x => x.Status == Models.Enums.InvoiceStatusEnum.UNPAID && 
                                                                                       DateTime.Compare(x.DueDate.Date, DateTime.Now.Date) > 0);
            foreach (var invoiceEntity in invoiceEntities)
            {
                invoiceEntity.Status = Models.Enums.InvoiceStatusEnum.OVERDUE;
            }

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk();
        }

        public async Task<ApiResponse> ContractExtensionJob()
        {
            var contractExtensionEntities = await _unitOfWork.ContractExtensionRepository.GetAllAsync(x => x.Status == Models.Enums.ContractExtensionStatusEnum.ACTIVE && 
                                                                                                           DateTime.Compare(x.EndDate.Date, DateTime.Now.Date) > 0, 
                                                                                                      x => x.Include(x => x.Contract));
            
            foreach (var contractExtensionEntity in contractExtensionEntities)
            {
                contractExtensionEntity.Status = Models.Enums.ContractExtensionStatusEnum.EXPIRED;
                contractExtensionEntity.Contract.Status = Models.Enums.ContractStatusEnum.EXPIRED;
            }

            await _unitOfWork.SaveChangeAsync();

            return new ApiResponse().SetOk();
        }
    }
}
