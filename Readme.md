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
* Avalonia UI
* SQLite
* Entity Framework Core
* Background Worker
* Linux notifications com `notify-send`

## Estrutura inicial

```text
BookPromoTracker/
├── src/
│   ├── BookPromoTracker.Api/
│   ├── BookPromoTracker.App/
│   ├── BookPromoTracker.Domain/
│   ├── BookPromoTracker.Infrastructure/
│   └── BookPromoTracker.Worker/
│
├── docs/
├── tests/
├── README.md
└── .gitignore
```

## Documentação

A documentação detalhada do projeto ficará disponível na pasta `docs`.
