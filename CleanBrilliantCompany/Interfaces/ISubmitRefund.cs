namespace CleanBrilliantCompany.Interfaces
{
    public interface ISubmitRefund
    {
        bool submitRefund (int orderId, int customerId, string status, string refundReason, string images, string videos);
    }
}