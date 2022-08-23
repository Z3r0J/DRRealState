using DRRealState.Core.Application.Interfaces.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.Features.SaleTypes.Commands.DeleteSaleTypeById
{
    public class DeleteSaleTypeByIdCommand : IRequest<int>
    {
        public int Id { get; set; }
    }

    public class DeleteSaleTypeByIdCommandHandler : IRequestHandler<DeleteSaleTypeByIdCommand, int>
    {
        private readonly ISaleTypeRepository _saleTypeRepository;
        public DeleteSaleTypeByIdCommandHandler(ISaleTypeRepository saleTypeRepository)
        {
            _saleTypeRepository = saleTypeRepository;
        }
        public async Task<int> Handle(DeleteSaleTypeByIdCommand command, CancellationToken cancellationToken)
        {
            var saleType = await _saleTypeRepository.GetByIdAsync(command.Id);

            if (saleType == null) { throw new Exception($"Types for sales not found."); }

            await _saleTypeRepository.DeleteAsync(saleType);

            return saleType.Id;
        }
    }
}
