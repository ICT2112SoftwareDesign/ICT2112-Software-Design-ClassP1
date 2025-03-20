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
        [Route("updateItemStatus")]
        public async Task<IActionResult> updateItemStatus(int itemStatusId, ItemStatus itemStatus)
        {
            Console.WriteLine("ItemID: " + itemStatusId);
            Console.WriteLine("Status: " + itemStatus);

            bool result = await _itemControl.updateItemStatus(itemStatusId, itemStatus);
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


    }
}