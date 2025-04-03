namespace CleanBrilliantCompany.Interfaces
{
    public interface IEmailService
    {
        void sendEmail(string toEmail, string subject, string body);
    }
}