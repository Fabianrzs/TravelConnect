using AutoMapper;
using TravelConnect.Commons.Models.Request;
using TravelConnect.Commons.Models.Response;
using TravelConnect.Domain.Entities;
using TravelConnect.Domain.Exceptions;
using TravelConnect.Domain.Ports.Persistence;
using TravelConnect.Domain.Services;

namespace TravelConnect.Application.Services;
[DomainService]
public class HotelService(IRepository<Hotel> hotelRepository, IUnitOfWork unitOfWork, IMapper mapper)
{
    public async Task<IEnumerable<HotelResponse>> GetAllHotelsAsync()
    {
        var result =  await hotelRepository.GetAllAsync();
        return mapper.Map<IEnumerable<HotelResponse>>(result);
    }

    public async Task<HotelResponse> GetHotelByIdAsync(Guid id)
    {
        var hotel = await hotelRepository.GetByIdAsync(id);
        return hotel == null ? throw new NotFoundException("Hotel", id) : mapper.Map<HotelResponse>(hotel);
    }
    /*El sistema deberá permitir crear un nuevo hotel*/
    public async Task CreateHotelAsync(HotelRequest hotelRequest)
    {
        Hotel hotel = mapper.Map<Hotel>(hotelRequest);
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
        var hotel = await hotelRepository.GetByIdAsync(id);
        hotel!.Disable();
        await hotelRepository.UpdateAsync(hotel);
        await unitOfWork.CommitAsync();
    }
    /*El sistema me deberá permitir habilitar cada uno de los hoteles*/
    public async Task EnaleHotelAsync(Guid id)
    {
        var hotel = await hotelRepository.GetByIdAsync(id);
        hotel!.Enable();
        await hotelRepository.UpdateAsync(hotel);
        await unitOfWork.CommitAsync();
    }
}
