using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CleanBrilliantCompany.Domain
{
    public class CarbonFootprintCalculatorControl : ICarbonData
    {
        private readonly IProductCFCalculator _productCalculator;
        private readonly IItemCFCalculator _itemCalculator;
        private readonly IShipmentCFCalculator _shipmentCalculator;

        public CarbonFootprintCalculatorControl(IProductCFCalculator productCalculator,
            IItemCFCalculator itemCalculator, IShipmentCFCalculator shipmentCalculator)
        {
            _productCalculator = productCalculator;
            _itemCalculator = itemCalculator;
            _shipmentCalculator = shipmentCalculator;
        }

        public float CalculateProductCF(float volume, float toxicPercent, int productId)
        {
            return _productCalculator.CalculateCarbonFootprint(volume, toxicPercent, productId);
        }

        public bool CalculateItemCF(int itemId, int productId)
        {
            return _itemCalculator.CalculateCarbonFootprint(itemId, productId);
        }

        public float CalculateShipmentCF(ShipmentSDM shipment)
        {
            return _shipmentCalculator.CalculateCarbonFootprint(shipment);
        }
    }
}
