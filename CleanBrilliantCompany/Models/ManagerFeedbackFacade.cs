namespace CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Interfaces;
// Facade class for Managing Feedback
public class ManageFeedbackFacade
{
    private readonly IFeedbackRetrieval feedbackRetrieval;
    private readonly IFeedbackManagement feedbackManagement;

    public ManageFeedbackFacade(IFeedbackRetrieval feedbackRetrieval, IFeedbackManagement feedbackManagement)
    {
        this.feedbackRetrieval = feedbackRetrieval;
        this.feedbackManagement = feedbackManagement;
    }

    public string GetFeedbackById(int feedbackId)
    {
        return feedbackRetrieval.GetFeedbackById(feedbackId);
    }

    public List<string> GetAllFeedback()
    {
        return feedbackRetrieval.GetAllFeedback();
    }

    public void DeleteFeedback()
    {
        feedbackManagement.DeleteFeedback();
    }

    public bool ResolveFeedback(int feedbackId)
    {
        return feedbackManagement.ResolveFeedback(feedbackId);
    }
}