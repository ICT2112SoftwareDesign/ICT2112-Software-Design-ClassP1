namespace CleanBrilliantCompany.Models
{
    public class OrderCarbonFootprintRDM
    {
        private int orderCFId;
        private int orderId;
        private string transportMode;
        private double orderWeight;
        private double distance;
        private double carbonEmission;
        private string ecoStatus;
        private DateTime dateCreated;

        // Constructor
        public OrderCarbonFootprintRDM(int orderCFId, int orderId, string transportMode, double orderWeight, double distance, double carbonEmission, string ecoStatus, DateTime dateCreated)
        {
            this.orderCFId = orderCFId;
            this.orderId = orderId;
            this.transportMode = transportMode;
            this.orderWeight = orderWeight;
            this.distance = distance;
            this.carbonEmission = carbonEmission;
            this.ecoStatus = ecoStatus;
            this.dateCreated = dateCreated;
        }

        // Getters and Setters
        private int getOrderCFId()
        {
            return orderCFId;
        }

        private void setOrderCFId(int orderCFId)
        {
            this.orderCFId = orderCFId;
        }

        private int getOrderId()
        {
            return orderId;
        }

        private void setOrderId(int orderId)
        {
            this.orderId = orderId;
        }

        private string getTransportMode()
        {
            return transportMode;
        }

        private void setTransportMode(string transportMode)
        {
            this.transportMode = transportMode;
        }

        private double getOrderWeight()
        {
            return orderWeight;
        }

        private void setOrderWeight(double orderWeight)
        {
            this.orderWeight = orderWeight;
        }

        private double getDistance()
        {
            return distance;
        }

        private void setDistance(double distance)
        {
            this.distance = distance;
        }

        private double getCarbonEmission()
        {
            return carbonEmission;
        }

        private void setCarbonEmission(double carbonEmission)
        {
            this.carbonEmission = carbonEmission;
        }

        private string getEcoStatus()
        {
            return ecoStatus;
        }

        private void setEcoStatus(string ecoStatus)
        {
            this.ecoStatus = ecoStatus;
        }

        private DateTime getDateCreated()
        {
            return dateCreated;
        }

        private void setDateCreated(DateTime dateCreated)
        {
            this.dateCreated = dateCreated;
        }

        public string retrieveTransportMode()
        {
            return transportMode;
        }

        public double calculateSelfEmission()
        {
            return carbonEmission;
        }

        public DateTime retrieveDateCreated()
        {
            return dateCreated;
        }
    }
}
