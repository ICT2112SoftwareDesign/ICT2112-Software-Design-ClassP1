using CleanBrilliantCompany.Interfaces;
using System.Globalization;
using Microsoft.AspNetCore.SignalR;
using CleanBrilliantCompany.Hubs;

namespace CleanBrilliantCompany.Services
{
    /// <summary>
    /// background service that checks monthly emission goals at the start of each month.
    /// </summary>
    public class MonthlyGoalCheckService : BackgroundService
    {
        private readonly ILogger<MonthlyGoalCheckService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHubContext<AlertHub> _hubContext;
        private (int year, int month) _lastProcessedPeriod = (0, 0); // track the last successfully processed period

        // check every hour to see if it's time to run the monthly check
        private readonly TimeSpan _checkInterval = TimeSpan.FromHours(1);
        // private readonly timespan _checkinterval = timespan.fromminutes(1); // shorter interval for testing

        private readonly TimeZoneInfo _targetTimeZone; // field to hold the target timezone

        public MonthlyGoalCheckService(
            ILogger<MonthlyGoalCheckService> logger,
            IServiceScopeFactory scopeFactory,
            IHubContext<AlertHub> hubContext
            )
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _hubContext = hubContext;

            // initialize the target timezone (change id if needed)
            try
            {
                _targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
                _logger.LogInformation("monthly goal check service configured to use timezone: {TimeZoneId}", _targetTimeZone.Id);
            }
            catch (TimeZoneNotFoundException)
            {
                _logger.LogError("target timezone id 'singapore standard time' not found. defaulting to utc.");
                _targetTimeZone = TimeZoneInfo.Utc;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error initializing target timezone. defaulting to utc.");
                _targetTimeZone = TimeZoneInfo.Utc;
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("monthly goal check service starting.");

            // wait a moment on startup before first check
            _logger.LogDebug("before initial 15 second delay.");
            await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
            _logger.LogDebug("after initial 15 second delay.");

            // perform initial check immediately after delay
            _logger.LogInformation("performing initial monthly goal check...");
            try
            {
                await CheckMonthlyGoalAsync(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("initial check cancelled, likely shutdown.");
                return; // exit if cancelled during initial check
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error during initial monthly goal check.");
                // continue to periodic checks even if initial fails
            }
            _logger.LogInformation("initial monthly goal check complete. starting periodic checks.");

            using var timer = new PeriodicTimer(_checkInterval);
            _logger.LogDebug("periodic timer created. entering while loop next.");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogTrace("top of while loop.");
                try
                {
                    _logger.LogTrace("waiting for next timer tick...");
                    bool timerTicked = await timer.WaitForNextTickAsync(stoppingToken);
                    _logger.LogTrace("timer tick awaited. timer ticked: {TimerTicked}", timerTicked);

                    if (!timerTicked) // should not happen unless cancelled, but good check
                    {
                        _logger.LogInformation("waitfornexttickasync returned false, likely due to cancellation.");
                        break; // exit loop if timer stops ticking (e.g., cancellation)
                    }

                    _logger.LogTrace("hourly check timer ticked (inside try)."); // existing log moved slightly
                    await CheckMonthlyGoalAsync(stoppingToken);
                }
                catch (OperationCanceledException) // specifically catch cancellation
                {
                    _logger.LogInformation("operation cancelled (likely shutdown), exiting loop.");
                    break; // exit loop on cancellation
                }
                catch (Exception ex) // catch other unexpected errors *within the loop*
                {
                    _logger.LogError(ex, "unexpected error occurred during periodic goal check loop.");
                    await Task.Delay(TimeSpan.FromSeconds(60), CancellationToken.None); // delay 1 min before retrying loop
                }
            }

            _logger.LogInformation("monthly goal check service stopping.");
        }

        private async Task CheckMonthlyGoalAsync(CancellationToken stoppingToken)
        {
            _logger.LogTrace("entering checkmonthlygoalasync method."); // log entry into the method

            var utcNow = DateTime.UtcNow;
            // convert utc time to the target local timezone
            var localNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, _targetTimeZone);
            _logger.LogTrace("utc time: {UtcNow}, target timezone time ({TimeZoneId}): {LocalNow}", utcNow, _targetTimeZone.Id, localNow);

