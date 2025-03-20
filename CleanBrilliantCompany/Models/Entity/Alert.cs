// Models/Alerts.cs
using System;

namespace CleanBrilliantCompany.Models
{
    public class Alert
    {
        public int AlertID { get; set; }
        public DateTime AlertDate { get; set; }
        public string AlertMessage { get; set; }
    }
}
