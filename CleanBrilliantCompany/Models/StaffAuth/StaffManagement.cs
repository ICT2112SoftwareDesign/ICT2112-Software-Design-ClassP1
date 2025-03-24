using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Interfaces.StaffAuth;


namespace CleanBrilliantCompany.Models.StaffAuth
{
    public class StaffManagement : IStaffDetails
    {
        private readonly IStaffDatabase _staffDatabase;

        // Constructor - Injects the staff database interface
        public StaffManagement(IStaffDatabase staffDatabase)
        {
            _staffDatabase = staffDatabase;
        }

        // 🔹 Get All Staff (Admin functionality - placeholder)
        public List<StaffRDM> GetAllStaff()
        {
            // Implementation: Fetch all staff from DB
            return new List<StaffRDM>();
        }

        // 🔹 Get Specific Staff Details
        //public StaffRDM GetStaff(int staffId)
        //{
        //    return _staffDatabase.GetStaffDetails(staffId);
        //}

        // 🔹 Create a New Staff Member
        public bool CreateStaff(string name, string contactNo, string address, string role, string email, string password)
        {
            return _staffDatabase.CreateStaff(name, contactNo, address, role, email, password);
        }

        // 🔹 Update Staff Details
        public bool UpdateStaff(int staffId, string name, string contactNo, string address, string role, string email)
        {
            return _staffDatabase.UpdateStaff(staffId, name, contactNo, address, role, email);
        }

        // 🔹 Delete Staff Member
        public bool DeleteStaff(int staffId)
        {
            return _staffDatabase.DeleteStaff(staffId);
        }

        // 🔹 Check if there are any pending Staff Queries (Placeholder)
        public bool CheckStaffQuery()
        {
            return false; // To be implemented later
        }

        public int GetIdByEmail(string email)
        {
            return _staffDatabase.GetIdByEmail(email);
        }

        public StaffRDM GetStaffDetails(int staffId)
        {
            return _staffDatabase.GetStaffDetails(staffId);
        }
    }
}

