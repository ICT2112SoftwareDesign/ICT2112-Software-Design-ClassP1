using CleanBrilliantCompany.DTO;

public class CostMapper
{

    private readonly ApplicationDbContext _db;

    public CostMapper(ApplicationDbContext dbContext)
    {
        _db = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
}

    // 🔹 Fetch Product Manufacturers DTOs
    public List<ProductManufacturerDTO> GetAllManufacturers()
    {
        return _db.Manufacturers
            .Select(m => new ProductManufacturerDTO
            {
                ManufacturerId = m.ManufacturerId,
                CompanyName = m.CompanyName,
                Address = m.ManufacturerAddress,
                Email = m.Email
            })
            .ToList();
    }

    // 🔹 Fetch Product Batches DTOs
    public List<ProductBatchDTO> GetAllProductBatches()
    {
        var productBatches = _db.ProductBatch.ToList();    
        var products = _db.Product.Select(p => new { p.ProductId, p.ManufacturerId }).ToList();  // Only fetch ProductId and ManufacturerId for the join

        var enrichedBatches = productBatches.Select(batch =>
        {
            var product = products.FirstOrDefault(p => p.ProductId == batch.ProductId);

            var productBatchDTO = new ProductBatchDTO
            {
                BatchCode = batch.BatchCode,
                ProductId = batch.ProductId,
                ExpiryDate = batch.ExpiryDate,
                ReceiveDate = batch.ReceiveDate,
                ManufactureDate = batch.ManufactureDate,
                BatchQuantity = batch.Quantity,
                BatchPrice = Convert.ToDecimal(batch.BatchCost),  // Explicitly convert BatchCost to Decimal
                ManufacturerId = product?.ManufacturerId ?? -1 // If no product found, set ManufacturerId to -1
            };

            return productBatchDTO;
        }).ToList();

        return enrichedBatches;
    }
  public List<ItemDTO> GetAllItems()
    {
        return _db.Items
            .Select(i => new ItemDTO
            {
                ItemId = i.ItemId,
                ProductId = i.ProductId,
                SalePrice = (decimal)i.SalePrice,
                BatchCode = i.BatchCode,
                WarehouseId = i.WarehouseId,
                ItemStatus = i.ItemStatus,
                ReservationId = i.ReservationId,
                OrderId = i.OrderId,
                TransferId = i.TransferId,
                ReturnId = i.ReturnId
            })
            .ToList();
    }

    public DashboardDTO ToDTO(DashboardTable table)
    {
        return new DashboardDTO
        {
            DashboardId = table.DashboardId,
            Name = table.Name,
            RequestedStartDate = table.RequestedStartDate,
            RequestedEndDate = table.RequestedEndDate,
            GeneratedDate = table.GeneratedDate,
            ValidityDuration = table.ValidityDuration,
            Type = table.TypeId
        };
    }

    public DashboardTable ToEntity(DashboardDTO dto)
    {
        return new DashboardTable
        {
            Name = dto.Name,
            RequestedStartDate = dto.RequestedStartDate,
            RequestedEndDate = dto.RequestedEndDate,
            GeneratedDate = dto.GeneratedDate ?? DateTime.Now,
            ValidityDuration = dto.ValidityDuration,
            TypeId = dto.Type
        };
    }


}