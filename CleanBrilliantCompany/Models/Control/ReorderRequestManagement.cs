
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
                ProductId = 228, // I change all the productId, ManufacturerId to match with what products we have in the db
                Quantity = 2,
                ManufacturerId = 8,
                ExpectedDeliveryDate = new DateTime(2025, 4, 1),
                Status = "Pending",
                DefectQuantity = 0 // idk what do i do with this?
            },
            new ReorderRequestSample
            {
                ReorderId = 2,
                ProductId = 1012,
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
            new ReorderRequestSample // Added new Approved
            {
                ReorderId = 4,
                ProductId = 1014,
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