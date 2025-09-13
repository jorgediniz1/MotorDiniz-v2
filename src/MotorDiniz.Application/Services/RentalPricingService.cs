using MotorDiniz.Application.Interfaces;

namespace MotorDiniz.Application.Services
{
    public sealed class RentalPricingService : IRentalPricingService
    {
        public decimal CalculateTotal(
            int planDays,
            decimal dailyPrice,
            DateTime start,
            DateTime expectedEnd,
            DateTime ret)
        {
            var baseTotal = planDays * dailyPrice;

            // No prazo
            if (ret.Date == expectedEnd.Date)
                return baseTotal;

            // Devolução antecipada
            if (ret.Date < expectedEnd.Date)
            {
                var usedDays = Math.Min(planDays, Math.Max(1, (ret.Date - start.Date).Days));
                var unused = planDays - usedDays;
                var penaltyRate = planDays == 7 ? 0.20m
                                 : planDays == 15 ? 0.40m
                                 : 0m;

                return usedDays * dailyPrice + (unused * dailyPrice * penaltyRate);
            }

            // Devolução atrasada
            var extraDays = Math.Max(1, (ret.Date - expectedEnd.Date).Days);
            return baseTotal + extraDays * 50m;
        }
    }

}
