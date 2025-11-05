using ExpenseDashboard.Api.Data;
using ExpenseDashboard.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore; // <-- needed for Include()
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseDashboard.Api.Pages
{
    public class ViewExpensesModel : PageModel
    {
        private readonly AppDbContext _context;

        public List<Expense> Expenses { get; set; }

        public ViewExpensesModel(AppDbContext context)
        {
            _context = context;
            Expenses = new List<Expense>();
        }

        // Use async OnGetAsync
        public async Task OnGetAsync()
        {
            Expenses = await _context.Expenses
                .Include(e => e.Category) // <-- important to load Category
                .OrderByDescending(e => e.Date) // optional, most recent first
                .ToListAsync();
        }
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense != null)
            {
                _context.Expenses.Remove(expense);
                await _context.SaveChangesAsync();
            }

            // Redirect to same page to refresh the list
            return RedirectToPage();
        }

    }
}
