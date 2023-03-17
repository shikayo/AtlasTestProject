using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class CreditInvoicePerDayViewModel
{
    [Required]
    [RegularExpression(@"^\d+(?:[\.,]\d+)?$")]
    public double Amount { get; set; }
    [Required]
    [RegularExpression(@"^\d+(?:[\.,]\d+)?$")]
    public int LoanDurationDays { get; set; }
    [Required]
    [RegularExpression(@"^\d+(?:[\.,]\d+)?$")]
    public double LoanRate { get; set; }
    [Required]
    [RegularExpression(@"^\d+(?:[\.,]\d+)?$")]
    public int PaymentStep { get; set; }
}