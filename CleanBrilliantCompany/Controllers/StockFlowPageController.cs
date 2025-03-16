using Microsoft.AspNetCore.Mvc;

namespace CleanBrilliantCompany.Controllers
{
	[Route("inventory/management/stockflow")]
	public class StockFlowPageController : Controller
	{
		private readonly ReturnFormController _returnFormController;

		public StockFlowPageController(ReturnFormController returnFormController)
		{
			_returnFormController = returnFormController;
		}

        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("returns")]
		public async Task<IActionResult> Returns()
		{
			var returnForms = await _returnFormController.DisplayAllReturnForms();
			return View(returnForms);
		}

		[Route("returns/create")]
		public IActionResult Create()
		{
			return View();
		}

		[Route("returns/error")]
		public IActionResult Error(string errorType)
		{
			ViewBag.ErrorType = errorType ?? "General";
			return View();
		}


	}
}
