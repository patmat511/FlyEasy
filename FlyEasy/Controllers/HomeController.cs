using FlyEasy.Data;
using FlyEasy.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace FlyEasy.Controllers
{
    public class HomeController : Controller
    {
        private readonly FlightContext _db;

        public HomeController(FlightContext flightContext)
        {
            _db = flightContext;
        }

        public async Task<IActionResult> Index(string flightNumber, string destination)
        {
            var builder = Builders<Flight>.Filter;
            var filter = builder.Empty;

            if (!string.IsNullOrEmpty(flightNumber))
                filter = filter & builder.Regex("FlightNumber", new BsonRegularExpression(flightNumber, "i"));
            if (!string.IsNullOrEmpty(destination))
                filter = filter & builder.Eq("Destination", destination);

            var flights = await _db.Flights.Find(filter).ToListAsync();
            ViewBag.FlightNumber = flightNumber;
            ViewBag.Destination = destination;
            return View(flights);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Flight flight)
        {
            flight.Id = null;

            if (ModelState.IsValid)
            {
                try
                {
                    Console.WriteLine($"Zapis lotu (23 maja 2025, 15:13 CEST): {flight.FlightNumber}, {flight.Departure}, {flight.Destination}, {flight.DepartureTime}");
                    await _db.Flights.InsertOneAsync(flight);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"B³¹d zapisu lotu (23 maja 2025, 15:13 CEST): {ex.Message}");
                    ModelState.AddModelError("", "Wyst¹pi³ b³¹d podczas zapisywania lotu: " + ex.Message);
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                Console.WriteLine($"B³êdy walidacji (23 maja 2025, 15:13 CEST): {string.Join("; ", errors)}");
            }
            return View(flight);
        }
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var flight = await _db.Flights.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefaultAsync();
            if (flight == null) return NotFound();
            return View(flight);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var flight = await _db.Flights.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefaultAsync();
            if (flight == null) return NotFound();
            return View(flight);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Flight flight)
        {
            if (id != flight.Id) return NotFound();
            if (ModelState.IsValid)
            {
                await _db.Flights.ReplaceOneAsync(new BsonDocument("_id", new ObjectId(id)), flight);
                return RedirectToAction(nameof(Index));
            }
            return View(flight);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var flight = await _db.Flights.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefaultAsync();
            if (flight == null) return NotFound();
            return View(flight);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _db.Flights.DeleteOneAsync(new BsonDocument("_id", new ObjectId(id)));
            return RedirectToAction(nameof(Index));
        }
    }
}