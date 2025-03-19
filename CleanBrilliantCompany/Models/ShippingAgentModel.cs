using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanBrilliantCompany.Models
{
    [Table("ShippingAgent")] // Maps the model to the "ShippingAgent" table in the database
    public class ShippingAgent
    {
        [Key] // Marks this as the primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-incremented ID
        public int ShippingAgentId { get; set; }

        [Required]
        [StringLength(255)] // Adjust length based on your database schema
        public string ShippingAgentCompany { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string ShippingMethod { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string ServiceType { get; set; } = string.Empty;
    }
}

public class ShippingAgentViewModel
{
    public List<ShippingAgent> ShippingAgents { get; set; } = new List<ShippingAgent>();
}
