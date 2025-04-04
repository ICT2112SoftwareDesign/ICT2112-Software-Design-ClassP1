namespace CleanBrilliantCompany.Models
{
    public class CarbonFootprintWithSavings
    {
        // Private fields
        private int _carbonFootprintId;
        private int _entityId;
        private string _entityType = string.Empty;
        private double _carbonEmission;
        private string _ecoStatus = string.Empty;
        private DateTime _dateCreated;
        private double _productCost;
        private double _costSavings;
        private double _reductionPercentage { get; set; }

        // Public properties with encapsulation logic.
        public int CarbonFootprintId
        {
            get => _carbonFootprintId;
            set
            {
                if (value <= 0) throw new ArgumentException("CarbonFootprintId must be a positive number.");
                _carbonFootprintId = value;
            }
        }

        public int EntityId
        {
            get => _entityId;
            set
            {
                if (value <= 0) throw new ArgumentException("EntityId must be a positive number.");
                _entityId = value;
            }
        }

        public string EntityType
        {
            get => _entityType;
            set
            {
                if (string.IsNullOrEmpty(value)) throw new ArgumentException("EntityType cannot be null or empty.");
                _entityType = value;
            }
        }

        public double CarbonEmission
        {
            get => _carbonEmission;
            set
            {
                if (value < 0) throw new ArgumentException("CarbonEmission cannot be negative.");
                _carbonEmission = value;
            }
        }

        public string EcoStatus
        {
            get => _ecoStatus;
            set
            {
                if (string.IsNullOrEmpty(value)) throw new ArgumentException("EcoStatus cannot be null or empty.");
                _ecoStatus = value;
            }
        }

        public DateTime DateCreated
        {
            get => _dateCreated;
            set
            {
                if (value == default) throw new ArgumentException("DateCreated must be a valid date.");
                _dateCreated = value;
            }
        }

        public double ProductCost
        {
            get => _productCost;
            set
            {
                if (value < 0) throw new ArgumentException("ProductCost cannot be negative.");
                _productCost = value;
            }
        }

        public double CostSavings
        {
            get => _costSavings;
            set
            {
                if (value < 0) throw new ArgumentException("CostSavings cannot be negative.");
                _costSavings = value;
            }
        }
        public double ReductionPercentage
        {
            get => _reductionPercentage;
            set
            {
                _reductionPercentage = value;
            }
        }


        // public int CarbonFootprintId { get; set; }
        // public int EntityId { get; set; }
        // public required string EntityType { get; set; }
        // public double CarbonEmission { get; set; }
        // public required string EcoStatus { get; set; }
        // public DateTime DateCreated { get; set; }
        // public double ProductCost { get; set; }
        // public double CostSavings { get; set; }
    }
}

