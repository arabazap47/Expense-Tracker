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
        public List<Expense> Expenses { get; set; } = new List<Expense>();

        public List<Category> Categories { get; set; } = new();

        public AddExpenseModel(AppDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            Categories = _context.Categories.ToList();
            Expenses = _context.Expenses
                .Include(e => e.Category)
                .OrderByDescending(e => e.Date)
                .ToList();
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

            // Redirect to dashboard after saving
            return RedirectToPage("/Index");
        }
    }
}
