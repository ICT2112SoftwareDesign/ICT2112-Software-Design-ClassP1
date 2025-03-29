using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models.Control;
using CleanBrilliantCompany.Models.Entity;
using Microsoft.Extensions.Configuration;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Controllers
{
    public class ItemController : Controller
    {
        private readonly ItemControl _itemControl;

        public ItemController(IConfiguration configuration, iProduct iProduct, iProductQuantity iProductQuantity)
        {
            // string connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found");
            _itemControl = new ItemControl(configuration, iProduct, iProductQuantity);
        }


        // default get all items
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            List<Dictionary<string, object>> itemsInfo = new List<Dictionary<string, object>>();
            List<Item> items = await _itemControl.getAllItems(page, pageSize);
            int itemsCount = _itemControl.getItemCount();

            foreach (var item in items)
            {
                itemsInfo.Add(item.retrieveItemInfo());
            }

            int totalPages = (int)Math.Ceiling((double)itemsCount / pageSize);
            // Pass data to the view
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            Console.WriteLine("TOTAL NUM OF ITEMS: " + itemsCount);

            Console.WriteLine("TOTAL PAGE NUMBER: " + totalPages);
            Console.WriteLine("TOTAL PAGE SIZE: " + pageSize);

            return View(itemsInfo);
        }


        // default get all items
        [HttpPost]
        public async Task<IActionResult> ClearSearch(int page = 1, int pageSize = 10)
        {
            List<Dictionary<string, object>> itemsInfo = new List<Dictionary<string, object>>();
            List<Item> items = await _itemControl.getAllItems(page, pageSize);

            foreach (var item in items)
            {
                itemsInfo.Add(item.retrieveItemInfo());
            }

            return RedirectToAction("Index", new { page = page, pageSize = pageSize });
        }


        // default get all items
        [HttpPost]
        [Route("getItems")]

        public async Task<IActionResult> getItems()
        {
            List<Dictionary<string, object>> itemsInfo = new List<Dictionary<string, object>>();
            List<Item> items = await _itemControl.getItems();

            foreach (var item in items)
            {
                itemsInfo.Add(item.retrieveItemInfo());
            }

            foreach (var info in itemsInfo)
            {
                Console.WriteLine($"ItemId: {info["ItemId"]}");
            }



            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("searchItemById")]
        public async Task<IActionResult> searchItemById(int searchedItemId)
        {
            List<Dictionary<string, object>> itemsInfo = new List<Dictionary<string, object>>();
            Item? item = await _itemControl.getItemById(searchedItemId);

            if (item != null)
            {
                itemsInfo.Add(item.retrieveItemInfo());
                TempData["SuccessMessage"] = "Item Found";
            }
            else TempData["ErrorMessage"] = "No item found with the searched ID";

            return View("Index", itemsInfo);  // Reuse Index view
        }

        [HttpPost]
        [Route("searchByName")]
        public async Task<IActionResult> searchByName(string searchedProductName)
        {
            List<Dictionary<string, object>> itemsInfo = new List<Dictionary<string, object>>();
            List<Item> items = await _itemControl.getItemByProductName(searchedProductName);
            if (items.Any())
            {
                foreach (var item in items)
                {
                    itemsInfo.Add(item.retrieveItemInfo());
                }
                TempData["SuccessMessage"] = "Item Found";
            }
            else
            {
                TempData["ErrorMessage"] = "No item found with searched Product Name";
            }


            return View("Index", itemsInfo);  // Reuse Index view
        }



        [HttpPost]
        [Route("addItem")]
        public async Task<IActionResult> addItem(int productId, int batchCode, int warehouseId, ItemStatus status)
        {
            bool result = await _itemControl.createItem(productId, batchCode, warehouseId, status);
            if (result)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return BadRequest(new { error = "Failed to add item." });
            }
        }

        [HttpPost]
        [Route("updateItem")]
        public async Task<IActionResult> updateItem(int itemId, float salePrice)
        {
            Console.WriteLine("ITemID: " + itemId);
            Console.WriteLine("sale price: " + salePrice);

            bool result = await _itemControl.updateItem(itemId, salePrice);
            if (result)
            {
                TempData["SuccessMessage"] = "Item Price Updated";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Item failed to update";
                return BadRequest(new { error = "Failed to add item." });
            }
        }

        [HttpPost]
        [Route("deleteItem")]
        public async Task<IActionResult> deleteItem(int deleteItemId)
        {
            Console.WriteLine("ItemID: " + deleteItemId);

            bool result = await _itemControl.deleteItem(deleteItemId);
            if (result)
            {
                TempData["SuccessMessage"] = "Item Successfully Deleted";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete item";
                return BadRequest(new { error = "Failed to delete item." });
            }
        }


        // to test update item status & id (related to iItemUpdate)
        [HttpPost]
        [Route("updateItemStatus")]
        public async Task<IActionResult> updateItemStatus(int itemId, int? reservationId, int? orderId, int? transferId, int? returnId, ItemStatus status)
        {
            Console.WriteLine("ItemID: " + itemId);
            Console.WriteLine("OrderId: " + orderId);
            Console.WriteLine("reservationId: " + reservationId);
            Console.WriteLine("transferId: " + transferId);
            Console.WriteLine("returnId: " + returnId);
            Console.WriteLine("status: " + status);

            bool result = await _itemControl.updateItemStatus(itemId, reservationId, orderId, transferId, returnId, status);
            if (result)
            {
                TempData["SuccessMessage"] = "Item Updated";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Item failed to update";
                return BadRequest(new { error = "Failed to add item." });
            }
        }

        // TESTING FOR IPRODUCT METHOD
        [HttpPost]
        [Route("retrieveProductDetails")]
        public async Task<IActionResult> retrieveProductDetails(int testProductId)
        {
            testProductId = 2;
            List<Dictionary<string, object>> productInfo = new List<Dictionary<string, object>>();
            Product? product = await _itemControl.retrieveProductDetails(testProductId);
            productInfo.Add(product.retrieveProductInfo());
            Console.WriteLine($"Product ID: {productInfo[0]["ProductId"]}");
            // Console.WriteLine($"Product Name: {productInfo[0]["ProductName"]}");
            // Console.WriteLine($"Product Category: {productInfo[0]["ProductCategory"]}");
            // Console.WriteLine($"Manufacturer ID: {productInfo[0]["ManufacturerId"]}");
            // Console.WriteLine($"Quantity: {productInfo[0]["Quantity"]}");

            return RedirectToAction("Index");
        }

        // TESTING FOR IWAREHOUSE METHOD
        [HttpPost]
        [Route("getProductQuantityByWarehouse")]
        public async Task<IActionResult> getProductQuantityByWarehouse(int productId, int warehouseId)
        {
            Console.WriteLine("PRODUCT ID: " + productId);
            Console.WriteLine("Warehouse ID: " + warehouseId);

            int result = await _itemControl.getProductQuantityByWarehouse(productId, warehouseId);

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

        // REFUNDED ITEMS FROM MOD 1
        [HttpPost]
        [Route("refundedItems")]
        public async Task<IActionResult> refundedItems(List<int> itemIds, string refundReason)
        {
            itemIds = [4, 5]; // list of items to be refunded (this is from order 1 so b4 running this prob ned to press the order items btn)
            refundReason = "Defective";
            _itemControl.returnItemToInventory(itemIds, refundReason);
            return RedirectToAction("Index");
        }

        // HANDLE ORDERING OF ITEMS
        [HttpPost]
        [Route("adjustInventory")]
        public async Task<IActionResult> adjustInventory(int orderId, Dictionary<int, int> orderProducts)
        {
            orderId = 1;
            orderProducts = new Dictionary<int, int> {
                {2, 2} // product id, quantity so prod id 2, quantity:2
            };

            _itemControl.adjustInventory(orderId, orderProducts);
            return RedirectToAction("Index");
        }

        // HANDLING CANCELLING OF ORDERS
        [HttpPost]
        [Route("processCancelledOrder")]
        public async Task<IActionResult> processCancelledOrder(int orderId)
        {
            orderId = 1;

            _itemControl.processCancelledOrder(orderId);
            return RedirectToAction("Index");
        }


        // retrieving to returned items
        [HttpPost]
        [Route("getToReturnItems")]
        public async Task<IActionResult> getToReturnItems()
        {
            List<Dictionary<string, object>> itemsInfo = new List<Dictionary<string, object>>();
            List<Item> items = await _itemControl.getToReturnItems();
            foreach (var item in items)
            {
                itemsInfo.Add(item.retrieveItemInfo());
            }

            foreach (var info in itemsInfo)
            {
                Console.WriteLine($"ItemId: {info["ItemId"]}");
            }

            // Console.WriteLine($"Product Category: {productInfo[0]["ProductCategory"]}");
            // Console.WriteLine($"Manufacturer ID: {productInfo[0]["ManufacturerId"]}");
            // Console.WriteLine($"Quantity: {productInfo[0]["Quantity"]}");

            return RedirectToAction("Index");
        }
    }
}