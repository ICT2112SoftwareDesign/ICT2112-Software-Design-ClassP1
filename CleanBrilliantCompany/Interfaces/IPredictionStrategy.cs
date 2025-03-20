using System.Collections.Generic;
using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Models.Control;
using CleanBrilliantCompany.DTO;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IPredictionStrategy
    {
        public List<EmissionPredDTO> retrievePrediction(List<DateOnly> days, List<float> data);
    }
}
