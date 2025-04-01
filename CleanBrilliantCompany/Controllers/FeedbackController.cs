using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Interfaces;

public class FeedbackController : Controller
{
    private readonly StaffFeedbackFacade _staffFeedbackFacade;
    private readonly IFeedbackRetrieval _feedbackRetrieval;
    private readonly IHttpContextAccessor _httpContextAccessor;

    // Constructor to inject services
    public FeedbackController(StaffFeedbackFacade staffFeedbackFacade, IFeedbackRetrieval feedbackRetrieval, IHttpContextAccessor httpContextAccessor)
    {
        _staffFeedbackFacade = staffFeedbackFacade;
        _feedbackRetrieval = feedbackRetrieval;
        _httpContextAccessor = httpContextAccessor;
    }

    // GET: /Feedback/Index
    public IActionResult Index()
    {
        // Check if the logged-in user is GeneralStaff
        string role = _httpContextAccessor.HttpContext.Session.GetString("StaffRole");

        if (role != "general")
        {
            TempData["ErrorMessage"] = "You must be a GeneralStaff to access the feedback page.";
            return RedirectToAction("Index", "Staff"); // Redirect to the Staff's dashboard if not GeneralStaff
        }

        // Retrieve all feedback from the database
        var feedbackList = _staffFeedbackFacade.GetAllFeedback(); // Now correctly returning List<FeedbackRDM>
        return View("feedback", feedbackList); // Pass the list to the view
    }

    [HttpPost]
    public IActionResult SubmitFeedback(int? staffId, string feedback)
    {
        try
        {
            // If staffId is null or less than 1, default it to 1
            if (staffId == null || staffId < 1)
            {
                staffId = 1;
            }

            _staffFeedbackFacade.AddFeedback(staffId.Value, feedback);
            TempData["SuccessMessage"] = "Feedback submitted successfully.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        // Redirect back to the correct action (e.g., Staff's Feedback page)
        return RedirectToAction("Feedback", "Staff"); // Ensure you redirect to StaffController's Feedback page
    }

    // GET: Load the feedback edit page
    [HttpGet]
    public IActionResult EditFeedback(int feedbackId)
    {
        var feedback = _feedbackRetrieval.GetFeedbackById(feedbackId);

        if (feedback == null)
        {
            TempData["ErrorMessage"] = "Feedback not found.";
            return RedirectToAction("Index");
        }

        return View("EditFeedback", feedback);
    }

    // POST: Save the edited feedback
    [HttpPost]
    public IActionResult EditFeedback(int feedbackId, string feedback)
    {
        try
        {
            _staffFeedbackFacade.EditFeedback(feedbackId, feedback);
            TempData["SuccessMessage"] = "Feedback updated successfully.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }
        return RedirectToAction("Index");
    }
}

