using System.Collections.Generic;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IReorderRequest
    {
        ReorderRequest_RDM getReorderRequestDetails(int reorderId);
        List<ReorderRequest_RDM> displayListOfReorders();
    }
}
