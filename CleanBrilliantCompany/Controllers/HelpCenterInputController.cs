using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Controllers
{
    public class HelpCenterInputController : ApplicationController 
    {
        private readonly SupportManagement _supportManagement;
        private readonly CustomerManagement _customerManagement;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HelpCenterInputController(SupportManagement supportManagement, CustomerManagement customerManagement, IHttpContextAccessor httpContextAccessor)
        : base(customerManagement, httpContextAccessor)
        {
            _supportManagement = supportManagement;
        } 

        public IActionResult viewFAQs()
        {
            Dictionary<string, string> faqs = _supportManagement.FetchFAQs();
            ViewBag.FAQs = faqs;

            return View("~/Views/Support/FAQs.cshtml");
        }

        [HttpGet]
        public IActionResult escalateIssue(String issueDescription)
        {
            // Retrieve customer ID from the session using the correct key
            int? customerId = base.getLoggedInCustomerId();
            if (customerId == -1)
            {
                TempData["Error"] = "User not logged in.";
                return Json(new { redirectUrl = Url.Action("Login", "BeforeLoginPage") });
            }

            bool success = _supportManagement.createSupportTicket(customerId ?? -1, issueDescription);
            if (success)
            {
                Console.WriteLine("Support ticket created, pop up will appear!");
                TempData["Success"] = "Issue has been successfully raised!";
            } else{
                Console.WriteLine("Support ticket failed to create!");
                TempData["Error"] = "Failed to raise the issue!";
            }

            return RedirectToAction("viewFAQs");
        }

        public IActionResult viewSupportTickets()
        {
            // Retrieve customer ID from the session using the correct key
            int? customerId = base.getLoggedInCustomerId();
            if (customerId == -1)
            {
                TempData["Error"] = "User not logged in.";
                return Json(new { redirectUrl = Url.Action("Login", "BeforeLoginPage") });
            }

            var allCustomerSupportTicket = _supportManagement.viewTicketByCustomer(customerId ?? -1);
            ViewBag.AllSupportTickets = allCustomerSupportTicket;        
            return View("~/Views/Support/ViewSupportTickets.cshtml");
        }
    }
}