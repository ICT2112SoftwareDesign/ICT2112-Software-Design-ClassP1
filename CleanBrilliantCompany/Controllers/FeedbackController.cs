using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly StaffFeedbackFacade _staffFeedbackFacade;
        private readonly IFeedbackRetrieval _feedbackRetrieval;

        public FeedbackController(StaffFeedbackFacade staffFeedbackFacade, IFeedbackRetrieval feedbackRetrieval)
        {
            _staffFeedbackFacade = staffFeedbackFacade;
            _feedbackRetrieval = feedbackRetrieval;
        }

        public IActionResult Index()
        {
            var feedbackList = _staffFeedbackFacade.GetAllFeedback(); // Now correctly returning List<FeedbackRDM>
            return View("Feedback", feedbackList); // ✅ Pass the list to the view
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
            return RedirectToAction("Index");
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

            // ✅ Ensure the correct model type is passed
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
}
