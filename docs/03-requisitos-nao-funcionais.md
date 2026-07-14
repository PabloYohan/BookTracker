# 03 - Requisitos Não Funcionais

Este documento descreve características de qualidade esperadas para o projeto.

## RNF01 - Simplicidade

A aplicação deve ser simples de instalar, executar e utilizar.

Como o projeto é pessoal, a prioridade inicial é manter a solução objetiva e fácil de manter.

**Status atual:** atendido, com execução via Docker Compose e `dotnet run`.

## RNF02 - Organização do código

O código deve ser organizado por pastas dentro de um único projeto, separando responsabilidades como:

- `Entities` — modelos de domínio
- `Data` — acesso a dados (EF Core)
- `Dtos` — contratos de entrada e saída da API
- `Services` — regras de negócio
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

A aplicação deve consumir poucos recursos de CPU e memória.

**Status atual:** atendido. Sem Background Service nem requisições HTTP externas, o consumo é mínimo.

## RNF07 - Tratamento de falhas

O sistema deve tratar falhas comuns, como:

- Livro ou alerta não encontrado
- URL inválida
- Preço inválido
- Livro inativo

**Status atual:** implementado. A API retorna `400` para validações, `404` para recursos inexistentes, `409` para conflitos de negócio e `410` para endpoints descontinuados.

## RNF08 - Logs básicos

O sistema deve registrar logs simples para ajudar na identificação de problemas.

Exemplos:

- Livro cadastrado
- URL normalizada
- Preço manual registrado
- Preço-alvo atingido
- Alerta criado

**Status atual:** implementado.

## RNF09 - Sem dependência de APIs externas

A aplicação não deve depender de APIs pagas, scraping ou requisições HTTP para a Amazon.

**Status atual:** atendido. Todos os preços são informados manualmente pelo usuário.

## RNF10 - Manutenibilidade

A validação de URL deve ser isolada em um serviço próprio, permitindo evoluir as regras sem afetar o restante do sistema.

**Status atual:** implementado em `AmazonProductUrlParser`.
