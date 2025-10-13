using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkyTravel.Models;
using SkyTravel.Infraestructure;

namespace SkyTravel.Controllers;

public class FlightController : Controller
{
    //-------------------------------------------------------
    //Acceso, conexion y posibilidad para usar la db
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public FlightController(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }
    //-------------------------------------------------------
    
    
    //-------------------------------------------------------
    //Vista principal de vuelos
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var flights = _context.Flights.ToList();
        return View(flights);
    }
    //-------------------------------------------------------
    
    
    //-------------------------------------------------------
    //Crear vuelo
    //Cargar vista
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    //Resivir y guardar los datos en la base de datos
    [HttpPost]
    public async Task<IActionResult> Create(Flight flight)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View("Create");
            }

            if (_context.Flights.Any(f => f.FlightCode == flight.FlightCode))
            {
                Console.WriteLine("No se puede crear un vuelo con el mismo codigo unico");
                
                //retornar el mensaje en la vista
                ViewBag.ErrorMessage = "No se puede crear un vuelo con el mismo codigo unico";
                
                return View("Create");
            }

            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Index");
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Problema resiviendo la informacion, codigo de error: {e}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ocurrio un error inesperado, codigo de error: {e}");
            return View("Index");
        }

        return View("Index");
    }
    //-------------------------------------------------------
    
    
    //-------------------------------------------------------
    //Eliminar vuelo
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var flight = _context.Flights.FirstOrDefaultAsync(f => f.Id == id);
        if (flight == null)
        {
            Console.WriteLine("No se puede eliminar algo que no existe");
            return NotFound();
        }

        _context.Flights.Remove(await flight);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");

    }
    //-------------------------------------------------------
    
    
    //-------------------------------------------------------
    // Editar vuelo (cargar la vista)
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var flight = await _context.Flights.FirstOrDefaultAsync(f => f.Id == id);
        if (flight == null)
            return NotFound();
        await _context.SaveChangesAsync();
        return View(flight);
    }
    // Editar vuelo resiver informacion
    [HttpPost]
    public async Task<IActionResult> Edit(int id, Flight flight)
    {
        var existFlight = await _context.Flights.FindAsync(id);
        if (existFlight == null)
            return NotFound();

        // Actualiza solo los campos que quieres permitir
        existFlight.Origin = flight.Origin;
        existFlight.Destination = flight.Destination;
        existFlight.Departure = flight.Departure;
        existFlight.Arrival = flight.Arrival;
        existFlight.Capacity = flight.Capacity;
        existFlight.State = flight.State;

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    //-------------------------------------------------------
}