            var previousMonthDate = localNow.AddMonths(-1); // use local time to determine previous month reliably
            int targetYear = previousMonthDate.Year;
            int targetMonth = previousMonthDate.Month;
            var currentPeriod = (targetYear, targetMonth);

            // only run the check on the first three day of the month (in the target timezone)
            // and only if we haven't successfully processed this period before
            if ((localNow.Day == 1 || localNow.Day == 2 || localNow.Day == 3) && currentPeriod != _lastProcessedPeriod)
            {
                _logger.LogInformation("starting monthly goal check for {Month}/{Year} based on {TimeZoneId} time ({LocalNow})...", targetMonth, targetYear, _targetTimeZone.Id, localNow);

                // create a scope to resolve scoped services
                using var scope = _scopeFactory.CreateScope();
                var alertService = scope.ServiceProvider.GetRequiredService<IAlertService>();

                try
                {
                    // simplified: call alertservice to generate the alert object
                    var generatedAlert = await alertService.GenerateAlertForPeriodAsync(targetYear, targetMonth);

                    if (generatedAlert != null)
                    {
                        // check if it already exists (should be rare with _lastprocessedperiod check, but good safeguard)
                        bool exists = await alertService.CheckAlertExistsAsync(targetYear, targetMonth);
                        if (!exists)
                        {
                            // save the generated alert
                            await alertService.CreateAlertAsync(generatedAlert);
                            // update the last processed period *only on success*
                            _lastProcessedPeriod = currentPeriod;

                            _logger.LogInformation("monthly alert created for {Month}/{Year}. broadcasting notification.", targetMonth, targetYear);

                            // <<< signalr broadcast start >>>
                            try
                            {
                                // prepare data to send to client
                                string goalPeriodStr = $"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(generatedAlert.GoalMonth)} {generatedAlert.GoalYear}";
                                string targetEmissionStr = generatedAlert.TargetEmission?.ToString("N2") ?? "n/a";
                                string actualEmissionStr = generatedAlert.ActualTotalEmission.ToString("N2");
                                string statusStr = generatedAlert.Status;

                                // send to all connected clients, invoking the 'receivemonthlyalert' javascript function
                                await _hubContext.Clients.All.SendAsync(
                                    "ReceiveMonthlyAlert", // must match the javascript listener name
                                    goalPeriodStr,
                                    targetEmissionStr,
                                    actualEmissionStr,
                                    statusStr,
                                    stoppingToken); // pass cancellationtoken if appropriate

                                _logger.LogInformation("successfully broadcasted signalr alert for {Month}/{Year}.", targetMonth, targetYear);
                            }
                            catch (Exception hubEx)
                            {
                                _logger.LogError(hubEx, "error broadcasting signalr message for {Month}/{Year}.", targetMonth, targetYear);
                                // decide if failure to broadcast should prevent marking as processed - probably not
                            }
                            // <<< signalr broadcast end >>>
                        }
                        else
                        {
                            _logger.LogWarning("alert already existed for {Month}/{Year}. skipping save and broadcast.", targetMonth, targetYear);
                            // ensure we still update last processed period if it already existed
                            _lastProcessedPeriod = currentPeriod;
                        }
                    }
                    else
                    {
                        _logger.LogError("alert generation failed for {Month}/{Year}, will retry later.", targetMonth, targetYear);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "unexpected error during monthly goal check processing for {Month}/{Year}.", targetMonth, targetYear);
                    // do not update _lastprocessedperiod, so it retries on the next check
                }
            }
            else
            {
                // log the specific reason(s) for skipping the check based on local time
                if (localNow.Day != 2)
                {
                    _logger.LogDebug("skipping monthly goal check: it is not the 2nd day of the month in {TimeZoneId} (current local day: {LocalDay}).", _targetTimeZone.Id, localNow.Day);
                }
                if (currentPeriod == _lastProcessedPeriod)
                {
                    _logger.LogDebug("skipping monthly goal check: period {CurPeriod} has already been processed (last processed: {LastPeriod}).", currentPeriod, _lastProcessedPeriod);
                }
            }
        }
    }
}