using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly CustomerManagement _customerManagement;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationController(CustomerManagement customerManagement, IHttpContextAccessor httpContextAccessor)
        {
            _customerManagement = customerManagement;
            _httpContextAccessor = httpContextAccessor;
        }

        // Get Customer Session From Here (Hopefully it works)
        public CustomerRDM GetCustomerSession()
        {
            string loggedInEmail = _httpContextAccessor.HttpContext.Session.GetString("LoggedInUserEmail");

            if (!string.IsNullOrEmpty(loggedInEmail))
            {
                var customerDetails = _customerManagement.getCustomer(loggedInEmail);
                if (customerDetails != null)
                {
                    return customerDetails;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}