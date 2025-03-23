using Microsoft.AspNetCore.Mvc;

namespace CleanBrilliantCompany.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

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
