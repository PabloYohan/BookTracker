# 01 - Visão Geral

## Book Promo Tracker

O **Book Promo Tracker** é uma aplicação local para acompanhar preços de livros na Amazon Brasil com **registro manual de preços**.

O usuário cadastra livros de interesse com a URL do produto, define um preço desejado, consulta o preço diretamente na Amazon e informa manualmente o valor encontrado. O sistema salva o histórico e gera alertas quando o preço atinge ou fica abaixo do desejado.

## Objetivo

Criar uma aplicação local e prática para monitorar promoções de livros sem depender de consulta automática à Amazon.

## Problema que o projeto resolve

Livros podem variar bastante de preço ao longo do tempo. O sistema organiza o acompanhamento: o usuário abre o produto pela URL salva, verifica o preço e registra manualmente. Quando o valor informado atinge o preço-alvo, um alerta é gerado.

## Público-alvo

Este projeto é voltado para uso pessoal, principalmente para pessoas que compram livros com frequência e desejam acompanhar preços de forma mais organizada.

## Escopo inicial

A primeira versão do projeto tem foco em:

- Cadastro de livros com validação de URL da Amazon Brasil
- Extração automática do ASIN
- Registro manual de preços
- Histórico de preços
- Alertas de promoção
- Notificação local no Linux *(pendente)*

## Status atual

O que já funciona:

- API Web em ASP.NET Core com Swagger em ambiente de desenvolvimento
- Banco PostgreSQL configurado via Docker Compose
- Entidades `Book`, `PriceHistory` e `Alert` modeladas e persistidas com EF Core
- CRUD completo de livros, com validação de entrada
- Validação de URL da Amazon Brasil e extração automática do ASIN
- URL canônica salva no formato `https://www.amazon.com.br/dp/{ASIN}`
- Registro manual de preços com fonte `Manual - Amazon`
- Histórico de preços e consulta de menor preço
- Identificação de promoção (preço ≤ preço desejado)
- Geração de alertas com regras contra duplicatas
- Endpoints para listar, filtrar e marcar alertas como lido/não lido

O que ainda não foi implementado:

- Notificações locais no Linux

## Tecnologias utilizadas

- .NET 9
- ASP.NET Core Web API (Minimal APIs)
- PostgreSQL
- Entity Framework Core
- Swagger / OpenAPI
- xUnit

## Tecnologias previstas

- Notificações Linux com `notify-send`

## Observação sobre coleta de preços

A aplicação **não consulta automaticamente** a Amazon. Não há scraping, APIs pagas nem requisições HTTP para páginas de produto. A URL é usada apenas para identificar o produto e permitir que o usuário abra a página no navegador.
