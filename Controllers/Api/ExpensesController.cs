using ExpenseDashboard.Api.Data;
using ExpenseDashboard.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseDashboard.Api.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class ExpensesController : ControllerBase
{
    private readonly AppDbContext _db;
    public ExpensesController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _db.Expenses.Include(e => e.Category).OrderByDescending(e => e.Date).ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Post(Expense e)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        _db.Expenses.Add(e);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = e.Id }, e);
    }
}