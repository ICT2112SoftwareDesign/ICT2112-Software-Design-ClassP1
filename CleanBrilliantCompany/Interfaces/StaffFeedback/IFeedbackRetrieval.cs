using System;
using CleanBrilliantCompany.Models;

using System.Collections.Generic;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IFeedbackRetrieval
    {
        //string GetFeedbackById(int feedbackId);
        FeedbackRDM GetFeedbackById(int feedbackId);
        List<FeedbackRDM> GetAllFeedback(); // ✅ Update return type to List<FeedbackRDM>
    }
}

