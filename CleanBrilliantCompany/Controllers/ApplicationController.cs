using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Controllers
{

// Define the abstract base class in the same file
    public abstract class ISession : Controller
    {
        // Define the contract that derived classes must implement
        public abstract CustomerRDM GetCustomerSession();
    }

    public class ApplicationController : ISession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICustomerSession _customerSession;

        public ApplicationController(ICustomerSession customerSession, IHttpContextAccessor httpContextAccessor)
        {
            _customerSession = customerSession ?? throw new ArgumentNullException(nameof(customerSession));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public int? getLoggedInCustomerId()
        {
            return _httpContextAccessor.HttpContext.Session.GetInt32("LoggedInUserId");
        }

        // Get Customer Session From Here
        // This is the ISession Interface 
        public override CustomerRDM GetCustomerSession()
        {
            int loggedInId = _httpContextAccessor.HttpContext.Session.GetInt32("LoggedInUserId") ?? -1;

            if (loggedInId != -1)
            {
                var customerDetails = _customerSession.getCustomer(loggedInId);
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
        // end of ISession Interface

        public ApplicationController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // Retrieve Staff ID from Session
        public int? GetLoggedInStaffId()
        {
            return _httpContextAccessor.HttpContext.Session.GetInt32("LoggedInStaffId");
        }

        // Log out Staff
        public virtual void Logout()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
        }

    }
}
