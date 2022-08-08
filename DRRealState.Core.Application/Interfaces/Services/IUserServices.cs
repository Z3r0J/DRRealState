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
        Task<AuthenticationResponse> LoginAsync(LoginViewModel model);
        Task<RegisterResponse> RegisterAdministratorAsync(SaveUserViewModel model);
        Task<ActivateResponse> ActivateAsync(ActivateViewModel model);
        Task<ActivateResponse> DeactivateAsync(ActivateViewModel model);
        Task LogOutAsync();
        Task<List<UserViewModel>> GetAllUserAsync();

    }
}
