using CleanBrilliantCompany.DTO;
using System;
using System.Collections.Generic;

namespace CleanBrilliantCompany.Interfaces
{
    public class IManufacturer
    {
        private readonly List<ManufacturerDTO> manufacturerList = new List<ManufacturerDTO>()
        {
            new ManufacturerDTO(1, "EcoTech Industries", "123 Punggol Road, Waterway Point", "contact@ecotech.com"),
            new ManufacturerDTO(2, "SmartProducts Pte Ltd", "18 Tai Seng", "contact@smartproducts.com"),
            new ManufacturerDTO(3, "Procter & Gamble", "1 Procter & Gamble Plaza, Cincinnati, OH, USA", "contact@pg.com")
        };

        public List<ManufacturerDTO> GetManufacturerList()
        {
            return manufacturerList;
        }
    }
}