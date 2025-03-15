using CleanBrilliantCompany.Models;
using System.Collections.Generic;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IRefundQuery
    {
        Refund_RDM GetRefundDetails(int refundId);
        List<Refund_RDM> GetAllRefunds();
    }
}
