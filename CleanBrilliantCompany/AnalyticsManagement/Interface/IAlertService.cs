public interface IAlertService
{
    Alert GenerateBudgetAlert(string message);
    void SendAlert(Alert alert);
}