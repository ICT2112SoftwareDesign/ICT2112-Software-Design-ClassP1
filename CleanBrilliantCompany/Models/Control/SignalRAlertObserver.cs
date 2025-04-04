using CleanBrilliantCompany.Hubs;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;
using Microsoft.AspNetCore.SignalR;
using System.Globalization;

namespace CleanBrilliantCompany.Services
{
    /// <summary>
    /// observer implementation that sends notifications via signalr when an alert is created.
    /// </summary>
    public class SignalRAlertObserver : IAlertCreationObserver
    {
        private readonly IHubContext<AlertHub> _hubContext;
        private readonly ILogger<SignalRAlertObserver> _logger;

        public SignalRAlertObserver(IHubContext<AlertHub> hubContext, ILogger<SignalRAlertObserver> logger)
        {
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// sends a notification message via signalr hub when a new alert is created.
        /// </summary>
        /// <param name="createdAlert">the alert that was created.</param>
        public async Task Update(Alert createdAlert)
        {
            if (createdAlert == null)
            {
                _logger.LogWarning("signalralertobserver received a null alert in update method.");
                return;
            }

            _logger.LogInformation("signalralertobserver triggered for alert period {Month}/{Year}.", createdAlert.GoalMonth, createdAlert.GoalYear);

            try
            {
                // prepare data for the client-side notification
                string goalPeriod = $"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(createdAlert.GoalMonth)} {createdAlert.GoalYear}";
                string targetEmission = createdAlert.TargetEmission.HasValue ? createdAlert.TargetEmission.Value.ToString("N2") : "n/a";
                string actualEmission = createdAlert.ActualTotalEmission.ToString("N2");
                string status = createdAlert.Status;

                // broadcast message to all connected clients
                await _hubContext.Clients.All.SendAsync("ReceiveMonthlyAlert",
                    goalPeriod,
                    targetEmission,
                    actualEmission,
                    status);

                _logger.LogInformation("successfully broadcasted alert notification via signalr for {GoalPeriod}.", goalPeriod);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error broadcasting alert notification via signalr for period {Month}/{Year}.", createdAlert.GoalMonth, createdAlert.GoalYear);
            }
        }
    }
} 