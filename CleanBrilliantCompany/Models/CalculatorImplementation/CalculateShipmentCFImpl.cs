using CleanBrilliantCompany.Interfaces;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CleanBrilliantCompany.Models.CalculatorImplementation
{
    public class CalculateShipmentCFImpl : IShipmentCFCalculator
    {
        private readonly IOrderCFManagement _orderCFManagement; // for inserting order record
        private readonly IItemCF _itemCF; // for retrieving item CF

        private readonly IOrder _order; // for retrieving list of item Ids

        // Calculate the carbon footprint for a shipment.
        public CalculateShipmentCFImpl(IOrderCFManagement orderCFManagement, IItemCF itemCF, IOrder order)
        {
            _orderCFManagement = orderCFManagement;
            _itemCF = itemCF;
            _order = order;
        }

        public float CalculateCarbonFootprint(ShipmentSDM shipment)
        {
            float totalEmission = 0.0f;

            List<int> orderItemsIdList = _order.getOrderItemIds(shipment.OrderId);
            foreach(int itemId in orderItemsIdList){
                double itemCF = _itemCF.getItemCarbonFootprintByItemId(itemId);
                totalEmission += (float)itemCF;
            }

            // Iterate over each route segment of the shipment.
            foreach (var segment in shipment.RouteSegments)
            {
                // Determine the emission factor based on the transport mode.
                float factor = segment.Mode switch
                {
                    TransportMode.Air => 1.5f,
                    TransportMode.Sea => 0.1f,
                    TransportMode.Truck => 0.5f,
                    _ => 1.0f,
                };

                // Calculate emission for the segment and add it to the total.
                totalEmission += segment.Distance * (float)shipment.TotalWeight * factor;
            }

            return totalEmission;
        }
    }
}