using System.Collections.Generic;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Models
{
    public class ReorderRequestManagement : IReorderQuery, IReorderRequest
    {   
        private readonly IReorderRequestDB _reorderDatabase;

        public ReorderRequestManagement(IReorderRequestDB reorderDatabase)
        {
            _reorderDatabase = reorderDatabase;
        }

        

        public List<ReorderRequest_RDM> displayListOfReorders()
        {
            return _reorderDatabase.displayListOfReorders(); 
        }

        public ReorderRequest_RDM getReorderRequestDetails(int reorderId) 
        {
            return _reorderDatabase.getReorderRequestDetails(reorderId);
        }
        
        public void createReorderRequest(ReorderRequest_RDM reorder)
        {
            _reorderDatabase.createReorderRequest(reorder);
        }

        public void updateReorderRequest(ReorderRequest_RDM reorder)
        {
            _reorderDatabase.updateReorderRequest(reorder);
        }


        

        public void cancelReorderRequest(int reorderId)
        {
            _reorderDatabase.cancelReorderRequest(reorderId);
        }

        public void deleteProductFromReorder(int reorder_product_Id)
        {
            _reorderDatabase.deleteProductFromReorder(reorder_product_Id);
        }


    }
}