 namespace CleanBrilliantCompany.Interfaces
{
    public interface ICustomerQueryObserver
    {
        void onAuthenticationAttempt(string email);
        void onAuthenticationSuccess(string email, int customerId);
        void onAuthenticationFailure(string email, string reason);
        void onCustomerRegistration(string username, string email);
        void onUpdateDetailsSuccess(int customerId, string username, string email, string address);
        void onUpdatedDetailsFailure(int customerId, string username, string email, string address, string reason);
        void onUpdatePasswordSuccess(int customerId, string password);
        void onUpdatePasswordFailure(int customerId, string password, string reason);
    }
}