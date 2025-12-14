using ExpenseDashboard.Api.Models;

namespace ExpenseDashboard.Api.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Expenses.Any()) return; // DB has been seeded

            var categories = new[]
            {
                new Category { Name = "Groceries" },
                new Category { Name = "Coffee" },
                new Category { Name = "Fuel" },
                new Category { Name = "Online" },
                new Category { Name = "Food & Drink" },
                new Category { Name = "Transport" },
                new Category { Name = "Rent & Utilities" }
            };

            context.Categories.AddRange(categories);

            //var expenses = new[]
            //{
            //    new Expense { Title = "Groceries", Amount = 85.30m, Date = DateTime.UtcNow.Date, Category = categories[0] },
            //    new Expense { Title = "Coffee", Amount = 4.50m, Date = DateTime.UtcNow.Date, Category = categories[1] },
            //    new Expense { Title = "Dinner", Amount = 42.00m, Date = DateTime.UtcNow.Date.AddDays(-1), Category = categories[0] },
            //    new Expense { Title = "Fuel", Amount = 0.00m, Date = DateTime.UtcNow.Date.AddDays(-2), Category = categories[2] },
            //    new Expense { Title = "Online Course", Amount = 1999.99m, Date = DateTime.UtcNow.Date.AddDays(-10), Category = categories[3] }
            //};

            //context.Expenses.AddRange(expenses);

            var budget = new Budget
            {
                Month = DateTime.UtcNow.Month,
                Year = DateTime.UtcNow.Year,
                Amount = 2000m
            };
            context.Budgets.Add(budget);

            context.SaveChanges();
        }
    }
}
