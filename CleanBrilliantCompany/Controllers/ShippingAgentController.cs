using System;
using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Interfaces;
using System.Threading.Tasks;
using System.Linq;

namespace CleanBrilliantCompany.Controllers
{
    [Route("Staff/shippingagent")]
    public class ShippingAgentController : ApplicationController
    {
        private readonly ShippingAgentManagement _shippingAgentManagement;

        public ShippingAgentController(ShippingAgentManagement shippingAgentManagement, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _shippingAgentManagement = shippingAgentManagement;
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

            var agents = await _shippingAgentManagement.GetAllShippingAgentsAsync();
            var agentsList = agents.ToList();

            System.Console.WriteLine($"ShippingAgentController retrieved {agentsList.Count} agents");
            foreach (var agent in agentsList)
            {
                System.Console.WriteLine($"Agent: {agent.ShippingAgentId} - {agent.ShippingAgentCompany}");
            }

            var viewModel = new ShippingAgentManagement(null) // Pass null because this is only for the view
            {
                ShippingAgents = agentsList
            };

            return View("~/Views/ShippingAgent/shippingagent.cshtml", viewModel);
        }

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
                    var result = await _shippingAgentManagement.AddShippingAgentAsync(shippingAgent);

                    if (result != null && result.ShippingAgentId > 0)
                    {
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
                    System.Console.WriteLine($"Error adding shipping agent: {ex.Message}");
                    ModelState.AddModelError("", "An error occurred while adding the shipping agent.");
                }
            }

            return View("~/Views/ShippingAgent/add-shippingagent.cshtml", shippingAgent);
        }

        [HttpGet("update/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            int? staffId = GetLoggedInStaffId();
            if (staffId == null)
            {
                TempData["Message"] = "You must log in to access the Shipping Agents page.";
                return RedirectToAction("Login", "StaffLogin");
            }

            var agentRDM = await _shippingAgentManagement.GetShippingAgentByIdAsync(id);

            if (agentRDM == null)
            {
                return NotFound();
            }

            return View("~/Views/ShippingAgent/edit-shippingagent.cshtml", agentRDM);
        }

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
                    var result = await _shippingAgentManagement.UpdateShippingAgentAsync(id, shippingAgent);

                    if (result != null && result.ShippingAgentId > 0)
                    {
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
                    System.Console.WriteLine($"Error updating shipping agent: {ex.Message}");
                    ModelState.AddModelError("", "An error occurred while updating the shipping agent.");
                }
            }

            return View("~/Views/ShippingAgent/edit-shippingagent.cshtml", shippingAgent);
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            int? staffId = GetLoggedInStaffId();
            if (staffId == null)
            {
                TempData["Message"] = "You must log in to access the Shipping Agents page.";
                return RedirectToAction("Login", "StaffLogin");
            }

            var result = await _shippingAgentManagement.DeleteShippingAgentAsync(id);

            if (result)
            {
                TempData["SuccessMessage"] = "Shipping agent deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete shipping agent.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}