using AutoMapper;
using DRRealState.Core.Application.Interfaces.Repository;
using DRRealState.Core.Application.Interfaces.Services;
using DRRealState.Core.Application.ViewModel.Estate;
using DRRealState.Core.Application.ViewModel.EstateFavorite;
using DRRealState.Core.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.Services
{
    public class EstateFavoriteServices : GenericServices<SaveEstateFavoriteViewModel, EstateFavoriteViewModel, EstateFavorite>, IEstateFavoriteServices
    {
        private readonly IEstateFavoriteRepository _estateFavoriteRepository;
        private readonly IMapper _mapper;

        public EstateFavoriteServices(IEstateFavoriteRepository estateFavoriteRepository, IMapper mapper) : base(estateFavoriteRepository, mapper)
        {
            _estateFavoriteRepository = estateFavoriteRepository;
            _mapper = mapper;
        }

        public async Task<List<EstateFavoriteViewModel>> GetAllViewModelWithInclude() {

            var favorite = await _estateFavoriteRepository.GetAllWithIncludeAsync();
            return _mapper.Map<List<EstateFavoriteViewModel>>(favorite);
        
        }

        public List<EstateFavoriteViewModel> AdvancedFilter(List<EstateFavoriteViewModel> favorite, FilterEstateViewModel filter) {

            var newList = new List<EstateFavoriteViewModel>();

            if (filter.EstateType != null && filter.EstateType.Count > 0)
            {
                foreach (int Id in filter.EstateType)
                {
                    var es = favorite.Where(x => x.Estate.PropertyTypeId == Id).ToList();

                    foreach (EstateFavoriteViewModel item in es)
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
                    return newList.Where(x => x.Estate.BedRoomQuantity == filter.BedQuantity
                    && x.Estate.BathroomQuantity == filter.BathQuantity &&
                    (x.Estate.Price >= filter.MinimumPrice && x.Estate.Price <= filter.MaximumPrice)).ToList();
                }
                if (filter.BedQuantity != null
                    && filter.BathQuantity == null
                    && filter.MaximumPrice == null
                    && filter.MinimumPrice == null)
                {
                    return newList.Where(x => x.Estate.BedRoomQuantity == filter.BedQuantity).ToList();
                }
                if (filter.BedQuantity == null
                    && filter.BathQuantity != null
                    && filter.MaximumPrice == null
                    && filter.MinimumPrice == null)
                {
                    return newList.Where(x => x.Estate.BathroomQuantity == filter.BathQuantity).ToList();
                }
                if (filter.BedQuantity == null
                    && filter.BathQuantity == null
                    && filter.MaximumPrice != null
                    && filter.MinimumPrice == null)
                {
                    return newList.Where(x => x.Estate.Price <= filter.MaximumPrice).ToList();
                }
                if (filter.BedQuantity == null
                    && filter.BathQuantity == null
                    && filter.MaximumPrice == null
                    && filter.MinimumPrice != null)
                {
                    return newList.Where(x => x.Estate.Price >= filter.MinimumPrice).ToList();
                }
                #endregion

                #region Bed IF
                if (filter.BedQuantity != null
                    && filter.BathQuantity == null
                    && filter.MaximumPrice != null
                    && filter.MinimumPrice == null)
                {
                    return newList.Where(x => x.Estate.BedRoomQuantity == filter.BedQuantity && x.Estate.Price <= filter.MaximumPrice).ToList();
                }
                if (filter.BedQuantity != null
                    && filter.BathQuantity == null
                    && filter.MaximumPrice == null
                    && filter.MinimumPrice != null)
                {
                    return newList.Where(x => x.Estate.BedRoomQuantity == filter.BedQuantity && x.Estate.Price >= filter.MinimumPrice).ToList();
                }
                if (filter.BedQuantity != null
                    && filter.BathQuantity == null
                    && filter.MaximumPrice != null
                    && filter.MinimumPrice != null)
                {
                    return newList.Where(x => x.Estate.BedRoomQuantity == filter.BedQuantity && x.Estate.Price >= filter.MinimumPrice && x.Estate.Price <= filter.MaximumPrice).ToList();
                }
                #endregion

                #region Bath IF
                if (filter.BedQuantity == null
                        && filter.BathQuantity != null
                        && filter.MaximumPrice != null
                        && filter.MinimumPrice == null)
                {
                    return newList.Where(x => x.Estate.BathroomQuantity == filter.BathQuantity && x.Estate.Price <= filter.MaximumPrice).ToList();
                }
                if (filter.BedQuantity == null
                    && filter.BathQuantity != null
                    && filter.MaximumPrice == null
                    && filter.MinimumPrice != null)
                {
                    return newList.Where(x => x.Estate.BathroomQuantity == filter.BathQuantity && x.Estate.Price >= filter.MinimumPrice).ToList();
                }

                if (filter.BedQuantity == null
                    && filter.BathQuantity != null
                    && filter.MaximumPrice != null
                    && filter.MinimumPrice != null)
                {
                    return newList.Where(x => x.Estate.BathroomQuantity == filter.BathQuantity && (x.Estate.Price >= filter.MinimumPrice && x.Estate.Price <= filter.MaximumPrice)).ToList();
                }

                #endregion
                if (filter.BedQuantity == null
                    && filter.BathQuantity == null
                    && filter.MaximumPrice != null
                    && filter.MinimumPrice != null)
                {
                    return newList.Where(x => (x.Estate.Price >= filter.MinimumPrice && x.Estate.Price <= filter.MaximumPrice)).ToList();
                }
                if (filter.BedQuantity != null
                    && filter.BathQuantity != null
                    && filter.MaximumPrice != null
                    && filter.MinimumPrice == null)
                {
                    return newList.Where(x => x.Estate.BedRoomQuantity == filter.BedQuantity && x.Estate.BathroomQuantity == filter.BathQuantity && x.Estate.Price <= filter.MaximumPrice).ToList();
                }
                if (filter.BedQuantity != null
                    && filter.BathQuantity != null
                    && filter.MaximumPrice == null
                    && filter.MinimumPrice != null)
                {
                    return newList.Where(x => x.Estate.BathroomQuantity == filter.BathQuantity && x.Estate.BedRoomQuantity == filter.BedQuantity && x.Estate.Price >= filter.MinimumPrice).ToList();
                }
                if (filter.BedQuantity != null
                    && filter.BathQuantity != null
                    && filter.MaximumPrice == null
                    && filter.MinimumPrice == null)
                {
                    return newList.Where(x => x.Estate.BathroomQuantity == filter.BathQuantity && x.Estate.BedRoomQuantity == filter.BedQuantity).ToList();
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
                return favorite.Where(x => x.Estate.BedRoomQuantity == filter.BedQuantity
                && x.Estate.BathroomQuantity == filter.BathQuantity &&
                (x.Estate.Price >= filter.MinimumPrice && x.Estate.Price <= filter.MaximumPrice)).ToList();
            }
            if (filter.BedQuantity != null
                && filter.BathQuantity == null
                && filter.MaximumPrice == null
                && filter.MinimumPrice == null)
            {
                return favorite.Where(x => x.Estate.BedRoomQuantity == filter.BedQuantity).ToList();
            }
            if (filter.BedQuantity == null
                && filter.BathQuantity != null
                && filter.MaximumPrice == null
                && filter.MinimumPrice == null)
            {
                return favorite.Where(x => x.Estate.BathroomQuantity == filter.BathQuantity).ToList();
            }
            if (filter.BedQuantity == null
                && filter.BathQuantity == null
                && filter.MaximumPrice != null
                && filter.MinimumPrice == null)
            {
                return favorite.Where(x => x.Estate.Price <= filter.MaximumPrice).ToList();
            }
            if (filter.BedQuantity == null
                && filter.BathQuantity == null
                && filter.MaximumPrice == null
                && filter.MinimumPrice != null)
            {
                return favorite.Where(x => x.Estate.Price >= filter.MinimumPrice).ToList();
            }
            #endregion

            #region Bed IF
            if (filter.BedQuantity != null
                && filter.BathQuantity == null
                && filter.MaximumPrice != null
                && filter.MinimumPrice == null)
            {
                return favorite.Where(x => x.Estate.BedRoomQuantity == filter.BedQuantity && x.Estate.Price <= filter.MaximumPrice).ToList();
            }
            if (filter.BedQuantity != null
                && filter.BathQuantity == null
                && filter.MaximumPrice == null
                && filter.MinimumPrice != null)
            {
                return favorite.Where(x => x.Estate.BedRoomQuantity == filter.BedQuantity && x.Estate.Price >= filter.MinimumPrice).ToList();
            }
            if (filter.BedQuantity != null
                && filter.BathQuantity == null
                && filter.MaximumPrice != null
                && filter.MinimumPrice != null)
            {
                return favorite.Where(x => x.Estate.BedRoomQuantity == filter.BedQuantity && x.Estate.Price >= filter.MinimumPrice && x.Estate.Price <= filter.MaximumPrice).ToList();
            }
            #endregion

            #region Bath IF
            if (filter.BedQuantity == null
                    && filter.BathQuantity != null
                    && filter.MaximumPrice != null
                    && filter.MinimumPrice == null)
            {
                return favorite.Where(x => x.Estate.BathroomQuantity == filter.BathQuantity && x.Estate.Price <= filter.MaximumPrice).ToList();
            }
            if (filter.BedQuantity == null
                && filter.BathQuantity != null
                && filter.MaximumPrice == null
                && filter.MinimumPrice != null)
            {
                return favorite.Where(x => x.Estate.BathroomQuantity == filter.BathQuantity && filter.MinimumPrice >= x.Estate.Price).ToList();
            }

            if (filter.BedQuantity == null
                && filter.BathQuantity != null
                && filter.MaximumPrice != null
                && filter.MinimumPrice != null)
            {
                return favorite.Where(x => x.Estate.BathroomQuantity == filter.BathQuantity && (x.Estate.Price >= filter.MinimumPrice && x.Estate.Price <= filter.MaximumPrice)).ToList();
            }

            #endregion
            if (filter.BedQuantity == null
                && filter.BathQuantity == null
                && filter.MaximumPrice != null
                && filter.MinimumPrice != null)
            {
                return favorite.Where(x => (x.Estate.Price >= filter.MinimumPrice && x.Estate.Price <= filter.MaximumPrice)).ToList();
            }
            if (filter.BedQuantity != null
                && filter.BathQuantity != null
                && filter.MaximumPrice != null
                && filter.MinimumPrice == null)
            {
                return favorite.Where(x => x.Estate.BedRoomQuantity == filter.BedQuantity && x.Estate.BathroomQuantity == filter.BathQuantity && x.Estate.Price <= filter.MaximumPrice).ToList();
            }
            if (filter.BedQuantity != null
                && filter.BathQuantity != null
                && filter.MaximumPrice == null
                && filter.MinimumPrice != null)
            {
                return favorite.Where(x => x.Estate.BathroomQuantity == filter.BathQuantity && x.Estate.BedRoomQuantity == filter.BedQuantity && x.Estate.Price >= filter.MinimumPrice).ToList();
            }

            if (filter.BedQuantity != null
                && filter.BathQuantity != null
                && filter.MaximumPrice == null
                && filter.MinimumPrice == null)
            {
                return favorite.Where(x => x.Estate.BathroomQuantity == filter.BathQuantity && x.Estate.BedRoomQuantity == filter.BedQuantity).ToList();
            }
            else
            {

                return favorite;
            }

        }
    }
}
