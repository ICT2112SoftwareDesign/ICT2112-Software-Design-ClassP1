using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Mapper;
using CleanBrilliantCompany.Models.Entity;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using System.Reflection;


namespace CleanBrilliantCompany.Models.Control
{
	public class ReturnFormControl : iReturnFormQuery
	{
		private readonly iReturnFormDatabase<ReturnForm> _mapper;
		//private readonly ReturnFormMapper _mapper;
		private readonly IItemUpdate _iItemUpdate;
        private readonly IItem _iItem;
        private readonly IReturnForm _iReturnForm;
		private readonly IManufacturer _iManufacturer;
        private readonly IConfiguration _configuration;

        public ReturnFormControl(iReturnFormDatabase<ReturnForm> mapper, IConfiguration configuration, IItemUpdate iItemUpdate, IItem iItem, IReturnForm iReturnForm, IManufacturer iManufacturer)
		{
            _configuration = configuration;
            _mapper = mapper;
            _iItemUpdate = iItemUpdate;
			_iItem = iItem;
			_iReturnForm = iReturnForm;
            _iManufacturer = iManufacturer;

        }

		public List<Item> displayAllToReturnItems()
		{
			return _iReturnForm.getToReturnItems().Result;

        }

		public List<ReturnForm> displayReturnForms()
		{
			var allReturnForms = _mapper.getDatabaseQueryStatus(_mapper.findAll());

			return allReturnForms;
		}

		public ReturnForm? getReturnFormById(int itemId)
		{
			ReturnForm? returnForm = _mapper.getDatabaseQueryStatus(_mapper.findByItemId(itemId));

			if (returnForm != null) {
                Item item = _iItem.getItemById(itemId).Result;
                Dictionary<string, object> itemDict = item.retrieveItemInfo();

                int productId = (int)itemDict["ProductId"];
                Product product = _iReturnForm.retrieveProductDetails(productId).Result;
                Dictionary<string, object> prodDict = product.retrieveProductInfo();

                int manufId = (int)prodDict["ManufacturerId"];
                string prodName = (string)prodDict["ProductName"];

				ProductManufacturer prodManuf = _iManufacturer.getManufacturerDetails(manufId);
                Dictionary<string, object> manufDict = prodManuf.retrieveProductManufacturerInfo();


                returnForm.SetProductId(productId);
                returnForm.SetProductName(prodName);
				returnForm.SetManufacturerName((string)manufDict["CompanyName"]);
                returnForm.SetManufacturerEmail((string)manufDict["Email"]);
            }

            return returnForm;
        }

		public bool deleteReturnForm(int productId, int itemId)
		{
			bool deleteResult = _mapper.getDatabaseQueryStatus(_mapper.delete(itemId));

            _iItemUpdate.updateItemStatus(itemId, null, null, null, null, ItemStatus.ToReturn);
            _iItemUpdate.updateProductQuantity(productId, 1, "increase");
            
            return deleteResult;
		}

		public ReturnForm generateReturnForm(int productId, int itemId) {

            Item item = _iItem.getItemById(itemId).Result;
            Dictionary<string, object> itemDict = item.retrieveItemInfo();

            Product product = _iReturnForm.retrieveProductDetails(productId).Result;
            Dictionary<string, object> prodDict = product.retrieveProductInfo();

            int manufId = (int)prodDict["ManufacturerId"];
            string prodName = (string)prodDict["ProductName"];

            ProductManufacturer prodManuf = _iManufacturer.getManufacturerDetails(manufId);
            Dictionary<string, object> manufDict = prodManuf.retrieveProductManufacturerInfo();

            string manufName = (string)manufDict["CompanyName"];
            string manufEmail = (string)manufDict["Email"];

            ReturnForm model = ReturnForm.createForm(
				0,
				manufId,
                manufEmail,
                manufName,
				itemId,
				productId,
                prodName,
				"-", // Placeholder for returnReason.
				1
			);

			return model;
        }

		public async Task<ReturnForm?> sendReturnForm(ReturnForm model)
		{
			Debug.WriteLine($"Finding item for item id: {model.GetItemId()}");

			Item item = await _iItem.getItemById((int)model.GetItemId());
			Dictionary<string, object> itemDict = item.retrieveItemInfo();

			// Check whether the Item is in ToReturn Status.
			var status = (ItemStatus)itemDict["ItemStatus"];
			Debug.WriteLine($"ItemStatus type: {status.GetType()}");


			// ItemId must not be in Return Forms table.
			ReturnForm? inRFTable = await _mapper.findByItemId((int)model.GetItemId());

			Debug.WriteLine($"Status: {status}");

			if (inRFTable == null && status == ItemStatus.ToReturn)
			{
				// Mapper function to insert return form.
				ReturnForm? form = _mapper.getDatabaseQueryStatus(_mapper.insert(model));
				await _iItemUpdate.updateItemStatus((int)model.GetItemId(), null, null, null, form!.GetReturnId(), ItemStatus.Returned);
                _iItemUpdate.updateProductQuantity((int)model.GetProductId(), 1, "decrease");

                if (form != null)
				{

                    // Get email from iManufacturer.
                    ProductManufacturer prodManuf = _iManufacturer.getManufacturerDetails((int)model.GetManufacturerId());
					string manufacturerEmail = (string)prodManuf.retrieveProductManufacturerInfo()["Email"];
					string manufacturerName = (string)prodManuf.retrieveProductManufacturerInfo()["CompanyName"];

                    try
					{

						string? fromEmail = _configuration.GetValue<string>("EMAIL_CONFIGURATION:EMAIL");
						string? password = _configuration.GetValue<string>("EMAIL_CONFIGURATION:PASSWORD");
						string? host = _configuration.GetValue<string>("EMAIL_CONFIGURATION:HOST");
						int port = _configuration.GetValue<int>("EMAIL_CONFIGURATION:PORT");


						// Set up the SMTP client
						SmtpClient smtpClient = new SmtpClient(host, port);
						smtpClient.EnableSsl = true;
						smtpClient.UseDefaultCredentials = false;
						smtpClient.Credentials = new NetworkCredential(fromEmail, password);

						// Create the email message. Replace toEmail with manufEmail
						MailMessage mailMessage = new MailMessage(fromEmail!, manufacturerEmail!)
						{
							Subject = "Clean Brilliant Company Return Form",
							Body = $"Dear {manufacturerName},\n\nClean Brilliant Company (CBC) is requesting a return of {manufacturerName}'s item due to the following reason: {model.GetReturnReason()}\n\nItem Details:\nCBC ItemID: {model.GetItemId()}\nPrice: ${itemDict["SalePrice"]}",
							IsBodyHtml = false
						};

						// Send the email (synchronous)
						await smtpClient.SendMailAsync(mailMessage);

						Debug.WriteLine("Email sent successfully!");
					}
					catch (Exception ex)
					{
						Debug.WriteLine($"Error sending email: {ex.Message}");
					}

					Debug.WriteLine("Sending return form to manufacturer by email...");

					model = ReturnForm.createForm(
						model.GetReturnId(),
						model.GetManufacturerId(),
						model.GetManufacturerName(),
						model.GetManufacturerEmail(),
						model.GetItemId(),
						model.GetProductId(),
						model.GetProductName(),
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
