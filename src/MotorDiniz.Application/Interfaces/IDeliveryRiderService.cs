using MotorDiniz.Application.DTOs.DeliveryRider;

namespace MotorDiniz.Application.Interfaces
{
    public interface IDeliveryRiderService
    {
        Task CreateAsync(DeliveryRiderCreateDto dto, CancellationToken cancellationToken);
        Task UploadCnhAsync(string identifier, DeliveryRiderCnhUploadDto dto, CancellationToken cancellationToken);
    }
}
