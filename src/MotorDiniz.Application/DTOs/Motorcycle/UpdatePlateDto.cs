using System.Text.Json.Serialization;

namespace MotorDiniz.Application.DTOs.Motorcycle
{
    public class UpdatePlateDto
    {
        [JsonPropertyName("placa")]
        public string Plate { get; set; } = default!;

    }
}
