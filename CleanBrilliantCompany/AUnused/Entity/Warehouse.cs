namespace CleanBrilliantCompany.Models.Entity
{
    public class Warehouse
    {
        // Private fields
        private int WarehouseId;
        private string Address;
        private int CurrentCapacity;
        private int MaxCapacity;

        private int ProductId;
        private int Quantity;
        private int ItemId;

        // Constructor to initialize the private fields
        public Warehouse(int warehouseId, string address, int currentCapacity, int maxCapacity)
        {
            WarehouseId = warehouseId;
            Address = address;
            CurrentCapacity = currentCapacity;
            MaxCapacity = maxCapacity;
        }

        public Warehouse(int warehouseId, string address, int currentCapacity, int maxCapacity, int itemId, int productId, int quantity)
        {
            WarehouseId = warehouseId;
            Address = address;
            CurrentCapacity = currentCapacity;
            MaxCapacity = maxCapacity;
            ProductId = productId;
            ItemId = itemId;
            Quantity = quantity;
        }

        public Dictionary<string, object> retrieveWarehouseInfo()
        {
            return new Dictionary<string, object>
            {
                { "WarehouseId", WarehouseId },
                { "Address", Address },
                { "CurrentCapacity", CurrentCapacity },
                { "MaxCapacity", MaxCapacity },
            };
        }

        // Private getters 
        private int getWarehouseId() => WarehouseId;
        private string getAddress() => Address;
        private int getMaxCapacity() => MaxCapacity;
        private int getCurrentCapacity() => CurrentCapacity;

        // Private setters 
        private void setWarehouseId(int warehouseId) => WarehouseId = warehouseId;
        private void setAddress(string address) => Address = address;
        private void setMaxCapacity(int maxCapacity) => MaxCapacity = maxCapacity;
        private void setCurrentCapacity(int currentCapacity) => CurrentCapacity = currentCapacity;


        public int getAvailableCapacity(){
            return MaxCapacity - CurrentCapacity;
        }

        public Warehouse() { } // dk if need anot 
    }
}
