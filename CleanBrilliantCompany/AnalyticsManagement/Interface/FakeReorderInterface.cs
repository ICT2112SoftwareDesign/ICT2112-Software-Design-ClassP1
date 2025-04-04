using System;
using System.Collections.Generic;

public class FakeReorderInterface
{
    // Dummy Data for Reorders
    private readonly List<ReorderData> reorderDataList;

    public FakeReorderInterface()
    {
        reorderDataList = new List<ReorderData>
        {
            new ReorderData(1, 7, 176, new DateTime(2024, 11, 19), "Delivered", 142, 98),
            new ReorderData(2, 14, 600, new DateTime(2025, 2, 23), "Delivered", 29, 24),
            new ReorderData(3, 4, 1010, new DateTime(2024, 11, 23), "Delivered", 66, 62),
            new ReorderData(4, 21, 228, new DateTime(2025, 2, 28), "Delivered", 656, 619),
            new ReorderData(5, 9, 228, new DateTime(2025, 1, 16), "Delivered", 806, 441),
            new ReorderData(6, 17, 1012, new DateTime(2024, 12, 2), "Delivered", 345, 112),
            new ReorderData(7, 15, 1013, new DateTime(2024, 12, 5), "Delivered", 478, 331),
            new ReorderData(8, 22, 1014, new DateTime(2025, 1, 4), "Delivered", 389, 98),
            new ReorderData(9, 8, 1015, new DateTime(2024, 12, 10), "Delivered", 565, 43),
            new ReorderData(10, 11, 2, new DateTime(2024, 12, 25), "Delivered", 194, 127),
            new ReorderData(11, 5, 14, new DateTime(2024, 11, 30), "Delivered", 210, 51),
            new ReorderData(12, 18, 23, new DateTime(2025, 1, 8), "Delivered", 320, 221),
            new ReorderData(13, 19, 167, new DateTime(2025, 1, 20), "Delivered", 710, 137),
            new ReorderData(14, 12, 999, new DateTime(2025, 2, 18), "Delivered", 512, 309),
            new ReorderData(15, 3, 997, new DateTime(2024, 12, 15), "Delivered", 950, 318),
            new ReorderData(16, 10, 1009, new DateTime(2024, 12, 3), "Delivered", 134, 106),
            new ReorderData(17, 16, 1011, new DateTime(2025, 2, 5), "Delivered", 463, 352),
            new ReorderData(18, 14, 23, new DateTime(2025, 1, 18), "Delivered", 802, 139),
            new ReorderData(19, 9, 1010, new DateTime(2024, 12, 30), "Delivered", 900, 195),
            new ReorderData(20, 5, 2, new DateTime(2024, 12, 27), "Delivered", 119, 77),
            new ReorderData(21, 4, 1013, new DateTime(2025, 2, 12), "Delivered", 450, 133),
            new ReorderData(22, 6, 999, new DateTime(2025, 1, 14), "Delivered", 230, 22),
            new ReorderData(23, 13, 2, new DateTime(2024, 11, 29), "Delivered", 651, 459),
            new ReorderData(24, 7, 14, new DateTime(2024, 11, 26), "Delivered", 501, 33),
            new ReorderData(25, 20, 23, new DateTime(2024, 12, 19), "Delivered", 850, 72),
            new ReorderData(26, 21, 176, new DateTime(2025, 2, 17), "Delivered", 317, 54),
            new ReorderData(27, 10, 1009, new DateTime(2025, 1, 21), "Delivered", 710, 91),
            new ReorderData(28, 3, 23, new DateTime(2024, 12, 6), "Delivered", 320, 24),
            new ReorderData(29, 8, 1014, new DateTime(2025, 2, 3), "Delivered", 438, 290),
            new ReorderData(30, 4, 228, new DateTime(2025, 2, 6), "Delivered", 556, 209),
            new ReorderData(31, 19, 346, new DateTime(2025, 2, 14), "Delivered", 222, 175),
            new ReorderData(32, 2, 1015, new DateTime(2025, 1, 12), "Delivered", 117, 81),
            new ReorderData(33, 22, 990, new DateTime(2024, 11, 15), "Delivered", 145, 102),
            new ReorderData(34, 17, 909, new DateTime(2025, 1, 30), "Delivered", 631, 57),
            new ReorderData(35, 15, 228, new DateTime(2024, 11, 24), "Delivered", 212, 125),
            new ReorderData(36, 12, 1011, new DateTime(2025, 2, 10), "Delivered", 378, 113),
            new ReorderData(37, 14, 346, new DateTime(2025, 2, 18), "Delivered", 568, 142),
            new ReorderData(38, 13, 167, new DateTime(2024, 12, 21), "Delivered", 951, 409),
            new ReorderData(39, 5, 1012, new DateTime(2024, 11, 13), "Delivered", 489, 307),
            new ReorderData(40, 8, 1013, new DateTime(2025, 2, 1), "Delivered", 212, 137),
            new ReorderData(41, 9, 1009, new DateTime(2025, 2, 5), "Delivered", 142, 102),
            new ReorderData(42, 2, 1010, new DateTime(2024, 11, 17), "Delivered", 657, 225),
            new ReorderData(43, 7, 1015, new DateTime(2024, 12, 7), "Delivered", 132, 112),
            new ReorderData(44, 14, 999, new DateTime(2024, 12, 13), "Delivered", 245, 160),
            new ReorderData(45, 1, 999, new DateTime(2025, 1, 9), "Delivered", 889, 119),
            new ReorderData(46, 22, 1009, new DateTime(2024, 11, 27), "Delivered", 378, 97),
            new ReorderData(47, 18, 1009, new DateTime(2025, 2, 9), "Delivered", 520, 322),
            new ReorderData(48, 20, 999, new DateTime(2025, 2, 15), "Delivered", 770, 528),
            new ReorderData(49, 3, 346, new DateTime(2024, 12, 4), "Delivered", 310, 58),
            new ReorderData(50, 13, 999, new DateTime(2025, 2, 21), "Delivered", 122, 18),
            new ReorderData(51, 19, 1010, new DateTime(2025, 2, 2), "Delivered", 690, 75),
            new ReorderData(52, 21, 1012, new DateTime(2025, 1, 13), "Delivered", 604, 321),
            new ReorderData(53, 10, 2, new DateTime(2025, 2, 16), "Delivered", 554, 112),
            new ReorderData(54, 12, 1013, new DateTime(2025, 2, 13), "Delivered", 602, 96),
            new ReorderData(55, 16, 999, new DateTime(2025, 1, 5), "Delivered", 143, 125),
            new ReorderData(56, 6, 228, new DateTime(2024, 12, 14), "Delivered", 510, 387),
            new ReorderData(57, 9, 228, new DateTime(2024, 12, 8), "Delivered", 877, 28),
            new ReorderData(58, 4, 1015, new DateTime(2025, 2, 8), "Delivered", 603, 429),
            new ReorderData(59, 1, 346, new DateTime(2025, 1, 26), "Delivered", 219, 99),
            new ReorderData(60, 11, 1009, new DateTime(2024, 11, 22), "Delivered", 831, 367),
            new ReorderData(61, 8, 999, new DateTime(2025, 2, 4), "Delivered", 722, 237),
            new ReorderData(62, 14, 167, new DateTime(2024, 12, 9), "Delivered", 383, 42),
            new ReorderData(63, 19, 2, new DateTime(2025, 1, 2), "Delivered", 712, 531),
            new ReorderData(64, 17, 1009, new DateTime(2025, 1, 11), "Delivered", 688, 43),
            new ReorderData(65, 2, 14, new DateTime(2025, 1, 19), "Delivered", 131, 49),
            new ReorderData(66, 4, 23, new DateTime(2025, 2, 22), "Delivered", 902, 602),
            new ReorderData(67, 3, 1014, new DateTime(2025, 1, 3), "Delivered", 144, 92),
            new ReorderData(68, 7, 23, new DateTime(2025, 2, 20), "Delivered", 540, 310),
            new ReorderData(69, 14, 346, new DateTime(2024, 12, 11), "Delivered", 199, 17),
            new ReorderData(70, 20, 346, new DateTime(2024, 12, 17), "Delivered", 836, 650),
            new ReorderData(71, 5, 167, new DateTime(2024, 11, 16), "Delivered", 352, 292),
            new ReorderData(72, 6, 167, new DateTime(2025, 1, 7), "Delivered", 306, 79),
            new ReorderData(73, 13, 999, new DateTime(2024, 12, 31), "Delivered", 532, 47),
            new ReorderData(74, 2, 1010, new DateTime(2025, 2, 1), "Delivered", 145, 60),
            new ReorderData(75, 12, 1009, new DateTime(2025, 2, 7), "Delivered", 755, 266),
            new ReorderData(76, 16, 999, new DateTime(2024, 12, 4), "Delivered", 691, 509),
            new ReorderData(77, 10, 1009, new DateTime(2024, 12, 1), "Delivered", 731, 532),
            new ReorderData(78, 18, 1011, new DateTime(2025, 1, 30), "Delivered", 575, 294),
            new ReorderData(79, 9, 999, new DateTime(2024, 12, 18), "Delivered", 539, 64),
            new ReorderData(80, 14, 1012, new DateTime(2024, 12, 26), "Delivered", 845, 728),
            new ReorderData(81, 19, 2, new DateTime(2024, 11, 14), "Delivered", 208, 94),
            new ReorderData(82, 16, 1013, new DateTime(2025, 1, 27), "Delivered", 657, 419),
            new ReorderData(83, 11, 999, new DateTime(2025, 2, 4), "Delivered", 780, 514),
            new ReorderData(84, 7, 999, new DateTime(2024, 12, 20), "Delivered", 703, 117),
            new ReorderData(85, 14, 346, new DateTime(2024, 11, 8), "Delivered", 196, 14),
            new ReorderData(86, 9, 1010, new DateTime(2024, 11, 22), "Delivered", 254, 136),
            new ReorderData(87, 3, 2, new DateTime(2025, 1, 15), "Delivered", 753, 488),
            new ReorderData(88, 5, 2, new DateTime(2025, 1, 5), "Delivered", 351, 307),
            new ReorderData(89, 1, 999, new DateTime(2025, 2, 7), "Delivered", 921, 172),
            new ReorderData(90, 19, 23, new DateTime(2024, 11, 24), "Delivered", 804, 43),
            new ReorderData(91, 13, 167, new DateTime(2025, 1, 10), "Delivered", 400, 183),
            new ReorderData(92, 22, 999, new DateTime(2025, 2, 3), "Delivered", 233, 212),
            new ReorderData(93, 20, 346, new DateTime(2024, 11, 5), "Delivered", 517, 249),
            new ReorderData(94, 4, 2, new DateTime(2025, 1, 28), "Delivered", 642, 509),
            new ReorderData(95, 6, 999, new DateTime(2024, 11, 29), "Delivered", 430, 309),
            new ReorderData(96, 18, 346, new DateTime(2025, 2, 6), "Delivered", 750, 332),
            new ReorderData(97, 15, 167, new DateTime(2025, 2, 22), "Delivered", 258, 172),
            new ReorderData(98, 12, 999, new DateTime(2025, 1, 17), "Delivered", 200, 132),
            new ReorderData(99, 19, 1012, new DateTime(2025, 2, 19), "Delivered", 513, 92),
            new ReorderData(100, 8, 1013, new DateTime(2024, 12, 1), "Delivered", 132, 107),
            new ReorderData(101, 20, 1013, new DateTime(2024, 11, 25), "Pending", 0, 0),
            new ReorderData(102, 14, 1014, new DateTime(2024, 12, 1), "Unsuccessful", 0, 0),
            new ReorderData(103, 5, 1020, new DateTime(2024, 12, 3), "Pending", 0, 0),
            new ReorderData(104, 22, 1030, new DateTime(2024, 12, 5), "Unsuccessful", 0, 0),
            new ReorderData(105, 19, 1023, new DateTime(2024, 12, 10), "Pending", 0, 0),
            new ReorderData(106, 8, 1010, new DateTime(2024, 12, 13), "Unsuccessful", 0, 0),
            new ReorderData(107, 17, 1009, new DateTime(2024, 12, 15), "Pending", 0, 0),
            new ReorderData(108, 4, 1007, new DateTime(2024, 12, 17), "Unsuccessful", 0, 0),
            new ReorderData(109, 11, 1011, new DateTime(2024, 12, 20), "Pending", 0, 0),
            new ReorderData(110, 1, 1016, new DateTime(2024, 12, 22), "Unsuccessful", 0, 0),
            new ReorderData(111, 15, 1021, new DateTime(2024, 12, 25), "Pending", 0, 0),
            new ReorderData(112, 13, 1015, new DateTime(2024, 12, 28), "Unsuccessful", 0, 0),
            new ReorderData(113, 18, 1012, new DateTime(2025, 1, 2), "Pending", 0, 0),
            new ReorderData(114, 9, 1022, new DateTime(2025, 1, 5), "Unsuccessful", 0, 0),
            new ReorderData(115, 16, 1029, new DateTime(2025, 1, 8), "Pending", 0, 0),
            new ReorderData(116, 2, 1031, new DateTime(2025, 1, 12), "Unsuccessful", 0, 0),
            new ReorderData(117, 12, 1028, new DateTime(2025, 1, 15), "Pending", 0, 0),
            new ReorderData(118, 7, 1008, new DateTime(2025, 1, 18), "Unsuccessful", 0, 0),
            new ReorderData(119, 3, 1006, new DateTime(2025, 1, 22), "Pending", 0, 0),
            new ReorderData(120, 6, 1027, new DateTime(2025, 1, 25), "Unsuccessful", 0, 0)
        };
    }

    // Method to fetch all reorder data
    public List<ReorderData> GetAllReorderDetails()
    {
        return reorderDataList;
    }
}

// Class representing the reorder data structure
public class ReorderData
{
    public int ReorderId { get; set; }
    public int ManufacturerId { get; set; }
    public int ProductId { get; set; }
    public DateTime ExpectedDeliveryDate { get; set; }
    public string Status { get; set; }
    public int Quantity { get; set; }
    public int DefectQuantity { get; set; }

    public ReorderData(int reorderId, int manufacturerId, int productId, DateTime expectedDeliveryDate, string status, int quantity, int defectQuantity)
    {
        ReorderId = reorderId;
        ManufacturerId = manufacturerId;
        ProductId = productId;
        ExpectedDeliveryDate = expectedDeliveryDate;
        Status = status;
        Quantity = quantity;
        DefectQuantity = defectQuantity;
    }
}
