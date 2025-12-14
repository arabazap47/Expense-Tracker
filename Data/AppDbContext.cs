using ExpenseDashboard.Api.Models;
using Microsoft.EntityFrameworkCore;
namespace ExpenseDashboard.Api.Data;
public class AppDbContext : DbContext
{
public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
{ }
public DbSet<Expense> Expenses { get; set; }
public DbSet<Category> Categories { get; set; }
public DbSet<Budget> Budgets { get; set; }
public DbSet<UserSettings> UserSettings { get; set; }

}