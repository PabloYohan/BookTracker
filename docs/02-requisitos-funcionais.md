# 02 - Requisitos Funcionais

Este documento descreve as principais funcionalidades esperadas para o projeto e o status de implementação de cada uma.

Legenda de status:

- **Implementado** — funcionalidade disponível na API
- **Parcial** — parte da funcionalidade existe, mas ainda depende de outras etapas
- **Pendente** — ainda não implementado

## RF01 - Cadastrar livro

**Status:** Implementado

O sistema deve permitir o cadastro de livros para monitoramento.

Informações principais:

- Título
- Autor
- URL do produto
- ASIN ou ISBN, quando disponível
- Preço desejado

Implementação atual:

- Endpoint `POST /api/books`
- Novos livros são criados com monitoramento ativo por padrão
- Campos obrigatórios validados: título, autor, URL do produto e preço desejado

## RF02 - Listar livros cadastrados

**Status:** Implementado

O sistema deve exibir os livros cadastrados pelo usuário.

A listagem deve mostrar, no mínimo:

- Título
- Autor
- Preço desejado
- Último preço encontrado
- Status do monitoramento

Implementação atual:

- Endpoint `GET /api/books`
- O campo `lastPrice` retorna `null` quando ainda não existe histórico de preços para o livro

## RF03 - Editar livro

**Status:** Implementado

O sistema deve permitir editar os dados de um livro já cadastrado.

Exemplos de dados editáveis:

- Título
- Autor
- URL
- Preço desejado
- Status ativo/inativo

Implementação atual:

- Endpoint `PUT /api/books/{id}`
- Endpoints auxiliares `PATCH /api/books/{id}/activate` e `PATCH /api/books/{id}/deactivate`

## RF04 - Remover livro

**Status:** Implementado

O sistema deve permitir remover um livro cadastrado.

Implementação atual:

- Endpoint `DELETE /api/books/{id}`
- Histórico de preços e alertas relacionados são removidos em cascata

## RF05 - Verificar preço manualmente

**Status:** Pendente

O sistema deve permitir executar uma verificação de preço manual para um livro específico.

## RF06 - Verificar preços automaticamente

**Status:** Pendente

O sistema deve verificar automaticamente os preços dos livros ativos em intervalos configuráveis.

Exemplo inicial:

- A cada 6 horas

## RF07 - Salvar histórico de preços

**Status:** Parcial

A cada verificação realizada, o sistema deve salvar o preço encontrado no histórico do livro.

Implementação atual:

- Entidade `PriceHistory` e tabela no banco já existem
- A gravação de histórico ainda depende da implementação da consulta de preços

## RF08 - Identificar promoção

**Status:** Pendente

O sistema deve identificar uma promoção quando o preço atual for menor ou igual ao preço desejado pelo usuário.

## RF09 - Gerar alerta de promoção

**Status:** Parcial

Quando uma promoção for identificada, o sistema deve gerar um alerta contendo:

- Livro
- Preço atual
- Preço desejado
- Data da verificação

Implementação atual:

- Entidade `Alert` e tabela no banco já existem
- A geração de alertas ainda depende da consulta de preços e da regra de promoção

## RF10 - Exibir notificação local

**Status:** Pendente

O sistema deve exibir uma notificação no sistema operacional quando um livro entrar em promoção.

No Linux, a notificação poderá ser feita usando `notify-send`.
