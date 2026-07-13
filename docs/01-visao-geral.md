# 01 - Visão Geral

## Book Promo Tracker

O **Book Promo Tracker** é uma aplicação simples para acompanhar preços de livros em lojas online, com foco inicial na Amazon.

A ideia é permitir que o usuário cadastre livros de interesse, defina um preço desejado e receba uma notificação quando o valor encontrado estiver abaixo ou igual ao preço definido.

## Objetivo

Criar uma aplicação local e prática para monitorar promoções de livros de forma automática, evitando que o usuário precise verificar manualmente os preços todos os dias.

## Problema que o projeto resolve

Livros podem variar bastante de preço ao longo do tempo. Como nem sempre o usuário acompanha essas mudanças, ele pode perder promoções interessantes.

O sistema busca resolver isso por meio de verificações periódicas e alertas quando algum livro entra em promoção.

## Público-alvo

Este projeto é voltado para uso pessoal, principalmente para pessoas que compram livros com frequência e desejam acompanhar preços de forma mais organizada.

## Escopo inicial

A primeira versão do projeto terá foco em:

- Cadastro de livros
- Definição de preço desejado
- Consulta de preço
- Histórico de preços
- Alertas de promoção
- Notificação local no Linux

## Status atual

Até o momento, as versões **0.1** e **0.2** do roadmap estão concluídas.

O que já funciona:

- API Web em ASP.NET Core com Swagger em ambiente de desenvolvimento
- Banco PostgreSQL configurado via Docker Compose
- Entidades `Book`, `PriceHistory` e `Alert` modeladas e persistidas com EF Core
- CRUD completo de livros, com validação de entrada
- Definição de preço desejado (`targetPrice`)
- Ativação e desativação de monitoramento (`isActive`)
- Listagem de livros cadastrados, incluindo último preço encontrado quando houver histórico

O que ainda não foi implementado:

- Consulta de preços em lojas online
- Gravação automática de histórico de preços
- Geração de alertas de promoção
- Verificação automática em background
- Notificações locais no Linux

## Tecnologias utilizadas

- .NET 9
- ASP.NET Core Web API (Minimal APIs)
- PostgreSQL
- Entity Framework Core
- Swagger / OpenAPI

## Tecnologias previstas

- Background Service
- Notificações Linux com `notify-send`

## Observação sobre coleta de preços

A coleta de preços deve ser feita com cuidado, priorizando APIs oficiais quando possível. Caso seja usada leitura de páginas HTML, o projeto deve considerar possíveis mudanças de layout, bloqueios e limitações dos sites consultados.
