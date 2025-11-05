using System.ComponentModel.DataAnnotations;

namespace ExpenseDashboard.Api.Models;

public class Category
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
}