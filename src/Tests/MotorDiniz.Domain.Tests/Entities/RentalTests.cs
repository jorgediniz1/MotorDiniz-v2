using System;
using MotorDiniz.Domain.Entities;
using MotorDiniz.Domain.Enums;
using MotorDiniz.Domain.Validation;
using Xunit;

namespace MotorDiniz.Domain.Tests.Entities
{
    public class RentalTests
    {
        [Fact(DisplayName = "Create rental with valid parameters")]
        public void Create_WithValidParameters_ShouldSucceed()
        {
            var start = new DateTime(2025, 09, 08, 0, 0, 0, DateTimeKind.Utc);
            var end = new DateTime(2025, 09, 14, 23, 59, 59, DateTimeKind.Utc);
            var expectedEnd = end;

            var rental = new Rental(
                identifier: "loc_123",
                deliveryRiderId: 1,
                motorcycleId: 1,
                startDate: start,
                endDate: end,
                expectedEndDate: expectedEnd,
                plan: RentalPlan.Days7,
                dailyPrice: 30m
            );

            Assert.Equal("loc_123", rental.Identifier);
            Assert.Equal(1, rental.DeliveryRiderId);
            Assert.Equal(1, rental.MotorcycleId);
            Assert.Equal(RentalPlan.Days7, rental.Plan);
            Assert.Equal(30m, rental.DailyPrice);
            Assert.Null(rental.ReturnDate);
        }

        [Fact(DisplayName = "Create rental with daily price <= 0 throws")]
        public void Create_WithInvalidDailyPrice_ShouldThrow()
        {
            var start = DateTime.UtcNow.Date.AddDays(1);
            var end = start.AddDays(7).AddSeconds(-1);
            var ex = Assert.Throws<DomainExceptionValidation>(() =>
                new Rental("loc_1", 1, 1, start, end, end, RentalPlan.Days7, 0m)
            );
            Assert.Equal("Daily price must be greater than zero.", ex.Message);
        }

        [Fact(DisplayName = "Create rental with invalid plan throws")]
        public void Create_WithInvalidPlan_ShouldThrow()
        {
            var start = DateTime.UtcNow.Date.AddDays(1);
            var end = start.AddDays(7).AddSeconds(-1);
            var ex = Assert.Throws<DomainExceptionValidation>(() =>
                new Rental("loc_1", 1, 1, start, end, end, (RentalPlan)99, 30m)
            );
            Assert.Equal("Rental plan is invalid.", ex.Message);
        }

        [Fact(DisplayName = "Create rental with end before start throws")]
        public void Create_EndBeforeStart_ShouldThrow()
        {
            var start = DateTime.UtcNow.Date.AddDays(1);
            var end = start.AddDays(-1);
            var ex = Assert.Throws<DomainExceptionValidation>(() =>
                new Rental("loc_1", 1, 1, start, end, start, RentalPlan.Days7, 30m)
            );
            Assert.Equal("End date cannot be before start date.", ex.Message);
        }

        [Fact(DisplayName = "Create rental with expected end before end throws")]
        public void Create_ExpectedBeforeEnd_ShouldThrow()
        {
            var start = DateTime.UtcNow.Date.AddDays(1);
            var end = start.AddDays(7);
            var expected = end.AddDays(-1);

            var ex = Assert.Throws<DomainExceptionValidation>(() =>
                new Rental("loc_1", 1, 1, start, end, expected, RentalPlan.Days7, 30m)
            );

            Assert.Equal("Expected end date cannot be before start date.", ex.Message);
        }

        [Fact(DisplayName = "Inform return before start throws")]
        public void InformReturn_BeforeStart_ShouldThrow()
        {
            var start = DateTime.UtcNow.Date.AddDays(1);
            var end = start.AddDays(7);
            var rental = new Rental("loc_1", 1, 1, start, end, end, RentalPlan.Days7, 30m);

            var ex = Assert.Throws<DomainExceptionValidation>(() =>
                rental.InformReturn(start.AddDays(-1), 10)
            );
            Assert.Equal("Return date cannot be before start date.", ex.Message);
        }

        [Fact(DisplayName = "Inform return sets date and touches UpdatedAtUtc")]
        public void InformReturn_Valid_ShouldSetAndTouch()
        {
            var start = DateTime.UtcNow.Date.AddDays(1);
            var end = start.AddDays(7);
            var rental = new Rental("loc_1", 1, 1, start, end, end, RentalPlan.Days7, 30m);

            var before = rental.UpdatedAt;
            var ret = end.AddHours(-2);
            rental.InformReturn(ret, 10);

            Assert.Equal(ret, rental.ReturnDate);
            Assert.NotEqual(before, rental.UpdatedAt);
        }
    }
}
