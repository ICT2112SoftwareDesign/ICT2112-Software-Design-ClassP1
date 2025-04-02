using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Services
{
    /// <summary>
    /// service implementation for managing alerts, acting as a wrapper around the data gateway.
    /// now also responsible for generating alert details.
    /// </summary>
    public class AlertService : IAlertService
    {
        private readonly IAlertsDB _alertsDB;
        private readonly IGoalsDB _goalsDB;
        private readonly IEmissionDataService _emissionService;
        private readonly ILogger<AlertService> _logger;

        public AlertService(
            IAlertsDB alertsDB,
            IGoalsDB goalsDB,
            IEmissionDataService emissionService,
            ILogger<AlertService> logger)
        {
            _alertsDB = alertsDB ?? throw new ArgumentNullException(nameof(alertsDB));
            _goalsDB = goalsDB ?? throw new ArgumentNullException(nameof(goalsDB));
            _emissionService = emissionService ?? throw new ArgumentNullException(nameof(emissionService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// creates a new alert by calling the underlying data gateway.
        /// </summary>
        public async Task CreateAlertAsync(Alert alert)
        {
            if (alert == null)
            {
                throw new ArgumentNullException(nameof(alert));
            }
            try
            {
                await _alertsDB.AddAlertAsync(alert);
                _logger.LogInformation("created alert for {Month}/{Year} with status: {Status}", alert.GoalMonth, alert.GoalYear, alert.Status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error creating alert in database for {Month}/{Year}.", alert.GoalMonth, alert.GoalYear);
                throw;
            }
        }

        /// <summary>
        /// retrieves all alerts by calling the underlying data gateway.
        /// </summary>
        public async Task<IEnumerable<Alert>> GetAllAlertsAsync()
        {
            try
            {
                return await _alertsDB.GetAllAlertsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error retrieving all alerts from database.");
                return Enumerable.Empty<Alert>();
            }
        }

        /// <summary>
        /// generates the alert details for a specific period without saving it.
        /// </summary>
        public async Task<Alert?> GenerateAlertForPeriodAsync(int year, int month)
        {
            _logger.LogInformation("generating alert details for {Month}/{Year}...", month, year);
            try
            {
                // 1. get the goal for the period
                var goal = await _goalsDB.FindGoalByDate(year, month);
                _logger.LogInformation("retrieved goal for {Month}/{Year}: {GoalValue}", month, year, goal?.GetTargetEmission().ToString("N2") ?? "none");

                // 2. get the actual emissions for the period
                decimal actualEmissions = await _emissionService.GetTotalEmissionsForMonthAsync(month, year);
                _logger.LogInformation("calculated actual emissions for {Month}/{Year}: {ActualValue}", month, year, actualEmissions.ToString("N2"));

                // 3. determine status and message
                string status;
                string message;
                decimal? targetEmissionValue = null;

                if (goal == null)
                {
                    status = "no goal set";
                    message = $"no emission goal was set for {month:00}/{year}.";
                }
                else
                {
                    targetEmissionValue = (decimal)goal.GetTargetEmission(); // cast from double
                    if (actualEmissions <= targetEmissionValue)
                    {
                        status = "met";
                        message = $"monthly emission goal of {targetEmissionValue:N2}g co2e was met for {month:00}/{year} (actual: {actualEmissions:N2}g co2e).";
                    }
                    else
                    {
                        status = "missed";
                        message = $"monthly emission goal of {targetEmissionValue:N2}g co2e was missed for {month:00}/{year} (actual: {actualEmissions:N2}g co2e).";
                    }
                }

                // 4. construct the alert object
                var generatedAlert = new Alert
                {
                    AlertTimestamp = DateTime.UtcNow, // timestamp generation, not the period end
                    GoalMonth = month,
                    GoalYear = year,
                    TargetEmission = targetEmissionValue,
                    ActualTotalEmission = actualEmissions,
                    Status = status,
                    Message = message
                };

                _logger.LogInformation("successfully generated alert details for {Month}/{Year} with status: {Status}", month, year, status);
                return generatedAlert;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "failed to generate alert details for {Month}/{Year}.", month, year);
                return null; // indicate failure
            }
        }

        /// <summary>
        /// checks if an alert already exists in the database for a specific period.
        /// </summary>
        public async Task<bool> CheckAlertExistsAsync(int year, int month)
        {
            try
            {
                return await _alertsDB.CheckAlertExistsAsync(year, month);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error checking if alert exists for {Month}/{Year}.", month, year);
                return false; // assume not found on error, or rethrow?
            }
        }

        /// <summary>
        /// deletes an alert record based on its goal period.
        /// </summary>
        public async Task<bool> DeleteAlertByPeriodAsync(int year, int month)
        {
            _logger.LogInformation("attempting to delete alert for {Month}/{Year}...", month, year);
            try
            {
                bool deleted = await _alertsDB.DeleteAlertByPeriodAsync(year, month);
                if (deleted)
                {
                    _logger.LogInformation("successfully deleted alert for {Month}/{Year}.", month, year);
                }
                else
                {
                    _logger.LogWarning("no alert found to delete for {Month}/{Year}.", month, year);
                }
                return deleted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error deleting alert for {Month}/{Year}.", month, year);
                return false;
            }
        }
    }
}