# MotorDiniz

API em **.NET 8 (C#)** para gestão de **motos**, **entregadores** e **locações**, com:
- **PostgreSQL** (EF Core)
- **RabbitMQ** (MassTransit) para eventos
- **MinIO** (S3-compatível) para armazenar **imagem da CNH**
- **CQRS (MediatR)**

---

## Como rodar (Docker Compose)

> Pré-requisitos: Docker Desktop + Docker Compose

Na raiz do repositório:

```bash
docker-compose up --build -d
```

Acessos rápidos:
- **Swagger**: http://localhost:7209/swagger
- **PostgreSQL**: `localhost:5432` (db: `MotorDiniz`, user: `postgres`, pass: `motor123`)
- **RabbitMQ Console**: http://localhost:15672  (user: `guest` / pass: `guest`)
- **MinIO Console**: http://localhost:9001      (user: `admin` / pass: `admin12345`)
- **MinIO S3 endpoint**: `http://localhost:9000`

Parar serviços:
```bash
docker-compose down
```

> Em dev/test, as **migrations** do EF podem ser aplicadas na inicialização (conforme configuração do projeto).

---

## Variáveis de ambiente (compose)

A API lê configuração via **Configuration** (12-factor). No Docker, sobrescreva por env vars:

```yaml

   environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - Connection_String=Host=postgres;Port=5432;Database=MotorDiniz;Username=postgres;Password=motor123
      - MassTransit_Server=rabbitmq
      - MassTransit_User=guest
      - MassTransit_Password=guest
      - MassTransit_Queue_MotorcycleQueue=motorcycle-queue
      - Minio__Endpoint=minio:9000
      - Minio__AccessKey=admin
      - Minio__SecretKey=admin12345
      - Minio__UseSSL=false
      - Minio__BucketCnh=cnh-images
```


---

## Exemplos de uso (Swagger)

### 1) Criar moto — `POST /motos`
```json
{
  "identificador": "moto123",
  "ano": 2024,
  "modelo": "FAN",
  "placa": "ABC-1234"
}
```
- Retorna **201 Created**
- Publica evento `motorcycle.created` no RabbitMQ

### 2) Listar motos (filtro por placa) — `GET /motos?placa=ABC-1234`

### 3) Criar entregador (com CNH em Base64) — `POST /entregadores`
> Use **apenas** a string Base64 (sem `data:image/...;base64,`) EXEMPLO:

```json
{
  "identificador": "entregador123",
  "nome": "João da Silva",
  "cnpj": "12345678901234",
  "data_nascimento": "1990-01-01T00:00:00Z",
  "numero_cnh": "12345678900",
  "tipo_cnh": "A",
  "imagem_cnh": "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mP8/x8AAwMCAO+Z7HcAAAAASUVORK5CYII="
}
```

### 4) Upload de CNH — `POST /entregadores/{id}/cnh`
```json
{
  "imagem_cnh": "iVBORw0K... (PNG/BMP base64)"
}
```

### 5) Criar locação — `POST /locacao`

- Planos válidos: **7/15/30/45/50** dias (diárias: 30/28/22/20/18)

### 6) Devolução e cálculo — `PUT /locacao/{id}/devolucao`
```json
{
  "data_devolucao": "2024-01-07T18:00:00Z"
}
```
- Calcula e persiste o total:
  - **Antecipada**: diárias usadas + multa (20% p/ 7d, 40% p/ 15d) sobre dias não usados
  - **No prazo**: total do plano
  - **Atrasada**: total do plano + R$50 por diária extra

---

## Regras principais (resumo)

- **Placa** da moto é **única**
- **CNH** aceita **A**, **B** ou **AB** (locação exige categoria **A**)
- **CNPJ** e **nº da CNH** do entregador são **únicos**
- Imagem da CNH deve ser **PNG** ou **BMP** (armazenada no **MinIO**, não no banco)

---

## O que foi usado

- **.NET 8** (C#), **ASP.NET Core Web API**, **Swagger**
- **DDD (enxuto)** + **CQRS (MediatR)**
- **EF Core + PostgreSQL**
- **MassTransit + RabbitMQ** (eventos)
- **MinIO** (S3-compatível) para imagens de CNH
- **Unit of Work** (simples por enquanto) 
- **ApiResponse** para mensagens no padrão do Swagger (`{"mensagem": "..."}`)

---


**Observação:** Este projeto está simples e direto para atender ao desafio. Melhorias naturais: validações avançadas, testes de integração, testes de unidade, etc.
