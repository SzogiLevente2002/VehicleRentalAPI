using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleRentalAPI.Context;
using VehicleRentalAPI.Entities;

[Route("api/[controller]")]
[ApiController]
public class RentalsController : ControllerBase
{
    private readonly VehicleRentalContext _context;

    public RentalsController(VehicleRentalContext context)
    {
        _context = context;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<Rental>>> GetRentals()
    {
        return await _context.Rentals
                             .Include(o => o.Customer)
                             .Include(o => o.Vehicle)
                             .ToListAsync();
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Rental>> GetRental(int id)
    {
        var rental = await _context.Rentals
                                   .Include(o => o.Customer)
                                   .Include(o => o.Vehicle)
                                   .FirstOrDefaultAsync(o => o.Id == id);

        if (rental == null)
        {
            return NotFound();
        }

        return rental;
    }


    [HttpPost]
    public async Task<ActionResult<Rental>> PostRental(Rental rental)
    {
        _context.Rentals.Add(rental);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetRental), new { id = rental.Id }, rental);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> PutRental(int id, Rental rental)
    {
        if (id != rental.Id)
        {
            return BadRequest();
        }

        _context.Entry(rental).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Rentals.Any(e => e.Id == id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRental(int id)
    {
        var rental = await _context.Rentals.FindAsync(id);
        if (rental == null)
        {
            return NotFound();
        }

        _context.Rentals.Remove(rental);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}