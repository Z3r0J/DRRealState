using DRRealState.Core.Application.DTOS.Account;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.Interfaces.Services
{
    public interface IAccountServices
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task<AuthenticationResponse> AuthenticateWebAppAsync(AuthenticationRequest request);
        Task<ActivateResponse> ActivateAsync(ActivateRequest request);
        Task<ActivateResponse> DeactivateAsync(ActivateRequest request);

        Task LogOutAsync();
        Task<List<AccountResponse>> GetUsersAsync();
        Task<RegisterResponse> RegisterAdministratorAsync(RegisterRequest request);
    }
}