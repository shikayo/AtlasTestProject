using WebApplication1.Entity;

namespace WebApplication1.Models;

public class ResultViewModel
{ 
    public List<Result>? Results { get; set; }
    public double OverPayment { get; set; }
}