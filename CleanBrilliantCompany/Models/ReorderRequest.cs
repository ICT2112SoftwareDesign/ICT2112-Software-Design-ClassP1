using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanBrilliantCompany.Models
{
    public class ReorderRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto increment for SQL Server
        [Column("reorderId")] 
        public int reorderId { get; set; }  // Ensures that `reorderId` matches the database column

        [Required]
        [Column("productId")]
        public int ProductId { get; set; }

        [Required]
        [Column("quantity")]
        public int Quantity { get; set; }

        [Required]
        [Column("manufacturerId")]
        public int ManufacturerId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Column("expectedDeliveryDate")]
        public DateTime ExpectedDeliveryDate { get; set; }

        [Required]
        [StringLength(50)]
        [Column("status")]
        public string Status { get; set; } = "Pending"; // Default value


        [Column("defectQuantity")]
        public int DefectQuantity { get; set; }
    }
}
