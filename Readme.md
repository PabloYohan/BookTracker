# Book Promo Tracker

Aplicação para monitoramento de preços de livros em lojas online, com foco inicial na Amazon.

A ideia principal é permitir que o usuário cadastre livros desejados, defina um preço-alvo e receba uma notificação quando algum item estiver em promoção.

## Objetivo

Criar uma aplicação local para acompanhar automaticamente o preço de livros e alertar o usuário quando o valor encontrado for menor ou igual ao preço desejado.

## Funcionalidades principais

* Cadastro de livros para monitoramento
* Definição de preço desejado
* Consulta manual de preço
* Verificação automática em intervalos configuráveis
* Histórico de preços
* Alertas de promoção
* Notificações no Linux

## Tecnologias planejadas

* .NET
* ASP.NET Core Web API
* PostgreSQL
* Entity Framework Core
* Background Service
* Linux notifications com `notify-send`

## Estrutura inicial

Projeto pessoal com um único `.csproj`. A organização é feita por pastas, sem múltiplos projetos:

```text
BookPromoTracker/
├── src/
│   ├── Entities/           # Entidades do domínio (Book, Alert, etc.)
│   ├── Data/               # DbContext e configuração do EF Core
│   ├── Services/           # Lógica de negócio (futuro)
│   ├── Program.cs
│   ├── BookPromoTracker.csproj
│   └── appsettings.json
│
├── docs/
├── docker-compose.yml
├── BookPromoTracker.sln
├── README.md
└── .gitignore
```

## Como executar

```bash
docker compose up -d
dotnet run --project src/BookPromoTracker.csproj
```

## Documentação

A documentação detalhada do projeto ficará disponível na pasta `docs`.
