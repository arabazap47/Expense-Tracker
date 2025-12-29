using ExpenseDashboard.Api.Data;
using ExpenseDashboard.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExpenseDashboard.Api.Pages
{
    public class EditExpenseModel : PageModel
    {
        private readonly AppDbContext _context;

        public EditExpenseModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Expense Expense { get; set; } = new();

        public List<SelectListItem> Categories { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Expense = await _context.Expenses
                .Include(e => e.Category)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (Expense == null)
                return NotFound();

            Categories = _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _context.Expenses.Update(Expense);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index", new { refresh = true });

        }
    }
}
