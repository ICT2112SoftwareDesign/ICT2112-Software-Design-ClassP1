using CleanBrilliantCompany.Models.StaffAuth;
using Microsoft.AspNetCore.Mvc;


namespace CleanBrilliantCompany.Interfaces.StaffAuth
{
    public interface IStaffDatabase
    {
        // 🔹 Authentication
        bool VerifyStaffCredentials(string username, string password);
        bool StaffExists(int staffId);

        // 🔹 Account Management
        bool CreateStaff(string name, string contactNo, string address, string role, string email, string password);
        bool UpdateStaff(int staffId, string name, string contactNo, string address, string role, string email);
        bool DeleteStaff(int staffId);

        // 🔹 Data Retrieval
        int GetIdByUsername(string username);

        int GetIdByEmail(string email); // ✅ Add this line

        StaffRDM GetStaffDetails(int staffId);
        List<StaffRDM> GetAllStaff();

        // 🔹 Validation Checks
        bool StaffEmailExists(int staffId, string email);
        bool StaffUsernameExists(int staffId, string username);

        string GetStaffRole(int staffId);
    }
}
