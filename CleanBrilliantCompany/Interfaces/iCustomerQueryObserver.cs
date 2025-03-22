namespace CleanBrilliantCompany.Interfaces
{
    public interface ICustomerQueryObserver
    {
        void onAuthenticationAttempt(string email);
        void onAuthenticationSuccess(string email, int customerId);
        void onAuthenticationFailure(string email, string reason);
        void onCustomerRegistration(string username, string email);
    }
}