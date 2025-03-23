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

        // Get Customer Session From Here
        public CustomerRDM GetCustomerSession()
        {
            int loggedInId = _httpContextAccessor.HttpContext.Session.GetInt32("LoggedInUserId") ?? -1;

            if (loggedInId != -1)
            {
                var customerDetails = _customerManagement.getCustomer(loggedInId);
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
