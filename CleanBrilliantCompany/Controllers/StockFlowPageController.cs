using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Models.Control;
namespace CleanBrilliantCompany.Controllers
{
	[Route("inventory/management/stockflow")]
	public class StockFlowPageController : Controller
	{
		private readonly ReturnFormController _returnFormController;
		// private readonly TransferController _transferController;
		private readonly TransferControl _transferControl;

		public StockFlowPageController(ReturnFormController returnFormController, TransferControl transferControl)
		{
			_transferControl = transferControl;
			_returnFormController = returnFormController;
		}


		[Route("")]
		public IActionResult Index()
		{
			return View();
		}

		[Route("returns")]
		public IActionResult Returns()
		{
			var returnForms = _returnFormController.DisplayAllReturnForms();
			return View(returnForms);
		}

		[Route("returns/error")]
		public IActionResult Error(string errorType)
		{
			ViewBag.ErrorType = errorType ?? "General";
			return View();
		}

		[Route("transfer")]
		public async Task<IActionResult> LowStockProduct()
		{
			try
			{
				List<Dictionary<string, object>> productsInfo = new List<Dictionary<string, object>>();
				List<Product> products = await _transferControl.getLowStockProductInWarehouse(); // Fetch warehouse data

				foreach (var product in products)
				{
					productsInfo.Add(product.retrieveLowStockInfo());
				}

				return View(productsInfo);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"ERROR: {ex.Message}");
				return View(new List<Dictionary<string, object>>()); // Return an empty list in case of an error
			}
		}

	}
}
