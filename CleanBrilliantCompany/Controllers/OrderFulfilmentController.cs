using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CleanBrilliantCompany.Controllers
{
    [Route("staff/orderfulfilment")]
    public class OrderFulfilmentController : Controller
    {
        private readonly IOrder _orderFulfilmentManagement;

        public OrderFulfilmentController(IOrder orderFulfilmentManagement)
        {
            _orderFulfilmentManagement = orderFulfilmentManagement;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            try
            {
                // If you need to pass data to the view, fetch it here
                return View("~/Views/OrderFulfilment/orderfulfilment.cshtml");
            }
            catch (Exception ex)
            {
                // Log error here if needed
                return StatusCode(500, "Error loading order fulfilment page");
            }
        }


        public IActionResult Details(int orderId)
        {
            try
            {
                var order = _orderFulfilmentManagement.getOrderDetails(orderId);
                if (order == null)
                {
                    return NotFound();
                }
                return View(order);
            }
            catch (Exception ex)
            {
                // Log error here if needed
                return StatusCode(500, "Error retrieving order details");
            }

        }

        [HttpPost("UpdateStatus")]
        public IActionResult UpdateStatus([FromBody] StatusUpdateRequest request)
        {
            try
            {
                Console.WriteLine($"DEBUG: UpdateStatus called with OrderID: {request.orderId}, Status: {request.status}");

                if (request.orderId <= 0)
                {
                    Console.WriteLine("ERROR: Invalid Order ID");
                    return Json(new { success = false, message = "Invalid Order ID" });
                }

                if (string.IsNullOrEmpty(request.status))
                {
                    Console.WriteLine("ERROR: Status is null or empty");
                    return Json(new { success = false, message = "Status cannot be empty" });
                }

                bool success = _orderFulfilmentManagement.updateOrderStatus(request.orderId, request.status);

                Console.WriteLine($"DEBUG: Update result: {success}");

                return Json(new
                {
                    success,
                    message = success ? "Status updated successfully" : "Failed to update status"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR in UpdateStatus: {ex}");
                return Json(new
                {
                    success = false,
                    error = ex.Message
                });
            }
        }

        public class StatusUpdateRequest
        {
            public int orderId { get; set; }
            public string status { get; set; }
        }
        // GET: /OrderFulfilment/History/5
        public IActionResult History(int customerId)
        {
            try
            {
                var orders = _orderFulfilmentManagement.getOrderHistory(customerId);
                return View(orders);
            }
            catch (Exception ex)
            {
                // Log error here if needed
                return StatusCode(500, "Error retrieving order history");
            }
        }

        // POST: /OrderFulfilment/Cancel/5
        [HttpPost]
        public IActionResult Cancel(int orderId)
        {
            try
            {
                bool success = _orderFulfilmentManagement.cancelOrder(orderId);
                return Json(new { success });
            }
            catch (Exception ex)
            {
                // Log error here if needed
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
}