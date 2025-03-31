using System.Threading.Tasks;
using CleanBrilliantCompany.Models; 

namespace CleanBrilliantCompany.Interfaces
{
    public interface IRoutingService
    {
        //asynchronous call
        Task<float> GetDistanceAsync(string origin, string destination, TransportMode mode);
        Task<string> GetNearestAirportAsync(double latitude, double longitude);
        Task<Coordinates> GeocodeAddressAsync(string address);
        Task<string> GetNearestPortAsync(double latitude, double longitude);
    }
}
