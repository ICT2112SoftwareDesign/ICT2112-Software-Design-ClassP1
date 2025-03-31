using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Controllers
{
    public class CustomerInputController : ApplicationController
    {
        private readonly ILogger<CustomerInputController> _logger;
        private readonly CustomerManagement _customerManagement;

        public CustomerInputController(ILogger<CustomerInputController> logger, CustomerManagement customerManagement, IHttpContextAccessor httpContextAccessor)
        : base(customerManagement, httpContextAccessor)
        {
            _logger = logger;
            _customerManagement = customerManagement;
        } 

        // Input Controller Methods (Customer Profile)
        public IActionResult CustomerDetails()
        {
            int? customerId = base.getLoggedInCustomerId();
            var customerDetails = base.GetCustomerSession();

            if (customerDetails != null)
            {
                TempData["CustomerId"] = customerId;
                TempData["Email"] = customerDetails.getSession<string>("email");
                TempData["Password"] = customerDetails.getSession<string>("password");
                TempData["Username"] = customerDetails.getSession<string>("username");
                TempData["CustomerAddress"] = customerDetails.getSession<string>("customerAddress");
                TempData["EmailPreference"] = customerDetails.getEmailPreferenceRaw() ?? "";

                HttpContext.Session.SetString("emailPreference", customerDetails.getEmailPreferenceRaw() ?? "");

            }
            else
            {
                TempData["Message"] = "No customer details available.";
            }

            return RedirectToAction("CustomerDetails", "CustomerPage");
        }

        [HttpPost]
        public IActionResult updateCustomerDetails(string username, string email, string address)
        {
            int? customerId = base.getLoggedInCustomerId();
            var customerDetails = base.GetCustomerSession();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email))
            {
                TempData["Message"] = "Username and/or email cannot be empty";
                CustomerDetails();
                return RedirectToAction("CustomerDetails", "CustomerPage");
            }

            bool isEmailChanged = email != customerDetails.getSession<string>("email");
            bool isUsernameChanged = username != customerDetails.getSession<string>("username");

            if (isEmailChanged || isUsernameChanged)
            {
                var validationMessage = validateChanges(customerId ?? -1, username, email, isEmailChanged, isUsernameChanged);
                if (validationMessage != null)
                {
                    TempData["Message"] = validationMessage;
                    CustomerDetails();
                    return RedirectToAction("CustomerDetails", "CustomerPage");
                }
            }

            bool updateSuccessful = _customerManagement.updateCustomerDetails(customerId ?? -1, username, email, address);
            if (updateSuccessful)
            {
                CustomerDetails();
                return RedirectToAction("CustomerDetails", "CustomerPage");
            }

            TempData["Message"] = "Failed to update details.";
            CustomerDetails();
            return RedirectToAction("CustomerDetails", "CustomerPage");
        }


        // Check if username or email exists
        private string validateChanges(int loggedInId, string username, string email, bool isEmailChanged, bool isUsernameChanged)
        {
            if (isEmailChanged && _customerManagement.customerEmailExists(loggedInId, email))
            {
                return "Email already exists.";
            }
            if (isUsernameChanged && _customerManagement.customerUsernameExists(loggedInId, username))
            {
                return "Username already exists.";
            }

            return null;
        }

        [HttpPost]
        public IActionResult updatePassword(string newPassword, string confirmPassword)
        {
            int? customerId = base.getLoggedInCustomerId();

            if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                TempData["Message"] = "New password and Confirm Password cannot be empty";
                CustomerDetails();
                return RedirectToAction("CustomerDetails", "CustomerPage");
            }

            if (newPassword != confirmPassword)
            {
                TempData["Message"] = "Passwords do not match!";
                CustomerDetails();
                return RedirectToAction("CustomerDetails", "CustomerPage");
            }

            bool passwordSuccess = _customerManagement.updatePassword(customerId ?? -1, newPassword);
            if (passwordSuccess)
            {
                CustomerDetails();
                return RedirectToAction("CustomerDetails", "CustomerPage");
            }
            else
            {
                TempData["Message"] = "Failed to update password.";
                CustomerDetails();
                return RedirectToAction("CustomerDetails", "CustomerPage");
            }
        }

        [HttpPost]
        public IActionResult updatePreferences(int customerId, bool suppressPaid, bool suppressCancelled)
        {
            var customer = _customerManagement.getCustomer(customerId);
            if (customer != null)
            {
                customer.SetPreferencesFromCheckbox(suppressPaid, suppressCancelled);
                _customerManagement.updateEmailPreference(customerId, customer.getEmailPreferenceRaw());
                Console.WriteLine("Email Preference: " + customer.getEmailPreferenceRaw());

                // 🆕 Refresh session
                var updatedCustomer = _customerManagement.getCustomer(customerId);
                HttpContext.Session.SetString("emailPreference", updatedCustomer.getEmailPreferenceRaw() ?? "");
            }

            return RedirectToAction("CustomerDetails", "CustomerInput");
        }


        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("~/");
        }
    }
}