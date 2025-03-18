using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CleanBrilliantCompany.Models.Control;
using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly CustomerManagement _customerManagement;
        private readonly CartManagement _cartManagement;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationController(CustomerManagement customerManagement, IHttpContextAccessor httpContextAccessor)
        {
            _customerManagement = customerManagement;
            _cartManagement = cartManagement;
            _httpContextAccessor = httpContextAccessor;
        }

        // Get Customer Session From Here (Hopefully it works)
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
           // Get Cart Details
        public List<CartItem> GetCartDetails()
        {
            return _cartManagement.ViewCart().Select(item => new CartItem
            {
                ProductImage = _cartManagement.GetProductImage(item.Key),
                ProductName = _cartManagement.GetProductName(item.Key),
                Quantity = item.Value,
                Price = _cartManagement.GetProductPrice(item.Key)
            }).ToList();
        }
    }
}
