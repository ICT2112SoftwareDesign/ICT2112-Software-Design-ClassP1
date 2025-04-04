
// ** FOR SIMULATION OF MODULE 1's CONTROL CLASS THAT
// IMPLEMENETS iReorderRequest. **

using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Models.Control
{
    public class ReorderRequestManagement :  iReorderRequest
    {
        public List<ReorderRequestSample> displayListOfReorders()
        {

            List<ReorderRequestSample> reorderRequests = new List<ReorderRequestSample>
{
            new ReorderRequestSample
            {
                ReorderId = 1,
                ProductId = 228, 
                Quantity = 2,
                ManufacturerId = 8,
                ExpectedDeliveryDate = new DateTime(2025, 4, 1),
                Status = "Pending",
                DefectQuantity = 0
            },
            new ReorderRequestSample
            {
                ReorderId = 2,
                ProductId = 909,
                Quantity = 2,
                ManufacturerId = 1,
                ExpectedDeliveryDate = new DateTime(2025, 4, 5),
                Status = "Approved",
                DefectQuantity = 1
            },
            new ReorderRequestSample
            {
                ReorderId = 3,
                ProductId = 600,
                Quantity = 2,
                ManufacturerId = 10,
                ExpectedDeliveryDate = new DateTime(2025, 4, 10),
                Status = "Pending",
                DefectQuantity = 1
            },
            new ReorderRequestSample 
            {
                ReorderId = 4,
                ProductId = 999,
                Quantity = 2,
                ManufacturerId = 10,
                ExpectedDeliveryDate = new DateTime(2025, 4, 11),
                Status = "Approved",
                DefectQuantity = 1
            },
        };
            return reorderRequests;
        }
    }
}