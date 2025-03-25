using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;


namespace CleanBrilliantCompany.Models
{
    [Table("ShippingAgent")] // Maps the model to the "ShippingAgent" table in the database
    public class ShippingAgent
    {
        [Key] // Marks this as the primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-incremented ID
        [Column("shippingAgentId")]
        public int ShippingAgentId { get; set; }

        [Required]
        [StringLength(255)] // Adjust length based on your database schema
        [Column("shippingAgentCompany")]
        public string ShippingAgentCompany { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        [Column("shippingMethod")] // Fixed column name to match database
        public string ShippingMethod { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        [Column("serviceType")]
        public string ServiceType { get; set; } = string.Empty;
    }
}

namespace CleanBrilliantCompany.Models
{
    public class ShippingAgentViewModel
    {
        public List<ShippingAgent> ShippingAgents { get; set; }

        public ShippingAgentViewModel()
        {
            ShippingAgents = new List<ShippingAgent>();
        }
    }
}