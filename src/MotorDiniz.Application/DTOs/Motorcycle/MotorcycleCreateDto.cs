using System.Text.Json.Serialization;

namespace MotorDiniz.Application.DTOs.Motorcycle
{
    public class MotorcycleCreateDto
    {
        [JsonPropertyName("identificador")]
        public string Identifier { get; set; } = default!;

        [JsonPropertyName("ano")]
        public int Year { get; set; }

        [JsonPropertyName("modelo")]
        public string Model { get; set; } = default!;

        [JsonPropertyName("placa")]
        public string Plate { get; set; } = default!;
    }
}
