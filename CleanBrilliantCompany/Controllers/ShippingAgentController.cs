using System;
using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Interfaces;
using System.Threading.Tasks;
using System.Linq;

namespace CleanBrilliantCompany.Controllers
{
    [Route("staff/shippingagent")]
    public class ShippingAgentController : ApplicationController
    {
        private readonly IShippingAgentDB _shippingAgentDB;

        // Add constructor with dependency injection
        public ShippingAgentController(IShippingAgentDB shippingAgentDB, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _shippingAgentDB = shippingAgentDB;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            int? staffId = GetLoggedInStaffId();
            if (staffId == null)
            {
                TempData["Message"] = "You must log in to access the Shipping Agents page.";
                return RedirectToAction("Login", "StaffLogin");
            }
            
            // Get shipping agents from the service
            var agents = await _shippingAgentDB.GetAllShippingAgentsAsync();
            var agentsList = agents.ToList();

            // Debug information to console
            System.Console.WriteLine($"ShippingAgentController retrieved {agentsList.Count} agents");
            foreach (var agent in agentsList)
            {
                System.Console.WriteLine($"Agent: {agent.ShippingAgentId} - {agent.ShippingAgentCompany}");
            }

            // Create and populate the view model
            var viewModel = new ShippingAgentManagement
            {
                ShippingAgents = agentsList
            };

            // Pass the view model to the view with absolute path
            return View("~/Views/ShippingAgent/shippingagent.cshtml", viewModel);
        }

        // Display Add Shipping Agent form
        [HttpGet("add")]
        public IActionResult Add()
        {
            int? staffId = GetLoggedInStaffId();
            if (staffId == null)
            {
                TempData["Message"] = "You must log in to access the Shipping Agents page.";
                return RedirectToAction("Login", "StaffLogin");
            }
            
            return View("~/Views/ShippingAgent/add-shippingagent.cshtml", new ShippingAgent_RDM());
        }

        // Process Add Shipping Agent form submission
        [HttpPost("AddShippingAgent")]
        public async Task<IActionResult> Add(ShippingAgent_RDM shippingAgent)
        {
            int? staffId = GetLoggedInStaffId();
            if (staffId == null)
            {
                TempData["Message"] = "You must log in to access the Shipping Agents page.";
                return RedirectToAction("Login", "StaffLogin");
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    // Convert ShippingAgent to ShippingAgent_RDM
                    var shippingAgentRDM = ConvertToShippingAgentRDM(shippingAgent);
                    
                    // Call service to add shipping agent
                    var result = await _shippingAgentDB.AddShippingAgentAsync(shippingAgentRDM);

                    if (result != null && result.ShippingAgentId > 0)
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
            return View("~/Views/ShippingAgent/add-shippingagent.cshtml", shippingAgent);
        }

        // Get ShippingAgent by ID method
        [HttpGet("update/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            int? staffId = GetLoggedInStaffId();
            if (staffId == null)
            {
                TempData["Message"] = "You must log in to access the Shipping Agents page.";
                return RedirectToAction("Login", "StaffLogin");
            }

            var agentRDM = await _shippingAgentDB.GetShippingAgentByIdAsync(id);

            if (agentRDM == null)
            {
                return NotFound();
            }

            // Convert ShippingAgent_RDM to ShippingAgent
            var agent = ConvertToShippingAgent(agentRDM);

            return View("~/Views/ShippingAgent/edit-shippingagent.cshtml", agent);
        }

        // Update ShippingAgent method
        [HttpPost("UpdateShippingAgent/{id}")]
        public async Task<IActionResult> Update(int id, ShippingAgent_RDM shippingAgent)
        {
            int? staffId = GetLoggedInStaffId();
            if (staffId == null)
            {
                TempData["Message"] = "You must log in to access the Shipping Agents page.";
                return RedirectToAction("Login", "StaffLogin");
            }
            
            if (id != shippingAgent.ShippingAgentId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Convert ShippingAgent to ShippingAgent_RDM
                    var shippingAgentRDM = ConvertToShippingAgentRDM(shippingAgent);
                    
                    // Call service to update shipping agent
                    var result = await _shippingAgentDB.UpdateShippingAgentAsync(id, shippingAgentRDM);

                    if (result != null && result.ShippingAgentId > 0)
                    {
                        // Redirect to shipping agent list with success message
                        TempData["SuccessMessage"] = "Shipping agent updated successfully.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to update shipping agent.");
                    }
                }
                catch (Exception ex)
                {
                    // Log the error
                    System.Console.WriteLine($"Error updating shipping agent: {ex.Message}");
                    ModelState.AddModelError("", "An error occurred while updating the shipping agent.");
                }
            }

            // If we got this far, something failed; redisplay form
            return View("~/Views/ShippingAgent/edit-shippingagent.cshtml", shippingAgent);
        }

        // Delete ShippingAgent method
        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            int? staffId = GetLoggedInStaffId();
            if (staffId == null)
            {
                TempData["Message"] = "You must log in to access the Shipping Agents page.";
                return RedirectToAction("Login", "StaffLogin");
            }
            
            var result = await _shippingAgentDB.DeleteShippingAgentAsync(id);

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

        // Helper method to convert ShippingAgent to ShippingAgent_RDM
        private ShippingAgent_RDM ConvertToShippingAgentRDM(ShippingAgent_RDM agent)
        {
            return new ShippingAgent_RDM
            {
                ShippingAgentId = agent.ShippingAgentId,
                ShippingAgentCompany = agent.ShippingAgentCompany,
                ShippingMethod = agent.ShippingMethod,
                ServiceType = agent.ServiceType
              
            };
        }

        // Helper method to convert ShippingAgent_RDM to ShippingAgent
        private ShippingAgent_RDM ConvertToShippingAgent(ShippingAgent_RDM agentRDM)
        {
            return new ShippingAgent_RDM
            {
                ShippingAgentId = agentRDM.ShippingAgentId,
                ShippingAgentCompany = agentRDM.ShippingAgentCompany,
                ShippingMethod = agentRDM.ShippingMethod,
                ServiceType = agentRDM.ServiceType
               
            };
        }
    }
}