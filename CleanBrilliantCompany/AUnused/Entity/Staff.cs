namespace CleanBrilliantCompany.Models.Entity
{
    public class Staff
    {

        public int StaffId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }

        public Staff(int staffId, string name, string username, string email, string contactNo, string address, string password)
        {
            StaffId = staffId;
            Name = name;
            Username = username;
            Email = email;
            ContactNo = contactNo;
            Address = address;
            Password = password;
        }

        public Staff() { }

    }
}
