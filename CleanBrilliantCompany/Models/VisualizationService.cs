public class VisualizationService : IVisualizationService{

    public object GenerateCostVisualization(CostDashboardRdm dashboard)
    {
       return dashboard.GetAllProductBatches()
            .Select(batch => new { 
                BatchCode = batch.BatchCode, 
                BatchPrice = batch.BatchPrice 
            })
            .ToList();
    }

    public object GenerateSupplierComparison(CostDashboardRdm dashboard)
    {
       return dashboard.GetAllManufacturers()
            .Select(manufacturer => new { 
                ManufacturerId = manufacturer.ManufacturerId, 
                ManufacturerName = manufacturer.CompanyName, 
                ManufacturerAddress = manufacturer.Address,
                ManufactuereEmail = manufacturer.Email,
                BatchCount = dashboard.GetBatchesByManufacturer(manufacturer.ManufacturerId).Count
            })
            .ToList(); 
    }
}