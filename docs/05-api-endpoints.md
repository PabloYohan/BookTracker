# 05 - API Endpoints

Este documento descreve os endpoints da API do projeto.

## Base URL

```http
/api
```

Em desenvolvimento, a aplicação expõe Swagger em `/swagger`.

## Endpoints implementados

### Livros

#### Listar livros

```http
GET /books
```

Retorna todos os livros cadastrados, ordenados por título.

Exemplo de resposta:

```json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "title": "O Iluminado",
    "author": "Stephen King",
    "targetPrice": 35.00,
    "lastPrice": null,
    "isActive": true
  }
]
```

#### Buscar livro por ID

```http
GET /books/{id}
```

Retorna os dados completos de um livro específico.

Exemplo de resposta:

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "title": "O Iluminado",
  "author": "Stephen King",
  "isbn": "",
  "asin": "8532520709",
  "productUrl": "https://www.amazon.com.br/dp/8532520709",
  "targetPrice": 35.00,
  "isActive": true,
  "createdAt": "2026-07-13T20:00:00Z"
}
```

#### Cadastrar livro

```http
POST /books
```

Exemplo de corpo:

```json
{
  "title": "O Iluminado",
  "author": "Stephen King",
  "productUrl": "https://www.amazon.com.br/dp/8532520709",
  "asin": "8532520709",
  "targetPrice": 35.00
}
```

Regras de validação:

- `title` é obrigatório
- `author` é obrigatório
- `productUrl` é obrigatório
- `targetPrice` deve ser maior que zero
- `isbn` e `asin` são opcionais
- O livro é criado com `isActive: true`

Resposta de sucesso: `201 Created`, com o livro criado e header `Location` apontando para `/api/books/{id}`.

#### Atualizar livro

```http
PUT /books/{id}
```

Exemplo de corpo:

```json
{
  "title": "O Iluminado",
  "author": "Stephen King",
  "productUrl": "https://www.amazon.com.br/dp/8532520709",
  "asin": "8532520709",
  "targetPrice": 30.00,
  "isActive": true
}
```

Atualiza todos os campos editáveis do livro, incluindo preço desejado e status de monitoramento.

#### Ativar monitoramento

```http
PATCH /books/{id}/activate
```

Define `isActive` como `true` para o livro informado.

#### Desativar monitoramento

```http
PATCH /books/{id}/deactivate
```

Define `isActive` como `false` para o livro informado.

#### Remover livro

```http
DELETE /books/{id}
```

Remove um livro cadastrado e seus registros relacionados.

Resposta de sucesso: `204 No Content`.

## Endpoints planejados

### Preços

#### Verificar preço de um livro

```http
POST /books/{id}/check-price
```

Executa uma verificação manual de preço para um livro específico.

#### Listar histórico de preços

```http
GET /books/{id}/price-history
```

Retorna o histórico de preços de um livro.

#### Buscar menor preço registrado

```http
GET /books/{id}/lowest-price
```

Retorna o menor preço já registrado para um livro.

### Alertas

#### Listar alertas

```http
GET /alerts
```

Retorna os alertas gerados pelo sistema.

#### Marcar alerta como lido

```http
PATCH /alerts/{id}/read
```

Marca um alerta como lido.

### Verificação geral

#### Executar verificação de todos os livros ativos

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
| 400 | Requisição inválida ou erro de validação |
| 404 | Recurso não encontrado |
| 500 | Erro interno |

## Exemplos de requisição

Arquivo de exemplos HTTP disponível em `src/BookPromoTracker.http`.
