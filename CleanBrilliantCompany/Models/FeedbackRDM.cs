using System;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Models{
    
    public class FeedbackRDM
    {
        
        // Private fields
        private int feedbackId;
        private int staffId;
        private string status;
        private DateTime dateSubmitted;
        private string feedback;
        private string managerComments;

        // Public getter and setter methods
        public int GetFeedbackId()
        {
            return feedbackId;
        }

        public void SetFeedbackId(int id)
        {
            feedbackId = id;
        }

        public string GetStatus()
        {
            return status;
        }

        public void SetStatus(string status)
        {
            this.status = status;
        }

        public DateTime GetDateSubmitted()
        {
            return dateSubmitted;
        }

        public void SetDateSubmitted(DateTime dateSubmitted)
        {
            this.dateSubmitted = dateSubmitted;
        }

        public string GetFeedback()
        {
            return feedback;
        }

        public void SetFeedback(string feedback)
        {
            this.feedback = feedback;
        }

        public string GetManagerComments()
        {
            return managerComments;
        }

        public void SetManagerComments(string managerComments)
        {
            this.managerComments = managerComments;
        }

        public object FeedbackDetails(string field, object value)
        {
            switch (field.ToLower())
            {
                case "feedbackid":
                    feedbackId = Convert.ToInt32(value);
                    return feedbackId;
                case "staffid":
                    staffId = Convert.ToInt32(value);
                    return staffId;
                case "status":
                    status = value.ToString();
                    return status;
                case "datesubmitted":
                    dateSubmitted = Convert.ToDateTime(value);
                    return dateSubmitted;
                case "feedback":
                    feedback = value.ToString();
                    return feedback;
                case "managercomments":
                    managerComments = value.ToString();
                    return managerComments;
                default:
                    return null;
            }
        }

        public override string ToString()
        {
            return $"FeedbackRDM [feedbackId={feedbackId}, staffId={staffId}, status={status}, dateSubmitted={dateSubmitted}, feedback={feedback}, managerComments={managerComments}]";
        }
    }

    public class FeedbackRepository
    {
        private readonly List<string> feedbackList;

        public FeedbackRepository()
        {
            // Initialize with some dummy data
            feedbackList = new List<string>
            {
                "Great service!",
                "Needs improvement.",
                "Fast response!",
                "Highly recommended!",
                "Would love more features."
            };
        }

        public List<string> GetFeedbackList()
        {
            return feedbackList;
        }

        public void AddFeedback(string feedback)
        {
            feedbackList.Add(feedback);
        }

        public void EditFeedback(int index, string feedback)
        {
            if (index >= 0 && index < feedbackList.Count)
                feedbackList[index] = feedback;
        }

        public void DeleteFeedback(int index)
        {
            if (index >= 0 && index < feedbackList.Count)
                feedbackList.RemoveAt(index);
        }
    }


    // Feedback Management Implementation
    public class FeedbackManagement : IFeedbackManagement
    {
        private readonly FeedbackRepository repository;

        public FeedbackManagement(FeedbackRepository repo)
        {
            this.repository = repo;
        }

        public void DeleteFeedback()
        {
            var feedbackList = repository.GetFeedbackList();
            if (feedbackList.Count > 0)
                feedbackList.RemoveAt(0);
        }

        public bool ResolveFeedback(int feedbackId)
        {
            var feedbackList = repository.GetFeedbackList();
            return feedbackId < feedbackList.Count;
        }
    }


    // Feedback Retrieval Implementation
    public class FeedbackRetrieval : IFeedbackRetrieval
    {
        private readonly FeedbackRepository repository;

        public FeedbackRetrieval(FeedbackRepository repo)
        {
            this.repository = repo;
        }

        public string GetFeedbackById(int feedbackId)
        {
            var feedbackList = repository.GetFeedbackList();
            return feedbackId < feedbackList.Count ? feedbackList[feedbackId] : "Feedback Not Found";
        }

        public List<string> GetAllFeedback()
        {
            return repository.GetFeedbackList();
        }
    }

    public class FeedbackSubmission : IFeedbackSubmission
    {
        private readonly FeedbackRepository _repository;

        public FeedbackSubmission(FeedbackRepository repository)
        {
            _repository = repository;
        }

        public void AddFeedback(string feedback)
        {
            _repository.AddFeedback(feedback);
        }

        public void EditFeedback(string feedback)
        {
            var feedbackList = _repository.GetFeedbackList();
            if (feedbackList.Count > 0)
                _repository.EditFeedback(0, feedback);
        }
    }


    
}
