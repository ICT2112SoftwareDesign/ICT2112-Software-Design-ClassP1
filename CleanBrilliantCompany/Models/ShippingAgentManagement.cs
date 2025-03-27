using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;



namespace CleanBrilliantCompany.Models
{
    public class ShippingAgentManagement
    {
        public List<ShippingAgent> ShippingAgents { get; set; }

        public ShippingAgentManagement()
        {
            ShippingAgents = new List<ShippingAgent>();
        }
    }
}
