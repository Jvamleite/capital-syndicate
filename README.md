# Capital Syndicate

> Jogo de estratégia corporativa digital para 2 a 5 jogadores, desenvolvido em C# com Blazor Server.

Capital Syndicate é uma adaptação digital do sistema **Archeos Society** para o contexto de competição corporativa. Cada jogador controla uma mega-corporação que recruta profissionais, executa projetos e disputa presença em mercados globais ao longo de múltiplos trimestres. Vence quem acumular mais **Pontos de Vitória** ao final da partida.

---

## Índice

- [Sobre o Projeto](#sobre-o-projeto)
- [Tecnologias](#tecnologias)
- [Arquitetura](#arquitetura)
- [Estrutura de Pastas](#estrutura-de-pastas)
- [Como Rodar](#como-rodar)
- [Regras do Jogo](#regras-do-jogo)
- [Cargos e Habilidades](#cargos-e-habilidades)
- [Mercados](#mercados)
- [Tokens Especiais](#tokens-especiais)
- [Diagrama de Classes](#diagrama-de-classes)

---

## Sobre o Projeto

O jogo foi desenvolvido como projeto acadêmico individual com metodologia **Scrum**, entregue em 6 sprints semanais. O objetivo era implementar todas as regras de um jogo de tabuleiro complexo em um ambiente digital funcional, do zero ao MVP jogável.

**Características principais:**

- Partidas para 2 a 5 jogadores com duração de 2 ou 3 trimestres
- 12 cargos únicos com habilidades distintas, sendo 6 sorteados por partida
- 6 mercados corporativos com modos avançados independentes
- Sistema de crises de mercado que interrompem e reestruturam o jogo
- Trilha Global de expansão internacional (ativada quando o Negociador Internacional está na partida)
- Três tipos de tokens especiais com regras de pontuação próprias
- Interface web responsiva em tempo real (Blazor Server)

---

## Tecnologias

| Camada | Tecnologia |
|---|---|
| Linguagem | C# 13 / .NET 9 |
| Front-end | Blazor Server (componentes interativos) |
| Estilo | CSS Scoped por componente |
| Arquitetura | Clean Architecture (Domain / Application / Infrastructure / Web) |
| Versionamento | Git / GitHub |

---

## Arquitetura

O projeto segue os princípios de **Clean Architecture**, separando as responsabilidades em quatro camadas:

```
CapitalSyndicate.Domain          ← Entidades, regras de negócio, interfaces de porta
CapitalSyndicate.Application     ← Casos de uso (TurnoService, PartidaService, CriseService)
CapitalSyndicate.Infrastructure  ← Implementações externas (reservado para expansão)
CapitalSyndicate.Web             ← UI Blazor Server, ViewModels, estado de sessão
```

**Fluxo de dependência:** `Web → Application → Domain`. A camada de domínio não conhece nenhuma outra camada.

**Padrões utilizados:**

- **Strategy** — cada `Cargo` possui uma implementação de `IHabilidadeLider` resolvida em tempo de execução via dicionário estático em `CargoExtensions`
- **Factory Method** — `Projeto.Criar()` centraliza toda a validação antes de construir o objeto
- **Port & Adapter** — `IEntradaJogador` isola o domínio das decisões de UI, permitindo que o Blazor injete os modais sem acoplar a lógica de jogo ao framework
- **TaskCompletionSource** — usado no `EntradaJogadorBlazor` para pausar a execução do domínio assincronamente enquanto aguarda input do usuário nos modais

---

## Estrutura de Pastas

```
src/
├── CapitalSyndicate.Domain/
│   ├── Baralho/
│   │   └── Baralho.cs
│   ├── Cartas/
│   │   ├── Habilidades/          ← Uma classe por cargo (Strategy)
│   │   │   ├── IHabilidadeLider.cs
│   │   │   ├── HabilidadeFacilitador.cs
│   │   │   └── ...
│   │   ├── Carta.cs
│   │   ├── CartaProfissional.cs
│   │   ├── CartaCrise.cs
│   │   ├── CriseCrashDaBolsa.cs
│   │   ├── CriseRecessaoEconomica.cs
│   │   └── CriseRumoresDeMercado.cs
│   ├── Enums/
│   │   ├── Cargo.cs              ← Enum + CargoExtensions (mapa cargo → habilidade)
│   │   ├── Setor.cs
│   │   └── TipoAtivos.cs
│   ├── Jogadores/
│   │   ├── Jogador.cs
│   │   └── Presenca.cs
│   ├── Mercados/
│   │   ├── Mercado.cs            ← Classe abstrata base
│   │   ├── Inovacao.cs
│   │   ├── Tecnologia.cs
│   │   ├── Logistica.cs
│   │   ├── Infraestrutura.cs
│   │   ├── MercadoFinanceiro.cs
│   │   ├── EstrategiasCorporativas.cs
│   │   ├── TrilhaGlobal.cs
│   │   └── Patamar.cs
│   ├── Partida/
│   │   ├── Partida.cs
│   │   ├── Trimestre.cs
│   │   └── Turno.cs
│   ├── Ports/
│   │   └── IEntradaJogador.cs
│   ├── Projetos/
│   │   └── Projeto.cs
│   └── Tokens/
│       ├── Token.cs
│       ├── TokenEsg.cs
│       ├── TokenAuditoria.cs
│       └── TokenAtivo.cs
│
├── CapitalSyndicate.Application/
│   ├── Crises/
│   │   ├── CriseService.cs
│   │   └── Interfaces/ICriseService.cs
│   ├── Partidas/
│   │   └── PartidaService.cs
│   └── Turnos/
│       ├── TurnoService.cs
│       └── Interfaces/ITurnoService.cs
│
├── CapitalSyndicate.Infrastructure/
│   └── (reservado)
│
└── CapitalSyndicate.Web/
    ├── Components/
    │   ├── Layout/
    │   │   ├── GameLayout.razor
    │   │   ├── MainLayout.razor
    │   │   └── NavMenu.razor
    │   └── Pages/
    │       ├── Setup.razor         ← Configuração da partida
    │       └── Tabuleiro.razor     ← Tela principal do jogo
    ├── Services/
    │   ├── EntradaJogadorBlazor.cs ← Adapter UI → IEntradaJogador
    │   └── PartidaEstado.cs        ← Estado de sessão (scoped)
    └── ViewModels/
        └── CriseViewModel.cs
```

---

## Como Rodar

**Pré-requisitos:** .NET 9 SDK instalado.

```bash
# Clonar o repositório
git clone https://github.com/seu-usuario/capital-syndicate.git
cd capital-syndicate

# Rodar o projeto web
dotnet run --project src/CapitalSyndicate.Web

# Acessar no navegador
# http://localhost:5094
```

Não há banco de dados nem dependências externas. O estado da partida é mantido em memória na sessão do servidor Blazor.

---

## Regras do Jogo

### Preparação

1. Acesse `/setup` e adicione de 2 a 5 jogadores
2. Selecione exatamente **6 dos 12 cargos** disponíveis — apenas cartas desses cargos entrarão no baralho
3. O sistema embaralha o baralho e posiciona as 3 Cartas de Crise na metade inferior, distribuídas aleatoriamente
4. Cada jogador recebe 1 carta inicial e o **Mercado de Talentos** é aberto com `(nº jogadores + 2)` cartas

O número de trimestres é definido automaticamente: **2 trimestres** para 2–3 jogadores, **3 trimestres** para 4–5 jogadores.

---

### No Seu Turno

Você realiza **uma** das duas ações:

#### Opção A — Recrutar (Comprar 1 Carta)

Escolha entre comprar a carta do topo do Monte (fechado) ou pegar uma carta visível do Mercado de Talentos.

- Se o **Mercado de Talentos** estiver vazio ao comprar do Monte, compre **1 carta adicional**
- Se você revelar uma **Carta de Crise**, ela é resolvida imediatamente e você compra outra para substituir
- Limite de **10 cartas** na mão — se já estiver com 10, esta ação está bloqueada

#### Opção B — Executar um Projeto

1. **Selecione as cartas** da sua mão — todas devem compartilhar o **mesmo Setor** (cor) ou o **mesmo Cargo**. Trainees e Consultores Externos podem entrar em qualquer projeto como coringas
2. **Escolha o Gerente** — define o Setor do projeto (onde sua presença avança) e a habilidade ativada. Trainees e Consultores Externos não podem ser gerentes
3. **Verifique o avanço** — se a Escala (nº de cartas) atingir o requisito do próximo espaço no mercado do Setor do Gerente, sua Presença avança 1 espaço
4. **Habilidade do Cargo** é ativada
5. **Layoff obrigatório** — todas as cartas restantes na mão vão para o Mercado de Talentos (salvo efeitos de retenção)

---

### Cartas de Crise

| Crise | Efeito |
|---|---|
| **1ª — Rumores de Mercado** | O Mercado de Talentos é descartado e reposto com novas cartas |
| **2ª — Recessão Econômica** | Cada jogador (começando por quem revelou) envia 1 carta da mão para o Mercado de Talentos |
| **3ª — Crash da Bolsa** | O Trimestre encerra imediatamente |

---

### Fim do Trimestre

Quando a 3ª Crise é revelada, em ordem:

1. Todas as cartas nas mãos e no Mercado de Talentos retornam ao baralho
2. Efeitos de **Fim do Trimestre** são resolvidos (cargos e mercado de Estratégias Corporativas)
3. **Pontuação dos Mercados** — cada jogador recebe os PV da sua posição em cada mercado
4. **Pontuação dos Projetos** — cada projeto rende PV igual à sua Escala
5. Efeitos pós-pontuação são aplicados (reset de Tecnologia, Cash Out do Mercado Financeiro)
6. **Trilha Global** — o jogador mais avançado recebe +2 PV

---

### Fim do Jogo

Após o último trimestre, efeitos de **Fim do Jogo** são resolvidos (Auditor, Gestor de Portfólio). Vence quem tiver mais Pontos de Vitória.

**Desempate:** maior projeto executado no último trimestre. Persistindo, compara-se o segundo maior, e assim por diante.

---

## Cargos e Habilidades

| Cargo | Tipo | Efeito |
|---|---|---|
| **Facilitador** | Imediato | +1 na Escala para fins de verificar avanço de mercado (não conta para pontuação) |
| **Diretor de Comunicação** | Fim do Trimestre | +1 PV adicional por carta na Escala deste projeto |
| **Investidor** | Imediato | Após o Layoff, compre cartas do Monte igual à Escala do projeto |
| **Gestor de RH** | Imediato | No Layoff, pode reter até (Escala) cartas na mão |
| **Diretor de Expansão** | Imediato | Em vez de avançar no mercado do Setor do Gerente, avança em **qualquer** mercado onde a Escala seja suficiente |
| **Diretor de Operações** | Imediato | Se houver avanço de mercado, execute um **segundo projeto** da mão. O Layoff ocorre só após o segundo projeto |
| **Especialista ESG** | Imediato + Fim do Trimestre | Recebe 1 Token ESG (+2 PV no fim do trimestre, depois expiram) |
| **Gestor de Portfólio** | Imediato + Fim do Jogo | Recebe 1 Token de Ativo (Patente, Startup ou Imóvel). Pontuam por concentração no fim do jogo |
| **Auditor** | Imediato + Fim do Jogo | Recebe 1 Token de Auditoria com valor secreto (−2 a +3). Revelado no fim do jogo |
| **Negociador Internacional** | Imediato | Em vez de avançar em mercados, avança na Trilha Global um número de espaços igual à Escala |
| **Trainee** | Restrição + Contínuo | Não pode ser Gerente. Pode entrar em qualquer projeto ignorando Setor e Cargo |
| **Consultor Externo** | Restrição + Contínuo + Fim do Trimestre | Não pode ser Gerente. Entra em qualquer projeto, mas é removido antes da pontuação de Escala do Fim do Trimestre |

---

## Mercados

Cada mercado possui um track de Patamares. Para avançar, a Escala do projeto deve atingir o **requisito do próximo espaço**. Todos os mercados possuem um modo avançado opcional configurável no setup.

| Mercado | Modo Avançado |
|---|---|
| **Inovação e P&D** | Ao avançar, compre cartas do Monte até o limite indicado pelo espaço recém-alcançado |
| **Tecnologia** | Apenas o jogador(es) líder(es) pontua no fim do trimestre. Após pontuar, todos os marcadores são resetados |
| **Logística** | Cada jogador tem **dois** marcadores neste mercado. Ao avançar, escolhe qual mover. A pontuação vem do marcador **menos avançado** |
| **Infraestrutura** | Liderança isolada recebe 100% dos pontos. Em empate na liderança, todos recebem **metade** |
| **Mercado Financeiro** | Ao avançar, pode fazer **Cash Out**: recebe os PV da posição atual imediatamente e reseta o marcador para o início |
| **Estratégias Corporativas** | Quando a 3ª Crise encerra o trimestre, cada jogador com presença aqui pode executar **1 projeto extra** antes da pontuação (Gerente deve ser do Setor correspondente) |

### Trilha Global

Ativada quando o cargo **Negociador Internacional** está na partida. Ao avançar, o jogador passa por espaços de **Memorando de Entendimento** — cada um permite avançar 1 marcador em qualquer mercado ignorando requisitos de Escala. O jogador mais avançado no fim de cada trimestre recebe +2 PV.

---

## Tokens Especiais

### Token ESG
Gerado pelo **Especialista ESG**. Vale **+2 PV** no Fim do Trimestre e é descartado em seguida.

### Token de Auditoria
Gerado pelo **Auditor**. O valor é sorteado secretamente entre −2, −1, 0, +1, +2 ou +3. Todos são revelados e somados no **Fim do Jogo**.

### Token de Ativo Corporativo
Gerado pelo **Gestor de Portfólio**. O jogador escolhe o tipo: **Patente**, **Startup** ou **Imóvel**. Pontuam no Fim do Jogo conforme a tabela:

| Quantidade do mesmo tipo | Pontos |
|---|---|
| 1 | 1 PV |
| 2 | 3 PV |
| 3 | 6 PV |
| 4 ou mais | 10 PV |

**Bônus de diversificação:** +4 PV se possuir ao menos 1 de cada tipo.

---

## Diagrama de Classes

O diagrama completo está disponível em https://mermaid.ai/d/d3339597-dd9a-4228-8db0-0757c1197fd3.

**Principais relações:**

```
Partida ──── Trimestre ──── Turno ──── Jogador
                │
                └──── Mercado (6 subclasses)
                
Jogador ──── Presenca ──── Mercado
        ──── Carta (mão)
        ──── Projeto ──── CartaProfissional ──── Cargo ──── IHabilidadeLider
        ──── Token (ESG / Auditoria / Ativo)
        
Baralho ──── Carta (Monte / MercadoDeTalentos / Descarte)
```

---

## Autor

**João Victor Amaral de M. Leite**  
Projeto desenvolvido individualmente — Disciplina de Gerenciamento de Projetos de Software
