using ExpenseDashboard.Api.Models;

namespace ExpenseDashboard.Api.Services;

public interface IDashboardService
{
    Task<decimal> GetTotalExpensesThisMonthAsync();
    Task<decimal> GetRemainingBudgetAsync();
    Task<decimal> GetTotalExpensesYtdAsync();
    Task<IEnumerable<Expense>> GetRecentTransactionsAsync(int count = 5);
    Task<IEnumerable<(string Label, decimal Value)>> GetMonthlyOverviewAsync(int monthsBack = 6);
}