namespace CleanBrilliantCompany.Models
{
    public class ShippingAgent_RDM
    {
        public int ShippingAgentId { get; set; }
        public string ShippingAgentCompany { get; set; } = string.Empty;
        public string ShippingMethod { get; set; } = string.Empty;
        public string ServiceType { get; set; } = string.Empty;
    }
}