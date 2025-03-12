using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Mapper;
using CleanBrilliantCompany.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace CleanBrilliantCompany.Control
{
	public class ReturnFormControl : iReturnFormQuery
    {
		private readonly ReturnFormMapper _mapper;

		public ReturnFormControl(ReturnFormMapper mapper)
		{
			_mapper = mapper;
		}

		public List<ReturnForm> displayReturnForms()
		{
			var allReturnForms = _mapper.getDatabaseQueryStatus(_mapper.findAll());

			return allReturnForms;
		}

		public ReturnForm? getReturnFormById(int returnId)
		{
			return _mapper.getDatabaseQueryStatus(_mapper.findByItemId(returnId));
		}

        public bool deleteReturnForm(int returnId)
		{
			bool deleteResult = _mapper.getDatabaseQueryStatus(_mapper.delete(returnId));
			
			return deleteResult;
		}

		public ReturnForm? insertReturnForm(ReturnForm model)
		{
			return _mapper.getDatabaseQueryStatus(_mapper.insert(model));
		}

		public ReturnForm? sendReturnForm(ReturnForm model)
		{

            // Check whether the Item is in Available Status.
            string status = _mapper.getDatabaseQueryStatus(_mapper.getItemStatusByItemId(model.GetItemId()));

			// Query DB to get the Warehouse Id using the Item Id. WarehouseId must not be -1 (placeholder).
			int warehouseId = _mapper.getDatabaseQueryStatus(_mapper.getWarehouseIdByItemId(model.GetItemId()));

			model.SetWarehouseId(warehouseId);

			// ItemId must not be in Return Forms table.
			ReturnForm? inRFTable = _mapper.getDatabaseQueryStatus(_mapper.findByItemId(model.GetItemId()));

			Debug.WriteLine($"Status: {status}");
			Debug.WriteLine($"Warehouse Id found: {warehouseId}");

			if (inRFTable == null && status == "Available" &&  warehouseId != -1)
			{
				// Mapper function to insert return form.
				ReturnForm? form = insertReturnForm(model);

				if (form != null)
				{
					// Query the Product Manufacturer table to find Email
					// Send the Email
					// var email = await _mapper.getEmailByManufacturerId(model.ManufacturerId);
					Debug.WriteLine("Sending return form to manufacturer by email...");

					model = ReturnForm.createForm(
						model.GetReturnId(),
						model.GetManufacturerId(),
						model.GetItemId(),
						model.GetWarehouseId(),
						model.GetReturnReason(),
						model.GetStaffId()
						);
					return model;
				}
				else
				{
					return null;
				}
			}

			return null;
		}


	}
}
