# 02 - Requisitos Funcionais

Este documento descreve as principais funcionalidades esperadas para o projeto e o status de implementação de cada uma.

Legenda de status:

- **Implementado** — funcionalidade disponível na API
- **Parcial** — parte da funcionalidade existe, mas ainda depende de outras etapas
- **Pendente** — ainda não implementado
- **Removido** — funcionalidade descontinuada

## RF01 - Cadastrar livro

**Status:** Implementado

O sistema deve permitir o cadastro de livros para monitoramento.

Informações principais:

- Título
- Autor
- URL do produto (Amazon Brasil)
- ISBN, quando disponível
- Preço desejado

Implementação atual:

- Endpoint `POST /api/books`
- Novos livros são criados com monitoramento ativo por padrão
- Campos obrigatórios validados: título, autor, URL do produto e preço desejado
- O ASIN é extraído automaticamente da URL; não precisa ser informado pelo usuário
- A URL é validada e salva no formato canônico

## RF02 - Listar livros cadastrados

**Status:** Implementado

O sistema deve exibir os livros cadastrados pelo usuário.

A listagem deve mostrar, no mínimo:

- Título
- Autor
- ASIN
- URL do produto (canônica)
- Preço desejado
- Último preço registrado
- Status do monitoramento

Implementação atual:

- Endpoint `GET /api/books`
- O campo `lastPrice` retorna o preço do registro manual mais recente, ou `null` quando ainda não existe histórico

## RF03 - Editar livro

**Status:** Implementado

O sistema deve permitir editar os dados de um livro já cadastrado.

Implementação atual:

- Endpoint `PUT /api/books/{id}`
- Endpoints auxiliares `PATCH /api/books/{id}/activate` e `PATCH /api/books/{id}/deactivate`
- A URL é revalidada e o ASIN é reextraído na edição

## RF04 - Remover livro

**Status:** Implementado

O sistema deve permitir remover um livro cadastrado.

Implementação atual:

- Endpoint `DELETE /api/books/{id}`
- Histórico de preços e alertas relacionados são removidos em cascata

## RF05 - Registrar preço manualmente

**Status:** Implementado

O sistema deve permitir que o usuário registre manualmente o preço encontrado na Amazon.

Implementação atual:

- Endpoint `POST /api/books/{id}/prices`
- Aceita `price`, `currency` (opcional, padrão BRL) e `observedAt` (opcional)
- Salva no histórico com fonte `Manual - Amazon`
- Retorna se o preço-alvo foi atingido e se um alerta foi criado
- Apenas livros ativos podem receber registros de preço

## RF06 - Verificar preços automaticamente

**Status:** Removido

A consulta automática de preços foi descontinuada. O endpoint `POST /api/books/{id}/check-price` retorna `410 Gone` e `POST /api/price-checks/run` foi removido.

## RF07 - Salvar histórico de preços

**Status:** Implementado

Cada registro manual de preço é salvo no histórico do livro.

Implementação atual:

- Entidade `PriceHistory` e tabela no banco
- Gravação a cada registro manual via `POST /api/books/{id}/prices`
- Endpoint `GET /api/books/{id}/price-history` para consultar o histórico
- Endpoint `GET /api/books/{id}/lowest-price` para consultar o menor preço registrado

## RF08 - Identificar promoção

**Status:** Implementado

O sistema identifica uma promoção quando o preço registrado é menor ou igual ao preço desejado.

Implementação atual:

- Avaliação feita em `PriceHistoryService` no momento do registro manual
- Campo `targetReached` na resposta de `POST /api/books/{id}/prices`

## RF09 - Gerar alerta de promoção

**Status:** Implementado

Quando uma promoção for identificada, o sistema gera um alerta contendo:

- Livro
- Preço atual
- Preço desejado
- Mensagem descritiva
- Data de criação

Regras contra duplicatas:

- Alerta no cruzamento do preço-alvo (preço anterior acima, novo igual ou abaixo)
- Alerta no primeiro preço já abaixo do alvo
- Novo alerta quando o preço cai abaixo do alerta mais recente
- Sem alerta repetido para o mesmo preço

Implementação atual:

- Entidade `Alert` e tabela no banco
- Endpoints em `/api/alerts`

## RF10 - Exibir notificação local

**Status:** Pendente

O sistema deve exibir uma notificação no sistema operacional quando um livro entrar em promoção.

No Linux, a notificação poderá ser feita usando `notify-send`.

## RF11 - Validar URL da Amazon Brasil

**Status:** Implementado

O sistema deve validar URLs da Amazon Brasil, extrair o ASIN e gerar URL canônica.

Implementação atual:

- Serviço `AmazonProductUrlParser`
- Validação no cadastro e edição de livros
- Domínios aceitos: `amazon.com.br`, `www.amazon.com.br`, `m.amazon.com.br` e subdomínios legítimos de `.amazon.com.br`
- Formatos aceitos: `/dp/{ASIN}`, `/gp/product/{ASIN}` e variantes com slug
