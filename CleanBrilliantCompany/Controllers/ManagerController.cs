using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Controllers
{
    //[Route("ManagerFeedback")]
    public class ManagerController : Controller
    {
        private readonly FeedbackRepository _repository;

        public ManagerController(FeedbackRepository repository)
        {
            _repository = repository;
        }

        // Display all feedback for the manager
        public IActionResult Index()
        {
            List<FeedbackRDM> feedbackList = _repository.GetAllFeedback();
            return View("ManagerFeedback", feedbackList);

        }

        // Save Manager Comment (Handles AJAX request)
        [HttpPost]
        public IActionResult SaveComment(int feedbackId, string managerComment)
        {
            if (string.IsNullOrWhiteSpace(managerComment))
            {
                return Json(new { success = false, message = "Comment cannot be empty!" });
            }

            _repository.UpdateManagerComment(feedbackId, managerComment);
            return Json(new { success = true, message = "Manager comment added successfully!" });
        }

        // Resolve Feedback
        [HttpGet] // Allow GET requests for ResolveFeedback
        public IActionResult ResolveFeedback(int feedbackId)
        {
            if (feedbackId <= 0)
            {
                TempData["ErrorMessage"] = "Invalid feedback ID!";
                return RedirectToAction("Index");
            }

            _repository.UpdateFeedbackStatus(feedbackId, "Resolved");
            TempData["SuccessMessage"] = "Feedback resolved successfully!";
            return RedirectToAction("Index");
        }


        // Delete Feedback
        public IActionResult DeleteFeedback(int feedbackId)
        {
            _repository.DeleteFeedback(feedbackId);
            TempData["SuccessMessage"] = "Feedback deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}
