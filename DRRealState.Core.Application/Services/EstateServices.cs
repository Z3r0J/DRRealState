using AutoMapper;
using DRRealState.Core.Application.Interfaces.Repository;
using DRRealState.Core.Application.Interfaces.Services;
using DRRealState.Core.Application.ViewModel.Estate;
using DRRealState.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.Services
{
    public class EstateServices : GenericServices<SaveEstateViewModel, EstateViewModel, Estate>, IEstateServices
    {
        private readonly IEstateRepository _estateRepository;
        public readonly IMapper _mapper;

        public EstateServices(IEstateRepository estateRepository, IMapper mapper) : base(estateRepository, mapper)
        {
            _estateRepository = estateRepository;
            _mapper = mapper;
        }

        public async Task<List<EstateViewModel>> GetAllViewModelWithInclude() {

            var estateList = await _estateRepository.GetAllExtensiveInclude();

            return _mapper.Map<List<EstateViewModel>>(estateList);

        }

        public async Task<EstateViewModel> GetViewModelWithIncludeById(int id) {

            var estateList = await GetAllViewModelWithInclude();

            return _mapper.Map<EstateViewModel>(estateList.FirstOrDefault(x=>x.Id == id));

        }



        public async Task<List<EstateViewModel>> FilterAsync(FilterEstateViewModel filter) {

            var estateList = await GetAllViewModelWithInclude();

            var newList = new List<EstateViewModel>();

            if (filter.EstateType != null && filter.EstateType.Count > 0)
            {
                foreach (int Id in filter.EstateType)
                {
                    var es = estateList.Where(x => x.PropertyTypeId == Id).ToList();

                    foreach (EstateViewModel item in es)
                    {
                        newList.Add(item);
                    }
                }

                #region General If
                if (filter.BedQuantity != null
                    && filter.BathQuantity != null
                    && filter.MaximumPrice != null
                    && filter.MinimumPrice != null)
                {
                    return newList.Where(x => x.BedRoomQuantity == filter.BedQuantity
                    && x.BathroomQuantity == filter.BathQuantity &&
                    (x.Price >= filter.MinimumPrice && x.Price <= filter.MaximumPrice)).ToList();
                }
                if (filter.BedQuantity != null
                    && filter.BathQuantity == null
                    && filter.MaximumPrice == null
                    && filter.MinimumPrice == null)
                {
                    return newList.Where(x => x.BedRoomQuantity == filter.BedQuantity).ToList();
                }
                if (filter.BedQuantity == null
                    && filter.BathQuantity != null
                    && filter.MaximumPrice == null
                    && filter.MinimumPrice == null)
                {
                    return newList.Where(x => x.BathroomQuantity == filter.BathQuantity).ToList();
                }
                if (filter.BedQuantity == null
                    && filter.BathQuantity == null
                    && filter.MaximumPrice != null
                    && filter.MinimumPrice == null)
                {
                    return newList.Where(x => x.Price <= filter.MaximumPrice).ToList();
                }
                if (filter.BedQuantity == null
                    && filter.BathQuantity == null
                    && filter.MaximumPrice == null
                    && filter.MinimumPrice != null)
                {
                    return newList.Where(x => x.Price >= filter.MinimumPrice).ToList();
                }
                #endregion

                #region Bed IF
                if (filter.BedQuantity != null
                    && filter.BathQuantity == null
                    && filter.MaximumPrice != null
                    && filter.MinimumPrice == null)
                {
                    return newList.Where(x => x.BedRoomQuantity == filter.BedQuantity && x.Price <= filter.MaximumPrice).ToList();
                }
                if (filter.BedQuantity != null
                    && filter.BathQuantity == null
                    && filter.MaximumPrice == null
                    && filter.MinimumPrice != null)
                {
                    return newList.Where(x => x.BedRoomQuantity == filter.BedQuantity && x.Price >= filter.MinimumPrice).ToList();
                }
                if (filter.BedQuantity != null
                    && filter.BathQuantity == null
                    && filter.MaximumPrice != null
                    && filter.MinimumPrice != null)
                {
                    return newList.Where(x => x.BedRoomQuantity == filter.BedQuantity && x.Price >= filter.MinimumPrice && x.Price <= filter.MaximumPrice).ToList();
                }
                #endregion

                #region Bath IF
                if (filter.BedQuantity == null
                        && filter.BathQuantity != null
                        && filter.MaximumPrice != null
                        && filter.MinimumPrice == null)
                {
                    return newList.Where(x => x.BathroomQuantity == filter.BathQuantity && x.Price <= filter.MaximumPrice).ToList();
                }
                if (filter.BedQuantity == null
                    && filter.BathQuantity != null
                    && filter.MaximumPrice == null
                    && filter.MinimumPrice != null)
                {
                    return newList.Where(x => x.BathroomQuantity == filter.BathQuantity && x.Price >= filter.MinimumPrice).ToList();
                }

                if (filter.BedQuantity == null
                    && filter.BathQuantity != null
                    && filter.MaximumPrice != null
                    && filter.MinimumPrice != null)
                {
                    return newList.Where(x => x.BathroomQuantity == filter.BathQuantity && (x.Price >= filter.MinimumPrice && x.Price <= filter.MaximumPrice)).ToList();
                }

                #endregion
                if (filter.BedQuantity == null
                    && filter.BathQuantity == null
                    && filter.MaximumPrice != null
                    && filter.MinimumPrice != null)
                {
                    return newList.Where(x => (x.Price >= filter.MinimumPrice && x.Price <= filter.MaximumPrice)).ToList();
                }
                if (filter.BedQuantity != null
                    && filter.BathQuantity != null
                    && filter.MaximumPrice != null
                    && filter.MinimumPrice == null)
                {
                    return newList.Where(x => x.BedRoomQuantity == filter.BedQuantity && x.BathroomQuantity == filter.BathQuantity && x.Price <= filter.MaximumPrice).ToList();
                }
                if (filter.BedQuantity != null
                    && filter.BathQuantity != null
                    && filter.MaximumPrice == null
                    && filter.MinimumPrice != null)
                {
                    return newList.Where(x => x.BathroomQuantity == filter.BathQuantity && x.BedRoomQuantity == filter.BedQuantity && x.Price >= filter.MinimumPrice).ToList();
                }
                if (filter.BedQuantity != null
                    && filter.BathQuantity != null
                    && filter.MaximumPrice == null
                    && filter.MinimumPrice == null)
                {
                    return newList.Where(x => x.BathroomQuantity == filter.BathQuantity && x.BedRoomQuantity == filter.BedQuantity).ToList();
                }
                else
                {

                    return newList;
                }
            }

            #region General If
            if (filter.BedQuantity != null
                && filter.BathQuantity != null
                && filter.MaximumPrice != null
                && filter.MinimumPrice != null)
            {
                return estateList.Where(x => x.BedRoomQuantity == filter.BedQuantity
                && x.BathroomQuantity == filter.BathQuantity &&
                (x.Price >= filter.MinimumPrice && x.Price <= filter.MaximumPrice)).ToList();
            }
            if (filter.BedQuantity != null
                && filter.BathQuantity == null
                && filter.MaximumPrice == null
                && filter.MinimumPrice == null)
            {
                return estateList.Where(x => x.BedRoomQuantity == filter.BedQuantity).ToList();
            }
            if (filter.BedQuantity == null
                && filter.BathQuantity != null
                && filter.MaximumPrice == null
                && filter.MinimumPrice == null)
            {
                return estateList.Where(x => x.BathroomQuantity == filter.BathQuantity).ToList();
            }
            if (filter.BedQuantity == null
                && filter.BathQuantity == null
                && filter.MaximumPrice != null
                && filter.MinimumPrice == null)
            {
                return estateList.Where(x => x.Price <= filter.MaximumPrice).ToList();
            }
            if (filter.BedQuantity == null
                && filter.BathQuantity == null
                && filter.MaximumPrice == null
                && filter.MinimumPrice != null)
            {
                return estateList.Where(x => x.Price >= filter.MinimumPrice).ToList();
            }
            #endregion

            #region Bed IF
            if (filter.BedQuantity != null
                && filter.BathQuantity == null
                && filter.MaximumPrice != null
                && filter.MinimumPrice == null)
            {
                return estateList.Where(x => x.BedRoomQuantity == filter.BedQuantity && x.Price<=filter.MaximumPrice).ToList();
            }
            if (filter.BedQuantity != null
                && filter.BathQuantity == null
                && filter.MaximumPrice == null
                && filter.MinimumPrice != null)
            {
                return estateList.Where(x => x.BedRoomQuantity == filter.BedQuantity && x.Price >= filter.MinimumPrice).ToList();
            }
            if (filter.BedQuantity != null
                && filter.BathQuantity == null
                && filter.MaximumPrice != null
                && filter.MinimumPrice != null)
            {
                return estateList.Where(x => x.BedRoomQuantity == filter.BedQuantity && x.Price >= filter.MinimumPrice && x.Price <= filter.MaximumPrice).ToList();
            }
            #endregion

            #region Bath IF
            if (filter.BedQuantity == null
                    && filter.BathQuantity != null
                    && filter.MaximumPrice != null
                    && filter.MinimumPrice == null)
            {
                return estateList.Where(x => x.BathroomQuantity == filter.BathQuantity && x.Price <= filter.MaximumPrice).ToList();
            }
            if (filter.BedQuantity == null
                && filter.BathQuantity != null
                && filter.MaximumPrice == null
                && filter.MinimumPrice != null)
            {
                return estateList.Where(x => x.BathroomQuantity == filter.BathQuantity && filter.MinimumPrice >= x.Price).ToList();
            }

            if (filter.BedQuantity == null
                && filter.BathQuantity != null
                && filter.MaximumPrice != null
                && filter.MinimumPrice != null)
            {
                return estateList.Where(x => x.BathroomQuantity == filter.BathQuantity && (x.Price >= filter.MinimumPrice && x.Price <= filter.MaximumPrice)).ToList();
            }

            #endregion
            if (filter.BedQuantity == null
                && filter.BathQuantity == null
                && filter.MaximumPrice != null
                && filter.MinimumPrice != null)
            {
                return estateList.Where(x => (x.Price >= filter.MinimumPrice && x.Price <= filter.MaximumPrice)).ToList();
            }
            if (filter.BedQuantity != null
                && filter.BathQuantity != null
                && filter.MaximumPrice != null
                && filter.MinimumPrice == null)
            {
                return estateList.Where(x => x.BedRoomQuantity==filter.BedQuantity&&x.BathroomQuantity==filter.BathQuantity&&x.Price <= filter.MaximumPrice).ToList();
            }
            if (filter.BedQuantity != null
                && filter.BathQuantity != null
                && filter.MaximumPrice == null
                && filter.MinimumPrice != null)
            {
                return estateList.Where(x => x.BathroomQuantity==filter.BathQuantity&&x.BedRoomQuantity==filter.BedQuantity&&x.Price >= filter.MinimumPrice).ToList();
            }

            if (filter.BedQuantity != null
                && filter.BathQuantity != null
                && filter.MaximumPrice == null
                && filter.MinimumPrice == null)
            {
                return newList.Where(x => x.BathroomQuantity == filter.BathQuantity && x.BedRoomQuantity == filter.BedQuantity).ToList();
            }
            else {

                return estateList;
            }
            
        }
    }
}
