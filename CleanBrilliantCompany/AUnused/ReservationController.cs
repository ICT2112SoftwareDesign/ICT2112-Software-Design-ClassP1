using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Mappers;
using CleanBrilliantCompany.Models.Control;
using CleanBrilliantCompany.Models.Entity;
using Microsoft.AspNetCore.Mvc;
// using CleanBrilliantCompany.Models.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CleanBrilliantCompany.Controllers
{
    [Route("inventory/management/stockflow/reservation")]
    public class ReservationController : Controller
    {
        private readonly ReservationControl _reservationControl;

        public ReservationController(IConfiguration configuration, IItem item, IItemUpdate itemUpdate, IReserve reserve)
        {
            _reservationControl = new ReservationControl(configuration, item, itemUpdate, reserve);
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
                return BadRequest(new { error = "Failed to reserve stock. : " + result });
            }
            else
            {   
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        [Route("reserveitemstatus")]
        public async Task<IActionResult> Index1()
        {
            List<Dictionary<string, object>> itemsInfo = new List<Dictionary<string, object>>();
            List<Item> items = await _reservationControl.GetItemStatus(ItemStatus.Available);

            foreach (var item in items)
            {
                itemsInfo.Add(item.retrieveItemInfo());
            }

            return View(itemsInfo);
        }

        [HttpGet]
        [Route("reserveproducts")]
        public async Task<IActionResult> Reserve()
        {
            List<Dictionary<string, object>> products = await _reservationControl.GetProductList();

            return View(products);
        }

        [HttpPost]
        [Route("returnReservation")]
        public async Task<IActionResult> returnReservation(int reservationId, int staffId)
        {
            Reservation reservation = await _reservationControl.GetReservationById(reservationId);
            string result = await _reservationControl.ReturnReservedStockToinventory(reservation, staffId);
            if (result.Contains("Error"))
            {
                return BadRequest(new { error = "Failed to return reservation : " + result });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Route("updateReservation")]
        public async Task<IActionResult> updateReservation(int reservationId, int? reservedQuantity, string reservationPurpose, int staffId)
        {
            Console.WriteLine("ReservationID: " + reservationId);
            Console.WriteLine("New Quantity: " + reservedQuantity);
            Console.WriteLine("New Purpose: " + reservationPurpose);
            string resultq = "", resultp = "";

            if (reservedQuantity != null)
            {
                resultq = await _reservationControl.UpdateReservationQuantity(reservationId, (int)reservedQuantity, staffId);
                Console.WriteLine("UpdateReservationQuantity Runned");
            }
            if (reservationPurpose !=null)
            {
                resultp = await _reservationControl.UpdateReservationPurpose(reservationId, reservationPurpose, staffId);
                Console.WriteLine("UpdateReservationPurpose Runned");
            }
            if (resultq.Contains("Error") && resultp.Contains("Error"))
            {
                return BadRequest(new { error = "Failed to update quantity & purpose. : " + resultq + " : " + resultp });
            }
            else if (resultq.Contains("Error"))
            {
                return BadRequest(new { error = "Failed to update quantity. : " + resultq });
            }
            else if (resultp.Contains("Error"))
            {
                return BadRequest(new { error = "Failed to update purpose. : " + resultp });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Route("updateReservationQuantity")]
        public async Task<IActionResult> updateReservationQuantity(int reservationId, int reservedQuantity, int staffId)
        {
            Console.WriteLine("ReservationID: " + reservationId);
            Console.WriteLine("New Quantity: " + reservedQuantity);

            string result = await _reservationControl.UpdateReservationQuantity(reservationId, reservedQuantity, staffId);
            if (result.Contains("Error"))
            {
                return BadRequest(new { error = "Failed to update quantity. : " + result });
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
                return BadRequest(new { error = "Failed to update purpose. : " + result });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

    }
}
