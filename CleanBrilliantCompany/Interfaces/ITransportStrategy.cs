using System.Collections.Generic;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface ITransportStrategy
    {
        // Builds the route (list of RouteSegments) from sender to recipient.
        List<RouteSegment> CreateRoute(string senderAddress, string recipientAddress);

        // Optional: calculates emission for a given segment.
        // (If your formula is always the same, you can choose to do this elsewhere.)
        float CalculateEmission(RouteSegment segment, float shipmentTotalWeight);
    }
}
