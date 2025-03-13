using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CleanBrilliantCompany.Models.CalculatorImplementation
{
    public class CalculateShipmentCFImpl
    {
        // calculate shipment
        public float CalculateCarbonFootprint(ShipmentSDM shipment)
        {
            //TransportMode tm = shipment.getRouteSegments().getTransportMode();
            // float transport_factor = 0.0f;
            // if(tm == TransportMode.AIR){
            //     transport_factor = 1.5f;
            // }
            // else if(tm == TransportMode.TRUCK){
            //     transport_factor = 0.5f;
            // }
            // else if(tm == TransportMode.SEA){
            //     transport_factor = 0.1f;
            // }

            // List<Item> itemList = IOrder.getOrderDetails(shipment.getOrderId())          <--- this returns a list of items
            // retrieve each product from order -> foreach i in itemList
            // int productId = i.getProductId()
            // total_cf += IProduct.getProductDetails(productId).getCarbonFootprint()
            // endloop

            // total_cf += [staff carbon emission if have]
            // total_cf * transport_factor

            // add carbonFootprintId/productId/shipmentId into carbonFootprintRecord DB
            // return total_cf;


            return 0.0f;
        }
    }
}
