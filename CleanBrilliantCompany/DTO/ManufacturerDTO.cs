namespace CleanBrilliantCompany.DTO
{
    public class ManufacturerDTO
    {
        private int id; 
        private string name;
        private string address;
        private string email;
        public int ID {  get { return id; } set { id = value; } }
        public string Name { get { return name; }set { name = value; } } 
        public string Address { get { return address; } set { address = value; } }
        public string Email { get { return email; } set { email = value; } }

        public ManufacturerDTO(int id, string name, string address, string email) {
            this.id = id;
            this.name = name;   
            this.address = address;
            this.email = email;
        }
    }
}