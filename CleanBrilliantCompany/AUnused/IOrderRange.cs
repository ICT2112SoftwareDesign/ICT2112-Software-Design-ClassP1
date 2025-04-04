using CleanBrilliantCompany.ForecastManagement.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;


namespace CleanBrilliantCompany.Interfaces
{
    public class IOrderRange
    {
        //private readonly List<SalesDTO> _sales = new List<SalesDTO>()
        //{
       
        //}

        public List<SalesDTO> getSalesData(int month)
        {   // Path to your JSON file
            string filePath = "updated_sales_dto.json";

            // Read JSON content
            string jsonString = File.ReadAllText(filePath);

            // Deserialize into a list of SalesDTO objects
            List<SalesDTO> salesData = JsonSerializer.Deserialize<List<SalesDTO>>(jsonString);
            return salesData.Where(s => s.DateTime.Month == month).ToList();
        }
    }
}
