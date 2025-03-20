using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Models
{
    public class ManageFeedbackFacade
    {
        private readonly IFeedbackRetrieval _feedbackRetrieval;
        private readonly IFeedbackManagement _feedbackManagement;

        public ManageFeedbackFacade(IFeedbackRetrieval feedbackRetrieval, IFeedbackManagement feedbackManagement)
        {
            _feedbackRetrieval = feedbackRetrieval;
            _feedbackManagement = feedbackManagement;
        }

        public List<FeedbackRDM> GetAllFeedback()
        {
            return _feedbackRetrieval.GetAllFeedback();
        }

        public void UpdateManagerComment(int feedbackId, string managerComment)
        {
            _feedbackManagement.UpdateManagerComment(feedbackId, managerComment);
        }

        public void UpdateFeedbackStatus(int feedbackId, string status)
        {
            _feedbackManagement.UpdateFeedbackStatus(feedbackId, status);
        }

        public void DeleteFeedback(int feedbackId)
        {
            _feedbackManagement.DeleteFeedback(feedbackId);
        }
    }
}
