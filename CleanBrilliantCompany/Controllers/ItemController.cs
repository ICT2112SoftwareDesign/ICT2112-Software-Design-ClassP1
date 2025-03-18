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
            _itemControl = new ItemControl(connectionString);
        }

        // public IActionResult Index()
        // {
        //     List<Item> items = _itemControl.getAllItems();
        //     return View("~/Views/Item/Item.cshtml", items); // Specify the full path to your view
        // }
        public async Task<IActionResult> Index()
        {
            // Retrieve all items from the database
            List<Item> items = _itemControl.getAllItems();

            // Create a list of dictionaries, where each dictionary contains the item info
            List<Dictionary<string, object>> itemsInfo = new List<Dictionary<string, object>>();

            // Iterate through each item and call retrieveItemInfo() to get the data
            foreach (var item in items)
            {
                itemsInfo.Add(item.retrieveItemInfo());
            }

            // Pass the list of item info dictionaries to the view
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




    }
}