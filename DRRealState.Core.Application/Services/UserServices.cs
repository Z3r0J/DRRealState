using AutoMapper;
using DRRealState.Core.Application.DTOS.Account;
using DRRealState.Core.Application.Interfaces.Services;
using DRRealState.Core.Application.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.Services
{
    public class UserServices : IUserServices
    {
        private readonly IAccountServices _accountServices;
        private readonly IMapper _mapper;

        public UserServices(IAccountServices accountServices, IMapper mapper)
        {
            _accountServices = accountServices;
            _mapper = mapper;
        }

        public async Task<AuthenticationResponse> LoginApiAsync(LoginViewModel model) {

            AuthenticationRequest request = _mapper.Map<AuthenticationRequest>(model);

            AuthenticationResponse response = await _accountServices.AuthenticateAsync(request);

            return response;
        }
        

        public async Task<AuthenticationResponse> LoginWebAppAsync(LoginViewModel model) {

            AuthenticationRequest request = _mapper.Map<AuthenticationRequest>(model);

            AuthenticationResponse response = await _accountServices.AuthenticateWebAppAsync(request);

            return response;
        }

        public async Task<RegisterResponse> RegisterAdministratorAsync(SaveUserViewModel model) {

            RegisterRequest request = _mapper.Map<RegisterRequest>(model);

            return await _accountServices.RegisterAdministratorAsync(request);

        }

        public async Task<RegisterResponse> RegisterDeveloper(SaveUserViewModel model) {

            RegisterRequest request = _mapper.Map<RegisterRequest>(model);

            return await _accountServices.RegisterDeveloperAsync(request);

        }

        public async Task<RegisterResponse> RegisterAgent(SaveUserViewModel model) {

            RegisterRequest request = _mapper.Map<RegisterRequest>(model);

            return await _accountServices.RegisterAgentAsync(request);

        }

        public async Task<string> ConfirmEmailAsync(string userId, string token) {

            return await _accountServices.ConfirmAccountAsync(userId, token);
        }

        public async Task<RegisterResponse> RegisterClient(SaveUserViewModel model,string origin) {

            RegisterRequest request = _mapper.Map<RegisterRequest>(model);

            return await _accountServices.RegisterClientAsync(request,origin);

        }
        
        public async Task<RegisterResponse> AddPhoto(string PhotoUrl,string Id) {

            return await _accountServices.AddPhotoAsync(PhotoUrl,Id);

        }


        public async Task<List<UserViewModel>> GetAllUserAsync() {

            var response = await _accountServices.GetUsersAsync();

            return _mapper.Map<List<UserViewModel>>(response);
        
        }

        public async Task<List<UserViewModel>> SearchAgentAsync(string Name) {

            var response = await GetAllUserAsync();
            var search = response.Select(x => new {
                name = $"{x.FirstName} {x.LastName}",
                x.Roles,
                x.IsVerified,
                x.UserName,
                x.Email,
                x.FirstName,
                x.LastName,
                x.Phone,
                x.PhotoUrl,
                x.Id
            }).Where(x => x.name.Trim().Contains(Name.Trim()) && x.Roles.Any(x => x == "AGENT") && x.IsVerified == true).ToList();

            return search.Select(x=>new UserViewModel() { Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Id = x.Id,
                IsVerified = x.IsVerified,
                Phone = x.Phone,
                PhotoUrl = x.PhotoUrl,
                Roles = x.Roles,
                UserName = x.UserName
            }).ToList();
        }
        public async Task<ActivateResponse> ActivateAsync(ActivateViewModel model)
        {

            return await _accountServices.ActivateAsync(_mapper.Map<ActivateRequest>(model));

        }
        public async Task<ActivateResponse> DeactivateAsync(ActivateViewModel model)
        {

            return await _accountServices.DeactivateAsync(_mapper.Map<ActivateRequest>(model));

        }
        public async Task LogOutAsync() {
            await _accountServices.LogOutAsync();    

        }
    }
}
