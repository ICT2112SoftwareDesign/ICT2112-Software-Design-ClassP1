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
		private readonly ReturnFormMapper _mapper;
		private readonly ItemControl _itemControl;
        private readonly IConfiguration _configuration;

        public ReturnFormControl(ReturnFormMapper mapper, IConfiguration configuration, ItemControl itemControl)
		{
            _configuration = configuration;
            _mapper = mapper;
            _itemControl = itemControl;

        }

		public List<Item> displayAllRefundedItems()
		{
			return _itemControl.getRefundedItems();

        }

		public List<ReturnForm> displayReturnForms()
		{
			var allReturnForms = _mapper.getDatabaseQueryStatus(_mapper.findAll());

			return allReturnForms;
		}

		public ReturnForm? getReturnFormById(int itemId)
		{
			ReturnForm? returnForm = _mapper.getDatabaseQueryStatus(_mapper.findByItemId(itemId));

            //Item item = _itemControl.getItemById(itemId).Result;
            //Dictionary<string, object> itemDict = item.retrieveItemInfo();

            //Product product = _itemControl.retrieveProductDetails(productId).Result;
            //Debug.WriteLine(product);
            //int manufId = product.ManufacturerId;
            //string prodName = product.ProductName;

            return returnForm;
        }

        //public bool deleteReturnForm(int returnId)
        //{
        //	bool deleteResult = _mapper.getDatabaseQueryStatus(_mapper.delete(returnId));

        //          return deleteResult;
        //}

        public ReturnForm generateReturnForm(int productId, int itemId) {

            Item item = _itemControl.getItemById(itemId).Result;
            Dictionary<string, object> itemDict = item.retrieveItemInfo();

            Product product = _itemControl.retrieveProductDetails(productId).Result;
			Debug.WriteLine(product);
            int manufId = product.ManufacturerId;
            string prodName = product.ProductName;

            ReturnForm model = ReturnForm.createForm(
				0,
				manufId,
				"test@test.com", // need iManufacturer
				"test",
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

			Item item = await _itemControl.getItemById(model.GetItemId());
			Dictionary<string, object> itemDict = item.retrieveItemInfo();

			// Check whether the Item is in Available Status.
			var status = (ItemStatus)itemDict["ItemStatus"];
			Debug.WriteLine($"ItemStatus type: {status.GetType()}");


			// ItemId must not be in Return Forms table.
			ReturnForm? inRFTable = await _mapper.findByItemId(model.GetItemId());

			Debug.WriteLine($"Status: {status}");

			if (inRFTable == null && status == ItemStatus.Refunded)
			{
				// Mapper function to insert return form.
				ReturnForm? form = _mapper.getDatabaseQueryStatus(_mapper.insert(model));
				await _itemControl.updateItemStatus(model.GetItemId(), null, null, null, form!.GetReturnId(), ItemStatus.Returned);


                if (form != null)
				{

					// Get email from iManufacturer.
					// var prodManuf = await iManufaccturer.getManufacturerDetails(model.ManufacturerId);
					// string manufacturerEmail = prodManuf.email
					// string manufacturerName = prodManuf.name

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
						MailMessage mailMessage = new MailMessage(fromEmail!, fromEmail!)
						{
							Subject = "Clean Brilliant Company Return Form",
							Body = $"Return Form sent to: [MANUFACTURER NAME] Clean Brilliant Company (CBC) is requesting a return of [MANUFACTURER NAME]'s item due to the following reason: {model.GetReturnReason()}\nItem Details:\nCBC ItemID: {model.GetItemId()}\nPrice: {itemDict["SalePrice"]}",
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
