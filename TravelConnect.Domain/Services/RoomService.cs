using TravelConnect.Domain.Entities;
using TravelConnect.Domain.Exceptions;
using TravelConnect.Domain.Ports;

namespace TravelConnect.Application.Services;

public class RoomService(IRepository<Room> roomRepository, IUnitOfWork unitOfWork)
{
    /*Habitaciones por Hotel*/
    public async Task<IEnumerable<Room>> GetRoomsByHotelAsync(Guid hotelId)
    {
        return await roomRepository.FindAsync(r => r.HotelId == hotelId);
    }
    /*Detalle de la habitacion*/
    public async Task<Room> GetRoomByIdAsync(Guid id)
    {
        var room = await roomRepository.GetByIdAsync(id);
        return room ?? throw new NotFoundException("Room", id);
    }
    /* El sistema deberá permitir asignar al hotel cada una de las habitaciones disponibles para reserva */
    public async Task CreateRoomAsync(Room room)
    {
        await roomRepository.AddAsync(room);
        await unitOfWork.CommitAsync();

    }
    /* El sistema deberá permitir modificar los valores de cada habitación */
    public async Task UpdateRoomAsync(Room room)
    {
        await roomRepository.UpdateAsync(room);
        await unitOfWork.CommitAsync();
    }
    /* El sistema me deberá permitir deshabilitar cada una de las habitaciones del hotel */

    public async Task DisableRoomAsync(Guid id)
    {
        var room = await GetRoomByIdAsync(id);
        room.Disable();
        await roomRepository.UpdateAsync(room);
        await unitOfWork.CommitAsync();
    }
    /* El sistema me deberá permitir habilitar cada una de las habitaciones del hotel */
    public async Task EnableRoomAsync(Guid roomId)
    {
        var room = await GetRoomByIdAsync(roomId);
        room.Enable();
        await roomRepository.UpdateAsync(room);
        await unitOfWork.CommitAsync();
    }

    public async Task<bool> HasActiveReservationsAsync(Guid roomId)
    {
        var room = await GetRoomByIdAsync(roomId);
        return room.HasActiveReservations();
    }
}
