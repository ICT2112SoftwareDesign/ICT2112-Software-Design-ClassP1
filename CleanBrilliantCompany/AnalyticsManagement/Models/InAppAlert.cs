public class InAppAlert : IAlertService
{
    public Alert GenerateBudgetAlert(string message)
    {
        return new Alert("Warning", message);
    }

    public void SendAlert(Alert alert)
    {
        Console.WriteLine($"📢 In-App Notification: {alert.Message} at {alert.Timestamp}");
    }
}