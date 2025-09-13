using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata;
using MotorDiniz.API.Integration.Tests.Configurations;

public class MotorcycleCreateAndPublishTests : IClassFixture<ContainersFixture>, IAsyncLifetime
{
    private readonly ContainersFixture _fx;
    private ApiFactory _factory = default!;
    private HttpClient _client = default!;

    private const string QueueName = "motorcycle-queue";

    public MotorcycleCreateAndPublishTests(ContainersFixture fx) => _fx = fx;

    public Task InitializeAsync()
    {
        _factory = new ApiFactory(_fx.ConnectionString, _fx.RabbitHost, _fx.RabbitPort);
        _client = _factory.CreateClient();
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        _client.Dispose();
        _factory.Dispose();
        return Task.CompletedTask;
    }

    [Fact]
    public async Task Post_Motos_Should_Return201_And_MessageInQueue()
    {
        // 1) Garante que a fila existe (declara antes de publicar)
        using var conn = CreateRabbitConnection(_fx.RabbitHost, _fx.RabbitPort, "guest", "guest");
        using var ch = conn.CreateModel();
        ch.QueueDeclare(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        // 2) Chama a API
        var body = new
        {
            identificador = "moto123",
            ano = 2024,
            modelo = "Sport",
            placa = "ABC-1234"
        };

        var resp = await _client.PostAsJsonAsync("/motos", body);
        resp.StatusCode.Should().Be(HttpStatusCode.Created);

        // 3) Consome a mensagem com retry (fila pode demorar alguns ms)
        var messageBody = await TryGetMessageBodyAsync(ch, QueueName, TimeSpan.FromSeconds(5));
        messageBody.Should().NotBeNull();

        // 4) Valida conteúdo do envelope MassTransit (JSON)
        // Por padrão, MassTransit manda um envelope com "messageType", "message", etc.
        // Vamos checar se o JSON contém os dados principais (mais tolerante a variações de envelope):
        var json = Encoding.UTF8.GetString(messageBody!);

        json.Should().Contain("moto123");
        json.Should().Contain("ABC-1234");
        json.Should().Contain("motorcycle.created"); // se você incluiu EventType no payload/envelope

        // Se quiser ser mais estrito, tente deserializar parcialmente:
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        // Tente achar "message" -> { Identifier, Year, Model, Plate }
        if (root.TryGetProperty("message", out var msg))
        {
            msg.GetProperty("Identifier").GetString().Should().Be("moto123");
            msg.GetProperty("Year").GetInt32().Should().Be(2024);
            msg.GetProperty("Model").GetString().Should().Be("Sport");
            msg.GetProperty("Plate").GetString().Should().Be("ABC-1234");
        }
        // Se seu producer envia o objeto "puro" (sem envelope), adapte a leitura acima.
    }

    private static IConnection CreateRabbitConnection(string host, int port, string user, string pass)
    {
        var factory = new ConnectionFactory
        {
            HostName = host,
            Port = port,
            UserName = user,
            Password = pass
            // ClientProvidedName = "MotorDiniz.IntegrationTests" // opcional
        };
        return factory.CreateConnectionAsync(factory);
    }

    private static async Task<byte[]?> TryGetMessageBodyAsync(IModel channel, string queue, TimeSpan timeout)
    {
        var start = DateTime.UtcNow;
        while (DateTime.UtcNow - start < timeout)
        {
            var result = channel.BasicGet(queue, autoAck: true);   // <- existe no RabbitMQ.Client
            if (result is not null)
                return result.Body.ToArray();

            await Task.Delay(150);
        }
        return null;
    }
}
