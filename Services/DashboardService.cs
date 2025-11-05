using ExpenseDashboard.Api.Data;
using ExpenseDashboard.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseDashboard.Api.Services;

public class DashboardService : IDashboardService
{
    private readonly AppDbContext _db;
    public DashboardService(AppDbContext db) => _db = db;

    public async Task<decimal> GetTotalExpensesThisMonthAsync()
    {
        var now = DateTime.UtcNow;
        return await _db.Expenses
            .Where(e => e.Date.Year == now.Year && e.Date.Month == now.Month)
            .SumAsync(e => (decimal?)e.Amount) ?? 0m;
    }

    public async Task<decimal> GetRemainingBudgetAsync()
    {
        var now = DateTime.UtcNow;
        var budget = await _db.Budgets.FirstOrDefaultAsync(b => b.Year == now.Year && b.Month == now.Month);
        var total = await GetTotalExpensesThisMonthAsync();
        return (budget?.Amount ?? 0m) - total;
    }

    public async Task<decimal> GetTotalExpensesYtdAsync()
    {
        var now = DateTime.UtcNow;
        return await _db.Expenses
            .Where(e => e.Date.Year == now.Year)
            .SumAsync(e => (decimal?)e.Amount) ?? 0m;
    }

    public async Task<IEnumerable<Expense>> GetRecentTransactionsAsync(int count = 5)
    {
        return await _db.Expenses
            .Include(e => e.Category)
            .OrderByDescending(e => e.Date)
            .Take(count)
            .ToListAsync();
    }

    public async Task<IEnumerable<(string Label, decimal Value)>> GetMonthlyOverviewAsync(int monthsBack = 6)
    {
        var results = new List<(string, decimal)>();
        var now = DateTime.UtcNow;
        for (int i = monthsBack - 1; i >= 0; i--)
        {
            var dt = now.AddMonths(-i);
            var total = await _db.Expenses
                .Where(e => e.Date.Year == dt.Year && e.Date.Month == dt.Month)
                .SumAsync(e => (decimal?)e.Amount) ?? 0m;
            results.Add((dt.ToString("MMM"), total));
        }
        return results;
    }
}