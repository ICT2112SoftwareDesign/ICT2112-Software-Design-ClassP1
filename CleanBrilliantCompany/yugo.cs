//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using CleanBrilliantCompany.Interfaces;
//using CleanBrilliantCompany.Models;

//namespace CleanBrilliantCompany
//{
//    public class Program
//    {
//        public static async Task Main(string[] args)
//        {
//            // Hardcoded addresses and transport mode.
//            string senderAddress = "Penjuru Ln, 34, Singapore 609201";
//            string recipientAddress = "Buckingham Palace, London, SW1A 1AA, United Kingdom";
//            string transportMode = "Air";

//            // Hardcoded items list for weight calculation.
//            List<ShipmentControl.Item> items = new List<ShipmentControl.Item>
//            {
//                new ShipmentControl.Item { Name = "Bleach", Weight = 2.0, Quantity = 2 },
//                new ShipmentControl.Item { Name = "Detergent", Weight = 1.0, Quantity = 4 }
//            };

//            // Create the routing service instance.
//            IRoutingService routingService = new RoutingAPI();

//            // Create the ShipmentControl instance.
//            ShipmentControl shipmentControl = new ShipmentControl(routingService);

//            // Calculate total weight from items.
//            double totalWeight = shipmentControl.CalculateTotalWeight(items);

//            // Asynchronously create a shipment using the specified transport mode.
//            ShipmentSDM shipment = await shipmentControl.CreateShipmentAsync(
//                orderId: 101,
//                totalWeight: totalWeight,
//                shippingMethod: transportMode,
//                senderAddress: senderAddress,
//                recipientAddress: recipientAddress
//            );

//            // Display shipment details.
//            Console.WriteLine("\nCreated Shipment:");
//            Console.WriteLine(shipment);
//            foreach (var seg in shipment.RouteSegments)
//            {
//                Console.WriteLine(seg);
//            }

//            // Calculate and display total emissions.
//            float totalEmission = 0f;
//            foreach (var seg in shipment.RouteSegments)
//            {
//                float factor = seg.Mode switch
//                {
//                    TransportMode.Air => 1.5f,
//                    TransportMode.Sea => 0.1f,
//                    TransportMode.Truck => 0.5f,
//                    _ => 1.0f
//                };
//                totalEmission += seg.Distance * (float)shipment.TotalWeight * factor;
//            }
//            Console.WriteLine($"\nTotal Emission: {totalEmission} kg CO2");

//            Console.WriteLine("\nPress any key to exit...");
//            Console.ReadKey();
//        }
//    }
//}
