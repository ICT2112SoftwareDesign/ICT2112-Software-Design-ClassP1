public abstract class Dashboard
{

    private string _name = string.Empty;
    private DateTime _requestedStartDate;
    private DateTime _requestedEndDate;
    private DateTime _generatedDate;
    private int _validityDuration;

    //protected int Type { get; set; }
    public int Type { get; set; }

    // 🔹 DashboardId should be set from the database, so allow protected set
    public int DashboardId { get; protected set; }

    // 🔹 Properties with validation
    public string Name
    {
        get => _name;
        protected set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Name cannot be empty.");
            _name = value;
        }
    }

    public DateTime RequestedStartDate
    {
        get => _requestedStartDate;
        protected set
        {
            if (value > DateTime.Now)
                throw new ArgumentException("Requested start date cannot be in the future.");
            _requestedStartDate = value;
        }
    }

    public DateTime RequestedEndDate
    {
        get => _requestedEndDate;
        protected set
        {
            if (value < _requestedStartDate)
                throw new ArgumentException("Requested end date cannot be earlier than start date.");
            _requestedEndDate = value;
        }
    }

    public int ValidityDuration
    {
        get => _validityDuration;
        protected set
        {
            if (value < 0)
                throw new ArgumentException("Validity duration cannot be negative.");
            _validityDuration = value;
        }
    }

    public DateTime GeneratedDate
    {
        get => _generatedDate;
        protected set
        {
            //if (value.HasValue && value < _requestedStartDate)
            //    throw new ArgumentException("Generated date cannot be earlier than start date.");
            //_generatedDate = value;

            if (value < _requestedStartDate)
                throw new ArgumentException("Generated date cannot be earlier than start date.");
            _generatedDate = value;
        }
    }

    // 🔹 Constructor for Retrieving from Database (Includes DashboardId)
    protected Dashboard(int dashboardId, string name, DateTime requestedStartDate, DateTime requestedEndDate, int validityDuration, int type, DateTime generatedDate)
    {
        DashboardId = dashboardId;
        Name = name;
        RequestedStartDate = requestedStartDate;
        RequestedEndDate = requestedEndDate;
        ValidityDuration = validityDuration;
        GeneratedDate = generatedDate;
        Type = type;
    }

    // 🔹 Constructor for Creating a New Dashboard (Without ID, Assigned Later)
    protected Dashboard(string name, DateTime requestedStartDate, DateTime requestedEndDate, int validityDuration, int type)
    {
        DashboardId = 0;
        Name = name;
        RequestedStartDate = requestedStartDate;
        RequestedEndDate = requestedEndDate;
        ValidityDuration = validityDuration;
        GeneratedDate = DateTime.Now;
        Type = type;
    }

    protected Dashboard(string name, DateTime requestedStartDate, DateTime requestedEndDate, int validityDuration)
    {
        Name = name;
        RequestedStartDate = requestedStartDate;
        RequestedEndDate = requestedEndDate;
        ValidityDuration = validityDuration;
    }
}