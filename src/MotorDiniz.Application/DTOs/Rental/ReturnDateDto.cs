using System.Text.Json.Serialization;

namespace MotorDiniz.Application.DTOs.Rental
{
    public class ReturnDateDto
    {
        [JsonPropertyName("data_devolucao")]
        public DateTime ReturnDate { get; set; }
    }
}
