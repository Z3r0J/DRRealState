using DRRealState.Core.Application.DTOS.Account;
using DRRealState.Core.Application.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.Interfaces.Services
{
    public interface IUserServices
    {
        Task<AuthenticationResponse> LoginApiAsync(LoginViewModel model);
        Task<AuthenticationResponse> LoginWebAppAsync(LoginViewModel model);
        Task<RegisterResponse> RegisterAdministratorAsync(SaveUserViewModel model);
        Task<RegisterResponse> RegisterDeveloper(SaveUserViewModel model);
        Task<RegisterResponse> RegisterAgent(SaveUserViewModel model);
        Task<RegisterResponse> RegisterClient(SaveUserViewModel model, string origin);
        Task<RegisterResponse> AddPhoto(string PhotoUrl, string Id);
        Task<ActivateResponse> ActivateAsync(ActivateViewModel model);
        Task<ActivateResponse> DeactivateAsync(ActivateViewModel model);
        Task<string> ConfirmEmailAsync(string userId, string token);
        Task LogOutAsync();
        Task<List<UserViewModel>> GetAllUserAsync();

    }
}
