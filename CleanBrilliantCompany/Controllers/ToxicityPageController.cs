using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;
using Microsoft.AspNetCore.Mvc;

namespace CleanBrilliantCompany.Controllers
{
    public class ToxicityPageController : Controller
    {
        private readonly IToxicity _toxicity;
        private readonly IIngredientDB _ingredientGateway;

        public ToxicityPageController(IToxicity toxicity, IIngredientDB ingredientGateway)
        {
            _toxicity = toxicity;
            _ingredientGateway = ingredientGateway;
        }

        public IActionResult Index(int ingredientId = 1)
        {
            Ingredient ingredient = _ingredientGateway.FindIngredientsbyID(ingredientId);
            if (ingredient == null)
            {
                return NotFound("Ingredient not found.");
            }

            float toxicityScore = _toxicity.RetrieveToxicity(ingredientId);
            ViewBag.ToxicityScore = toxicityScore;
            
            return View(ingredient);
        }
    }
}
