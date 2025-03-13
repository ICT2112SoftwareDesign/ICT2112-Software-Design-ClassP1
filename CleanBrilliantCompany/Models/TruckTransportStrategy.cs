using System.Collections.Generic;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Models
{
	public class TruckTransportStrategy : ITransportStrategy
	{
		private readonly IRoutingService _routingService;

		public TruckTransportStrategy(IRoutingService routingService)
		{
			_routingService = routingService;
		}

		public List<RouteSegment> CreateRoute(string senderAddress, string recipientAddress)
		{
			var segments = new List<RouteSegment>();
			// Single truck leg.
			float distance = _routingService.GetDistance(senderAddress, recipientAddress, TransportMode.Truck);
			segments.Add(new RouteSegment(1, TransportMode.Truck, distance));
			return segments;
		}

		public float CalculateEmission(RouteSegment segment, float shipmentTotalWeight)
		{
			// Simple formula: distance * weight * factor.
			float factor = segment.Mode switch
			{
				TransportMode.Air => 1.5f,
				TransportMode.Sea => 0.1f,
				TransportMode.Truck => 0.5f,
				_ => 1.0f
			};
			return segment.Distance * shipmentTotalWeight * factor;
		}
	}
}
