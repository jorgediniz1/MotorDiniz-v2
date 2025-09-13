using System.Text.Json.Serialization;

namespace MotorDiniz.Application.DTOs.DeliveryRider
{
    public class DeliveryRiderCnhUploadDto
    {
        [JsonPropertyName("imagem_cnh")]
        public string CnhImageBase64 { get; set; } = default!;
    }
}
