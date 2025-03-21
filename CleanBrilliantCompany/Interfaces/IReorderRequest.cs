using CleanBrilliantCompany.DTO;
using System;
using System.Collections.Generic;

namespace CleanBrilliantCompany.Interfaces
{
    public class IReorderRequest
    {
        private readonly List<ReorderRequestDTO> reorderRequestList = new List<ReorderRequestDTO>()
        {
            new ReorderRequestDTO(1, 1, 50, 1, DateTime.Parse("2025-02-01"), "Delivered", 23),
            new ReorderRequestDTO(2, 2, 30, 1, DateTime.Parse("2025-02-05"), "Delivered", 6),
            new ReorderRequestDTO(3, 6, 10, 1, DateTime.Parse("2025-01-18"), "Delivered", 0),
            new ReorderRequestDTO(4, 3, 40, 1, DateTime.Parse("2025-02-16"), "Delivered", 11),
            new ReorderRequestDTO(5, 9, 70, 1, DateTime.Parse("2025-03-02"), "Delivered", 7),
            new ReorderRequestDTO(6, 8, 80, 1, DateTime.Parse("2025-01-07"), "Delivered", 2),
            new ReorderRequestDTO(7, 4, 60, 1, DateTime.Parse("2025-02-08"), "Delivered", 25),
            new ReorderRequestDTO(8, 7, 90, 1, DateTime.Parse("2025-03-11"), "Delivered", 2),
            new ReorderRequestDTO(9, 2, 10, 1, DateTime.Parse("2025-01-21"), "Delivered", 14),
            new ReorderRequestDTO(10, 2, 20, 1, DateTime.Parse("2025-02-05"), "Delivered", 6),

            new ReorderRequestDTO(11, 5, 12, 2, DateTime.Parse("2025-02-01"), "Delivered", 13),
            new ReorderRequestDTO(12, 6, 86, 2, DateTime.Parse("2025-03-05"), "Delivered", 6),
            new ReorderRequestDTO(13, 8, 44, 2, DateTime.Parse("2025-02-18"), "Delivered", 0),
            new ReorderRequestDTO(14, 3, 69, 2, DateTime.Parse("2025-02-16"), "Delivered", 1),
            new ReorderRequestDTO(15, 1, 82, 2, DateTime.Parse("2025-03-02"), "Delivered", 17),
            new ReorderRequestDTO(16, 8, 22, 2, DateTime.Parse("2025-02-07"), "Delivered", 12),
            new ReorderRequestDTO(17, 4, 53, 2, DateTime.Parse("2025-02-08"), "Delivered", 5),
            new ReorderRequestDTO(18, 7, 97, 2, DateTime.Parse("2025-01-11"), "Delivered", 22),
            new ReorderRequestDTO(19, 3, 15, 2, DateTime.Parse("2025-02-21"), "Delivered", 8),
            new ReorderRequestDTO(20, 2, 27, 2, DateTime.Parse("2025-01-05"), "Delivered", 2),

            new ReorderRequestDTO(21, 1, 12, 3, DateTime.Parse("2025-03-01"), "Delivered", 11),
            new ReorderRequestDTO(22, 7, 86, 3, DateTime.Parse("2025-01-05"), "Delivered", 6),
            new ReorderRequestDTO(23, 6, 44, 3, DateTime.Parse("2025-02-18"), "Delivered", 10),
            new ReorderRequestDTO(24, 3, 69, 3, DateTime.Parse("2025-01-16"), "Delivered", 1),
            new ReorderRequestDTO(25, 1, 82, 3, DateTime.Parse("2025-02-02"), "Delivered", 27),
            new ReorderRequestDTO(26, 8, 22, 3, DateTime.Parse("2025-02-07"), "Delivered", 12),
            new ReorderRequestDTO(27, 2, 53, 3, DateTime.Parse("2025-03-08"), "Delivered", 5),
            new ReorderRequestDTO(28, 7, 97, 3, DateTime.Parse("2025-01-11"), "Delivered", 19),
            new ReorderRequestDTO(29, 7, 15, 3, DateTime.Parse("2025-02-21"), "Delivered", 15),
            new ReorderRequestDTO(30, 8, 27, 3, DateTime.Parse("2025-03-05"), "Delivered", 1)
        };

        public List<ReorderRequestDTO> GetReorderRequestList()
        {
            return reorderRequestList;
        }
    }
}