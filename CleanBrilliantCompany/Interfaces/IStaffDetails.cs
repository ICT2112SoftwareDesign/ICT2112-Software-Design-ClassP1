using CleanBrilliantCompany.Models;
using Microsoft.AspNetCore.Mvc;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IStaffDetails
    {
        StaffRDM GetStaffDetails(int staffId);
    }
}
