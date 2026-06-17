# Puzzle Deslizante

Projeto desenvolvido em Unity como demo para análise de processo seletivo.

> Versão 1.0 concluída. O jogo está finalizado e pronto para demonstração.
Demo: https://danielribeir0.github.io/PuzzleDeslizante/

## Objetivo

Criar um jogo de Puzzle Deslizante 3x3 com mecânicas de tabuleiro, tempo e vitória.

## Funcionalidades implementadas

- Menu inicial funcional
- Troca de cenas entre menu, jogo, vitória e derrota
- Tabuleiro 3x3 com peças numeradas
- Criação e organização automática das peças no layout correto
- Sistema de arraste para mover peças
- Movimentação apenas para o espaço vazio adjacente
- Embaralhamento automático seguro
- Contador de movimentos atualizado em tempo real
- Cronômetro com modos Casual (300s) e Contra o Tempo (40s)
- Verificação de vitória após cada movimento
- Tela de vitória com resultados da partida
- Tela de derrota quando o tempo se esgota

## Como jogar

1. Abra a cena `MenuInicial` em Unity.
2. Escolha o modo de jogo desejado.
3. Arraste as peças para o espaço vazio para reorganizar o tabuleiro.
4. Complete o tabuleiro na ordem correta para vencer.

## Tecnologias

- Unity 2022.3.19f1
- C#

## Estrutura do projeto

- `MenuInicial` - cena de seleção de modo
- `Jogo` - cena principal do tabuleiro
- `Vitoria` - cena de resultado de vitória
- `Derrota` - cena de resultado de derrota
## Status

Todas as funcionalidades listadas foram implementadas. O projeto está pronto para uso como demo e pode ser testado no Unity.
