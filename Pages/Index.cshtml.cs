using Microsoft.AspNetCore.Mvc.RazorPages;
using ExpenseDashboard.Api.Data;
using ExpenseDashboard.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ExpenseDashboard.Api.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public List<Expense> Expenses { get; set; } = new List<Expense>();

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            // Include category details for display
            Expenses = _context.Expenses
                .Include(e => e.Category)
                .OrderByDescending(e => e.Date)
                .ToList();
        }
    }
}
