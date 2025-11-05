using ExpenseDashboard.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseDashboard.Api.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _svc;
    public DashboardController(IDashboardService svc) => _svc = svc;

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var totalThisMonth = await _svc.GetTotalExpensesThisMonthAsync();
        var remaining = await _svc.GetRemainingBudgetAsync();
        var ytd = await _svc.GetTotalExpensesYtdAsync();
        var recent = await _svc.GetRecentTransactionsAsync();
        var overview = await _svc.GetMonthlyOverviewAsync(6);

        return Ok(new { totalThisMonth, remaining, ytd, recent, overview });
    }
}