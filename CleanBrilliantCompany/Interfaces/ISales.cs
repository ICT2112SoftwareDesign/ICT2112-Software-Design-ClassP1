using CleanBrilliantCompany.DTO;
using System;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace CleanBrilliantCompany.Interfaces
{
    public class ISales
    {
        private readonly List<SalesDTO> _sales = new List<SalesDTO>()
        {
            // Sales for 2023 (January - December)
            new SalesDTO(1, new DateTime(2023, 1, 5), 7),
            new SalesDTO(2, new DateTime(2023, 1, 10), 5),
            new SalesDTO(3, new DateTime(2023, 2, 15), 9),
            new SalesDTO(4, new DateTime(2023, 2, 20), 6),
            new SalesDTO(5, new DateTime(2023, 3, 25), 4),
            new SalesDTO(6, new DateTime(2023, 3, 30), 8),
            new SalesDTO(7, new DateTime(2023, 4, 5), 3),
            new SalesDTO(8, new DateTime(2023, 4, 10), 11),
            new SalesDTO(1, new DateTime(2023, 5, 15), 6),
            new SalesDTO(2, new DateTime(2023, 5, 20), 9),
            new SalesDTO(3, new DateTime(2023, 6, 25), 7),
            new SalesDTO(4, new DateTime(2023, 6, 30), 4),
            new SalesDTO(5, new DateTime(2023, 7, 5), 10),
            new SalesDTO(6, new DateTime(2023, 7, 10), 3),
            new SalesDTO(7, new DateTime(2023, 8, 15), 8),
            new SalesDTO(8, new DateTime(2023, 8, 20), 2),
            new SalesDTO(1, new DateTime(2023, 9, 25), 5),
            new SalesDTO(2, new DateTime(2023, 9, 30), 7),
            new SalesDTO(3, new DateTime(2023, 10, 5), 6),
            new SalesDTO(4, new DateTime(2023, 10, 10), 4),
            new SalesDTO(5, new DateTime(2023, 11, 15), 9),
            new SalesDTO(6, new DateTime(2023, 11, 20), 6),
            new SalesDTO(7, new DateTime(2023, 12, 25), 7),
            new SalesDTO(8, new DateTime(2023, 12, 30), 3),

            // Sales for 2022 (January - December)
            new SalesDTO(1, new DateTime(2022, 1, 5), 10),
            new SalesDTO(2, new DateTime(2022, 1, 10), 4),
            new SalesDTO(3, new DateTime(2022, 2, 15), 6),
            new SalesDTO(4, new DateTime(2022, 2, 20), 8),
            new SalesDTO(5, new DateTime(2022, 3, 25), 3),
            new SalesDTO(6, new DateTime(2022, 3, 30), 9),
            new SalesDTO(7, new DateTime(2022, 4, 5), 5),
            new SalesDTO(8, new DateTime(2022, 4, 10), 7),
            new SalesDTO(1, new DateTime(2022, 5, 15), 8),
            new SalesDTO(2, new DateTime(2022, 5, 20), 2),
            new SalesDTO(3, new DateTime(2022, 6, 25), 7),
            new SalesDTO(4, new DateTime(2022, 6, 30), 4),
            new SalesDTO(5, new DateTime(2022, 7, 5), 9),
            new SalesDTO(6, new DateTime(2022, 7, 10), 6),
            new SalesDTO(7, new DateTime(2022, 8, 15), 3),
            new SalesDTO(8, new DateTime(2022, 8, 20), 5),
            new SalesDTO(1, new DateTime(2022, 9, 25), 11),
            new SalesDTO(2, new DateTime(2022, 9, 30), 3),
            new SalesDTO(3, new DateTime(2022, 10, 5), 7),
            new SalesDTO(4, new DateTime(2022, 10, 10), 4),
            new SalesDTO(5, new DateTime(2022, 11, 15), 6),
            new SalesDTO(6, new DateTime(2022, 11, 20), 8),
            new SalesDTO(7, new DateTime(2022, 12, 25), 10),
            new SalesDTO(8, new DateTime(2022, 12, 30), 5)
        };

        public List<SalesDTO> getSalesData(int month)
        {
            return _sales.Where(s => s.DateTime.Month == month).ToList();
        }
    }
}
