using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Controllers
{
    public class CustomerPageController : Controller
    {
        private readonly ILogger<CustomerPageController> _logger;
        private readonly CustomerManagement _customerManagement;
        private readonly SupportManagement _supportManagement;

        public CustomerPageController(ILogger<CustomerPageController> logger, CustomerManagement customerManagement, SupportManagement supportManagement)
        {
            _logger = logger;
            _customerManagement = customerManagement;
            _supportManagement = supportManagement;
        }

        public IActionResult CustomerDetails()
        {
            string loggedInEmail = HttpContext.Session.GetString("LoggedInUserEmail");
            var applicationController = new ApplicationController(_customerManagement, HttpContext.RequestServices.GetService<IHttpContextAccessor>());

            // Retrieve Customer Details via Session
            var customerDetails = applicationController.GetCustomerSession();

            if (customerDetails != null)
            {
                ViewBag.CustomerId = customerDetails.GetSession<int>("customerId");
                ViewBag.Email = loggedInEmail;
                ViewBag.Username = customerDetails.GetSession<string>("username");
                ViewBag.CustomerAddress = customerDetails.GetSession<string>("customerAddress");
            }
            else
            {
                ViewBag.Message = "No customer details available.";
            }

            return View("~/Views/CustomerPage/Profile/CustomerDetails.cshtml");
        }

        // INPUT CONTROLLER METHODS

        // HelpCenterInputController Methods

        // public IActionResult displayHelpCenterOptions()
        // {
            
        // }

        // public IActionResult submitQuery(String query)
        // {
            
        // }

        public IActionResult viewFAQs(String query)
        {
            List<String> faqs = _supportManagement.FetchFAQs();
            ViewBag.FAQs = faqs;

            return View("~/Views/Support/FAQs.cshtml");
        }

        // public IActionResult trackTicket(Int32 ticketId)
        // {
            
        // }

        // public IActionResult escalateIssue(Int32 ticketId)
        // {
            
        // }

        // // ChatbotInputController Methods

        public IActionResult startChatSession()
        {
            return View("~/Views/Support/Chatbot.cshtml");
        }

        // public IActionResult provideAutomatedResponse(String query)
        // {
            
        // }

        // public IActionResult escalateToAgent(String query)
        // {
            
        // }
    }
}
