using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IReservationQuery
    {
        Task<List<Reservation>> GetAllReservations();

        Task<Reservation> GetReservationById(int reservationId);

        Task<Reservation> CreateReservation(Reservation reservation);

        Task<Reservation> UpdateReservationQuantity(int reservationId, int quantity, int staffId);

        Task<Reservation> UpdateReservationPurpose(int reservationId, string reservationPurpose, int staffId);
    }
}
