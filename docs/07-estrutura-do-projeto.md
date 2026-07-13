# 07 - Estrutura do Projeto

Este documento descreve a organização atual do código-fonte.

## Visão geral

O projeto usa um único `.csproj` com separação por pastas. A API segue uma organização em camadas simples:

```text
Requisição HTTP
    ↓
Endpoints
    ↓
Services
    ↓
Data (EF Core)
    ↓
PostgreSQL
```

## Estrutura de pastas

```text
BookPromoTracker/
├── src/
│   ├── Entities/              # Entidades de domínio
│   │   ├── Book.cs
│   │   ├── PriceHistory.cs
│   │   └── Alert.cs
│   │
│   ├── Data/                  # Persistência
│   │   ├── AppDbContext.cs
│   │   └── Migrations/
│   │
│   ├── Dtos/                  # Contratos da API
│   │   └── Books/
│   │       ├── CreateBookRequest.cs
│   │       ├── UpdateBookRequest.cs
│   │       ├── BookResponse.cs
│   │       └── BookListItemResponse.cs
│   │
│   ├── Services/              # Regras de negócio
│   │   ├── IBookService.cs
│   │   └── BookService.cs
│   │
│   ├── Endpoints/             # Rotas HTTP
│   │   └── BooksEndpoints.cs
│   │
│   ├── Program.cs             # Configuração da aplicação
│   ├── BookPromoTracker.csproj
│   ├── BookPromoTracker.http  # Exemplos de requisição
│   └── appsettings.json
│
├── docs/
├── docker-compose.yml
├── BookPromoTracker.sln
└── Readme.md
```

## Responsabilidade de cada camada

### Entities

Contém as classes de domínio persistidas no banco. Não devem depender de DTOs, endpoints ou detalhes da API.

### Data

Centraliza o `AppDbContext`, configuração de relacionamentos, índices e migrations do EF Core.

### Dtos

Define os modelos expostos pela API. Separa o contrato HTTP das entidades de domínio.

Exemplos atuais:

- `CreateBookRequest` — entrada para cadastro
- `UpdateBookRequest` — entrada para edição
- `BookResponse` — detalhe de um livro
- `BookListItemResponse` — item da listagem

### Services

Implementa regras de negócio e orquestra o acesso ao banco.

Implementação atual:

- `BookService` — CRUD de livros, validações e controle de monitoramento

### Endpoints

Mapeia rotas HTTP para os serviços correspondentes.

Implementação atual:

- `BooksEndpoints` — rotas em `/api/books`

### Program.cs

Responsável por:

- Configurar o `DbContext`
- Registrar serviços no container de DI
- Habilitar Swagger em desenvolvimento
- Mapear os endpoints da aplicação

## Convenções adotadas

- Rotas agrupadas com `MapGroup`
- Serviços registrados com `AddScoped`
- Validações simples no serviço, retornando `ValidationProblem` na API
- Consultas de leitura usando `AsNoTracking`
- Identificadores `Guid` gerados na criação dos registros
- Datas de criação em UTC

## Como evoluir o projeto

Para adicionar uma nova funcionalidade, a sequência recomendada é:

1. Criar ou ajustar entidades em `Entities`, se necessário
2. Atualizar `AppDbContext` e gerar migration
3. Criar DTOs em `Dtos`
4. Implementar regra de negócio em `Services`
5. Expor endpoint em `Endpoints`
6. Registrar o serviço e mapear a rota em `Program.cs`
7. Documentar em `docs/05-api-endpoints.md`

Próxima evolução prevista: serviço de consulta de preços e endpoints de histórico.
