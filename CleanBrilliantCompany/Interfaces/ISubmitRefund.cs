namespace CleanBrilliantCompany.Interfaces
{
    public interface ISubmitRefund
    {
        bool submitRefund (int orderId, string refundReason, float refundAmount, Dictionary<int, int> refundedProducts);
    }
}