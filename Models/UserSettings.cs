namespace ExpenseDashboard.Api.Models
{
    public class UserSettings
    {
        public int Id { get; set; }
        public string UserName { get; set; } = "";
        public decimal MonthlyBudget { get; set; }
    }

}
