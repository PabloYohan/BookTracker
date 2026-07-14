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
    "asin": "8532520709",
    "productUrl": "https://www.amazon.com.br/dp/8532520709",
    "targetPrice": 35.00,
    "lastPrice": 29.90,
    "isActive": true
  }
]
```

#### Buscar livro por ID

```http
GET /books/{id}
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
  "targetPrice": 35.00
}
```

Regras de validação:

- `title` é obrigatório
- `author` é obrigatório
- `productUrl` é obrigatório e deve ser uma URL válida da Amazon Brasil com ASIN
- `targetPrice` deve ser maior que zero
- `isbn` é opcional
- O ASIN é extraído automaticamente da URL (não informar no corpo)
- O livro é criado com `isActive: true`
- A URL é salva no formato canônico `https://www.amazon.com.br/dp/{ASIN}`

Resposta de sucesso: `201 Created`.

#### Atualizar livro

```http
PUT /books/{id}
```

#### Ativar / desativar monitoramento

```http
PATCH /books/{id}/activate
PATCH /books/{id}/deactivate
```

#### Remover livro

```http
DELETE /books/{id}
```

Resposta de sucesso: `204 No Content`.

### Preços

#### Registrar preço manualmente

```http
POST /books/{id}/prices
```

Exemplo de corpo:

```json
{
  "price": 29.90,
  "currency": "BRL",
  "observedAt": "2026-07-13T23:15:00Z"
}
```

Regras:

- `price` é obrigatório e deve ser maior que zero
- `currency` é opcional (padrão `BRL`; somente BRL aceito nesta versão)
- `observedAt` é opcional (padrão: UTC atual)
- Apenas livros ativos podem receber registros
- Fonte salva: `Manual - Amazon`

Exemplo de resposta (`201 Created`):

```json
{
  "id": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
  "bookId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "bookTitle": "O Iluminado",
  "price": 29.90,
  "currency": "BRL",
  "source": "Manual - Amazon",
  "checkedAt": "2026-07-13T23:15:00Z",
  "targetPrice": 35.00,
  "targetReached": true,
  "alertCreated": true
}
```

#### Listar histórico de preços

```http
GET /books/{id}/price-history
```

#### Buscar menor preço registrado

```http
GET /books/{id}/lowest-price
```

### Alertas

#### Listar alertas

```http
GET /alerts
GET /alerts?unreadOnly=true
GET /alerts?bookId={bookId}
```

Ordenação: do mais recente para o mais antigo.

Exemplo de resposta:

```json
[
  {
    "id": "7ba85f64-5717-4562-b3fc-2c963f66afa6",
    "bookId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "bookTitle": "O Iluminado",
    "currentPrice": 29.90,
    "targetPrice": 35.00,
    "message": "O livro \"O Iluminado\" atingiu o preço desejado. Preço atual: R$ 29,90. Preço desejado: R$ 35,00.",
    "wasRead": false,
    "createdAt": "2026-07-13T23:15:00Z"
  }
]
```

#### Marcar alerta como lido

```http
PATCH /alerts/{id}/read
```

Resposta: `204 No Content`.

#### Marcar alerta como não lido

```http
PATCH /alerts/{id}/unread
```

Resposta: `204 No Content`.

#### Contagem de alertas não lidos

```http
GET /alerts/unread-count
```

Exemplo:

```json
{
  "count": 3
}
```

## Endpoints descontinuados

#### Verificar preço de um livro (descontinuado)

```http
POST /books/{id}/check-price
```

Retorna `410 Gone`:

```json
{
  "message": "A consulta automática foi desativada. Registre o preço manualmente em POST /api/books/{id}/prices."
}
```

#### Verificação geral automática (removido)

```http
POST /price-checks/run
```

Este endpoint foi removido.

## Códigos de resposta

| Código | Descrição |
|---|---|
| 200 | Sucesso |
| 201 | Criado com sucesso |
| 204 | Operação concluída sem conteúdo |
| 400 | Requisição inválida ou erro de validação |
| 404 | Recurso não encontrado |
| 409 | Conflito de negócio (ex.: livro inativo) |
| 410 | Endpoint descontinuado |
| 500 | Erro interno |

## Exemplos de requisição

Arquivo de exemplos HTTP disponível em `src/BookPromoTracker.http`.
