using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly FeedbackRepository _repository;

        public FeedbackController(FeedbackRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"] as string;
            ViewBag.ErrorMessage = TempData["ErrorMessage"] as string;

            TempData.Remove("SuccessMessage"); // Ensure it's cleared after reading
            TempData.Remove("ErrorMessage");   // Prevents reappearing messages

            List<FeedbackRDM> feedbackList = _repository.GetFeedbackWithDetails();
            return View("Feedback", feedbackList);
        }



        [HttpPost]
        public IActionResult SubmitFeedback(int? staffId, string feedback)
        {
            try
            {
                if (staffId == null || staffId <= 0)
                {
                    staffId = _repository.GetDefaultStaffId();
                }

                _repository.AddFeedback((int)staffId, feedback);

                // Pass a success message using TempData
                TempData["SuccessMessage"] = "Feedback submitted successfully!";

                return RedirectToAction("Index"); // Redirect to the feedback page
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to submit feedback: " + ex.Message;
                return RedirectToAction("Index"); // Redirect with an error message
            }
        }

        [HttpGet]
        public IActionResult EditFeedback(int feedbackId)
        {
            FeedbackRDM feedback = _repository.GetFeedbackById(feedbackId);
            if (feedback == null)
            {
                return NotFound();
            }

            return View("EditFeedback", feedback); // Pass feedback to the edit view
        }

        [HttpPost]
        public IActionResult EditFeedback(int feedbackId, string updatedFeedback)
        {
            try
            {
                _repository.UpdateFeedback(feedbackId, updatedFeedback);
                TempData["SuccessMessage"] = "Feedback updated successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error updating feedback: " + ex.Message;
                return RedirectToAction("Index");
            }
        }



    }
}
