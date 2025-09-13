using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace MotorDiniz.API.Integration.Tests.Configurations
{
    public sealed class ApiFactory : WebApplicationFactory<Program>
    {
        public ApiFactory(string connectionString, string rabbitHost, int rabbitPort)
        {        
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            Environment.SetEnvironmentVariable("Connection_String", connectionString);
       
            Environment.SetEnvironmentVariable("MassTransit_Server", rabbitHost);
            Environment.SetEnvironmentVariable("MassTransit_User", "guest");
            Environment.SetEnvironmentVariable("MassTransit_Password", "guest");
            Environment.SetEnvironmentVariable("MassTransit_Queue_MotorcycleQueue", "motorcycle-queue");
        }
    }
}
