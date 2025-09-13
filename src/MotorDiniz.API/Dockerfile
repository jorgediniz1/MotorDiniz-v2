# ===== Build =====
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY . ./

RUN dotnet restore "src/MotorDiniz.API/MotorDiniz.API.csproj"
RUN dotnet publish "src/MotorDiniz.API/MotorDiniz.API.csproj" -c Release -o /app/out/api


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS api
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Development
COPY --from=build-env /app/out/api ./
EXPOSE 8080
EXPOSE 443
ENTRYPOINT ["dotnet", "MotorDiniz.API.dll"]
