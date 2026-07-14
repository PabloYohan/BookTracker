# Book Promo Tracker

Aplicação local para acompanhar preços de livros na Amazon Brasil com **registro manual de preços**.

O usuário cadastra livros com a URL do produto, define um preço-alvo, consulta o preço diretamente na Amazon e informa manualmente o valor encontrado. O sistema salva o histórico e gera alertas quando o preço atinge ou fica abaixo do desejado.

## Objetivo

Permitir monitorar promoções de livros de forma organizada, sem consulta automática à Amazon. A URL serve apenas para identificar o produto, extrair o ASIN e abrir a página no navegador.

## Funcionalidades principais

* Cadastro de livros com validação de URL da Amazon Brasil
* Extração automática do ASIN e URL canônica
* Registro manual de preços
* Histórico de preços e consulta de menor preço
* Identificação de promoção (preço ≤ preço desejado)
* Alertas de promoção com leitura/não leitura
* Notificações no Linux *(pendente)*

## Tecnologias

* .NET 9
* ASP.NET Core Minimal APIs
* PostgreSQL
* Entity Framework Core
* Swagger
* xUnit

## Requisitos

* .NET SDK 9
* Docker e Docker Compose (para PostgreSQL)

## Como executar

### 1. Iniciar o PostgreSQL

```bash
docker compose up -d
```

### 2. Restaurar dependências e aplicar migrations

```bash
dotnet restore
dotnet ef database update --project src/BookPromoTracker.csproj
```

### 3. Executar a aplicação

```bash
dotnet run --project src/BookPromoTracker.csproj
```

### 4. Abrir o Swagger

Em ambiente de desenvolvimento, acesse:

```text
http://localhost:5195/swagger
```

## Fluxo de uso

### Cadastrar um livro

```http
POST /api/books
```

```json
{
  "title": "O Iluminado",
  "author": "Stephen King",
  "productUrl": "https://www.amazon.com.br/dp/8532520709",
  "targetPrice": 35.00
}
```

O ASIN é extraído automaticamente da URL. O campo `asin` não precisa ser informado na requisição.

### Registrar um preço manualmente

1. Abra a `productUrl` retornada pela API no navegador.
2. Verifique o preço na página da Amazon.
3. Registre o valor:

```http
POST /api/books/{id}/prices
```

```json
{
  "price": 29.90,
  "currency": "BRL"
}
```

### Consultar alertas

```http
GET /api/alerts
GET /api/alerts?unreadOnly=true
GET /api/alerts/unread-count
PATCH /api/alerts/{id}/read
```

## Testes

```bash
dotnet test
```

## Documentação

A documentação detalhada está na pasta `docs/`.

## Observação importante

Esta aplicação **não consulta automaticamente** a Amazon. Não há scraping, APIs pagas nem requisições HTTP para páginas de produto. Todos os preços são informados manualmente pelo usuário.
