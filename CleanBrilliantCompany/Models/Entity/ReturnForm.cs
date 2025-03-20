namespace CleanBrilliantCompany.Models.Entity
{
	public class ReturnForm
	{
		private int _returnId;
		private int _manufacturerId;
		private string _manufacturerName;
        private string _manufacturerEmail;
        private int _productId;
        private string _productName;
        private int _itemId;
		private string? _returnReason;
		private int _staffId;

		// Private getters and setters
		private int returnId
		{
			get { return _returnId; }
			set { _returnId = value; }
		}

		private int manufacturerId
		{
			get { return _manufacturerId; }
			set { _manufacturerId = value; }
		}
        private string manufacturerName
        {
            get { return _manufacturerName; }
            set { _manufacturerName = value; }
        }
        private string manufacturerEmail
        {
            get { return _manufacturerEmail; }
            set { _manufacturerEmail = value; }
        }
        private int itemId
		{
			get { return _itemId; }
			set { _itemId = value; }
		}
        private int productId
		{
			get { return _productId; }
			set { _productId = value; }
		}
        private string productName
        {
            get { return _productName; }
            set { _productName = value; }
        }
        private string? returnReason
		{
			get { return _returnReason; }
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new ArgumentException("Return reason cannot be empty.");
				}

				_returnReason = value;
			}
		}

		private int staffId
		{
			get { return _staffId; }
			set { _staffId = value; }
		}


		// Public getters and setters
		public int GetReturnId() => returnId;
		public void SetReturnId(int value) => returnId = value;

		public int GetManufacturerId() => manufacturerId;
		public void SetManufacturerId(int value) => manufacturerId = value;
        public string GetManufacturerName() => manufacturerName;
        public void SetManufacturerName(string value) => manufacturerName = value;
        public string GetManufacturerEmail() => manufacturerEmail;
        public void SetManufacturerEmail(string value) => manufacturerEmail = value;
        public int GetItemId() => itemId;
		public void SetItemId(int value) => itemId = value;
        public int GetProductId() => productId;
		public void SetProductId(int value) => productId = value;
        public string GetProductName() => productName;
        public void SetProductName(string value) => productName = value;
        public string? GetReturnReason() => returnReason;
		public void SetReturnReason(string? value) => returnReason = value;

		public int GetStaffId() => staffId;
		public void SetStaffId(int value) => staffId = value;


		public ReturnForm() { }

		private ReturnForm(int returnId, int manufId, string manufName, string manufEmail, int itemId, int productId, string productName, string returnReason, int staffId)
		{
			SetReturnId(returnId);
			SetItemId(itemId);
            SetManufacturerId(manufId);
            SetManufacturerName(manufName);
            SetManufacturerEmail(manufEmail);
            SetProductId(productId);
            SetProductName(productName);
            SetReturnReason(returnReason);
			SetStaffId(staffId);
		}

		private ReturnForm(int returnId, int manufId, int itemId, string returnReason, int staffId) {
            SetReturnId(returnId);
            SetItemId(itemId);
            SetManufacturerId(manufId);
            SetReturnReason(returnReason);
            SetStaffId(staffId);
        }
		public static ReturnForm createForm(int returnId, int manufId, int itemId, string returnReason, int staffId) {

            var form = new ReturnForm(returnId, manufId, itemId, returnReason, staffId);
            return form;
        }

        // Public method to create a return form.
        public static ReturnForm createForm(int returnId, int manufId, string manufName, string manufEmail, int itemId, int productId, string productName, string returnReason, int staffId)
		{
			var form = new ReturnForm(returnId, manufId, manufName, manufEmail, itemId, productId, productName, returnReason, staffId);
			return form;
		}
	}
}
