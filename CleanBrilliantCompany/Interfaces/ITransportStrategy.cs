using System.Collections.Generic;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface ITransportStrategy
    { 
        Task<List<RouteSegment>> CreateRouteAsync(string senderAddress, string recipientAddress);
    }
}
