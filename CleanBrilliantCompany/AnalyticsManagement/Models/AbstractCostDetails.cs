public abstract class AbstractCostDetails
{
    protected readonly IAlertService alertService;
    protected List<Alert> alerts = new List<Alert>();  

    // ✅ Constructor injects alert service
    protected AbstractCostDetails(IAlertService alertService)
    {
        this.alertService = alertService;
    }

    // ✅ Abstract methods that MUST be implemented by subclasses
    public abstract object CheckBatchPerformance();

    public abstract object CheckProductPerformance(int productId);
    // public abstract float CalculateSavings(int manufacturerId);
    // public abstract int GetCheapestManufacturer(int productId);
    public abstract object GetBatchBudgetSummary();  

    // ✅ Returns a list of alerts for the front end
    public virtual List<Alert> GetAlerts()
    {
        return alerts;
    }
}