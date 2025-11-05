using ExpenseDashboard.Api.Data;
using ExpenseDashboard.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ExpenseDashboard.Api.Pages
{
    public class AddExpenseModel : PageModel
    {
        private readonly AppDbContext _context;

        [BindProperty]
        public Expense NewExpense { get; set; } = new Expense();

        // Only display expenses after adding, so empty by default
        public List<Expense> Expenses { get; set; } = new List<Expense>();

        public List<Category> Categories { get; set; } = new();

        public AddExpenseModel(AppDbContext context)
        {
            _context = context;
        }

        
        public async Task OnGetAsync()
        {
            Categories = _context.Categories.ToList();
            // Load the latest 5 expenses only if there are any
            var allExpenses = await _context.Expenses
                .Include(e => e.Category)
                .OrderByDescending(e => e.Date)
                .ToListAsync();

            Expenses = allExpenses.Any() ? allExpenses.Take(5).ToList() : new List<Expense>();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Categories = _context.Categories.ToList();
                return Page();
            }

            if (NewExpense.Date == DateTime.MinValue)
                NewExpense.Date = DateTime.UtcNow;

            _context.Expenses.Add(NewExpense);
            _context.SaveChanges();

            // After adding, show only the newly added expense
            Expenses = new List<Expense> { NewExpense };
            Categories = _context.Categories.ToList();

            // Return to the same page so right panel updates
            return Page();
        }
    }
}
