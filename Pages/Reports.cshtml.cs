using ExpenseDashboard.Api.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpenseDashboard.Api.Pages
{
    public class ReportsModel : PageModel
    {
        private readonly AppDbContext _context;

        public ReportsModel(AppDbContext context)
        {
            _context = context;
        }

        public decimal TotalExpenses { get; set; }
        public decimal ThisMonthExpenses { get; set; }
        public decimal AverageMonthlyExpenses { get; set; }

        public List<string> MonthLabels { get; set; } = new();
        public List<decimal> MonthTotals { get; set; } = new();

        public List<CategoryReport> CategoryTotals { get; set; } = new();

        public void OnGet()
        {
            var expenses = _context.Expenses
    .Include(e => e.Category)
    .ToList();


            TotalExpenses = expenses.Sum(e => e.Amount);

            ThisMonthExpenses = expenses
                .Where(e => e.Date.Month == DateTime.Now.Month && e.Date.Year == DateTime.Now.Year)
                .Sum(e => e.Amount);

            AverageMonthlyExpenses = expenses
                .GroupBy(e => new { e.Date.Year, e.Date.Month })
                .Select(g => g.Sum(x => x.Amount))
                .DefaultIfEmpty()
                .Average();

            MonthLabels = expenses
     .GroupBy(e => new { e.Date.Year, e.Date.Month })
     .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
     .Select(g => new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM"))
     .ToList();

            MonthTotals = expenses
                .GroupBy(e => new { e.Date.Year, e.Date.Month })
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                .Select(g => g.Sum(x => x.Amount))
                .ToList();


            CategoryTotals = expenses
    .Where(e => e.Category != null)
    .GroupBy(e => e.Category.Name)
    .Select(g => new CategoryReport
    {
        CategoryName = g.Key,
        Total = g.Sum(x => x.Amount)
    })
    .ToList();

        }
    }

    public class CategoryReport
    {
        public string CategoryName { get; set; }
        public decimal Total { get; set; }
    }
}
