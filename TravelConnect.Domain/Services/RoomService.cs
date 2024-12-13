using AutoMapper;
using TravelConnect.Commons.Models.Request;
using TravelConnect.Commons.Models.Response;
using TravelConnect.Domain.Entities;
using TravelConnect.Domain.Exceptions;
using TravelConnect.Domain.Ports.Persistence;
using TravelConnect.Domain.Services;

namespace TravelConnect.Application.Services;
[DomainService]
public class RoomService(IRepository<Room> roomRepository, IUnitOfWork unitOfWork, IMapper mapper)
{
    /* El sistema deberá permitir asignar al hotel cada una de las habitaciones disponibles para reserva */
    public async Task CreateRoomAsync(RoomRequest roomRequest)
    {
        roomRequest.Id = Guid.NewGuid();
        Room room = mapper.Map<Room>(roomRequest);   
        await roomRepository.AddAsync(room);
        await unitOfWork.CommitAsync();

    }

    /* El sistema deberá permitir modificar los valores de cada habitación */
    public async Task UpdateRoomAsync(Guid roomId, RoomRequest roomRequest)
    {
        var room = await roomRepository.GetByIdAsync(roomId, r => r.Reservations, r => r.Hotel)
            ?? throw new ArgumentException($"Hotel with Id {roomId} not found.");
       
        room.UpdateDetails(roomRequest.BaseCost, roomRequest.Taxes, roomRequest.RoomType, roomRequest.Location);
        
        await roomRepository.UpdateAsync(room);
        await unitOfWork.CommitAsync();
    }

    /* El sistema me deberá permitir deshabilitar cada una de las habitaciones del hotel */
    public async Task DisableRoomAsync(Guid id)
    {
        var room = await roomRepository.GetByIdAsync(id);
        room!.Disable();
        await roomRepository.UpdateAsync(room);
        await unitOfWork.CommitAsync();
    }

    /* El sistema me deberá permitir habilitar cada una de las habitaciones del hotel */
    public async Task EnableRoomAsync(Guid roomId)
    {
        var room = await roomRepository.GetByIdAsync(roomId);
        room!.Enable();
        await roomRepository.UpdateAsync(room);
        await unitOfWork.CommitAsync();
    }

    public async Task<bool> HasActiveReservationsAsync(Guid roomId)
    {
        var room = await roomRepository.GetByIdAsync(roomId, r => r.Hotel,
            r => r.Reservations);
        return room!.HasActiveReservations();
    }

    /*Habitaciones por Hotel*/
    public async Task<IEnumerable<RoomResponse>> GetRoomsByHotelAsync(Guid hotelId)
    {
        var response = await roomRepository.FindAsync(r => r.HotelId == hotelId,
            r => r.Hotel, r => r.Reservations);
        return mapper.Map< IEnumerable<RoomResponse>>(response);
    }

    /*Detalle de la habitacion*/
    public async Task<RoomResponse> GetRoomByIdAsync(Guid id)
    {
        var room = await roomRepository.GetByIdAsync(id, r => r.Hotel,
            r => r.Reservations);
        var roomResponse = mapper.Map<RoomResponse>(room);
        return roomResponse ?? throw new NotFoundException("Room", id);
    }
}
