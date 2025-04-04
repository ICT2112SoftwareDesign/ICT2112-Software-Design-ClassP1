using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanBrilliantCompany.Models.Entity
{
    // Maps to the existing DB table.
    [Table("CarbonFootprintRecord")]
    public class CarbonFootprintRecord
    {
        [Key]
        [Column("carbonFootprintId")]
        // Primary key
        public int carbonFootprintId { get; set; }

        [Column("entityId")]
        // ID of the Product/Order
        public int EntityId { get; set; }

        [Column("entityType")]
        // "Product" or "Order"
        public required string EntityType { get; set; }

        [Column("carbonEmission")]
        // CO₂ Emission (kg)
        public double CarbonEmission { get; set; }

        [Column("ecoStatus")]
        // "Eco-Friendly" / "Not Eco-Friendly"
        public required string EcoStatus { get; set; }

        [Column("dateCreated")]
        // Record Creation Date.
        public DateTime DateCreated { get; set; }

    }
}