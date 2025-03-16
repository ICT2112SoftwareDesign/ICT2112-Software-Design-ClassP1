namespace CleanBrilliantCompany.Models.Entity
{
	public class ReturnForm
	{
		private int _returnId;
		private int _manufacturerId;
		private int _itemId;
		private int _warehouseId;
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

		private int itemId
		{
			get { return _itemId; }
			set { _itemId = value; }
		}

		private int warehouseId
		{
			get { return _warehouseId; }
			set { _warehouseId = value; }
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

		public int GetItemId() => itemId;
		public void SetItemId(int value) => itemId = value;
		public int GetWarehouseId() => warehouseId;
		public void SetWarehouseId(int value) => warehouseId = value;

		public string? GetReturnReason() => returnReason;
		public void SetReturnReason(string? value) => returnReason = value;

		public int GetStaffId() => staffId;
		public void SetStaffId(int value) => staffId = value;


		public ReturnForm() { }

		private ReturnForm(int returnId, int manufacturerId, int itemId, int warehouseId, string returnReason, int staffId)
		{
			SetReturnId(returnId);
			SetItemId(itemId);
			SetManufacturerId(manufacturerId);
			SetWarehouseId(warehouseId);
			SetReturnReason(returnReason);
			SetStaffId(staffId);
		}

		// Public method to create a return form.
		public static ReturnForm createForm(int returnId, int manufacturerId, int itemId, int warehouseId, string returnReason, int staffId)
		{
			var form = new ReturnForm(returnId, manufacturerId, itemId, warehouseId, returnReason, staffId);
			return form;
		}
	}
}
