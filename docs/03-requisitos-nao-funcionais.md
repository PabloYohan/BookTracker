# 03 - Requisitos Não Funcionais

Este documento descreve características de qualidade esperadas para o projeto.

## RNF01 - Simplicidade

A aplicação deve ser simples de instalar, executar e utilizar.

Como o projeto é pessoal, a prioridade inicial é manter a solução objetiva e fácil de manter.

## RNF02 - Organização do código

O código deve ser organizado em camadas ou módulos, separando responsabilidades como:

- Interface
- Regras de negócio
- Acesso a dados
- Consulta de preços
- Notificações

## RNF03 - Persistência local

A aplicação deve armazenar os dados localmente.

Para a primeira versão, o banco recomendado é o SQLite.

## RNF04 - Compatibilidade com Linux

A aplicação deve funcionar em ambiente Linux, com foco inicial no Arch Linux.

## RNF05 - Notificações do sistema

As notificações devem utilizar recursos nativos do ambiente Linux sempre que possível.

Exemplo:

```bash
notify-send "Livro em promoção!" "O livro está abaixo do preço desejado."
```

## RNF06 - Baixo consumo de recursos

A aplicação deve consumir poucos recursos de CPU e memória, já que ficará em execução em segundo plano.

## RNF07 - Tratamento de falhas

O sistema deve tratar falhas comuns, como:

- Produto não encontrado
- Erro ao consultar preço
- Falha de conexão
- Resposta inesperada da fonte de dados

## RNF08 - Logs básicos

O sistema deve registrar logs simples para ajudar na identificação de problemas.

Exemplos:

- Início de verificação
- Preço encontrado
- Promoção detectada
- Erro ao consultar produto

## RNF09 - Manutenibilidade

A fonte de consulta de preços deve ser isolada em um serviço próprio, permitindo trocar a implementação futuramente sem afetar o restante do sistema.
