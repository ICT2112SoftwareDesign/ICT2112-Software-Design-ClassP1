using System;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    // Interface for Staff Feedback
    public interface IStaffFeedback
    {
        void AddFeedback(string feedback);
        void EditFeedback(string feedback);
    }




}