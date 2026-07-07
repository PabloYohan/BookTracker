# 02 - Requisitos Funcionais

Este documento descreve as principais funcionalidades esperadas para o projeto.

## RF01 - Cadastrar livro

O sistema deve permitir o cadastro de livros para monitoramento.

Informações principais:

- Título
- Autor
- URL do produto
- ASIN ou ISBN, quando disponível
- Preço desejado

## RF02 - Listar livros cadastrados

O sistema deve exibir os livros cadastrados pelo usuário.

A listagem deve mostrar, no mínimo:

- Título
- Autor
- Preço desejado
- Último preço encontrado
- Status do monitoramento

## RF03 - Editar livro

O sistema deve permitir editar os dados de um livro já cadastrado.

Exemplos de dados editáveis:

- Título
- Autor
- URL
- Preço desejado
- Status ativo/inativo

## RF04 - Remover livro

O sistema deve permitir remover um livro cadastrado.

## RF05 - Verificar preço manualmente

O sistema deve permitir executar uma verificação de preço manual para um livro específico.

## RF06 - Verificar preços automaticamente

O sistema deve verificar automaticamente os preços dos livros ativos em intervalos configuráveis.

Exemplo inicial:

- A cada 6 horas

## RF07 - Salvar histórico de preços

A cada verificação realizada, o sistema deve salvar o preço encontrado no histórico do livro.

## RF08 - Identificar promoção

O sistema deve identificar uma promoção quando o preço atual for menor ou igual ao preço desejado pelo usuário.

## RF09 - Gerar alerta de promoção

Quando uma promoção for identificada, o sistema deve gerar um alerta contendo:

- Livro
- Preço atual
- Preço desejado
- Data da verificação

## RF10 - Exibir notificação local

O sistema deve exibir uma notificação no sistema operacional quando um livro entrar em promoção.

No Linux, a notificação poderá ser feita usando `notify-send`.
