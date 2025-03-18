using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Mapper;
using CleanBrilliantCompany.Models.Entity;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Net.Mail;
using System;
using System.Net;
using Google.Apis.Gmail.v1;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.Gmail.v1.Data;
using System.Text;


namespace CleanBrilliantCompany.Models.Control
{
	public class ReturnFormControl : iReturnFormQuery
	{
		private readonly ReturnFormMapper _mapper;
        private readonly IConfiguration _configuration;

        public ReturnFormControl(ReturnFormMapper mapper, IConfiguration configuration)
		{
            _configuration = configuration;
            _mapper = mapper;
		}

		public List<ReturnForm> displayReturnForms()
		{
			var allReturnForms = _mapper.findAll().Result;

			return allReturnForms;
		}

		public ReturnForm? getReturnFormById(int returnId)
		{
			return _mapper.findByItemId(returnId).Result;
		}

		public bool deleteReturnForm(int returnId)
		{
			bool deleteResult = _mapper.delete(returnId).Result;

			return deleteResult;
		}

		public ReturnForm? insertReturnForm(ReturnForm model)
		{
			return _mapper.insert(model).Result;
		}




        public ReturnForm? sendReturnForm(ReturnForm model)
		{

			// Check whether the Item is in Available Status.
			string status = _mapper.getItemStatusByItemId(model.GetItemId()).Result;

			// Query DB to get the Warehouse Id using the Item Id. WarehouseId must not be -1 (placeholder).
			int warehouseId = _mapper.getWarehouseIdByItemId(model.GetItemId()).Result;

			model.SetWarehouseId(warehouseId);

			// ItemId must not be in Return Forms table.
			ReturnForm? inRFTable = _mapper.findByItemId(model.GetItemId()).Result;

			Debug.WriteLine($"Status: {status}");
			Debug.WriteLine($"Warehouse Id found: {warehouseId}");

			if (inRFTable == null && status == "Available" && warehouseId != -1)
			{
				// Mapper function to insert return form.
				ReturnForm? form = insertReturnForm(model);

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
							Body = "Return Form sent to: [MANUFACTURER NAME] Clean Brilliant Company (CBC) is requesting a return of [MANUFACTURER NAME]'s [PRODUCT DETAILS], itemID #[ITEMID] due to the following reason: [RETURN REASON]",
							IsBodyHtml = false
						};

						// Send the email (synchronous)
						smtpClient.Send(mailMessage);

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
