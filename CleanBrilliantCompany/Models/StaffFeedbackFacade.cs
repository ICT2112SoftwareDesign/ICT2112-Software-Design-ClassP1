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

    public void AddFeedback(int staffId, string feedback)
    {
        feedbackSubmission.AddFeedback(staffId, feedback);
    }

    public void EditFeedback(int feedbackId, string feedback)
    {
        feedbackSubmission.EditFeedback(feedbackId, feedback);
    }
}