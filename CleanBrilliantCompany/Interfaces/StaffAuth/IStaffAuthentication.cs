using Microsoft.AspNetCore.Mvc;

namespace CleanBrilliantCompany.Interfaces.StaffAuth
{
    public interface IStaffAuthentication
    {
        bool Login(string email, string password);
        void Logout(HttpContext httpContext);
        bool IsAuthenticated(int staffId);

        int GetIdByEmail(string email);

        string GetStaffRole(int staffId);
    }
}
