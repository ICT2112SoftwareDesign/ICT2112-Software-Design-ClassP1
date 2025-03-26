using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Services;
using System.Threading.Tasks;

namespace CleanBrilliantCompany.Controllers.Staff
{
    [Route("staff")]
    public class StaffController : Controller
    {
        private readonly IShippingAgentService _shippingAgentService;

        // Add constructor with dependency injection
        public StaffController(IShippingAgentService shippingAgentService)
        {
            _shippingAgentService = shippingAgentService;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View(); // Renders Views/Staff/Index.cshtml
        }

        [HttpGet("shippingagent")]
        public async Task<IActionResult> ShippingAgent()
        {
            // Get shipping agents from the service
            var agents = await _shippingAgentService.GetShippingAgentsAsync();

            // Debug information to console
            System.Console.WriteLine($"StaffController retrieved {agents.Count} agents");
            foreach (var agent in agents)
            {
                System.Console.WriteLine($"Agent: {agent.ShippingAgentId} - {agent.ShippingAgentCompany}");
            }

            // Create and populate the view model
            var viewModel = new ShippingAgentViewModel
            {
                ShippingAgents = agents
            };

            // Pass the view model to the view
            return View("ShippingAgent/shippingagent", viewModel);
        }

        // Display Add Shipping Agent form
        [HttpGet("shippingagent/add")]
        public IActionResult AddShippingAgent()
        {
            return View("ShippingAgent/add-shippingagent", new ShippingAgent());
        }

        // Process Add Shipping Agent form submission
        [HttpPost("shippingagent/add")]
        public async Task<IActionResult> AddShippingAgent(ShippingAgent shippingAgent)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Call service to add shipping agent
                    var result = await _shippingAgentService.AddShippingAgentAsync(shippingAgent);

                    if (result)
                    {
                        // Redirect to shipping agent list with success message
                        TempData["SuccessMessage"] = "Shipping agent added successfully.";
                        return RedirectToAction(nameof(ShippingAgent));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to add shipping agent.");
                    }
                }
                catch (Exception ex)
                {
                    // Log the error
                    System.Console.WriteLine($"Error adding shipping agent: {ex.Message}");
                    ModelState.AddModelError("", "An error occurred while adding the shipping agent.");
                }
            }

            // If we got this far, something failed; redisplay form
            return View("ShippingAgent/add-shippingagent", shippingAgent);
        }

        // Get ShippingAgent by ID method
        [HttpGet("shippingagent/edit/{id}")]
        public async Task<IActionResult> EditShippingAgent(int id)
        {
            var agent = await _shippingAgentService.GetShippingAgentByIdAsync(id);

            if (agent == null)
            {
                return NotFound();
            }

            return View("ShippingAgent/edit-shippingagent", agent);
        }

        // Update ShippingAgent method
        [HttpPost("shippingagent/update/{id}")]
        public async Task<IActionResult> UpdateShippingAgent(int id, ShippingAgent shippingAgent)
        {
            if (id != shippingAgent.ShippingAgentId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Call service to update shipping agent
                    await _shippingAgentService.UpdateShippingAgentAsync(shippingAgent);

                    // Redirect to shipping agent list with success message
                    TempData["SuccessMessage"] = "Shipping agent updated successfully.";
                    return RedirectToAction(nameof(ShippingAgent));
                }
                catch (Exception ex)
                {
                    // Log the error
                    System.Console.WriteLine($"Error updating shipping agent: {ex.Message}");
                    ModelState.AddModelError("", "An error occurred while updating the shipping agent.");
                }
            }

            // If we got this far, something failed; redisplay form
            return View("ShippingAgent/edit-shippingagent", shippingAgent);
        }


        // Delete ShippingAgent method
        [HttpPost("shippingagent/delete/{id}")]
        public async Task<IActionResult> DeleteShippingAgent(int id)
        {
            var result = await _shippingAgentService.DeleteShippingAgentAsync(id);

            if (result)
            {
                TempData["SuccessMessage"] = "Shipping agent deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete shipping agent.";
            }

            // Redirect to shipping agent list - corrected
            return RedirectToAction(nameof(ShippingAgent));
        }

        [HttpGet("orderfufilment")]
        public IActionResult OrderFufilment()
        {
            return View(); // Renders Views/Staff/OrderFufilment.cshtml
        }

        [HttpGet("refund")]
        public IActionResult Refund()
        {
            return View(); // Renders Views/Staff/Refund.cshtml
        }

        [HttpGet("reorder")]
        public IActionResult Reorder()
        {
            return View(); // Renders Views/Staff/Reorder.cshtml
        }

        [HttpGet("ticket")]
        public IActionResult Ticket()
        {
            return View(); // Renders Views/Staff/Ticket.cshtml
        }

        // Add this method for debugging - access it via /staff/debug
        [HttpGet("debug")]
        public IActionResult Debug()
        {
            return Content("StaffController is working!");
        }
    }
}