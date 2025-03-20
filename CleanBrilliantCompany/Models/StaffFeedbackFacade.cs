namespace CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Interfaces;
using System.Collections.Generic;

// Facade class for Staff Feedback
public class StaffFeedbackFacade
{
    private readonly IFeedbackSubmission _feedbackSubmission;
    private readonly IFeedbackRetrieval _feedbackRetrieval; // Now retrieves full feedback objects

    public StaffFeedbackFacade(IFeedbackSubmission feedbackSubmission, IFeedbackRetrieval feedbackRetrieval)
    {
        _feedbackSubmission = feedbackSubmission;
        _feedbackRetrieval = feedbackRetrieval;
    }

    public void AddFeedback(int staffId, string feedback)
    {
        _feedbackSubmission.AddFeedback(staffId, feedback);
    }

    public void EditFeedback(int feedbackId, string feedback)
    {
        _feedbackSubmission.EditFeedback(feedbackId, feedback);
    }

    // ✅ Now correctly fetching all feedback details
    public List<FeedbackRDM> GetAllFeedback()
    {
        return _feedbackRetrieval.GetAllFeedback();
    }
}
