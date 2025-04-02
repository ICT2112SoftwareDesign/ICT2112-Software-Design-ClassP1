using System.Threading.Tasks;
using CleanBrilliantCompany.DTO;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models.Entity;
using Microsoft.ML;
using Microsoft.ML.TimeSeries;
using Microsoft.ML.Data;


namespace CleanBrilliantCompany.Models.Control
{
    public class PredictionSSA : IPredictionStrategy {
        
        private readonly MLContext _mlContext;
        private ITransformer _model;

        public PredictionSSA()
        {
            _mlContext = new MLContext();
        }
        private void trainModel(List<DateTime> days, List<double> data){
            if (days.Count != data.Count || days.Count == 0)
                throw new ArgumentException("Days and data must have the same length and not be empty.");

            var trainingData = days.Select((date, index) => new PredictionDataFormatDTO
                {
                    date = date,
                    emission = (float)data[index]
                }).ToList();

            IDataView dataView = _mlContext.Data.LoadFromEnumerable(trainingData);

            int windowSize = 12;   // track by year trends
            int seriesLength = data.Count;

            DateTime lastDate = days.Last();
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
            

            var pipeline = _mlContext.Forecasting.ForecastBySsa(
            outputColumnName: "predictedEmission",
            inputColumnName: "emission",
            windowSize: windowSize,
            seriesLength: seriesLength,
            trainSize: seriesLength,
            horizon: horizon);

            _model = pipeline.Fit(dataView);
        }

        public List<EmissionPredDTO> retrievePrediction(List<DateTime> days, List<double> data){
            if (_model == null)
            {
                trainModel(days, data);
            }

            return predictSSA(days, data);
        }
        private List<EmissionPredDTO> predictSSA(List<DateTime> days, List<double> data)
        {
            if (_model == null)
                throw new InvalidOperationException("Model has not been trained.");

            List<EmissionPredDTO> result = new List<EmissionPredDTO>();
            var trainingData = days.Select((date, index) => new PredictionDataFormatDTO
                {
                    date = date,
                    emission = (float)data[index]
                }).ToList();

            IDataView dataView = _mlContext.Data.LoadFromEnumerable(trainingData);
            var forecastedData = _model.Transform(dataView);

            DateTime lastDate = days.Last();

            var predictions = _mlContext.Data.CreateEnumerable<PredictionDataOutputDTO>(forecastedData, reuseRowObject: false).FirstOrDefault();
            if (predictions != null)
            {
                var predictedEmissions = predictions.predictedEmission.Select((value, index) =>
                    new EmissionPredDTO
                    {
                        day = new DateTime(lastDate.Year, lastDate.Month, 1).AddMonths(index + 1),
                        emisission = (double)value
                    }).ToList();

                result = predictedEmissions;
            }
            return result;
        }
    }
}