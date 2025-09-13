namespace MotorDiniz.Application.Interfaces
{
    public interface IRentalPricingService
    {
        decimal CalculateTotal(
            int planDays,
            decimal dailyPrice,
            DateTime startDate,
            DateTime expectedEnd,
            DateTime returnDate);
    }
}
