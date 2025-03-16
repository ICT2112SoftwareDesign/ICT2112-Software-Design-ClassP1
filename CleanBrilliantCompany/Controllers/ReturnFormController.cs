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

		public async Task<List<ReturnForm>> DisplayAllReturnForms()
		{
			return await _returnFormControl.displayReturnForms();
		}


		[Route("returns/view/{returnId}")]
		public async Task<IActionResult> DisplayReturnForm(int returnId)
		{
			ReturnForm? returnForm = await _returnFormControl.getReturnFormById(returnId);
			if (returnForm == null)
			{
				RedirectToAction("Error", "StockFlowPage", new { errorType = "General" });
			}

			return View(returnForm);
		}


		// Handle deleting return forms.
		[Route("returns/delete")]
		public async Task<IActionResult> DeleteReturnForm(int returnId)
		{
			bool result = await _returnFormControl.deleteReturnForm(returnId);

			if (result)
			{
				return RedirectToAction("Returns", "StockFlowPage");
			}
			return RedirectToAction("Error", "StockFlowPage", new { errorType = "DeleteError" });
		}


		// Handle creating new return forms.
		[Route("returns/confirm")]
		public async Task<IActionResult> ConfirmReturnForm(int manufacturerId, int itemId, string returnReason)
		{

			// Example staff ID set to 1
			// Placeholder for returnId is 0.
			// Placeholder for warehouseId is -1, the function will query the DB and update to the correct warehouseId.
			var model = ReturnForm.createForm(0, manufacturerId, itemId, -1, returnReason, 1);

			if (ModelState.IsValid)
			{
				var result = await _returnFormControl.sendReturnForm(model);

				if (result == null)
				{
					return RedirectToAction("Error", "StockFlowPage", new { errorType = "InputError" });
				}
			}

			return RedirectToAction("Returns", "StockFlowPage");
		}
	}

}
