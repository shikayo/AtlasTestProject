using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class CreditInvoiceViewModel
{
    [Required]
    [RegularExpression(@"^\d+(?:[\.,]\d+)?$")]
    public double Amount { get; set; }
    [Required]
    [RegularExpression(@"^\d+(?:[\.,]\d+)?$")]
    public int LoanDuration { get; set; }
    [Required]
    [RegularExpression(@"^\d+(?:[\.,]\d+)?$")]
    public double LoanRate { get; set; }
}