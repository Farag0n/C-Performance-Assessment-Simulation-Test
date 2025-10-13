using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkyTravel.Models;
using SkyTravel.Infraestructure;

namespace SkyTravel.Controllers;

public class UserController : Controller
{
    //-------------------------------------------------------
    //Uso y acceso de la base de datos
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public UserController(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }
    //-------------------------------------------------------
    
    
    //-------------------------------------------------------
    //Vista principal de usuarios
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var users = _context.Users.ToList();
        return View(users);
    }
    //-------------------------------------------------------
    
    
    //-------------------------------------------------------\
    //Crear usuario
    //Cargar vista
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    
    //Resivir y guardar la informacion en la base de datos
    [HttpPost]
    public async Task<IActionResult> Create(User user)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View("Create");
            }

            if (_context.Users.Any(u => u.DocNumber == user.DocNumber))
            {
                Console.WriteLine("No se puede crear un usuario existente");
                
                //retornar el mensaje en la vista
                ViewBag.ErrorMessage = "No se puede crear un usuario existente";
                
                return View("Create");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Problema resiviendo la informacion, codigo de error: {e}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ha ocurrido un error inesperado: \n{e}");
            return View("Index");
        }

        return View("Index");
    }
    //-------------------------------------------------------
    
    
    //-------------------------------------------------------
    //Elimar usuario
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var user = _context.Users.FirstOrDefaultAsync(f => f.Id == id);
        if (user == null)
        {
            Console.WriteLine("No se puede eliminar algo que no existe");
            return NotFound();
        }
            
        _context.Users.Remove(await user);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
    //-------------------------------------------------------
    
    
    //-------------------------------------------------------
    // Editar usuario (cargar la vista)
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
            return NotFound();
        await _context.SaveChangesAsync();
        return View(user);
    }
    
    // Editar vuelo resivir informacion
    [HttpPost]
    public async Task<IActionResult> Edit(int id, User user)
    {
        var existUser = await _context.Users.FindAsync(id);
        if (existUser == null)
            return NotFound();

        // Actualiza solo los campos que quieres permitir
        existUser.Name = user.Name;
        existUser.DocNumber = user.DocNumber;
        existUser.Email = user.Email;
        existUser.CelNumber = user.CelNumber;

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    //-------------------------------------------------------
}