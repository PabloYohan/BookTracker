# 05 - API Endpoints

Este documento descreve os endpoints iniciais da API do projeto.

## Base URL

```http
/api
```

## Livros

### Listar livros

```http
GET /books
```

Retorna todos os livros cadastrados.

### Buscar livro por ID

```http
GET /books/{id}
```

Retorna os dados de um livro específico.

### Cadastrar livro

```http
POST /books
```

Exemplo de corpo:

```json
{
  "title": "O Iluminado",
  "author": "Stephen King",
  "productUrl": "https://www.amazon.com.br/...",
  "asin": "8532520709",
  "targetPrice": 35.00
}
```

### Atualizar livro

```http
PUT /books/{id}
```

Atualiza os dados de um livro cadastrado.

### Remover livro

```http
DELETE /books/{id}
```

Remove um livro cadastrado.

## Preços

### Verificar preço de um livro

```http
POST /books/{id}/check-price
```

Executa uma verificação manual de preço para um livro específico.

### Listar histórico de preços

```http
GET /books/{id}/price-history
```

Retorna o histórico de preços de um livro.

### Buscar menor preço registrado

```http
GET /books/{id}/lowest-price
```

Retorna o menor preço já registrado para um livro.

## Alertas

### Listar alertas

```http
GET /alerts
```

Retorna os alertas gerados pelo sistema.

### Marcar alerta como lido

```http
PATCH /alerts/{id}/read
```

Marca um alerta como lido.

## Verificação geral

### Executar verificação de todos os livros ativos

```http
POST /price-checks/run
```

Executa a verificação de preço para todos os livros ativos.

## Códigos de resposta

| Código | Descrição |
|---|---|
| 200 | Sucesso |
| 201 | Criado com sucesso |
| 204 | Removido com sucesso |
| 400 | Requisição inválida |
| 404 | Recurso não encontrado |
| 500 | Erro interno |
