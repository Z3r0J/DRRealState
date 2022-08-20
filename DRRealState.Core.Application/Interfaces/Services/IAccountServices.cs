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
        Task<RegisterResponse> RegisterClientAsync(RegisterRequest request, string origin);
        Task<RegisterResponse> RegisterDeveloperAsync(RegisterRequest request);
        Task<RegisterResponse> RegisterAgentAsync(RegisterRequest request);
        Task<RegisterResponse> RegisterAdministratorAsync(RegisterRequest request);
        Task<RegisterResponse> AddPhotoAsync(string photoUrl, string Id);
        Task<EditResponse> EditAsync(EditRequest request);
        Task<PasswordResponse> ChangePasswordAsync(PasswordRequest request);
        Task<EditResponse> EditAgentAsync(EditRequest request);
        Task<string> ConfirmAccountAsync(string userId, string token);
        Task<DeleteResponse> DeleteUserAsync(string Id);
        Task LogOutAsync();
        Task<List<AccountResponse>> GetUsersAsync();
    }
}