using System;
using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Services;
using System.Threading.Tasks;

namespace CleanBrilliantCompany.Controllers
{
    [Route("staff/shippingagent")]
    public class ShippingAgentController : Controller
    {
        private readonly _IShippingAgentDB _IShippingAgentDB2;

        // Add constructor with dependency injection
        public ShippingAgentController(_IShippingAgentDB ShippingAgentDB)
        {
            _IShippingAgentDB2 = ShippingAgentDB;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            // Get shipping agents from the service
            var agents = await _IShippingAgentDB2.GetShippingAgentsAsync();

            // Debug information to console
            System.Console.WriteLine($"ShippingAgentController retrieved {agents.Count} agents");
            foreach (var agent in agents)
            {
                System.Console.WriteLine($"Agent: {agent.ShippingAgentId} - {agent.ShippingAgentCompany}");
            }

            // Create and populate the view model
            var viewModel = new ShippingAgentViewModel
            {
                ShippingAgents = agents
            };

            // Pass the view model to the view with absolute path
            return View("~/Views/Staff/ShippingAgent/shippingagent.cshtml", viewModel);
        }

        // Display Add Shipping Agent form
        [HttpGet("add")]
        public IActionResult Add()
        {
            return View("~/Views/Staff/ShippingAgent/add-shippingagent.cshtml", new ShippingAgent());
        }

        // Process Add Shipping Agent form submission
        [HttpPost("AddShippingAgent")]
        public async Task<IActionResult> Add(ShippingAgent shippingAgent)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Call service to add shipping agent
                    var result = await _IShippingAgentDB2.AddShippingAgentAsync(shippingAgent);

                    if (result)
                    {
                        // Redirect to shipping agent list with success message
                        TempData["SuccessMessage"] = "Shipping agent added successfully.";
                        return RedirectToAction(nameof(Index));
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
            return View("~/Views/Staff/ShippingAgent/add-shippingagent.cshtml", shippingAgent);
        }

        // Get ShippingAgent by ID method
        [HttpGet("update/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var agent = await _IShippingAgentDB2.GetShippingAgentByIdAsync(id);

            if (agent == null)
            {
                return NotFound();
            }

            return View("~/Views/Staff/ShippingAgent/edit-shippingagent.cshtml", agent);
        }

        // Update ShippingAgent method
        [HttpPost("UpdateShippingAgent/{id}")]
        public async Task<IActionResult> Update(int id, ShippingAgent shippingAgent)
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
                    await _IShippingAgentDB2.UpdateShippingAgentAsync(shippingAgent);

                    // Redirect to shipping agent list with success message
                    TempData["SuccessMessage"] = "Shipping agent updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Log the error
                    System.Console.WriteLine($"Error updating shipping agent: {ex.Message}");
                    ModelState.AddModelError("", "An error occurred while updating the shipping agent.");
                }
            }

            // If we got this far, something failed; redisplay form
            return View("~/Views/Staff/ShippingAgent/edit-shippingagent.cshtml", shippingAgent);
        }

        // Delete ShippingAgent method
        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _IShippingAgentDB2.DeleteShippingAgentAsync(id);

            if (result)
            {
                TempData["SuccessMessage"] = "Shipping agent deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete shipping agent.";
            }

            // Redirect to shipping agent list
            return RedirectToAction(nameof(Index));
        }
    }
}