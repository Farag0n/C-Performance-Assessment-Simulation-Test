using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkyTravel.Models;
using SkyTravel.Infraestructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace SkyTravel.Controllers;
public class ReservFlightController : Controller
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public ReservFlightController(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    // ----------------------------------------------------------
    // INDEX - Mostrar todas las reservas
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var reservFlights = await _context.ReservFlights
            .Include(r => r.Flight)
            .Include(r => r.User)
            .ToListAsync();

        return View(reservFlights);
    }

    //-------------------------------------------------------
    // GET: Crear reserva
    //-------------------------------------------------------
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var availableFlights = await _context.Flights
            .Where(f => f.Departure.Date >= DateTime.Now.Date.AddDays(20))
            .ToListAsync();

        var users = await _context.Users.ToListAsync();

        ViewBag.Flights = new SelectList(availableFlights, "Id", "FlightCode");
        ViewBag.Users = new SelectList(users, "Id", "Name");

        return View();
    }

    //-------------------------------------------------------
    // POST: Crear reserva
    //-------------------------------------------------------
    [HttpPost]
    public async Task<IActionResult> Create(ReservFlight reservFlight)
    {
        var userHasReservation = await _context.ReservFlights
            .AnyAsync(r => r.UserId == reservFlight.UserId);

        if (userHasReservation)
            ModelState.AddModelError("UserId", "Este usuario ya tiene una reserva registrada.");

        var flight = await _context.Flights.FindAsync(reservFlight.FlightId);
        if (flight == null)
        {
            ModelState.AddModelError("FlightId", "El vuelo seleccionado no existe.");
        }
        else if (flight.Departure.Date < DateTime.Now.Date.AddDays(20))
        {
            ModelState.AddModelError("FlightId", "Solo se pueden reservar vuelos que salgan en 20 días o más.");
        }

        if (!ModelState.IsValid)
        {
            var availableFlights = await _context.Flights
                .Where(f => f.Departure.Date >= DateTime.Now.Date.AddDays(20))
                .ToListAsync();

            var users = await _context.Users.ToListAsync();

            ViewBag.Flights = new SelectList(availableFlights, "Id", "FlightCode");
            ViewBag.Users = new SelectList(users, "Id", "Name");

            return View(reservFlight);
        }

        reservFlight.ReservationDate = DateOnly.FromDateTime(DateTime.Now);
        _context.ReservFlights.Add(reservFlight);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "ReservFlight");
    }

    // ----------------------------------------------------------
    // EDITAR - Mostrar formulario
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var reservFlight = await _context.ReservFlights.FindAsync(id);
        if (reservFlight == null)
            return NotFound();

        var availableFlights = await _context.Flights
            .Where(f => f.Departure.Date >= DateTime.Now.Date.AddDays(20))
            .ToListAsync();

        var users = await _context.Users.ToListAsync();

        ViewBag.Flights = new SelectList(availableFlights, "Id", "FlightCode");
        ViewBag.Users = new SelectList(users, "Id", "Name");

        return View(reservFlight);
    }

    // ----------------------------------------------------------
    // EDITAR - Guardar cambios
    [HttpPost]
    public async Task<IActionResult> Edit(int id, ReservFlight reservFlight)
    {
        var existing = await _context.ReservFlights.FindAsync(id);
        if (existing == null)
            return NotFound();

        var flight = await _context.Flights.FindAsync(reservFlight.FlightId);
        if (flight == null)
        {
            ModelState.AddModelError("", "El vuelo seleccionado no existe.");
        }
        else if (flight.Departure < DateTime.Now.AddDays(20))
        {
            ModelState.AddModelError("", "Solo se pueden reservar vuelos que salgan en 20 días o más.");
        }

        bool otraReserva = await _context.ReservFlights
            .AnyAsync(r => r.UserId == reservFlight.UserId && r.Id != id);

        if (otraReserva)
            ModelState.AddModelError("", "El usuario ya tiene otra reserva registrada.");

        if (!ModelState.IsValid)
        {
            var availableFlights = await _context.Flights
                .Where(f => f.Departure.Date >= DateTime.Now.Date.AddDays(20))
                .ToListAsync();

            var users = await _context.Users.ToListAsync();

            ViewBag.Flights = new SelectList(availableFlights, "Id", "FlightCode");
            ViewBag.Users = new SelectList(users, "Id", "Name");

            return View(reservFlight);
        }

        existing.FlightId = reservFlight.FlightId;
        existing.UserId = reservFlight.UserId;
        existing.Pay = reservFlight.Pay;

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // ----------------------------------------------------------
    // ELIMINAR
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var reserv = await _context.ReservFlights
            .Include(r => r.Flight)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (reserv == null)
            return NotFound();

        if (int.TryParse(reserv.Flight.Capacity, out int capacidad))
            reserv.Flight.Capacity = (capacidad + 1).ToString();

        _context.ReservFlights.Remove(reserv);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    // ----------------------------------------------------------
    // GENERAR TICKET PDF (versión QuestPDF)
    [HttpGet]
    public async Task<IActionResult> GenerateTicket(int id)
    {
        var reserv = await _context.ReservFlights
            .Include(r => r.User)
            .Include(r => r.Flight)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (reserv == null)
            return NotFound();

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(40);
                page.Size(PageSizes.A4);
                page.DefaultTextStyle(x => x.FontSize(14));

                page.Header()
                    .Text("🎟️ SKYTRAVEL - TICKET DE RESERVA")
                    .Bold()
                    .FontSize(20)
                    .AlignCenter();

                page.Content().PaddingVertical(15).Column(col =>
                {
                    col.Item().Text($"Pasajero: {reserv.User?.Name ?? "N/A"}");
                    col.Item().Text($"Documento: {reserv.User?.DocNumber ?? "N/A"}");
                    col.Item().Text($"Correo: {reserv.User?.Email ?? "N/A"}");
                    col.Item().Text($"Vuelo: {reserv.Flight?.FlightCode ?? "N/A"}");
                    col.Item().Text($"Origen: {reserv.Flight?.Origin ?? "N/A"}");
                    col.Item().Text($"Destino: {reserv.Flight?.Destination ?? "N/A"}");
                    col.Item().Text($"Salida: {reserv.Flight?.Departure:g}");
                    col.Item().Text($"Fecha de reserva: {reserv.ReservationDate:dd/MM/yyyy}");
                    col.Item().Text($"Estado del pago: {reserv.Pay}");
                    col.Item().LineHorizontal(1);
                    col.Item().AlignCenter().Text("Gracias por volar con SkyTravel ✈️");
                });

                page.Footer()
                    .AlignCenter()
                    .Text("“El buen código es como un buen espresso: concentrado, potente y mejor si se sirve caliente.” — J.A.R.V.I.S.");
            });
        });

        var pdfBytes = document.GeneratePdf();
        return File(pdfBytes, "application/pdf", $"Ticket_Reserva_{reserv.Id}.pdf");
    }
}

