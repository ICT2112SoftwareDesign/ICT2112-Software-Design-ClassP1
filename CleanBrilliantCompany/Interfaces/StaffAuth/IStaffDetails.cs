using CleanBrilliantCompany.Models.StaffAuth;
using Microsoft.AspNetCore.Mvc;

namespace CleanBrilliantCompany.Interfaces.StaffAuth
{
    public interface IStaffDetails
    {
        StaffRDM GetStaffDetails(int staffId);
    }
}
