using CleanBrilliantCompany.Models;
using System.Collections.Generic;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IReorderQuery
    {
        ReorderRequest_RDM getReorderRequestDetails(int reorderId);
        List<ReorderRequest_RDM> displayListOfReorders();
        void updateReorderRequest(ReorderRequest_RDM reorder);

        void createReorderRequest(ReorderRequest_RDM reorder);

        void cancelReorderRequest(int reorderId);

        void deleteProductFromReorder(int reorder_product_Id);

    }
}