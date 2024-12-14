using AutoMapper;
using TravelConnect.Commons.Models.Request;
using TravelConnect.Commons.Models.Response;
using TravelConnect.Domain.Entities;
using TravelConnect.Domain.Exceptions;
using TravelConnect.Domain.Ports.Persistence;
using TravelConnect.Domain.Services;
using TravelConnect.Domain.ValueObjects;

namespace TravelConnect.Application.Services;
[DomainService]
public class HotelService(IRepository<Hotel> hotelRepository, IUnitOfWork unitOfWork, IMapper mapper)
{
    /*El sistema deberá permitir crear un nuevo hotel*/
    public async Task CreateHotelAsync(HotelRequest hotelRequest)
    {
        Hotel hotel = mapper.Map<Hotel>(hotelRequest);
        hotel.Address = new()
        {
            Id = Guid.NewGuid(),
            Street = hotelRequest.Street,
            City = hotelRequest.City,
            StateAdrees = hotelRequest.StateAddress,
            ZipCode = hotelRequest.ZipCode
        };
        await hotelRepository.AddAsync(hotel);
        await unitOfWork.CommitAsync();
    }
    /*El sistema deberá permitir modificar los valores de cada hotel*/
    public async Task UpdateHotelAsync(Guid hotelId, HotelRequest hotelRequest)
    {
        Hotel hotel = (await hotelRepository.GetByIdAsync(hotelId, h => h.Address))
                ?? throw new ArgumentException($"Hotel with Id {hotelId} not found.");
        hotel.Name = hotelRequest.Name;

        hotel.Address.Street = hotelRequest.Street;
        hotel.Address.City = hotelRequest.City;
        hotel.Address.StateAdrees = hotelRequest.StateAddress;
        hotel.Address.ZipCode = hotelRequest.ZipCode;

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
    public async Task<IEnumerable<HotelResponse>> GetAllHotelsAsync()
    {
        var result = await hotelRepository.GetAllAsync(h => h.Address);
        return mapper.Map<IEnumerable<HotelResponse>>(result);
    }

    public async Task<HotelResponse> GetHotelByIdAsync(Guid id)
    {
        var hotel = await hotelRepository.GetByIdAsync(id, h => h.Address);
        return hotel == null ? throw new NotFoundException("Hotel", id) : mapper.Map<HotelResponse>(hotel);
    }
}
