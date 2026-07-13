# 06 - Roadmap

Este documento apresenta um planejamento simples para evolução do projeto.

## Situação atual

O projeto concluiu a base estrutural e o cadastro de livros. A próxima etapa é implementar a consulta de preços e o histórico automático.

## Versão 0.1 - Estrutura inicial

- [X] Criar solução .NET
- [X] Criar estrutura de pastas do projeto
- [X] Criar entidades principais
- [X] Configurar Postgress
- [X] Criar documentação inicial

## Versão 0.2 - Cadastro de livros

- [X] Criar CRUD de livros
- [X] Permitir definir preço desejado
- [X] Permitir ativar ou desativar monitoramento
- [X] Criar listagem de livros cadastrados

## Versão 0.3 - Consulta de preços

- [ ] Criar serviço de consulta de preço
- [ ] Criar verificação manual de preço
- [ ] Salvar histórico de preços
- [ ] Exibir último preço encontrado
- [ ] Exibir menor preço registrado

Observação: a listagem de livros já expõe `lastPrice`, mas o valor só será preenchido após a implementação da consulta de preços.

## Versão 0.4 - Alertas

- [ ] Criar regra para identificar promoção
- [ ] Gerar alerta quando preço estiver abaixo do desejado
- [ ] Marcar alerta como lido
- [ ] Exibir notificações no Linux

## Versão 0.5 - Execução automática

- [ ] Criar Background Service de verificação automática
- [ ] Configurar intervalo de execução
- [ ] Verificar apenas livros ativos
- [ ] Registrar logs básicos das verificações

## Versão 1.0 - Primeira versão utilizável

- [ ] Aplicação funcionando localmente
- [X] Cadastro de livros
- [ ] Histórico de preços
- [ ] Alertas de promoção
- [ ] Notificações no sistema operacional

## Melhorias futuras

- [ ] Interface desktop (Avalonia UI ou frontend web)
- [ ] Ícone na bandeja do sistema
- [ ] Gráfico de histórico de preços
- [ ] Integração com Telegram ou Discord
- [ ] Comparação com outras lojas
