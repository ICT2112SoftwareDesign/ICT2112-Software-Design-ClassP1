using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanBrilliantCompany.Models.Entity
{
    [Table("Ingredient")]
    public class IngredientSDM
    {
        // Private backing fields
        private int _ingredientId;
        private int _productId;
        private string _ingredientName = string.Empty; // Initialize with empty string
        private double _ingredientToxicity;
        private int _thresholdQuantity;
        private int _quantity;
        private string _measurementUnit = "ml"; // Initialize with default value
        private bool _reorderStatus;
        private DateTime _createdAt = DateTime.Now; // Initialize with current time
        private DateTime _updatedAt = DateTime.Now; // Initialize with current time

        // Constructor with basic initialization
        public IngredientSDM()
        {
            // Default constructor
        }

        // Public properties with getters and setters
        public int IngredientId
        {
            get { return _ingredientId; }
            set { _ingredientId = value; }
        }

        public int ProductId
        {
            get { return _productId; }
            set { _productId = value; }
        }

        public string IngredientName
        {
            get { return _ingredientName; }
            set { _ingredientName = value; }
        }

        public double IngredientToxicity
        {
            get { return _ingredientToxicity; }
            set { _ingredientToxicity = value; }
        }

        public int ThresholdQuantity
        {
            get { return _thresholdQuantity; }
            set { _thresholdQuantity = value; }
        }

        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        public string MeasurementUnit
        {
            get { return _measurementUnit; }
            set { _measurementUnit = value; }
        }

        public bool ReorderStatus
        {
            get { return _reorderStatus; }
            set { _reorderStatus = value; }
        }

        public DateTime CreatedAt
        {
            get { return _createdAt; }
            set { _createdAt = value; }
        }

        public DateTime UpdatedAt
        {
            get { return _updatedAt; }
            set { _updatedAt = value; }
        }
    }
}