
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY . ./

RUN dotnet restore "src/MotorDiniz.Consumer/MotorDiniz.Consumer.csproj"
RUN dotnet publish "src/MotorDiniz.Consumer/MotorDiniz.Consumer.csproj" -c Release -o /app/out/worker --no-restore /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS worker
WORKDIR /app
COPY --from=build-env /app/out/worker ./
ENTRYPOINT ["dotnet", "MotorDiniz.Consumer.dll"]
