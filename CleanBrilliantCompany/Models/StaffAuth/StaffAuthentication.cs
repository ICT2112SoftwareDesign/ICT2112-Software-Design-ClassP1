using CleanBrilliantCompany.Interfaces.StaffAuth;
using Microsoft.AspNetCore.Mvc;

namespace CleanBrilliantCompany.Models.StaffAuth
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

        public string GetStaffRole(int staffId)
        {
            // Fetch the staff details
            var staffDetails = _staffDatabase.GetStaffDetails(staffId);

            // Access the computed Role property
            return staffDetails?.Role ?? "unknown"; // Return the role or default to "unknown"
        }
    }
}
