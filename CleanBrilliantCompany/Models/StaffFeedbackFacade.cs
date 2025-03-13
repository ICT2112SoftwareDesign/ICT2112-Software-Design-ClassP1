namespace CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Interfaces;

// Facade class for Staff Feedback
public class StaffFeedbackFacade
{
    private readonly IFeedbackSubmission feedbackSubmission;

    public StaffFeedbackFacade(IFeedbackSubmission feedbackSubmission)
    {
        this.feedbackSubmission = feedbackSubmission;
    }

    public void AddFeedback(string feedback)
    {
        feedbackSubmission.AddFeedback(feedback);
    }

    public void EditFeedback(string feedback)
    {
        feedbackSubmission.EditFeedback(feedback);
    }
}