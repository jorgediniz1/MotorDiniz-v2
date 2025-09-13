using System.Text.Json.Serialization;

namespace MotorDiniz.API.Utilities
{
    public sealed class ApiResponse
    {
        [JsonPropertyName("mensagem")]
        public string Message { get; }

        public ApiResponse(string message) => Message = message;


        public static ApiResponse InvalidData() => new("Dados inválidos");
        public static ApiResponse MalformedRequest() => new("Request mal formada");
        public static ApiResponse MotorcycleNotFound() => new("Moto não encontrada");
        public static ApiResponse RentalNotFound() => new("Locação não encontrada");
        public static ApiResponse PlateUpdated() => new("Placa modificada com sucesso");
        public static ApiResponse ReturnDateInformed() => new("Data de devolução informada com sucesso");
    }
}
