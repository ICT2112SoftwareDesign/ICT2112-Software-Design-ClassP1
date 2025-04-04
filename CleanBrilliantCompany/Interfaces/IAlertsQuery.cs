using System.Threading.Tasks;
using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IAlertsQuery
    {
        Task<bool> checkAlertsQuery(int goalYear, int goalMonth);
    }   
}
