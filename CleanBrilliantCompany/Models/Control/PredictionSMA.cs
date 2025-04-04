using CleanBrilliantCompany.DTO;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Models.Control
{
    public class PredictionSMA : IPredictionStrategy {
        
        private int _smaWindowSize = 3;
        public PredictionSMA()
        {
            _smaWindowSize = 3;
        }
        public List<EmissionPredDTO> retrievePrediction(List<DateTime> days, List<double> data)
        {
            if (days == null || data == null || days.Count != data.Count || days.Count == 0)
                throw new ArgumentException("Invalid input data for SMA prediction.");

            var result = new List<EmissionPredDTO>();

            // Get last date in dataset
            DateTime lastDate = days.Max();
            int lastMonth = lastDate.Month;
            int horizon;

            if (lastMonth == 12)
            {
                // Predict next year if last data is in december
                horizon = 12;
            }
            else
            {
                // Predict rest of month
                horizon = 12 - lastMonth;
            }

            // Generate future months (until end of the year)
            var futureDates = Enumerable.Range(1, horizon)
                .Select(i => new DateTime(lastDate.Year, lastDate.Month, 1).AddMonths(i))
                .ToList();

            List<double> smaValues = new List<double>(data);

            foreach (var futureDate in futureDates)
            {
                // use min 1 or _smaWindowSize as the window size for sma
                int windowSize = Math.Min(_smaWindowSize, smaValues.Count);

                double smaPrediction = smaValues.TakeLast(windowSize).Average();

                result.Add(new EmissionPredDTO { day = futureDate, emisission = smaPrediction });

                smaValues.Add(smaPrediction);
            }

            return result;
        }
    }
}