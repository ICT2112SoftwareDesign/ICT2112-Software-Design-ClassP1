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

        public ItemController(IConfiguration configuration, IProduct iProduct, IProductQuantity iProductQuantity)
        {
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


        // clear search route
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


        // update of status & relevant ids 
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

        // HANDLE ORDERING OF ITEMS (MOD 1)
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

        // HANDLING CANCELLING OF ORDERS (MOD 1)
        [HttpPost]
        [Route("processCancelledOrder")]
        public async Task<IActionResult> processCancelledOrder(int orderId)
        {
            orderId = 1;

            _itemControl.processCancelledOrder(orderId);
            return RedirectToAction("Index");
        }


    }
}