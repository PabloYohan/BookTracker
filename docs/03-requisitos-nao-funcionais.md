# 03 - Requisitos Não Funcionais

Este documento descreve características de qualidade esperadas para o projeto.

## RNF01 - Simplicidade

A aplicação deve ser simples de instalar, executar e utilizar.

Como o projeto é pessoal, a prioridade inicial é manter a solução objetiva e fácil de manter.

**Status atual:** atendido na versão inicial, com execução via Docker Compose e `dotnet run`.

## RNF02 - Organização do código

O código deve ser organizado por pastas dentro de um único projeto, separando responsabilidades como:

- `Entities` — modelos de domínio
- `Data` — acesso a dados (EF Core)
- `Dtos` — contratos de entrada e saída da API
- `Services` — regras de negócio e integrações
- `Endpoints` — definição das rotas HTTP
- `Program.cs` — configuração e composição da aplicação

**Status atual:** implementado. Detalhes em [07 - Estrutura do Projeto](07-estrutura-do-projeto.md).

## RNF03 - Persistência local

A aplicação deve armazenar os dados localmente.

Para a primeira versão, o banco utilizado é o PostgreSQL, executado via Docker Compose.

**Status atual:** implementado, com migrations do EF Core para `Books`, `PriceHistories` e `Alerts`.

## RNF04 - Compatibilidade com Linux

A aplicação deve funcionar em ambiente Linux, com foco inicial no Arch Linux.

**Status atual:** atendido no ambiente de desenvolvimento atual.

## RNF05 - Notificações do sistema

As notificações devem utilizar recursos nativos do ambiente Linux sempre que possível.

Exemplo:

```bash
notify-send "Livro em promoção!" "O livro está abaixo do preço desejado."
```

**Status atual:** pendente.

## RNF06 - Baixo consumo de recursos

A aplicação deve consumir poucos recursos de CPU e memória, já que ficará em execução em segundo plano.

**Status atual:** ainda não avaliado em execução contínua.

## RNF07 - Tratamento de falhas

O sistema deve tratar falhas comuns, como:

- Produto não encontrado
- Erro ao consultar preço
- Falha de conexão
- Resposta inesperada da fonte de dados

**Status atual:** parcial. A API já retorna `404` para recursos inexistentes e `400` para validações de entrada.

## RNF08 - Logs básicos

O sistema deve registrar logs simples para ajudar na identificação de problemas.

Exemplos:

- Início de verificação
- Preço encontrado
- Promoção detectada
- Erro ao consultar produto

**Status atual:** pendente.

## RNF09 - Manutenibilidade

A fonte de consulta de preços deve ser isolada em um serviço próprio, permitindo trocar a implementação futuramente sem afetar o restante do sistema.

**Status atual:** parcial. A camada `Services` já existe para livros; o serviço de consulta de preços ainda será adicionado.
