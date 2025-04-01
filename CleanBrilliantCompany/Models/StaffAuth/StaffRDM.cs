using Microsoft.AspNetCore.Mvc;

namespace CleanBrilliantCompany.Models.StaffAuth
{
    public class StaffRDM
    {
        private int staffId;
        private string name;
        private string username;
        private string email;
        private string contactNo;
        private string address;
        private string password;
        private string department; // General Staff Only
        private string managementLevel; // Management Staff Only

        public int StaffId
        {
            get => staffId;
            set => staffId = value;
        }

        public string Name
        {
            get => name;
            set => name = value;
        }

        public string Username
        {
            get => username;
            set => username = value;
        }

        public string Email
        {
            get => email;
            set => email = value;
        }

        public string ContactNo
        {
            get => contactNo;
            set => contactNo = value;
        }

        public string Address
        {
            get => address;
            set => address = value;
        }

        public string Password
        {
            get => password;
            set => password = value;
        }

        public string Department
        {
            get => department;
            set => department = value;
        }

        public string ManagementLevel
        {
            get => managementLevel;
            set => managementLevel = value;
        }

        // Getters
        private int getStaffId() { return staffId; }
        private string getName() { return name; }
        private string getUsername() { return username; }
        private string getEmail() { return email; }
        private string getContactNo() { return contactNo; }
        private string getAddress() { return address; }
        private string getPassword() { return password; }
        private string getDepartment() { return department; } // Only for General Staff
        private string getManagementLevel() { return managementLevel; } // Only for Management Staff

        // Setters
        private void setStaffId(int id) { staffId = id; }
        private void setName(string name) { this.name = name; }
        private void setUsername(string username) { this.username = username; }
        private void setEmail(string email) { this.email = email; }
        private void setContactNo(string contactNo) { this.contactNo = contactNo; }
        private void setAddress(string address) { this.address = address; }
        private void setPassword(string password) { this.password = password; }
        private void setDepartment(string department) { this.department = department; } // Only for General Staff
        private void setManagementLevel(string level) { managementLevel = level; } // Only for Management Staff

        // Session Handling
        public T GetSession<T>(string propertyName)
        {
            switch (propertyName)
            {
                case "staffId": return (T)(object)getStaffId();
                case "name": return (T)(object)getName();
                case "username": return (T)(object)getUsername();
                case "email": return (T)(object)getEmail();
                case "contactNo": return (T)(object)getContactNo();
                case "address": return (T)(object)getAddress();
                case "password": return (T)(object)getPassword();
                case "department": return (T)(object)getDepartment();
                case "managementLevel": return (T)(object)getManagementLevel();
                default: throw new Exception("Unknown property");
            }
        }

        public void SetSession<T>(string propertyName, T value)
        {
            switch (propertyName)
            {
                case "staffId": setStaffId(Convert.ToInt32(value)); break;
                case "name": setName(value?.ToString()); break;
                case "username": setUsername(value?.ToString()); break;
                case "email": setEmail(value?.ToString()); break;
                case "contactNo": setContactNo(value?.ToString()); break;
                case "address": setAddress(value?.ToString()); break;
                case "password": setPassword(value?.ToString()); break;
                case "department": setDepartment(value?.ToString()); break;
                case "managementLevel": setManagementLevel(value?.ToString()); break;
                default: throw new Exception("Unknown property");
            }
        }

        // Sample Method: Create a new Staff (Stub, implementation needed)
        public bool CreateStaff(int staffId, string name, string username, string email, string contactNo, string address, string password)
        {
            // Implementation logic here (e.g., call StaffManagement to insert into DB)
            return false;
        }

        // Sample Method: Fetch staff details (Stub, implementation needed)
        public bool GetStaffDetails(int staffId, string name, string username, string email, string contactNo, string address, string department, string managementLevel)
        {
            // Implementation logic here (e.g., call StaffManagement to retrieve from DB)
            return false;
        }

        // Sample Method: Update Address (Stub, implementation needed)
        public bool UpdateStaffAddress(int staffId, string address)
        {
            // Implementation logic here
            return false;
        }

        // New Role property based on the department or management level
        public string Role
        {
            get
            {
                if (!string.IsNullOrEmpty(department))
                {
                    return "general"; // Staff is in General Staff
                }
                else if (!string.IsNullOrEmpty(managementLevel))
                {
                    return "management"; // Staff is in Management Staff
                }
                return "unknown"; // Unknown role
            }
        }
    }
}
