using Microsoft.AspNetCore.Mvc.RazorPages;
using ExpenseDashboard.Api.Data;
using ExpenseDashboard.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task OnGetAsync()
        {
            // Load the latest 5 expenses only if there are any
            var allExpenses = await _context.Expenses
                .Include(e => e.Category)
                .OrderByDescending(e => e.Date)
                .ToListAsync();

            Expenses = allExpenses.Any() ? allExpenses.Take(5).ToList() : new List<Expense>();
        }

        public decimal TotalThisMonth => Expenses
            .Where(e => e.Date.Month == System.DateTime.Now.Month && e.Date.Year == System.DateTime.Now.Year)
            .Sum(e => e.Amount);

        public decimal RemainingBudget => 10000 - TotalThisMonth;

        public decimal TotalYTD => Expenses
            .Where(e => e.Date.Year == System.DateTime.Now.Year)
            .Sum(e => e.Amount);
    }
}
