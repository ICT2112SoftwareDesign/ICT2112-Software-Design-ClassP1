using CleanBrilliantCompany.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CleanBrilliantCompany.Interfaces
{
    public class ISales
    {
        private readonly List<SalesDTO> _sales = new List<SalesDTO>()
        {
            // 2023 Data

            // January 2023 (8 items)
            new SalesDTO(1, new DateTime(2023, 1, 2), 5),
            new SalesDTO(2, new DateTime(2023, 1, 4), 6),
            new SalesDTO(3, new DateTime(2023, 1, 8), 7),
            new SalesDTO(4, new DateTime(2023, 1, 10), 8),
            new SalesDTO(5, new DateTime(2023, 1, 15), 5),
            new SalesDTO(6, new DateTime(2023, 1, 18), 7),
            new SalesDTO(7, new DateTime(2023, 1, 22), 6),
            new SalesDTO(8, new DateTime(2023, 1, 28), 8),

            // February 2023 (8 items)
            new SalesDTO(1, new DateTime(2023, 2, 1), 6),
            new SalesDTO(2, new DateTime(2023, 2, 3), 7),
            new SalesDTO(3, new DateTime(2023, 2, 7), 5),
            new SalesDTO(4, new DateTime(2023, 2, 9), 8),
            new SalesDTO(5, new DateTime(2023, 2, 12), 7),
            new SalesDTO(6, new DateTime(2023, 2, 15), 6),
            new SalesDTO(7, new DateTime(2023, 2, 20), 8),
            new SalesDTO(8, new DateTime(2023, 2, 25), 7),

            // March 2023 (8 items)
            new SalesDTO(1, new DateTime(2023, 3, 2), 7),
            new SalesDTO(2, new DateTime(2023, 3, 4), 6),
            new SalesDTO(3, new DateTime(2023, 3, 8), 8),
            new SalesDTO(4, new DateTime(2023, 3, 10), 7),
            new SalesDTO(5, new DateTime(2023, 3, 14), 6),
            new SalesDTO(6, new DateTime(2023, 3, 18), 8),
            new SalesDTO(7, new DateTime(2023, 3, 22), 7),
            new SalesDTO(8, new DateTime(2023, 3, 26), 6),

            // April 2023 (8 items)
            new SalesDTO(1, new DateTime(2023, 4, 2), 8),
            new SalesDTO(2, new DateTime(2023, 4, 5), 7),
            new SalesDTO(3, new DateTime(2023, 4, 9), 6),
            new SalesDTO(4, new DateTime(2023, 4, 12), 8),
            new SalesDTO(5, new DateTime(2023, 4, 16), 7),
            new SalesDTO(6, new DateTime(2023, 4, 20), 6),
            new SalesDTO(7, new DateTime(2023, 4, 24), 8),
            new SalesDTO(8, new DateTime(2023, 4, 28), 7),

            // May 2023 (8 items)
            new SalesDTO(1, new DateTime(2023, 5, 1), 7),
            new SalesDTO(2, new DateTime(2023, 5, 4), 8),
            new SalesDTO(3, new DateTime(2023, 5, 8), 7),
            new SalesDTO(4, new DateTime(2023, 5, 11), 6),
            new SalesDTO(5, new DateTime(2023, 5, 15), 8),
            new SalesDTO(6, new DateTime(2023, 5, 19), 7),
            new SalesDTO(7, new DateTime(2023, 5, 23), 6),
            new SalesDTO(8, new DateTime(2023, 5, 27), 8),

            // June 2023 (8 items)
            new SalesDTO(1, new DateTime(2023, 6, 2), 8),
            new SalesDTO(2, new DateTime(2023, 6, 5), 7),
            new SalesDTO(3, new DateTime(2023, 6, 9), 6),
            new SalesDTO(4, new DateTime(2023, 6, 12), 8),
            new SalesDTO(5, new DateTime(2023, 6, 16), 7),
            new SalesDTO(6, new DateTime(2023, 6, 20), 6),
            new SalesDTO(7, new DateTime(2023, 6, 24), 8),
            new SalesDTO(8, new DateTime(2023, 6, 28), 7),

            // July 2023 (8 items)
            new SalesDTO(1, new DateTime(2023, 7, 2), 7),
            new SalesDTO(2, new DateTime(2023, 7, 5), 8),
            new SalesDTO(3, new DateTime(2023, 7, 9), 7),
            new SalesDTO(4, new DateTime(2023, 7, 12), 6),
            new SalesDTO(5, new DateTime(2023, 7, 16), 8),
            new SalesDTO(6, new DateTime(2023, 7, 20), 7),
            new SalesDTO(7, new DateTime(2023, 7, 24), 6),
            new SalesDTO(8, new DateTime(2023, 7, 28), 8),

            // August 2023 (8 items)
            new SalesDTO(1, new DateTime(2023, 8, 2), 8),
            new SalesDTO(2, new DateTime(2023, 8, 6), 7),
            new SalesDTO(3, new DateTime(2023, 8, 10), 6),
            new SalesDTO(4, new DateTime(2023, 8, 14), 8),
            new SalesDTO(5, new DateTime(2023, 8, 18), 7),
            new SalesDTO(6, new DateTime(2023, 8, 22), 6),
            new SalesDTO(7, new DateTime(2023, 8, 26), 8),
            new SalesDTO(8, new DateTime(2023, 8, 30), 7),

            // September 2023 (8 items)
            new SalesDTO(1, new DateTime(2023, 9, 3), 7),
            new SalesDTO(2, new DateTime(2023, 9, 6), 8),
            new SalesDTO(3, new DateTime(2023, 9, 10), 7),
            new SalesDTO(4, new DateTime(2023, 9, 13), 6),
            new SalesDTO(5, new DateTime(2023, 9, 17), 8),
            new SalesDTO(6, new DateTime(2023, 9, 20), 7),
            new SalesDTO(7, new DateTime(2023, 9, 24), 6),
            new SalesDTO(8, new DateTime(2023, 9, 27), 8),

            // October 2023 (8 items)
            new SalesDTO(1, new DateTime(2023, 10, 2), 8),
            new SalesDTO(2, new DateTime(2023, 10, 5), 7),
            new SalesDTO(3, new DateTime(2023, 10, 9), 6),
            new SalesDTO(4, new DateTime(2023, 10, 12), 8),
            new SalesDTO(5, new DateTime(2023, 10, 16), 7),
            new SalesDTO(6, new DateTime(2023, 10, 20), 6),
            new SalesDTO(7, new DateTime(2023, 10, 24), 8),
            new SalesDTO(8, new DateTime(2023, 10, 28), 7),

            // November 2023 (8 items)
            new SalesDTO(1, new DateTime(2023, 11, 3), 7),
            new SalesDTO(2, new DateTime(2023, 11, 6), 8),
            new SalesDTO(3, new DateTime(2023, 11, 10), 7),
            new SalesDTO(4, new DateTime(2023, 11, 13), 6),
            new SalesDTO(5, new DateTime(2023, 11, 17), 8),
            new SalesDTO(6, new DateTime(2023, 11, 20), 7),
            new SalesDTO(7, new DateTime(2023, 11, 24), 6),
            new SalesDTO(8, new DateTime(2023, 11, 27), 8),

            // December 2023 (8 items)
            new SalesDTO(1, new DateTime(2023, 12, 3), 8),
            new SalesDTO(2, new DateTime(2023, 12, 6), 7),
            new SalesDTO(3, new DateTime(2023, 12, 10), 6),
            new SalesDTO(4, new DateTime(2023, 12, 13), 8),
            new SalesDTO(5, new DateTime(2023, 12, 17), 7),
            new SalesDTO(6, new DateTime(2023, 12, 20), 6),
            new SalesDTO(7, new DateTime(2023, 12, 24), 8),
            new SalesDTO(8, new DateTime(2023, 12, 27), 7),

            // 2024 Data

            // January 2024 (8 items)
            new SalesDTO(1, new DateTime(2024, 1, 2), 8),
            new SalesDTO(2, new DateTime(2024, 1, 5), 7),
            new SalesDTO(3, new DateTime(2024, 1, 9), 6),
            new SalesDTO(4, new DateTime(2024, 1, 12), 8),
            new SalesDTO(5, new DateTime(2024, 1, 16), 7),
            new SalesDTO(6, new DateTime(2024, 1, 20), 6),
            new SalesDTO(7, new DateTime(2024, 1, 24), 8),
            new SalesDTO(8, new DateTime(2024, 1, 28), 7),

            // February 2024 (8 items)
            new SalesDTO(1, new DateTime(2024, 2, 1), 7),
            new SalesDTO(2, new DateTime(2024, 2, 4), 8),
            new SalesDTO(3, new DateTime(2024, 2, 8), 7),
            new SalesDTO(4, new DateTime(2024, 2, 11), 6),
            new SalesDTO(5, new DateTime(2024, 2, 15), 8),
            new SalesDTO(6, new DateTime(2024, 2, 19), 7),
            new SalesDTO(7, new DateTime(2024, 2, 23), 6),
            new SalesDTO(8, new DateTime(2024, 2, 27), 8),

            // 2025 Data

            // January 2025 (8 items)
            new SalesDTO(1, new DateTime(2025, 1, 3), 8),
            new SalesDTO(2, new DateTime(2025, 1, 6), 7),
            new SalesDTO(3, new DateTime(2025, 1, 10), 6),
            new SalesDTO(4, new DateTime(2025, 1, 13), 8),
            new SalesDTO(5, new DateTime(2025, 1, 17), 7),
            new SalesDTO(6, new DateTime(2025, 1, 21), 6),
            new SalesDTO(7, new DateTime(2025, 1, 25), 8),
            new SalesDTO(8, new DateTime(2025, 1, 29), 7),

            // February 2025 (8 items)
            new SalesDTO(1, new DateTime(2025, 2, 2), 7),
            new SalesDTO(2, new DateTime(2025, 2, 5), 8),
            new SalesDTO(3, new DateTime(2025, 2, 9), 7),
            new SalesDTO(4, new DateTime(2025, 2, 12), 6),
            new SalesDTO(5, new DateTime(2025, 2, 16), 8),
            new SalesDTO(6, new DateTime(2025, 2, 20), 7),
            new SalesDTO(7, new DateTime(2025, 2, 24), 6),
            new SalesDTO(8, new DateTime(2025, 2, 28), 8)
        };

        public List<SalesDTO> getSalesData(int month)
        {
            return _sales.Where(s => s.DateTime.Month == month).ToList();
        }
    }
}
