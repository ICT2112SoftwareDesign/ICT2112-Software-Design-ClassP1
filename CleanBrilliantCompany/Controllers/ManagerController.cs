using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Controllers
{
    [Route("Manager")]
    public class ManagerFeedbackController : Controller
    {
        private readonly ManageFeedbackFacade _manageFeedbackFacade;

        public ManagerFeedbackController(ManageFeedbackFacade manageFeedbackFacade)
        {
            _manageFeedbackFacade = manageFeedbackFacade;
        }

        [HttpGet]
        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            var feedbackList = _manageFeedbackFacade.GetAllFeedback();
            return View("ManagerFeedback", feedbackList);
        }

        [HttpPost]
        [Route("SaveComment")]
        public IActionResult SaveComment(int feedbackId, string managerComment)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(managerComment))
                {
                    TempData["ErrorMessage"] = "Manager comment cannot be empty.";
                    return RedirectToAction("Index");
                }

                _manageFeedbackFacade.UpdateManagerComment(feedbackId, managerComment);
                TempData["SuccessMessage"] = "Comment saved successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("ResolveFeedback")]
        public IActionResult ResolveFeedback(int feedbackId)
        {
            try
            {
                _manageFeedbackFacade.UpdateFeedbackStatus(feedbackId, "Resolved");
                TempData["SuccessMessage"] = "Feedback marked as Resolved.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("DeleteFeedback")]
        public IActionResult DeleteFeedback(int feedbackId)
        {
            try
            {
                _manageFeedbackFacade.DeleteFeedback(feedbackId);
                TempData["SuccessMessage"] = "Feedback deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("Index");
        }
    }
}
