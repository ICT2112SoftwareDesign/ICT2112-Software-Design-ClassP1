using CleanBrilliantCompany.DTO;
using CleanBrilliantCompany.Interface;
using CleanBrilliantCompany.DatabaseEntities;

public class CostMapper
{
    private readonly iProduct _iProduct;
    private readonly iManufacturer _iManufacturer;
    private readonly iBatch _iBatch;
    private readonly iItem _iItem;
    private readonly ApplicationDbContext _db;

    public CostMapper(ApplicationDbContext dbContext, iProduct iProduct, iManufacturer iManufacturer, iBatch iBatch, iItem iItem)
    {
        _db = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _iProduct = iProduct;
        _iManufacturer = iManufacturer;
        _iBatch = iBatch;
        _iItem = iItem;
    }

    public List<ProductManufacturerDTO> GetAllManufacturers()
    {
        var manufacturers = _iManufacturer.GetAllProductManufacturer();

        return manufacturers.Select(m =>
        {
            var info = m.retrieveProductManufacturerInfo();
            return new ProductManufacturerDTO
            {
                ManufacturerId = Convert.ToInt32(info["ManufacturerId"]),
                CompanyName = info["CompanyName"]?.ToString() ?? string.Empty,
                Address = info["ManufacturerAddress"]?.ToString() ?? string.Empty,
                Email = info["Email"]?.ToString() ?? string.Empty
            };
        }).ToList();
    }

    public List<ProductBatchDTO> GetAllProductBatches()
    {
        var batches = _iBatch.getAllProductBatch();
        var products = _iProduct.getAllProducts();

        var productManufacturerMap = products
            .Select(p => p.retrieveProductInfo())
            .Select(info => new
            {
                ProductId = Convert.ToInt32(info["ProductId"]),
                ManufacturerId = Convert.ToInt32(info["ManufacturerId"])
            })
            .ToDictionary(x => x.ProductId, x => x.ManufacturerId);

        return batches.Select(pb =>
        {
            var info = pb.retrieveProductBatchInfo();
            int prodId = Convert.ToInt32(info["ProductId"]);
            int manufacturerId = productManufacturerMap.ContainsKey(prodId) ? productManufacturerMap[prodId] : -1;

            return new ProductBatchDTO
            {
                BatchCode = Convert.ToInt32(info["BatchCode"]),
                ProductId = prodId,
                ExpiryDate = Convert.ToDateTime(info["ExpiryDate"]),
                ReceiveDate = Convert.ToDateTime(info["ReceiveDate"]),
                ManufactureDate = Convert.ToDateTime(info["ManufactureDate"]),
                BatchQuantity = Convert.ToInt32(info["Quantity"]),
                BatchPrice = Convert.ToDecimal(info["BatchCost"]),
                ManufacturerId = manufacturerId
            };
        }).ToList();
    }

    public List<ItemDTO> GetAllItems()
    {
        var items = _iItem.getItems().Result;

        return items.Select(i =>
        {
            var info = i.retrieveItemInfo();
            return new ItemDTO
            {
                ItemId = Convert.ToInt32(info["ItemId"]),
                ProductId = Convert.ToInt32(info["ProductId"]),
                SalePrice = Convert.ToDecimal(info["SalePrice"]),
                BatchCode = Convert.ToInt32(info["BatchCode"]),
                WarehouseId = Convert.ToInt32(info["WarehouseId"]),
                ItemStatus = info["ItemStatus"]?.ToString() ?? string.Empty,
                ReservationId = info["ReservationId"] != null ? Convert.ToInt32(info["ReservationId"]) : (int?)null,
                OrderId = info["OrderId"] != null ? Convert.ToInt32(info["OrderId"]) : (int?)null,
                TransferId = info["TransferId"] != null ? Convert.ToInt32(info["TransferId"]) : (int?)null,
                ReturnId = info["ReturnId"] != null ? Convert.ToInt32(info["ReturnId"]) : (int?)null,
            };
        }).ToList();
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