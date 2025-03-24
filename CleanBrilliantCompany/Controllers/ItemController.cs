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

        public ItemController(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found");
            _itemControl = new ItemControl(connectionString, null);
        }
        public async Task<IActionResult> Index(int? searchedItemId)
        {
            List<Dictionary<string, object>> itemsInfo = new List<Dictionary<string, object>>();
            if (searchedItemId != null)
            {
                Item item = await _itemControl.getItemById(searchedItemId.Value);
                itemsInfo.Add(item.retrieveItemInfo());
            }
            else
            {
                List<Item> items = await _itemControl.getAllItems();

                foreach (var item in items)
                {
                    itemsInfo.Add(item.retrieveItemInfo());
                }
            }

            return View(itemsInfo);
        }

        [HttpPost]
        [Route("addItem")]
        public async Task<IActionResult> addItem(int itemId, int productId, float salePrice, int batchCode, int warehouseId, ItemStatus status)
        {
            bool result = await _itemControl.createItem(itemId, productId, salePrice, batchCode, warehouseId, status);
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
                return RedirectToAction("Index");
            }
            else
            {
                return BadRequest(new { error = "Failed to add item." });
            }
        }

        [HttpPost]
        [Route("updateItemStatusOld")]
        public async Task<IActionResult> updateItemStatusOld(int itemStatusIdOld, ItemStatus itemStatus)
        {
            Console.WriteLine("ItemID: " + itemStatusIdOld);
            Console.WriteLine("Status: " + itemStatus);

            bool result = await _itemControl.updateItemStatusOld(itemStatusIdOld, itemStatus);
            if (result)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return BadRequest(new { error = "Failed to add item." });
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
                return RedirectToAction("Index");
            }
            else
            {
                return BadRequest(new { error = "Failed to add item." });
            }
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
        // [HttpPost]
        // [Route("refundedItems")]
        // public async Task<IActionResult> refundedItems(List<int> itemIds, string refundReason)
        // {
        //     await _itemControl.ReturnItemToInventory(itemIds, refundReason);
        //     return RedirectToAction("Index");
        // }

        // HANDLE ORDERING OF ITEMS
        [HttpPost]
        [Route("adjustInventory")]
        public async Task<IActionResult> adjustInventory(int orderId, Dictionary<int, int> orderProducts)
        {
            orderId = 1;
            orderProducts = new Dictionary<int, int> {
                {2, 2} // product id, quantity so prod id 2, quantity:2
            };

            await _itemControl.adjustInventory(orderId, orderProducts);
            return RedirectToAction("Index");
        }

        // HANDLING CANCELLING OF ORDERS
        [HttpPost]
        [Route("processCancelledOrder")]
        public async Task<IActionResult> processCancelledOrder(int orderId)
        {
            Console.WriteLine("PROCESS ITEM CONTROLLER RUNNING");
            orderId = 1;
          
            _itemControl.processCancelledOrder(orderId);
            return RedirectToAction("Index");
        }

    }
}