using Shop.Core.Domain;
using Shop.Core.Dto;
using ShopTARgv21.Core.Domain;
using ShopTARgv21.Core.Dto;

namespace Shop.Core.ServiceInterface
{
    public interface ISpaceShipServices : IApplicationServices
    {
        Task<Spaceship> Add(SpaceshipDto dto);

        Task<Spaceship> GetAsync(Guid id);

        Task<Spaceship> Update(SpaceshipDto dto);

        Task<Spaceship> Delete(Guid id);
    }
   
}
