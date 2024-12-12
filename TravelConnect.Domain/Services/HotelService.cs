using TravelConnect.Domain.Entities;
using TravelConnect.Domain.Exceptions;
using TravelConnect.Domain.Ports.Persistence;
using TravelConnect.Domain.Services;

namespace TravelConnect.Application.Services;
[DomainService]
public class HotelService(IRepository<Hotel> hotelRepository, IUnitOfWork unitOfWork)
{
    public async Task<IEnumerable<Hotel>> GetAllHotelsAsync()
    {
        return await hotelRepository.GetAllAsync();
    }

    public async Task<Hotel> GetHotelByIdAsync(Guid id)
    {
        var hotel = await hotelRepository.GetByIdAsync(id);
        return hotel ?? throw new NotFoundException("Hotel", id);
    }
    /*El sistema deberá permitir crear un nuevo hotel*/
    public async Task CreateHotelAsync(Hotel hotel)
    {
        await hotelRepository.AddAsync(hotel);
        await unitOfWork.CommitAsync();
    }
    /*El sistema deberá permitir modificar los valores de cada hotel*/
    public async Task UpdateHotelAsync(Hotel hotel)
    {
        await hotelRepository.UpdateAsync(hotel);
        await unitOfWork.CommitAsync();
    }
    /*El sistema me deberá permitir deshabilitar cada uno de los hoteles*/
    public async Task DisableHotelAsync(Guid id)
    {
        var hotel = await GetHotelByIdAsync(id);
        hotel.Disable();
        await hotelRepository.UpdateAsync(hotel);
        await unitOfWork.CommitAsync();
    }
    /*El sistema me deberá permitir habilitar cada uno de los hoteles*/
    public async Task EnaleHotelAsync(Guid id)
    {
        var hotel = await GetHotelByIdAsync(id);
        hotel.Enable();
        await hotelRepository.UpdateAsync(hotel);
        await unitOfWork.CommitAsync();
    }
}
