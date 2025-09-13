using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MotorDiniz.Application.DTOs.Rental
{
    public class RentalViewDto
    {
        [JsonPropertyName("identificador")]
        public string Identifier { get; set; } = default!;

        [JsonPropertyName("valor_diaria")]
        public decimal DailyPrice { get; set; }

        [JsonPropertyName("entregador_id")]
        public string DeliveryRiderIdentifier { get; set; } = default!;

        [JsonPropertyName("moto_id")]
        public string MotorcycleIdentifier { get; set; } = default!;

        [JsonPropertyName("data_inicio")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("data_termino")]
        public DateTime EndDate { get; set; }

        [JsonPropertyName("data_previsao_termino")]
        public DateTime ExpectedEndDate { get; set; }

        [JsonPropertyName("data_devolucao")]
        public DateTime? ReturnDate { get; set; }
    }
}
