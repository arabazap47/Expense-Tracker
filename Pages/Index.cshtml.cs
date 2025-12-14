//using Microsoft.AspNetCore.Mvc.RazorPages;
//using ExpenseDashboard.Api.Data;
//using ExpenseDashboard.Api.Models;
//using Microsoft.EntityFrameworkCore;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace ExpenseDashboard.Api.Pages
//{
//    public class IndexModel : PageModel
//    {
//        private readonly AppDbContext _context;

//        public List<Expense> Expenses { get; set; } = new List<Expense>();

//        public IndexModel(AppDbContext context)
//        {
//            _context = context;
//        }

//        public async Task OnGetAsync()
//        {
//            // Load the latest 5 expenses only if there are any
//            var allExpenses = await _context.Expenses
//                .Include(e => e.Category)
//                .OrderByDescending(e => e.Date)
//                .ToListAsync();

//            Expenses = allExpenses.Any() ? allExpenses.Take(5).ToList() : new List<Expense>();
//        }

//        public decimal TotalThisMonth => Expenses
//            .Where(e => e.Date.Month == System.DateTime.Now.Month && e.Date.Year == System.DateTime.Now.Year)
//            .Sum(e => e.Amount);

//        public decimal RemainingBudget => 10000 - TotalThisMonth;

//        public decimal TotalYTD => Expenses
//            .Where(e => e.Date.Year == System.DateTime.Now.Year)
//            .Sum(e => e.Amount);
//    }
//}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ExpenseDashboard.Api.Data;
using ExpenseDashboard.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseDashboard.Api.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        // ===== DB DATA =====
        public List<Expense> RecentExpenses { get; set; } = new();
        public List<Expense> AllExpenses { get; set; } = new();
        public UserSettings? Settings { get; set; }

        // ===== FORM INPUT (IMPORTANT) =====
        [BindProperty]
        public string UserName { get; set; } = string.Empty;

        [BindProperty]
        public decimal MonthlyBudget { get; set; }

        // ===== GET =====
        public async Task OnGetAsync()
        {
            Settings = await _context.UserSettings.FirstOrDefaultAsync();

            AllExpenses = await _context.Expenses
                .Include(e => e.Category)
                .OrderByDescending(e => e.Date)
                .ToListAsync();

            RecentExpenses = AllExpenses.Take(5).ToList();
        }

        // ===== POST (FIRST TIME SETUP) =====
        public async Task<IActionResult> OnPostAsync()
        {
            var settings = await _context.UserSettings.FirstOrDefaultAsync();

            if (settings == null)
            {
                settings = new UserSettings();
                _context.UserSettings.Add(settings);
            }

            settings.UserName = UserName;
            settings.MonthlyBudget = MonthlyBudget;

            await _context.SaveChangesAsync();

            return RedirectToPage();
        }


        // ===== CALCULATIONS =====
        public decimal TotalThisMonth =>
            AllExpenses
                .Where(e => e.Date.Month == DateTime.Now.Month &&
                            e.Date.Year == DateTime.Now.Year)
                .Sum(e => e.Amount);

        public decimal TotalYTD =>
            AllExpenses
                .Where(e => e.Date.Year == DateTime.Now.Year)
                .Sum(e => e.Amount);

        public decimal RemainingBudget =>
            (Settings?.MonthlyBudget ?? 0) - TotalThisMonth;

        public string DisplayName =>
            Settings?.UserName ?? "Welcome";
    }
}
