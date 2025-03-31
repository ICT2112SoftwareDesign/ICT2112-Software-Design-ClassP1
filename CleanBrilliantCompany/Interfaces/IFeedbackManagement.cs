namespace CleanBrilliantCompany.Interfaces
{
    public interface IFeedbackManagement
    {
        void UpdateManagerComment(int feedbackId, string managerComment);
        void UpdateFeedbackStatus(int feedbackId, string status);
        void DeleteFeedback(int feedbackId);
    }
}
