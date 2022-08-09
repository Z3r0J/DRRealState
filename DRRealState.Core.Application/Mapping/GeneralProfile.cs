using AutoMapper;
using Microsoft.EntityFrameworkCore;
using DRRealState.Core.Application.DTOS.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DRRealState.Core.Application.ViewModel.User;
using DRRealState.Core.Application.ViewModel.PropertiesType;
using DRRealState.Core.Application.ViewModel.SaleType;
using DRRealState.Core.Domain.Entities;
using DRRealState.Core.Application.ViewModel.Estate;
using DRRealState.Core.Application.ViewModel.Upgrade;

namespace DRRealState.Core.Application.Mapping
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<AuthenticationRequest, LoginViewModel>()
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<RegisterRequest, SaveUserViewModel>()
                .ForMember(x => x.UserType, opt => opt.Ignore())
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<AccountResponse, UserViewModel>()
                .ForMember(x => x.Roles, opt => opt.MapFrom(ac => ac.Roles))
                .ReverseMap();

            CreateMap<PropertiesType, PropertiesTypeViewModel>()
                   .ForMember(x=> x.EstatesQuantity, opt => opt.MapFrom(pt=>pt.Estates.Count))
                   .ReverseMap()                   
                   .ForMember(x => x.Created, opt => opt.Ignore())
                   .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                   .ForMember(x => x.Modified, opt => opt.Ignore())
                   .ForMember(x => x.ModifiedBy, opt => opt.Ignore());

            CreateMap<PropertiesType, SavePropertiesTypeViewModel>()
                   .ReverseMap()
                   .ForMember(x => x.Created, opt => opt.Ignore())
                   .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                   .ForMember(x => x.Modified, opt => opt.Ignore())
                   .ForMember(x => x.ModifiedBy, opt => opt.Ignore());

            CreateMap<SaleType, SaleTypeViewModel>()
                   .ForMember(x => x.EstatesQuantity, opt => opt.MapFrom(pt => pt.Estates.Count))
                   .ReverseMap()                   
                   .ForMember(x => x.Created, opt => opt.Ignore())
                   .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                   .ForMember(x => x.Modified, opt => opt.Ignore())
                   .ForMember(x => x.ModifiedBy, opt => opt.Ignore());

            CreateMap<SaleType, SaveSaleTypeViewModel>()
                   .ReverseMap()
                   .ForMember(x => x.Created, opt => opt.Ignore())
                   .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                   .ForMember(x => x.Modified, opt => opt.Ignore())
                   .ForMember(x => x.ModifiedBy, opt => opt.Ignore());

            CreateMap<Estate,EstateViewModel>()
                .ForMember(x=>x.Upgrade,opt=>opt.MapFrom(x=>x.Upgrade.Select(up=>up.Upgrade).ToList()))
                .ReverseMap()
                .ForMember(x => x.Created, opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.Modified, opt => opt.Ignore())
                .ForMember(x => x.ModifiedBy, opt => opt.Ignore());

            CreateMap<Estate, SaveEstateViewModel>()
                .ForMember(x => x.UpgradeIds, opt => opt.MapFrom(x => x.Upgrade.Select(up => up.UpgradeId).ToList()))
                .ForMember(x => x.SaleTypes, opt => opt.Ignore())
                .ForMember(x => x.Properties, opt => opt.Ignore())
                .ForMember(x => x.Photos, opt => opt.Ignore())
                .ForMember(x => x.Upgrades, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(x => x.Created, opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.Modified, opt => opt.Ignore())
                .ForMember(x => x.ModifiedBy, opt => opt.Ignore())
                .ForMember(x => x.Favorites, opt => opt.Ignore());

            CreateMap<Upgrade, UpgradeViewModel>()
                .ReverseMap()
                .ForMember(x => x.Created, opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.Modified, opt => opt.Ignore())
                .ForMember(x => x.ModifiedBy, opt => opt.Ignore())
                .ForMember(x => x.Estates, opt => opt.Ignore());
        }
    }
}
