//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using CleanBrilliantCompany.Interfaces;
//using CleanBrilliantCompany.Models;
//using CleanBrilliantCompany.Models.CalculatorImplementation;

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

//            // Create the routing service instance.
//            IRoutingService routingService = new RoutingAPI();

//            // Create the ShipmentControl instance.
//            ShipmentControl shipmentControl = new ShipmentControl(routingService);

//            // Asynchronously create a shipment using the specified parameters.
//            ShipmentSDM shipment = await shipmentControl.CreateShipmentAsync(
//                orderId: 101,
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

//            // Instantiate the carbon footprint calculator.
//            CalculateShipmentCFImpl cfCalculator = new CalculateShipmentCFImpl();
//            // Calculate the total carbon footprint for the shipment.
//            float totalEmission = cfCalculator.CalculateCarbonFootprint(shipment);
//            Console.WriteLine($"\nTotal Emission: {totalEmission} kg CO2");

//            Console.WriteLine("\nPress any key to exit...");
//            Console.ReadKey();
//        }
//    }
//}

