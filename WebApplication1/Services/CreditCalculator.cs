using WebApplication1.Entity;

namespace WebApplication1.Services;

public class CreditCalculator : ICreditCalculator
{
    private readonly List<Result> _results;

    public CreditCalculator()
    {
        _results = new List<Result>();
    }

    private double CalculatePayment(double amount,int loanDuration,double loanRate)
    {
        return (amount * Math.Pow(loanRate/100/12+1,loanDuration)*loanRate/100/12)
               /(Math.Pow(loanRate/100/12+1,loanDuration)-1);
    }

    private double CalculatePaymentPerDay(double amount, int loanDurationDays, double loanRate, int paymentStep)
    {
        var paymentCount = loanDurationDays / paymentStep;
        
        return (amount * Math.Pow(loanRate/100/loanDurationDays+1,paymentCount)*loanRate/100/loanDurationDays)/(Math.Pow(loanRate/100/loanDurationDays+1,paymentCount)-1);

    }
    
    public List<Result> CalculateResult(double amount,int loanDuration,double loanRate)
    {
        var payment = CalculatePayment(amount, loanDuration, loanRate);

        for (int i = 0; i < loanDuration; i++)
        {
            var result = new Result
            {
                Number = i + 1,
                Date = DateOnly.Parse(DateTime.Today.ToShortDateString()).AddMonths(i+1),
                Amount = Math.Round(payment, 2),
                Debt = Math.Round(payment - amount * loanRate /100/ 12, 2),
                Percentages = Math.Round(amount * loanRate /100/ 12, 2)
            };

            result.RemainingDebt = Math.Round(amount - payment + result.Percentages, 1);

            amount = amount - payment + result.Percentages;

            _results.Add(result);
        }

        return _results;
    }

    public double CalculateOverPayment(double amount, int loanDuration, double loanRate)
    {
        var payment = CalculatePayment(amount, loanDuration, loanRate);

        var overPayment = payment * loanDuration - amount;
        return Math.Round(overPayment,2);
    }

    public double CalculateOverPaymentPerDay(double amount, int loanDurationDays, double loanRate, int paymentStep)
    {
        var paymentCount = loanDurationDays / paymentStep;

        var payment = CalculatePaymentPerDay(amount, loanDurationDays, loanRate, paymentStep);

        var overPayment = 0.0;
        
        if (loanDurationDays % paymentStep != 0)
        {
            
            for (int i = 0; i < paymentCount; i++)
            {
                var percentages = Math.Round(amount * loanRate/100 / paymentCount, 2);
                overPayment += percentages;
                amount = amount - payment + percentages;
            }
            
            var remainingDays = loanDurationDays % paymentStep;
            var remainingPercentages = Math.Round(amount * Math.Pow(loanRate/100, remainingDays),2);

            overPayment += Math.Round(amount * Math.Pow(loanRate/100, remainingDays), 2);
        }
        else
        {
            overPayment=payment * paymentCount - amount;
        }

        return Math.Round(overPayment,2);
    }

    public List<Result> CalculatePerDayResult(double amount, int loanDurationDays, double loanRate, int paymentStep)
    {
        //A = (P x i x (1 + i)^n) / ((1 + i)^n - 1)
        /*      xdxdxdxd wtfdsfsdf
         *   • A - аннуитетный платеж
         *   • P - сумма займа
         *   • i - процентная ставка за один день (необходимо привести к дробному виду)
         *   • n - количество платежей (выражается в днях и приводится к количеству периодов с помощью деления на количество дней в году)
        */
        int i;
        
        var paymentCount = loanDurationDays / paymentStep;

        var payment = CalculatePaymentPerDay(amount, loanDurationDays, loanRate, paymentStep);

        for (i = 0; i < paymentCount; i++)
        {
            var result = new Result
            {
                Number = i+1,
                Date = DateOnly.Parse(DateTime.Today.AddDays(paymentStep*i).ToShortDateString()),
                Amount = Math.Round(payment,2),
                Debt = Math.Round(payment-amount*loanRate/100/paymentCount,2),
                Percentages = Math.Round(amount * loanRate/100 / paymentCount, 2)
            };
            result.RemainingDebt=Math.Round(amount - payment + result.Percentages, 1);
            
            amount = amount - payment + result.Percentages;
            
            _results.Add(result);
        }

        if (loanDurationDays % paymentStep != 0)
        {
            var remainingDays = loanDurationDays % paymentStep;
            var remainingPercentages = Math.Round(amount * Math.Pow(loanRate/100, remainingDays),2);
            var remainingPayment = Math.Round(amount + remainingPercentages,2);
            var mainDebt = Math.Round(remainingPayment - remainingPercentages,2);
            var remainDebt = Math.Round(remainingPayment - mainDebt - remainingPercentages,2);

            var remainingResult = new Result
            {
                Number = i+1,
                Date = DateOnly.Parse(DateTime.Today.AddDays(paymentStep*i-remainingDays).ToShortDateString()),
                Amount = remainingPayment,
                Debt = mainDebt,
                Percentages = remainingPercentages,
                RemainingDebt = remainDebt,
            };
            _results.Add(remainingResult);
        }
        
        return _results;
    }
}