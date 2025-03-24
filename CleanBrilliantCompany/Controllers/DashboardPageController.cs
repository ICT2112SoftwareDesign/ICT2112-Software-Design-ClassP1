using CleanBrilliantCompany.DomainControl;
using CleanBrilliantCompany.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace CleanBrilliantCompany.Controllers
{
    public class DashboardPageController : Controller
    {
        private readonly ProductCarbonFootprintControl _productControl;
        private readonly ItemCarbonFootprintControl _itemControl;
        private readonly OrderCarbonFootprintControl _orderControl;

        public DashboardPageController(ProductCarbonFootprintControl productControl, ItemCarbonFootprintControl itemControl, OrderCarbonFootprintControl orderControl)
        {
            _productControl = productControl;
            _itemControl = itemControl;
            _orderControl = orderControl;
        }

        public IActionResult Dashboard()
        {
            var products = _productControl.getAllProductCarbonFootprint();
            var ecoFriendlyCount = products.Count(p => p.calculateSelfEmission() < 250);
            var nonEcoFriendlyCount = products.Count - ecoFriendlyCount;

            var orders = _orderControl.getAllOrderCarbonFootprint();

            var transportModeCounts = orders
            .GroupBy(o => o.retrieveTransportMode().ToUpper())
            .ToDictionary(g => g.Key, g => g.Count());

            var viewModel = new CarbonDashboardViewModel
            {
                TotalProductCF = _productControl.getTotalCarbonFootprint(),
                TotalItemCF = _itemControl.getTotalCarbonFootprint(),
                TotalOrderCF = _orderControl.getTotalCarbonFootprint(),

                EcoFriendlyProductCount = ecoFriendlyCount,
                NonEcoFriendlyProductCount = nonEcoFriendlyCount,

                ProductEcoBreakdown = new Dictionary<string, int>
                {
                    { "Eco-Friendly", ecoFriendlyCount },
                    { "Not Eco-Friendly", nonEcoFriendlyCount }
                },

                OrderTransportBreakdown = transportModeCounts,

                EmissionTrendOverTime = new Dictionary<string, float>()
            };

            viewModel.EmissionTrendDaily = orders
                .GroupBy(o => o.retrieveDateCreated().ToString("yyyy-MM-dd"))
                .OrderBy(g => DateTime.Parse(g.Key))
                .ToDictionary(g => g.Key, g => g.Sum(o => (float)o.calculateSelfEmission()));

            viewModel.EmissionTrendWeekly = orders
                .GroupBy(o =>
                    CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
                        o.retrieveDateCreated(), CalendarWeekRule.FirstDay, DayOfWeek.Monday))
                .OrderBy(g => g.Key)
                .ToDictionary(g => "Week " + g.Key, g => g.Sum(o => (float)o.calculateSelfEmission()));

            viewModel.EmissionTrendMonthly = orders
                .GroupBy(o => o.retrieveDateCreated().ToString("yyyy-MM"))
                .OrderBy(g => g.Key)
                .ToDictionary(g => g.Key, g => g.Sum(o => (float)o.calculateSelfEmission()));

            return View(viewModel);
        }

        public IActionResult EcoFriendlyReport()
        {
            var allItemEmission = _itemControl.getTotalCarbonFootprint();
            var allItemCF = _itemControl.getAllItemCarbonFootprint();

            var ecoFriendlyItemEmission = allItemCF
                .Where(x => x.getEcoStatus().Equals("Eco-Friendly"))
                .Sum(x => x.calculateSelfEmission());

            ecoFriendlyItemEmission = Math.Round(ecoFriendlyItemEmission, 2);

            var ecoFriendlyEmissionPercent = Math.Round((ecoFriendlyItemEmission / allItemEmission) * 100.0, 2);

            var ecoFriendlyProducts = _productControl.getAllProductCarbonFootprint()
                .Where(x => x.getEcoStatus()
                .Equals("Eco-Friendly"));

            var viewModel = new EcoFriendlyReportViewModel
            {
                TotalItemCF = allItemEmission,
                TotalEcoFriendlyItemCF = (float)ecoFriendlyItemEmission,
                EcoFriendlyCFPercentage = (float)ecoFriendlyEmissionPercent
            };

            viewModel.ItemCarbonFootprints = new List<object>();

            foreach (ProductCarbonFootprintRDM prod in ecoFriendlyProducts)
            {
                viewModel.ItemCarbonFootprints.Add(new
                {
                    name = prod.getProductName(),
                    baseEmission = prod.calculateSelfEmission(),
                    averagePerItemEmission = Math.Round(allItemCF
                            .Where(x => x.getProductId() == prod.getProductId())
                            .Average(x => x.calculateSelfEmission()), 
                        2)
                });
            }
            return View("EcoFriendlyReport", viewModel);
        }
    }
}