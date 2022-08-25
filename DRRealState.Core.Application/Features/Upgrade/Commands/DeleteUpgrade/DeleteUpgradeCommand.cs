using DRRealState.Core.Application.Interfaces.Services;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.Features.Upgrade.Commands.DeleteUpgrade
{
    /// <summary>
    /// Parameters to Delete an Upgrade
    /// </summary>
    public class DeleteUpgradeCommand :IRequest<int>
    {
        /// <example>
        /// 1
        /// </example>
        [SwaggerParameter(Description = "Id of Upgrade to delete")]
        public int Id { get; set; }
    }

    public class DeleteUpgradeCommandHandler : IRequestHandler<DeleteUpgradeCommand, int>
    {
        private readonly IUpgradeServices _upgradeServices;
        public DeleteUpgradeCommandHandler(IUpgradeServices upgradeServices)
        {
            _upgradeServices = upgradeServices;
        }
        public async Task<int> Handle(DeleteUpgradeCommand request, CancellationToken cancellationToken)
        {

            var up = await _upgradeServices.GetByIdSaveViewModel(request.Id);

            if (up == null) { throw new Exception("Upgrade not found"); }

            await _upgradeServices.Delete(request.Id);

            return up.Id;
        }
    }
}
