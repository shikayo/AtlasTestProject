using Microsoft.AspNetCore.DataProtection.Internal;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Entity;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

public class CreditController : Controller
{
    private readonly ICreditCalculator _creditCalculator;

    public CreditController(ICreditCalculator creditCalculator)
    {
        _creditCalculator = creditCalculator;
    }
    public IActionResult Invoice()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Calculate([FromForm]CreditInvoiceViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = new ResultViewModel();
            result.Results = _creditCalculator.CalculateResult(model.Amount, model.LoanDuration, model.LoanRate);
            
            result.OverPayment =
                _creditCalculator.CalculateOverPayment(model.Amount, model.LoanDuration, model.LoanRate);
            
            return View("Result", result);
        }

        return View("Invoice");
    }

    public IActionResult InvoicePerDay()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult CalculatePerDay([FromForm] CreditInvoicePerDayViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = new ResultViewModel();
            result.Results = _creditCalculator.CalculatePerDayResult(model.Amount, model.LoanDurationDays,
                model.LoanRate, model.PaymentStep);
            
            result.OverPayment = _creditCalculator.CalculateOverPaymentPerDay(model.Amount, model.LoanDurationDays,
                model.LoanRate, model.PaymentStep);
            return View("Result", result);
        }
        
        return View("InvoicePerDay");
    }
}