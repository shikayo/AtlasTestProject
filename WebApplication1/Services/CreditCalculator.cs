using WebApplication1.Entity;

namespace WebApplication1.Services;

public class CreditCalculator : ICreditCalculator
{
    private readonly List<Result> _results=new List<Result>();
    
    public List<Result> CalculateResult(double amount,int loanDuration,double loanRate)
    {
        loanRate /= 100;

        var payment = (amount * Math.Pow(loanRate/12+1,loanDuration)*loanRate/12)/(Math.Pow(loanRate/12+1,loanDuration)-1);

        for (int i = 0; i < loanDuration; i++)
        {
            var result = new Result
            {
                Number = i + 1,
                Date = DateOnly.Parse(DateTime.Today.ToShortDateString()).AddMonths(i),
                Amount = Math.Round(payment, 2),
                Debt = Math.Round(payment - amount * loanRate / 12, 2),
                Percentages = Math.Round(amount * loanRate / 12, 2)
            };

            result.RemainingDebt = Math.Round(amount - payment + result.Percentages, 1);

            amount = amount - payment + result.Percentages;

            _results.Add(result);
        }

        return _results;
    }

    public double CalculateOverPayment(double amount, int loanDuration, double loanRate)
    {
        loanRate /= 100;

        var payment = (amount * Math.Pow(loanRate/12+1,loanDuration)*loanRate/12)/(Math.Pow(loanRate/12+1,loanDuration)-1);

        var overPayment = payment * loanDuration - amount;
        return Math.Round(overPayment,2);
    }

    public double CalculateOverPaymentPerDay(double amount, int loanDurationDays, double loanRate, int paymentStep)
    {
        loanRate /= 100;
        
        var paymentCount = loanDurationDays / paymentStep;
        
        var payment = (amount * Math.Pow(loanRate/loanDurationDays+1,paymentCount)*loanRate/loanDurationDays)/(Math.Pow(loanRate/loanDurationDays+1,paymentCount)-1);

        var overPayment = 0.0;
        
        if (loanDurationDays % paymentStep != 0)
        {
            
            for (int i = 0; i < paymentCount; i++)
            {
                var percentages = Math.Round(amount * loanRate / paymentCount, 2);
                overPayment += percentages;
                amount = amount - payment + percentages;
            }
            
            var remainingDays = loanDurationDays % paymentStep;
            var remainingPercentages = Math.Round(amount * Math.Pow(loanRate, remainingDays),2);

            overPayment += Math.Round(amount * Math.Pow(loanRate, remainingDays), 2);
        }
        else
        {
            overPayment=payment * loanDurationDays - amount;
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

        loanRate /= 100;
        
        var paymentCount = loanDurationDays / paymentStep;
        
        var payment = (amount * Math.Pow(loanRate/loanDurationDays+1,paymentCount)*loanRate/loanDurationDays)/(Math.Pow(loanRate/loanDurationDays+1,paymentCount)-1);

        for (i = 0; i < paymentCount; i++)
        {
            var result = new Result
            {
                Number = i+1,
                Date = DateOnly.Parse(DateTime.Today.AddDays(paymentStep*i).ToShortDateString()),
                Amount = Math.Round(payment,2),
                Debt = Math.Round(payment-amount*loanRate/paymentCount,2),
                Percentages = Math.Round(amount * loanRate / paymentCount, 2)
            };
            result.RemainingDebt=Math.Round(amount - payment + result.Percentages, 1);
            
            amount = amount - payment + result.Percentages;
            
            _results.Add(result);
        }

        if (loanDurationDays % paymentStep != 0)
        {
            var remainingDays = loanDurationDays % paymentStep;
            var remainingPercentages = Math.Round(amount * Math.Pow(loanRate, remainingDays),2);
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