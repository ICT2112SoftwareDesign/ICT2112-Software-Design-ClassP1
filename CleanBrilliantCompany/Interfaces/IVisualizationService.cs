public interface IVisualizationService
{
    object GenerateCostVisualization(CostDashboardRdm dashboard);
    object GenerateSupplierComparison(CostDashboardRdm dashboard);
}