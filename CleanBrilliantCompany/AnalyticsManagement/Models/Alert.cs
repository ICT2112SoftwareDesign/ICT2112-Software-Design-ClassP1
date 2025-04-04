public class Alert
{
    public string Type { get; set; } // e.g., "Warning", "Info", "Error"
    public string Message { get; set; }
    public DateTime Timestamp { get; set; }

    public Alert(string type, string message)
    {
        Type = type;
        Message = message;
        Timestamp = DateTime.UtcNow;
    }
}