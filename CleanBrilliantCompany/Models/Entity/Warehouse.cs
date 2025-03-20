namespace CleanBrilliantCompany.Models.Entity
{


    public class Warehouse
    {
        private int warehouseId;
        private string? address;
        private int currentCapacity;
        private int maxCapacity;


        //Getter Method
        private int getWarehouseId() => warehouseId;
        private string? getAddress() => address;
        private int getCurrentCapacity() => currentCapacity;
        private int getMaxCapacity() => maxCapacity;

        //Setter Method
        private void setWarehouseId(int warehouseId) => this.warehouseId = warehouseId;
        private void setAddress(string address) => this.address = address;
        private void setCurrentCapacity(int currentCapacity) => this.currentCapacity = currentCapacity;
        private void setMaxCapacity(int maxCapacity) => this.maxCapacity = maxCapacity;

        public Warehouse(int warehouseId, string address, int currentCapacity, int maxCapacity)
        {
            setWarehouseId(warehouseId);
            setAddress(address);
            setCurrentCapacity(currentCapacity);
            setMaxCapacity(maxCapacity);
        }

        public Dictionary<string, object> getWarehouseDetails()
        {
            return new Dictionary<string, object>
            {
                { "warehouseId", warehouseId },
                { "address", address?? "N/A" },
                { "currentCapacity", currentCapacity },
                { "maxCapacity", maxCapacity }
            };
        }




    }


}