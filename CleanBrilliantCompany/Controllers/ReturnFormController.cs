using System.Diagnostics;
using CleanBrilliantCompany.Models.Control;
using CleanBrilliantCompany.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace CleanBrilliantCompany.Controllers
{
    
    public class ReturnFormController : Controller
    {

		private readonly ReturnFormControl _returnFormControl;

		public ReturnFormController(ReturnFormControl returnFormControl)
		{

			_returnFormControl = returnFormControl;
		}

        [HttpPost]
        [Route("inventory/management/stockflow/returns/display")]
        public List<ReturnForm> DisplayAllReturnForms()
		{
			return _returnFormControl.displayReturnForms();
		}

        [Route("inventory/management/stockflow/returns/view/{itemId}")]
		public IActionResult DisplayReturnForm(int itemId)
		{
			ReturnForm? returnForm = _returnFormControl.getReturnFormById(itemId);
			if (returnForm == null)
			{
				RedirectToAction("Error", "StockFlowPage", new { errorType = "General" });
			}

			return View(returnForm);
		}


        // Handle deleting return forms.
        [HttpGet]
        [Route("inventory/management/stockflow/returns/delete")]
        public IActionResult DeleteReturnForm(int productId, int itemId)
		{
			bool result = _returnFormControl.deleteReturnForm(productId, itemId);

			if (result)
			{
				return RedirectToAction("Returns", "StockFlowPage");
			}
			return RedirectToAction("Error", "StockFlowPage", new { errorType = "DeleteError" });
		}


        // Handle confirm sending return forms.
        [HttpPost]
        [Route("inventory/management/stockflow/returns/confirm-create")]
        public async Task<IActionResult> ConfirmReturnForm(int manufacturerId, string manufacturerName, string manufacturerEmail, int productId, string productName, int itemId, string returnReason, int staffId)
		{

			// Example staff ID set to 1.
			// Placeholder for returnId is 0.
			var model = ReturnForm.createForm(0, manufacturerId, manufacturerName, manufacturerEmail, itemId, productId, productName, returnReason, staffId);

			var result = await _returnFormControl.sendReturnForm(model);

			if (result == null)
			{
				return RedirectToAction("Error", "StockFlowPage", new { errorType = "InputError" });
			}
			
			return RedirectToAction("Returns", "StockFlowPage");
		}

        // Generate new return forms for sending (not sent yet)
        [Route("inventory/management/stockflow/returns/create")]
        public IActionResult Create(int productId, int itemId) {

            ReturnForm model = _returnFormControl.generateReturnForm(productId, itemId);

			return View(model);
		}

        [Route("inventory/management/stockflow/returns/to-return")]
        public ActionResult ShowAllToReturn()
        {
            List<Dictionary<string, object>> itemsInfo = new List<Dictionary<string, object>>();

            List<Item> items = _returnFormControl.displayAllRefundedItems();

            foreach (var item in items)
            {
                itemsInfo.Add(item.retrieveItemInfo());
            }

            return View(itemsInfo);
        }

    }

}
