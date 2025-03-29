using CleanBrilliantCompany.DTO;
using CleanBrilliantCompany.Interface;

public class CostMapper
{

    private readonly ApplicationDbContext _db;

    private readonly IItem _itemService;
    private readonly IBatch _batchService;
    private readonly IManufacturer _manufacturerService;


    // public CostMapper(ApplicationDbContext dbContext)
    // {
    //  _db = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    // }

      public CostMapper(IItem itemService, IBatch batchService, IManufacturer manufacturerService,ApplicationDbContext dbContext)
    {
        _itemService = itemService;
        _batchService = batchService;
        _manufacturerService = manufacturerService;
        _db = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }



    // ----------------------------------------------------------------
    // Fetching via Retrieval Service from External Interfaces

     // 🔹 Fetch Product Manufacturers DTOs (maps ManufacturerTable → ProductManufacturerDTO)
    public List<ProductManufacturerDTO> GetAllManufacturers()
    {
        var manufacturers = _manufacturerService.GetAllManufacturers();

        return manufacturers.Select(m => new ProductManufacturerDTO
        {
            ManufacturerId = m.ManufacturerId,
            CompanyName = m.CompanyName,
            Address = m.ManufacturerAddress,
            Email = m.Email
        }).ToList();
    }

    // 🔹 Fetch Product Batches DTOs (maps ProductBatchTable → ProductBatchDTO)
    public List<ProductBatchDTO> GetAllProductBatches()
    {
        var productBatches = _batchService.GetAllProductBatch();
        var products = _db.Product.Select(p => new { p.ProductId, p.ManufacturerId }).ToList();

        return productBatches.Select(batch =>
        {
            var product = products.FirstOrDefault(p => p.ProductId == batch.ProductId);
            return new ProductBatchDTO
            {
                BatchCode = batch.BatchCode,
                ProductId = batch.ProductId,
                ExpiryDate = batch.ExpiryDate,
                ReceiveDate = batch.ReceiveDate,
                ManufactureDate = batch.ManufactureDate,
                BatchQuantity = batch.Quantity,
                BatchPrice = Convert.ToDecimal(batch.BatchCost),
                ManufacturerId = product?.ManufacturerId ?? -1
            };
        }).ToList();
    }

    // 🔹 Fetch Items DTOs (maps ItemTable → ItemDTO)
    public List<ItemDTO> GetAllItems()
    {
        var items = _itemService.getItems();

        return items.Select(i => new ItemDTO
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
        }).ToList();
    }


    // ---------------------------------------------------------------
    // 🔹 Fetch Dashboard DTOs

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