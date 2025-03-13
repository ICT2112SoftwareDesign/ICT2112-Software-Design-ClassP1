namespace CleanBrilliantCompany.Interfaces
{
    public interface IRoutingService
    {
        // Returns a distance (in kilometers) for a leg between two addresses for a given mode.
        float GetDistance(string origin, string destination, CleanBrilliantCompany.Models.TransportMode mode);
    }
}
