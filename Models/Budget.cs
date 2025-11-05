using System.ComponentModel.DataAnnotations;

namespace ExpenseDashboard.Api.Models;

public class Budget
{
    [Key]
    public int Id { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal Amount { get; set; }
}