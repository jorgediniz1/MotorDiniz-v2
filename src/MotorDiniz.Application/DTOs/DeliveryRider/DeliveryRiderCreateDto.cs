using System.Text.Json.Serialization;

namespace MotorDiniz.Application.DTOs.DeliveryRider
{
    public class DeliveryRiderCreateDto
    {
        [JsonPropertyName("identificador")]
        public string Identifier { get; set; } = default!;

        [JsonPropertyName("nome")]
        public string Name { get; set; } = default!

;
        [JsonPropertyName("cnpj")]
        public string Cnpj { get; set; } = default!;

        [JsonPropertyName("data_nascimento")]
        public DateTime BirthDate { get; set; }

        [JsonPropertyName("numero_cnh")]
        public string CnhNumber { get; set; } = default!;

        [JsonPropertyName("tipo_cnh")]
        public string CnhType { get; set; } = default!; 

        [JsonPropertyName("imagem_cnh")]
        public string? CnhImageBase64 { get; set; } 
    }
}
