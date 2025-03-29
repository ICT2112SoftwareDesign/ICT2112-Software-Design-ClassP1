using System.Diagnostics;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models.Control;
using CleanBrilliantCompany.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace CleanBrilliantCompany.Controllers
{
    [Route("inventory/management/stockflow/returns")]
    public class ReturnFormController : Controller
    {

		private readonly ReturnFormControl _returnFormControl;

		public ReturnFormController(ReturnFormControl returnFormControl)
		{

			_returnFormControl = returnFormControl;
		}

        [Route("")]
        public IActionResult Index()
		{
            var returnForms = _returnFormControl.displayReturnForms();

            return View(returnForms);

        }

        [Route("view/{itemId}")]
		public IActionResult DisplayReturnForm(int itemId)
		{
			ReturnForm? returnForm = _returnFormControl.getReturnFormById(itemId);
			if (returnForm == null)
			{
				RedirectToAction("Error", new { errorType = "General" });
			}

			return View(returnForm);
		}


        // Handle deleting return forms.
        [HttpGet]
        [Route("delete")]
        public IActionResult DeleteReturnForm(int productId, int itemId)
		{
			bool result = _returnFormControl.deleteReturnForm(productId, itemId);

			if (result)
			{
				return RedirectToAction("Index");
			}
			return RedirectToAction("Error", new { errorType = "DeleteError" });
		}


        // Handle confirm sending return forms.
        [HttpPost]
        [Route("confirm-create")]
        public async Task<IActionResult> ConfirmReturnForm(int manufacturerId, string manufacturerName, string manufacturerEmail, int productId, string productName, int itemId, string returnReason, int staffId)
		{

			// Example staff ID set to 1.
			// Placeholder for returnId is 0.
			var model = ReturnForm.createForm(0, manufacturerId, manufacturerName, manufacturerEmail, itemId, productId, productName, returnReason, staffId);

			var result = await _returnFormControl.sendReturnForm(model);

			if (result == null)
			{
				return RedirectToAction("Error", new { errorType = "InputError" });
			}
			
			return RedirectToAction("Index");
		}

        // Generate new return forms for sending (not sent yet)
        [Route("create")]
        public IActionResult Create(int productId, int itemId) {

            ReturnForm model = _returnFormControl.generateReturnForm(productId, itemId);

			return View(model);
		}

        [Route("to-return")]
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

        [Route("error")]
        public IActionResult Error(string errorType)
        {
            ViewBag.ErrorType = errorType ?? "General";
            return View();
        }

    }

}
