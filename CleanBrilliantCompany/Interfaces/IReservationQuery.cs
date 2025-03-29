using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IReservationQuery
    {
        Task<List<Reservation>> GetAllReservations();

        Task<Reservation> GetReservationById(int reservationId);

        Task<Reservation> CreateReservation(Reservation reservation);
    }
}
