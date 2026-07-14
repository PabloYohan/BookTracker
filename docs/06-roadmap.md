# 06 - Roadmap

Este documento apresenta um planejamento simples para evolução do projeto.

## Situação atual

O projeto concluiu o cadastro de livros, registro manual de preços, histórico, identificação de promoção e alertas. A próxima etapa é implementar notificações locais no Linux.

## Versão 0.1 - Estrutura inicial

- [X] Criar solução .NET
- [X] Criar estrutura de pastas do projeto
- [X] Criar entidades principais
- [X] Configurar PostgreSQL
- [X] Criar documentação inicial

## Versão 0.2 - Cadastro de livros

- [X] Criar CRUD de livros
- [X] Permitir definir preço desejado
- [X] Permitir ativar ou desativar monitoramento
- [X] Criar listagem de livros cadastrados

## Versão 0.3 - Registro manual de preços

- [X] Validar URL da Amazon Brasil
- [X] Extrair ASIN automaticamente
- [X] Gerar URL canônica
- [X] Criar endpoint de registro manual de preço
- [X] Salvar histórico de preços
- [X] Exibir último preço registrado
- [X] Exibir menor preço registrado

## Versão 0.4 - Alertas

- [X] Criar regra para identificar promoção
- [X] Gerar alerta quando preço estiver abaixo do desejado
- [X] Marcar alerta como lido / não lido
- [X] Listar alertas e contagem de não lidos
- [ ] Exibir notificações no Linux

## Versão 0.5 - Execução automática

- [X] ~~Criar Background Service de verificação automática~~ *(removido — substituído por registro manual)*
- [X] ~~Configurar intervalo de execução~~ *(removido)*
- [X] ~~Verificar apenas livros ativos~~ *(removido)*

## Versão 1.0 - Primeira versão utilizável

- [X] Aplicação funcionando localmente
- [X] Cadastro de livros com validação de URL
- [X] Histórico de preços
- [X] Alertas de promoção
- [ ] Notificações no sistema operacional

## Melhorias futuras

- [ ] Interface desktop (Avalonia UI ou frontend web)
- [ ] Ícone na bandeja do sistema
- [ ] Gráfico de histórico de preços
- [ ] Integração com Telegram ou Discord
- [ ] Comparação com outras lojas
