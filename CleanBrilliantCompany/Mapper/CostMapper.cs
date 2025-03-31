using CleanBrilliantCompany.DTO;
using CleanBrilliantCompany.Interface;
using CleanBrilliantCompany.DatabaseEntities;
using CleanBrilliantCompany.Models.Control;  // for ProductControl

public class CostMapper
{


    private readonly ProductControl _productControl;

    private readonly ItemControl _itemControl;
    private readonly ApplicationDbContext _db;

    private readonly CostIItem _itemService;
    private readonly CostIBatch _batchService;
    private readonly CostIManufacturer _manufacturerService;


    // public CostMapper(ApplicationDbContext dbContext)
    // {
    //  _db = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    // }

      public CostMapper(CostIItem itemService, CostIBatch batchService, CostIManufacturer manufacturerService,ApplicationDbContext dbContext,ProductControl productControl,ItemControl itemControl)
    {
        _itemService = itemService;
        _batchService = batchService;
        _manufacturerService = manufacturerService;
        _db = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _productControl = productControl;  
        _itemControl = itemControl;  
    }



    // ----------------------------------------------------------------
    // Fetching via Retrieval Service from External Interfaces

     // 🔹 Fetch Product Manufacturers DTOs (maps ManufacturerTable → ProductManufacturerDTO)
    // public List<ProductManufacturerDTO> GetAllManufacturers()
    // {
    //     var manufacturers = _manufacturerService.GetAllManufacturers();

    //     return manufacturers.Select(m => new ProductManufacturerDTO
    //     {
    //         ManufacturerId = m.ManufacturerId,
    //         CompanyName = m.CompanyName,
    //         Address = m.ManufacturerAddress,
    //         Email = m.Email
    //     }).ToList();
    // }


    // Map the domain model to a DTO using ProductControl's method
   
   public List<ProductManufacturerDTO> GetAllManufacturers()
    {
        // Retrieve the list of ProductManufacturer objects from ProductControl.
        var manufacturers = _productControl.getAllProductManufacturer();

        // Map each ProductManufacturer to a DTO using its public method to get the values.
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


    // // 🔹 Fetch Product Batches DTOs (maps ProductBatchTable → ProductBatchDTO)
    // public List<ProductBatchDTO> GetAllProductBatches()
    // {
    //     var productBatches = _batchService.GetAllProductBatch();
    //     var products = _db.Product.Select(p => new { p.productId, p.manufacturerId }).ToList();

    //     return productBatches.Select(batch =>
    //     {
    //         var product = products.FirstOrDefault(p => p.productId == batch.ProductId);
    //         return new ProductBatchDTO
    //         {
    //             BatchCode = batch.BatchCode,
    //             ProductId = batch.ProductId,
    //             ExpiryDate = batch.ExpiryDate,
    //             ReceiveDate = batch.ReceiveDate,
    //             ManufactureDate = batch.ManufactureDate,
    //             BatchQuantity = batch.Quantity,
    //             BatchPrice = Convert.ToDecimal(batch.BatchCost),
    //             ManufacturerId = product?.manufacturerId ?? -1
    //         };
    //     }).ToList();
    // }
    
    public List<ProductBatchDTO> GetAllProductBatches()
    {
        // Retrieve a list of domain model ProductBatch objects via ProductControl
        var batches = _productControl.getAllProductBatch();

        // Retrieve all products (domain model) to get manufacturer info.
        var products = _productControl.getAllProducts();

        // Build a dictionary mapping ProductId to ManufacturerId using each product's public method
        var productManufacturerMap = products
            .Select(p => p.retrieveProductInfo())
            .Select(info => new
            {
                ProductId = Convert.ToInt32(info["ProductId"]),
                ManufacturerId = Convert.ToInt32(info["ManufacturerId"])
            })
            .ToDictionary(x => x.ProductId, x => x.ManufacturerId);

        // Map each ProductBatch to a DTO, and set ManufacturerId by looking it up in the dictionary.
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

    // 🔹 Fetch Items DTOs (maps ItemTable → ItemDTO)
    // public List<ItemDTO> GetAllItems()
    // {
    //     var items = _itemService.getItems();

    //     return items.Select(i => new ItemDTO
    //     {
    //         ItemId = i.ItemId,
    //         ProductId = i.ProductId,
    //         SalePrice = (decimal)i.SalePrice,
    //         BatchCode = i.BatchCode,
    //         WarehouseId = i.WarehouseId,
    //         ItemStatus = i.ItemStatus,
    //         ReservationId = i.ReservationId,
    //         OrderId = i.OrderId,
    //         TransferId = i.TransferId,
    //         ReturnId = i.ReturnId
    //     }).ToList();
    // }

    public List<ItemDTO> GetAllItems()
    {
        // Synchronously get the items (using .Result here for demonstration; consider using async/await)
        var items = _itemControl.getItems().Result;

        // Map each Item to an ItemDTO using its public retrieveItemInfo() method.
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