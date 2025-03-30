namespace CleanBrilliantCompany.DTO
{
    public class ProductDTO
    {
        private int id;
        private string name;
        private string description;
        public int ID {  get { return id; } set { id = value; } }
        public string Name { get { return name; }set { name = value; } } 
        public string Description { get { return description; } set { description = value; } }

        public ProductDTO(int id, string name, string description) {
            this.id = id;
            this.name = name;   
            this.description = description;
        }
    }
}
