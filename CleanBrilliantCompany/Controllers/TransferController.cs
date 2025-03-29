using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models.Control;
using CleanBrilliantCompany.Models.Entity;
using Microsoft.Extensions.Configuration;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Controllers
{
    [Route("inventory/management/stockflow/transfer")]
    public class TransferController : Controller
    {
        // private readonly ItemControl _itemControl;
        private readonly TransferControl _transferControl;
        //private readonly IWarehouse? _iWarehouse;

        public TransferController(IConfiguration configuration, IWarehouse warehouseInterface, IItemUpdate itemUpdateInterface)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found");
            // _itemControl = new ItemControl(connectionString, null);
            //_transferControl = transferControl;


            _transferControl = new TransferControl(connectionString, warehouseInterface, itemUpdateInterface);

        }

        [Route("Test")]
        public async Task<IActionResult> Index()
        {
            try
            {
                List<Dictionary<string, object>> warehousesInfo = new List<Dictionary<string, object>>();
                // List<Item> items = await _itemControl.getAllItems();
                List<Warehouse> warehouses = await _transferControl.getAllWarehouseDetails(); // Fetch warehouse data
                foreach (var warehouse in warehouses)
                {
                    warehousesInfo.Add(warehouse.retrieveWarehouseInfo());
                }
                // Console.WriteLine("WAREHOUSES: " + warehouses);
                return View(warehousesInfo); // Pass data to the view
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                return View(new List<Dictionary<string, object>>()); // Return an empty list in case of an error
            }
        }

        // [HttpPost]
        [Route("ViewTransfer")]
        public async Task<IActionResult> Transfer()
        {
            try
            {
                List<Dictionary<string, object>> transfersInfo = new List<Dictionary<string, object>>();
                // List<Item> items = await _itemControl.getAllItems();
                List<Transfer> transfers = await _transferControl.getAllTransfers(); // Fetch warehouse data
                foreach (var transfer in transfers)
                {
                    transfersInfo.Add(transfer.retrieveTransferInfo());
                }
                // Console.WriteLine("WAREHOUSES: " + warehouses);
                return View(transfersInfo); // Pass data to the view
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                return View(new List<Dictionary<string, object>>()); // Return an empty list in case of an error
            }
        }

        [Route("")]
        public async Task<IActionResult> LowStockProduct()
        {
            try
            {
                List<Dictionary<string, object>> productsInfo = new List<Dictionary<string, object>>();
                // List<Item> items = await _itemControl.getAllItems();
                List<Product> products = await _transferControl.getLowStockProductInWarehouse(); // Fetch warehouse data
                foreach (var product in products)
                {
                    productsInfo.Add(product.retrieveLowStockInfo());
                }
                // Console.WriteLine("WAREHOUSES: " + warehouses);
                return View(productsInfo); // Pass data to the view
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                return View(new List<Dictionary<string, object>>()); // Return an empty list in case of an error
            }
        }

        [HttpGet]
        [Route("getStockForWarehouse")]
        public async Task<IActionResult> GetStockForWarehouse(int productId, int warehouseId)
        {
            try
            {
                int quantity = await _transferControl.getProductQuantityByWarehouse(productId, warehouseId);
                return Json(new { success = true, quantity = quantity });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                return Json(new { success = false, message = "Failed to retrieve stock quantity." });
            }
        }


        [HttpPost]
        [Route("addTransfer")]
        public async Task<IActionResult> addTransfer(int transferId, int productId, int sourceWarehouse, int destinationWarehouse, int quantity, TransferStatus status)
        {
            //  Console.WriteLine("TRANSFER ID: " + transferId);
            Console.WriteLine("PRODUCT ID: " + productId);
            Console.WriteLine("SOURCE WAREHOUSE ID: " + sourceWarehouse);
            Console.WriteLine("DESTINATION WAREHOUSE ID: " + destinationWarehouse);
            Console.WriteLine("QUANTITY: " + quantity);
            Console.WriteLine("STATUS: " + status);

            // Fetch warehouse details
            Warehouse destinationWarehouseDetails = await _transferControl.getWarehouseDetails(destinationWarehouse);
            int sourceWarehouseQuantity = await _transferControl.getProductQuantityByWarehouse(productId, sourceWarehouse);

            if (destinationWarehouseDetails == null)
            {
                return BadRequest(new { error = "Invalid destination warehouse." });
                // return Json(new { success = false, error = "Invalid Destination Warehouse" });
            }

            int availableCapacity = destinationWarehouseDetails.getAvailableCapacity();
            Console.WriteLine("AVAILABLE CAPACITY: " + availableCapacity);

            // Validation: Check if adding the quantity exceeds max capacity
            if (quantity > availableCapacity)
            {
                // return BadRequest(new { error = "Quantity exceeds available capacity." });
                return Json(new { success = false, message = "Quantity exceeds available capacity." });
            }

            if (quantity > sourceWarehouseQuantity)
            {
                // return BadRequest(new { error = "Quantity exceeds available stock in source warehouse." });
                return Json(new { success = false, message = "Quantity exceeds available stock in source warehouse." });
            }


            // Proceed with transfer if capacity check passes
            int newTransferId = await _transferControl.createTransfer(transferId, productId, sourceWarehouse, destinationWarehouse, quantity, status);
            List<Item> transferItems = await _transferControl.getItemByProductAndWarehouse(productId, quantity, sourceWarehouse);
            //Console.WriteLine("ITEMS TO TRANSFER: " + transferItems);


            foreach (var item in transferItems)
            {
                Console.WriteLine("ITEM ID: " + item.getItem());
                bool updateItemStatus = await _transferControl.updateItemStatus(item.getItem(), null, null, newTransferId, null, ItemStatus.Transferred);
                Console.WriteLine("TRANSFER ID: " + newTransferId);
                if (!updateItemStatus)
                {
                    // Log or handle failed status update
                    Console.WriteLine($"Failed to update status for item {item.getItem()}");
                    // Potentially rollback the transfer or take corrective action
                }

            }

            // for (int i = 0; i < getItemsToTransfer.Count; i++)
            // {
            //     bool updateItemStatus = await _transferControl.updateItemStatus(getItemsToTransfer[i].ItemId, null, null, transferId, null, ItemStatus.Transferred);
            // }

            if (newTransferId > 0)
            {
                // currentCapacity += quantity;
                // Console.WriteLine("CURRENT CAPACITY: " + currentCapacity);

                return Json(new { success = true, message = "Transfer Request Successfully Submitted" });


            }
            else
            {
                // return BadRequest(new { error = "Failed to add transfer" });
                return Json(new { success = false, message = "Failed to add transfer!" });
            }
        }


        [HttpPost]
        [Route("deleteTransfer")]
        public async Task<IActionResult> deleteTransfer(int transferId)
        {
            Console.WriteLine("TRANSFER ID: " + transferId);

            bool result = await _transferControl.deleteTransfer(transferId);
            List<Item> transferredItems = await _transferControl.getTransferredItems(transferId);

            foreach (var item in transferredItems)
            {
                Console.WriteLine("ITEM ID: " + item.getItem());
                bool updateItemStatus = await _transferControl.updateItemStatus(item.getItem(), null, null, null, null, ItemStatus.Available);
                if (!updateItemStatus)
                {
                    // Log or handle failed status update
                    Console.WriteLine($"Failed to update status for item {item.getItem()}");
                    // Potentially rollback the transfer or take corrective action
                }
            }

            if (result)
            {
                return RedirectToAction("Transfer");
            }
            else
            {
                return BadRequest(new { error = "Failed to delete transfer" });
            }

        }

        [HttpPost]
        [Route("updateTransfer")]
        public async Task<IActionResult> updateTransfer(int transferId, int destinationWarehouse, TransferStatus status)
        {
            Console.WriteLine("TRANSFER ID: " + transferId);
            Console.WriteLine("DESTINATION WAREHOUSE ID: " + destinationWarehouse);
            Console.WriteLine("STATUS: " + status);

            bool result = await _transferControl.updateTransfer(transferId, destinationWarehouse, status);
            List<Item> transferredItems = await _transferControl.getTransferredItems(transferId);
            //Console.WriteLine("TRANSFERRED ITEMS: " + transferredItems);
            //bool updateItemStatus = await _transferControl.updateItemStatus(transferId, null, null, null, null, ItemStatus.Available);
            if (status == TransferStatus.Completed)
            {
                foreach (var item in transferredItems)
                {
                    Console.WriteLine("ITEM ID: " + item.getItem());
                    bool updateItemStatus = await _transferControl.updateItemStatus(item.getItem(), null, null, null, null, ItemStatus.Available);
                    if (!updateItemStatus)
                    {
                        // Log or handle failed status update
                        Console.WriteLine($"Failed to update status for item {item.getItem()}");
                        // Potentially rollback the transfer or take corrective action
                    }
                }
            }
            if (result)
            {
                return RedirectToAction("Transfer");
            }
            else
            {
                return BadRequest(new { error = "Failed to update transfer" });
            }

        }

        // [HttpPost]
        // [Route("getAllWarehouseDetails")]
        // public async Task<IActionResult> GetAllWarehouseDetails()
        // {
        //     var warehouses = await _itemControl.getAllWarehouseDetails(); // Fetch warehouse data
        //     Console.WriteLine("WAREHOUSES: " + warehouses);

        //     if (warehouses == null || !warehouses.Any())
        //     {
        //         return NotFound(new { error = "No warehouses found." });
        //     }

        //     return Ok(warehouses); // Return data as JSON
        // }

        [HttpPost]
        [Route("getProductQuantityByWarehouse")]
        public async Task<IActionResult> getProductQuantityByWarehouse(int productId, int warehouseId)
        {
            Console.WriteLine("PRODUCT ID: " + productId);
            Console.WriteLine("Warehouse ID: " + warehouseId);

            int result = await _transferControl.getProductQuantityByWarehouse(productId, warehouseId);

            if (result >= 0)
            {
                Console.WriteLine("QUANTITY: " + result);
                return RedirectToAction("Index", new { quantity = result });
            }
            else
            {
                return BadRequest(new { error = "Failed to get quantity" });
            }
        }



    }
}