namespace CleanBrilliantCompany.DTO  // ✅ Correct way
{
    public class ProductManufacturerDTO
    {
        public int ManufacturerId { get; set; }
        public required string CompanyName { get; set; }
        public required string Address { get; set; }
        public required string Email { get; set; }
    }
}