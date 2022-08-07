using DRRealState.Core.Application.DTOS.Account;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.Interfaces.Services
{
    public interface IAccountServices
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task LogOutAsync();
        Task<List<AccountResponse>> GetUsersAsync();
        Task<RegisterResponse> RegisterAdministratorAsync(RegisterRequest request);
    }
}