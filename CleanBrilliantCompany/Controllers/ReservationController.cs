using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models.Control;
using CleanBrilliantCompany.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models.ViewModel;
using Microsoft.Extensions.Configuration;

namespace CleanBrilliantCompany.Controllers
{
    [Route("inventory/management/reservation")]
    public class ReservationController : Controller
    {
        private readonly ReservationControl _reservationControl;

        public ReservationController(IConfiguration configuration)
        {
            _reservationControl = new ReservationControl(configuration);
        }

        // default get all items
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Dictionary<string, object>> reservationDict = new List<Dictionary<string, object>>();
            List<Reservation> reservations = await _reservationControl.ShowReservations();

            foreach (var reservation in reservations)
            {
                reservationDict.Add(reservation.GetReservationDetails());
            }

            return View(reservationDict);
        }

        [HttpPost]
        [Route("searchById")]
        public async Task<IActionResult> searchById(int reservationId)
        {
            List<Dictionary<string, object>> reservationDict = new List<Dictionary<string, object>>();
            Reservation? reservation = await _reservationControl.GetReservationById(reservationId);

            if (reservation != null)
            {
                reservationDict.Add(reservation.GetReservationDetails());
            }

            return View("Index", reservationDict);  // Reuse Index view
        }


        [HttpPost]
        [Route("reserveStock")]
        public async Task<IActionResult> reserveStock(int reservedQuantity, int warehouseId, int productId, string reservationPurpose, int staffId)
        {
            string result = await _reservationControl.ReserveStock(reservedQuantity, warehouseId, productId, reservationPurpose, staffId);
            if (result.Contains("Error"))
            {
                return BadRequest(new { error = "Failed to reserve stock." });
            }
            else
            {   
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Route("updateReservationQuantity")]
        public async Task<IActionResult> updateReservationQuantity(int reservationId, int quantity, int staffId)
        {
            Console.WriteLine("ReservationID: " + reservationId);
            Console.WriteLine("New Quantity: " + quantity);

            string result = await _reservationControl.UpdateReservationQuantity(reservationId, quantity, staffId);
            if (result.Contains("Error"))
            {
                return BadRequest(new { error = "Failed to update quantity." });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Route("updateReservationPurpose")]
        public async Task<IActionResult> updateReservationPurpose(int reservationId, string reservationPurpose, int staffId)
        {
            Console.WriteLine("ReservationID: " + reservationId);
            Console.WriteLine("New Purpose: " + reservationPurpose);

            string result = await _reservationControl.UpdateReservationPurpose(reservationId, reservationPurpose, staffId);
            if (result.Contains("Error"))
            {
                return BadRequest(new { error = "Failed to update purpose." });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

    }
}
