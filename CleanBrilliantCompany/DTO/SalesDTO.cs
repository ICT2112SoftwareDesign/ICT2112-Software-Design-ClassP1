namespace CleanBrilliantCompany.DTO
{
    public class SalesDTO
    {
        private int productID;
        private DateTime dateTime;
        private int quantity;

        public int ItemID { get { return productID; } set { productID = value; } }
        public DateTime DateTime { get { return dateTime; } set { dateTime = value; } }             
        public int Quantity { get { return quantity; } set { quantity = value; } }  
        //public float SalesAmount { get { return salesAmount; } set { salesAmount = value; } }
        public SalesDTO(int productId,DateTime dateTime, int quantity) {
            this.productID = productId;
            this.dateTime = dateTime;
            this.quantity = quantity;
        }
        public SalesDTO() { }

    }
}
