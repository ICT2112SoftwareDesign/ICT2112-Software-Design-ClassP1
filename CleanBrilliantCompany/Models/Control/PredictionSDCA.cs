using System.Threading.Tasks;
using CleanBrilliantCompany.DTO;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models.Entity;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace CleanBrilliantCompany.Models.Control
{
    public class PredictionSDCA : IPredictionStrategy {
        
        private readonly MLContext _mlContext;
        private ITransformer _model;

        public PredictionSDCA()
        {
            _mlContext = new MLContext();
        }
        private void trainModel(List<DateOnly> days, List<float> data){
            if (days.Count != data.Count || days.Count == 0)
                throw new ArgumentException("Days and data must have the same length and not be empty.");
            List<EmissionPredDTO> trainingData = new List<EmissionPredDTO>();
            for(int i=0;i<days.Count;i++){
                trainingData.Add(new EmissionPredDTO(
                    day: days[i],
                    emisission: data[i]
                ));
            }
            // Load data into ML.NET
            var dataView = _mlContext.Data.LoadFromEnumerable(trainingData);

            // Define data preparation and pipeline
            var pipeline = _mlContext.Transforms.Concatenate("Features", nameof(EmissionPredDTO.day))
                .Append(_mlContext.Regression.Trainers.Sdca(labelColumnName: nameof(EmissionPredDTO.emisission), featureColumnName: "Features"));

            // Train the model
            _model = pipeline.Fit(dataView);           

        }

        public List<EmissionPredDTO> retrievePrediction(List<DateOnly> days, List<float> data){
            if (_model == null)
            {
                trainModel(days, data);
            }

            return predictSDCA(days, data);
        }
        private List<EmissionPredDTO> predictSDCA(List<DateOnly> days, List<float> data){

            if (_model == null)
                throw new InvalidOperationException("Model has not been trained.");

            var predictions = new List<EmissionPredDTO>();

            // Get the current date
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Today); // Using DateOnly
            int currentMonth = currentDate.Month;
            int currentYear = currentDate.Year;

            // Separate the days that are in the current month
            List<EmissionPredDTO> actualDataThisMonth = new List<EmissionPredDTO>();

            // Identify actual data for the current month
            for (int i = 0; i < days.Count; i++)
            {
                if (days[i].Month == currentMonth && days[i].Year == currentYear)
                {
                    actualDataThisMonth.Add(new EmissionPredDTO(days[i], data[i]));
                }
            }

            // Get the last actual data day for prediction logic
            DateOnly lastActualDay = actualDataThisMonth
                .OrderByDescending(d => d.day)
                .FirstOrDefault()
                ?.day ?? DateOnly.MinValue;

            // Handle case where there is no actual data for the current month
            if (lastActualDay == DateOnly.MinValue)
            {
                // If there's no data in the current month, predict for the entire month using historical data
                lastActualDay = new DateOnly(currentYear, currentMonth, 1).AddDays(-1); // Consider the day before the current month started
            }

            // Get the days of the current month (we'll predict for the upcoming days after the last actual data)
            DateOnly firstDayOfMonth = new DateOnly(currentYear, currentMonth, 1);
            DateOnly lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            // Get all days in the current month and only predict after the last actual data day
            List<DateOnly> predictedDays = new List<DateOnly>();

            for (DateOnly day = lastActualDay.AddDays(1); day <= lastDayOfMonth; day = day.AddDays(1))
            {
                // Check if the day is not in the actual data (skip missing dates between actual data)
                if (!actualDataThisMonth.Exists(d => d.day == day))
                {
                    predictedDays.Add(day); // Only add days to predict
                }
            }

            // Prepare the data for prediction (only dates after the last actual day)
            var predictionData = predictedDays.Select(d => new EmissionPredDTO(d, 0)).ToList();

            // Use the trained model to predict emissions for the upcoming days
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<EmissionPredDTO, EmissionPredDTO>(_model);

            foreach (var prediction in predictionData)
            {
                var predictedValue = predictionEngine.Predict(prediction);
                predictions.Add(new EmissionPredDTO(predictedValue.day, predictedValue.emisission));
            }

            // Add the actual data for the current month
            predictions.AddRange(actualDataThisMonth);

            // Return the combined list of actual and predicted data
            return predictions.OrderBy(p => p.day).ToList();
        }

    }
}