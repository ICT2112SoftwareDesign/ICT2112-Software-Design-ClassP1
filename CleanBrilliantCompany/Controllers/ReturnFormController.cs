using CleanBrilliantCompany.Control;
using CleanBrilliantCompany.Models;
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

		public List<ReturnForm> DisplayAllReturnForms()
		{
			return _returnFormControl.displayReturnForms();
		}


		[Route("view/{returnId}")]
		public IActionResult DisplayReturnForm(int returnId)
		{
			ReturnForm? returnForm = _returnFormControl.getReturnFormById(returnId);
			if (returnForm == null)
			{
				RedirectToAction("Error", "StockFlowPage", new { errorType = "General" });
			}

			return View(returnForm);
		}


		// Handle deleting return forms.
		[Route("returns/delete")]
		public IActionResult DeleteReturnForm(int returnId, int itemId)
		{
			bool result = _returnFormControl.deleteReturnForm(returnId, itemId);

			if (result)
			{
				return RedirectToAction("Returns", "StockFlowPage");
			}
			return RedirectToAction("Error", "StockFlowPage", new { errorType = "DeleteError" });
		}


		// Handle creating new return forms.
		[Route("returns/confirm")]
		public IActionResult ConfirmReturnForm(int manufacturerId, int itemId, string returnReason)
		{

			// Example staff ID set to 10
			// Placeholder for returnId is 0.
			// Placeholder for warehouseId is -1, the function will query the DB and update to the correct warehouseId.
			var model = ReturnForm.createForm(0, manufacturerId, itemId, -1, returnReason, 10);

			if (ModelState.IsValid)
			{
				var result = _returnFormControl.sendReturnForm(model);

				if (result == null)
				{
					return RedirectToAction("Error", "StockFlowPage", new { errorType = "InputError" });
				}
			}

			return RedirectToAction("Returns", "StockFlowPage");
		}
	}

}
