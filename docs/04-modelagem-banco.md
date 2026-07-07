# 04 - Modelagem do Banco de Dados

Este documento apresenta uma modelagem inicial e simples para o banco de dados do projeto.

## Banco recomendado

Para a primeira versão, será utilizado **SQLite**, por ser leve, local e simples de configurar.

## Entidades principais

## Book

Representa um livro cadastrado para monitoramento.

| Campo | Tipo | Descrição |
|---|---|---|
| Id | Guid | Identificador do livro |
| Title | string | Título do livro |
| Author | string | Autor do livro |
| Isbn | string | ISBN, quando disponível |
| Asin | string | Código ASIN da Amazon, quando disponível |
| ProductUrl | string | URL do produto |
| TargetPrice | decimal | Preço desejado |
| IsActive | bool | Indica se o livro está sendo monitorado |
| CreatedAt | DateTime | Data de cadastro |

## PriceHistory

Representa o histórico de preços encontrados para um livro.

| Campo | Tipo | Descrição |
|---|---|---|
| Id | Guid | Identificador do registro |
| BookId | Guid | Livro relacionado |
| Price | decimal | Preço encontrado |
| Currency | string | Moeda do preço |
| Source | string | Fonte da consulta |
| CheckedAt | DateTime | Data da verificação |

## Alert

Representa um alerta gerado quando um livro entra em promoção.

| Campo | Tipo | Descrição |
|---|---|---|
| Id | Guid | Identificador do alerta |
| BookId | Guid | Livro relacionado |
| CurrentPrice | decimal | Preço encontrado |
| TargetPrice | decimal | Preço desejado |
| Message | string | Mensagem do alerta |
| WasRead | bool | Indica se o alerta foi lido |
| CreatedAt | DateTime | Data de criação |

## Relacionamentos

- Um livro pode ter vários registros de histórico de preço.
- Um livro pode gerar vários alertas.
- Cada histórico de preço pertence a um único livro.
- Cada alerta pertence a um único livro.

## Diagrama simplificado

```text
Book
 ├── PriceHistory
 └── Alert
```

## Observações

A modelagem pode ser expandida futuramente para suportar múltiplas lojas, usuários, categorias e listas personalizadas.
