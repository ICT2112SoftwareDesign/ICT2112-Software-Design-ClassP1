public abstract class Dashboard
{
    private string _name = string.Empty;
    private DateTime _requestedStartDate;
    private DateTime _requestedEndDate;
    private DateTime? _generatedDate;
    private int _validityDuration;

    protected int type { get; set; }

    // 🔹 dashboardId should be set from the database, so allow protected set
    public int dashboardId { get; protected set; }

    // 🔹 Properties with validation
    public string name
    {
        get => _name;
        protected set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Name cannot be empty.");
            _name = value;
        }
    }

    public DateTime requestedStartDate
    {
        get => _requestedStartDate;
        protected set
        {
            if (value > DateTime.Now)
                throw new ArgumentException("Requested start date cannot be in the future.");
            _requestedStartDate = value;
        }
    }

    public DateTime requestedEndDate
    {
        get => _requestedEndDate;
        protected set
        {
            if (value < _requestedStartDate)
                throw new ArgumentException("Requested end date cannot be earlier than start date.");
            _requestedEndDate = value;
        }
    }

    public int validityDuration
    {
        get => _validityDuration;
        protected set
        {
            if (value < 0)
                throw new ArgumentException("Validity duration cannot be negative.");
            _validityDuration = value;
        }
    }

    public DateTime? generatedDate
    {
        get => _generatedDate;
        protected set
        {
            if (value.HasValue && value < _requestedStartDate)
                throw new ArgumentException("Generated date cannot be earlier than start date.");
            _generatedDate = value;
        }
    }

    // 🔹 Constructor for retrieving from database (includes dashboardId)
    protected Dashboard(int dashboardId, string name, DateTime requestedStartDate, DateTime requestedEndDate, int validityDuration, int type, DateTime? generatedDate = null)
    {
        this.dashboardId = dashboardId;
        this.name = name;
        this.requestedStartDate = requestedStartDate;
        this.requestedEndDate = requestedEndDate;
        this.validityDuration = validityDuration;
        this.generatedDate = generatedDate ?? DateTime.Now;
        this.type = type;
    }
}
