using CleanBrilliantCompany.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CleanBrilliantCompany.Models
{
    public class StaffAuthentication : IStaffAuthentication
    {
        private readonly IStaffDatabase _staffDatabase;

        public StaffAuthentication(IStaffDatabase staffDatabase)
        {
            _staffDatabase = staffDatabase;
        }

        // ✅ Authenticate Staff Login (Checks email & password)
        public bool Login(string email, string password)
        {
            return _staffDatabase.VerifyStaffCredentials(email, password);
        }

        // ✅ Logout Staff (Clears session, implementation in controller)
        public void Logout(HttpContext httpContext)
        {
            httpContext.Session.Clear();
        }

        // ✅ Check if Staff is Authenticated (Session-based check)
        public bool IsAuthenticated(int staffId)
        {
            return _staffDatabase.StaffExists(staffId);
        }

        // ✅ Get Staff ID by Email
        public int GetIdByEmail(string email)
        {
            return _staffDatabase.GetIdByEmail(email);
        }

        public bool AuthenticateStaff(string email, string password)
        {
            return _staffDatabase.VerifyStaffCredentials(email, password);
        }
    }
}
