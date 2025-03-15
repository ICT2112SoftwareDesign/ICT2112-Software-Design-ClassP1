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
        public IActionResult Index()
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

        public IActionResult Details(int itemId)
        {
            var item = _itemControl.getItem(itemId);
            if (item == null)
            {
                return NotFound();
            }

            return View(item); // Pass Item directly to view
        }

    }
}