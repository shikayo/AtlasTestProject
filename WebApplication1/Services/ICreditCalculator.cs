using WebApplication1.Entity;

namespace WebApplication1.Services;

public interface ICreditCalculator
{
    List<Result> CalculateResult(double amount,int loanDuration,double loanRate);
    List<Result> CalculatePerDayResult(double amount, int loanDurationDays, double loanRate, int paymentStep);
    double CalculateOverPayment(double amount, int loanDuration, double loanRate);
    double CalculateOverPaymentPerDay(double amount, int loanDurationDays, double loanRate, int paymentStep);
}