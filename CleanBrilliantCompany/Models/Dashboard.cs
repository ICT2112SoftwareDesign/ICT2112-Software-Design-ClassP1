public abstract class Dashboard {
    // your priv fields 
    private int _dashboardId;
    private string _name;
    private DateTime _requestedStartDate;
    private DateTime _requestedEndDate;
    private DateTime? _generatedDate;
    private int _validityDuration;

    protected Dashboard(){}

    protected Dashboard(string name, DateTime requestedStartDate, DateTime requestedEndDate, int validityDuration)
    {
        SetName(name);
        SetRequestedStartDate(requestedStartDate);
        SetRequestedEndDate(requestedEndDate);
        SetValidityDuration(validityDuration);
        _generatedDate = null;
    }


    // Private Getters and Setters - Used Internally Only
    private int GetDashboardId() => _dashboardId;
    private void SetDashboardId(int dashboardId) => _dashboardId = dashboardId;

    private string GetName() => _name;
    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.");
        _name = name;
    }

    private DateTime GetRequestedStartDate() => _requestedStartDate;
    private void SetRequestedStartDate(DateTime requestedStartDate)
    {
        if (requestedStartDate > DateTime.Now)
            throw new ArgumentException("Requested start date cannot be in the future.");
        _requestedStartDate = requestedStartDate;
    }

    private DateTime GetRequestedEndDate() => _requestedEndDate;
    private void SetRequestedEndDate(DateTime requestedEndDate)
    {
        if (requestedEndDate < _requestedStartDate)
            throw new ArgumentException("Requested end date cannot be earlier than the start date.");
        _requestedEndDate = requestedEndDate;
    }

    private DateTime? GetGeneratedDate() => _generatedDate;
    private void SetGeneratedDate(DateTime generatedDate)
    {
        if (generatedDate < _requestedStartDate)
            throw new ArgumentException("Generated date cannot be earlier than the start date.");
        _generatedDate = generatedDate;
    }

    private int GetValidityDuration() => _validityDuration;
    private void SetValidityDuration(int validityDuration)
    {
        if (validityDuration < 0)
            throw new ArgumentException("Validity duration cannot be negative.");
        _validityDuration = validityDuration;
    }

    // Public Methods to Access Data - Renamed to be more descriptive
    public int RetrieveDashboardId() => GetDashboardId();
    public string RetrieveName() => GetName();
    public DateTime RetrieveRequestedStartDate() => GetRequestedStartDate();
    public DateTime RetrieveRequestedEndDate() => GetRequestedEndDate();
    public DateTime? RetrieveGeneratedDate() => GetGeneratedDate();
    public int RetrieveValidityDuration() => GetValidityDuration();

    //! I made this abstract so the children must implement this
    public abstract Dashboard CreateNewDashboard(string name, DateTime requestedStartDate, DateTime requestedEndDate, int validityDuration)
    

    // Public Method for Updating a Dashboard
    public void UpdateDashboard(string name, DateTime requestedStartDate, DateTime requestedEndDate, int validityDuration)
    {
        SetName(name);
        SetRequestedStartDate(requestedStartDate);
        SetRequestedEndDate(requestedEndDate);
        SetValidityDuration(validityDuration);
    }

}