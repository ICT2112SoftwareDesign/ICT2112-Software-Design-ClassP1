



using System;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IFeedbackSubmission
    {
        void AddFeedback(string feedback);
        void EditFeedback(string feedback);
    }

    

}