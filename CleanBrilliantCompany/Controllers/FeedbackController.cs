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
            // Retrieve actual feedback list from the repository
            List<string> feedbackList = _repository.GetFeedbackList();

            return View("Feedback", feedbackList);
        }

        [HttpPost]
        public IActionResult SubmitFeedback(string feedback)
        {
            if (!string.IsNullOrWhiteSpace(feedback))
            {
                _repository.AddFeedback(feedback);
            }
            return RedirectToAction("Index");
        }
    }
}