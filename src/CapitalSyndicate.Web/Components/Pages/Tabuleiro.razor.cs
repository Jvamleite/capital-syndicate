using CapitalSyndicate.Application.Partidas;
using CapitalSyndicate.Application.Turnos.Interfaces;
using CapitalSyndicate.Domain.Cartas;
using CapitalSyndicate.Domain.Enums;
using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Mercados;
using CapitalSyndicate.Domain.Partidas;
using CapitalSyndicate.Domain.Projetos;
using CapitalSyndicate.Web.Services;
using CapitalSyndicate.Web.ViewModels;
using Microsoft.AspNetCore.Components;

namespace CapitalSyndicate.Web.Components.Pages
{
    public partial class Tabuleiro
    {
        [Inject] private PartidaEstado EstadoPartida { get; set; } = default!;
        [Inject] private ITurnoService TurnoService { get; set; } = default!;
        [Inject] private PartidaService PartidaService { get; set; } = default!;
        [Inject] private NavigationManager Nav { get; set; } = default!;
        [Inject] private EntradaJogadorBlazor Entrada { get; set; } = default!;

        private HashSet<CartaProfissional> _cartasSelecionadas = [];
        private CriseViewModel? _criseAtual;
        private CartaProfissional? _gerenteSelecionado;
        private string? _erro;
        private bool _modalManterCartas;
        private int _escalaProjeto;
        private TaskCompletionSource<int>? _manterCartasTcs;
        private bool _modalMercadoExpansao;
        private List<Mercado> _mercadosExpansao = [];
        private TaskCompletionSource<Mercado>? _mercadoExpansaoTcs;
        private bool _modalMemorando;
        private List<Presenca> _presencasMemorando = [];
        private TaskCompletionSource<Presenca>? _memorandoTcs;
        private bool _modalAtivoCorporativo;
        private TaskCompletionSource<TipoAtivo>? _ativoCorporativoTcs;
        private bool _escolhendoSegundoProjeto;
        private TaskCompletionSource<Projeto?>? _segundoProjetoTcs;
        private TaskCompletionSource? _fecharCriseTcs;

        private static readonly string[] _cores =
            ["#c9a84c", "#4ca8c9", "#a84cc9", "#4cc97a", "#c94c4c"];

        private string CorJogador(int i) => _cores[i % _cores.Length];

        private async Task ComprarDoMonte()
        {
            _erro = null;
            try
            {
                await TurnoService.ComprarCartas(1, EstadoPartida.Partida!);
                LimparSelecao();
            }
            catch (Exception ex) { _erro = ex.Message; }
        }

        private void ComprarDoMercado(CartaProfissional carta)
        {
            _erro = null;
            try
            {
                Partida partida = EstadoPartida.Partida!;
                Trimestre trimestre = partida.ObterTrimestreAtual();
                Jogador jogador = trimestre.TurnoAtual.Jogador;

                Carta comprada = partida.Baralho.ComprarCartaDoMercadoDeTalentos(carta.Id);
                jogador.ComprarCartas([comprada]);
                trimestre.TurnoAtual.Encerrar();
                trimestre.AvancarTurno();
                LimparSelecao();
            }
            catch (Exception ex) { _erro = ex.Message; }
        }

        private async Task ExecutarProjeto()
        {
            _erro = null;
            if (_gerenteSelecionado is null) { _erro = "Escolha um gerente para o projeto."; return; }

            try
            {
                if (_escolhendoSegundoProjeto)
                {
                    Projeto segundoProjeto = Projeto.Criar(
                        [.. _cartasSelecionadas],
                        _gerenteSelecionado);

                    _escolhendoSegundoProjeto = false;
                    _segundoProjetoTcs!.SetResult(segundoProjeto);
                    LimparSelecao();
                    return;
                }

                Projeto projeto = Projeto.Criar([.. _cartasSelecionadas], _gerenteSelecionado);

                IEnumerable<Carta> cartasDescartadas = await TurnoService.ExecutarProjeto(projeto, EstadoPartida.Partida!);

                EstadoPartida.Partida!.Baralho.AdicionarCartasNoMercadoDeTalentos([.. cartasDescartadas]);

                LimparSelecao();
            }
            catch (Exception ex) { _erro = ex.Message; }
        }

        private void AvancarTrimestre()
        {
            _erro = null;
            try
            {
                Partida partida = EstadoPartida.Partida!;

                ResultadoAvancoTrimestre resultado = PartidaService.AvancarTrimestre(
                    partida,
                    partida.JogadorQueRevelouUltimaCrise ?? partida.Jogadores[0]);

                if (resultado.JogoEncerrado)
                {
                    EstadoPartida.ResultadoFinal = resultado;
                    Nav.NavigateTo("/resultado");
                }

                LimparSelecao();
            }
            catch (Exception ex) { _erro = ex.Message; }
        }

        private void ToggleCarta(CartaProfissional carta)
        {
            _erro = null;
            if (_cartasSelecionadas.Contains(carta))
            {
                _cartasSelecionadas.Remove(carta);
                if (_gerenteSelecionado == carta)
                {
                    _gerenteSelecionado = null;
                }
            }
            else
            {
                _cartasSelecionadas.Add(carta);
            }
        }

        private void LimparSelecao()
        {
            _cartasSelecionadas.Clear();
            _gerenteSelecionado = null;
        }

        private bool PodeComprar(Jogador ativo, Trimestre trimestre) =>
            !trimestre.Encerrado &&
            !trimestre.TurnoAtual.Encerrado &&
            ativo.CartasNaMao.Count < 10;

        private bool PodeExecutar() =>
            _cartasSelecionadas.Count >= 1 &&
            _gerenteSelecionado is not null &&
            EstadoPartida.Partida is not null &&
            !EstadoPartida.Partida.ObterTrimestreAtual().Encerrado;

        private CriseViewModel CriarViewModelCrise(CartaCrise crise) => crise switch
        {
            CriseCrashDaBolsa => new()
            {
                Nome = crise.Descricao(),
                Flavor = "Uma onda de vendas em pânico destruiu bilhões em valor de mercado em poucas horas.",
                Efeito = "Mercados financeiros entram em colapso e investidores recuam."
            },
            CriseRecessaoEconomica => new()
            {
                Nome = crise.Descricao(),
                Flavor = "O consumo desacelerou drasticamente e empresas começaram demissões em massa.",
                Efeito = "Projetos mais caros se tornam difíceis de sustentar."
            },
            CriseRumoresDeMercado => new()
            {
                Nome = crise.Descricao(),
                Flavor = "Informações desencontradas e especulações criaram instabilidade nos setores estratégicos.",
                Efeito = "A confiança do mercado foi abalada."
            },
            _ => new() { Nome = "CRISE", Flavor = "", Efeito = "" }
        };

        private async Task<int> EscolherNumManterCartasAsync(
            Jogador jogador,
            Partida partida,
            int escalaProjeto)
        {
            int cartasRestantes = jogador.CartasNaMao.Count - escalaProjeto;
            if (cartasRestantes <= 0)
            {
                return 0;
            }

            _escalaProjeto = Math.Min(escalaProjeto, cartasRestantes);
            _modalManterCartas = true;
            _manterCartasTcs = new();
            await InvokeAsync(StateHasChanged);
            return await _manterCartasTcs.Task;
        }

        private void ConfirmarManterCartas(int quantidade)
        {
            _modalManterCartas = false;
            _manterCartasTcs?.SetResult(quantidade);
        }

        private async Task<Mercado> EscolherMercadoExpansaoAsync(
            Jogador jogador,
            List<Mercado> mercadosValidos)
        {
            _mercadosExpansao = mercadosValidos;
            _modalMercadoExpansao = true;
            _mercadoExpansaoTcs = new();
            await InvokeAsync(StateHasChanged);
            return await _mercadoExpansaoTcs.Task;
        }

        private void ConfirmarMercadoExpansao(Mercado mercado)
        {
            _modalMercadoExpansao = false;
            _mercadoExpansaoTcs?.SetResult(mercado);
        }

        private async Task<Presenca> EscolherPresencaParaMemorandoAsync(
            Jogador jogador,
            List<Presenca> presencasDisponiveis)
        {
            _presencasMemorando = presencasDisponiveis;
            _modalMemorando = true;
            _memorandoTcs = new();
            await InvokeAsync(StateHasChanged);
            return await _memorandoTcs.Task;
        }

        private void ConfirmarPresencaMemorando(Presenca presenca)
        {
            _modalMemorando = false;
            _memorandoTcs?.SetResult(presenca);
        }

        private async Task<TipoAtivo> EscolherTipoAtivoCorporativoAsync(Jogador jogador)
        {
            _modalAtivoCorporativo = true;
            _ativoCorporativoTcs = new();
            await InvokeAsync(StateHasChanged);
            return await _ativoCorporativoTcs.Task;
        }

        private void ConfirmarAtivoCorporativo(TipoAtivo tipo)
        {
            _modalAtivoCorporativo = false;
            _ativoCorporativoTcs?.SetResult(tipo);
        }

        private async Task<Projeto?> EscolherSegundoProjetoAsync(
            Jogador jogador,
            Partida partida)
        {
            LimparSelecao();
            _escolhendoSegundoProjeto = true;
            _segundoProjetoTcs = new TaskCompletionSource<Projeto?>();
            await InvokeAsync(StateHasChanged);
            return await _segundoProjetoTcs.Task;
        }

        private void CancelarSegundoProjeto()
        {
            _escolhendoSegundoProjeto = false;
            _segundoProjetoTcs!.SetResult(null);
            LimparSelecao();
        }

        private void FecharCrise()
        {
            _criseAtual = null;
            _fecharCriseTcs?.SetResult();
        }

        protected override void OnInitialized()
        {
            Entrada.OnEscolherNumManterCartas = EscolherNumManterCartasAsync;
            Entrada.OnEscolherMercadoExpansao = EscolherMercadoExpansaoAsync;
            Entrada.OnEscolherPresencaParaMemorando = EscolherPresencaParaMemorandoAsync;
            Entrada.OnEscolherTipoAtivoCorporativo = EscolherTipoAtivoCorporativoAsync;
            Entrada.OnEscolherSegundoProjeto = EscolherSegundoProjetoAsync;

            Entrada.OnCriseRevelada = async (crise) =>
            {
                _criseAtual = CriarViewModelCrise(crise);
                _fecharCriseTcs = new TaskCompletionSource();
                await InvokeAsync(StateHasChanged);
                await _fecharCriseTcs.Task;
            };
        }

        // ── Helpers de exibição ───────────────────────────────────

        private static string SetorCss(Mercado m) => m switch
        {
            Inovacao => "inovacao",
            Tecnologia => "tecnologia",
            Logistica => "logistica",
            Infraestrutura => "infra",
            EstrategiasCorporativas => "estrategia",
            MercadoFinanceiro => "financeiro",
            _ => "default"
        };

        private static string SetorCssEnum(Setor s) => s switch
        {
            Setor.Inovacao => "inovacao",
            Setor.Tecnologia => "tecnologia",
            Setor.Logistica => "logistica",
            Setor.Infraestrutura => "infra",
            Setor.EstrategiasCorporativas => "estrategia",
            Setor.MercadoFinanceiro => "financeiro",
            _ => "default"
        };

        private static string SetorNome(Setor s) => s switch
        {
            Setor.Inovacao => "Inovação",
            Setor.Tecnologia => "Tecnologia",
            Setor.Logistica => "Logística",
            Setor.Infraestrutura => "Infraestrutura",
            Setor.EstrategiasCorporativas => "Estratégias Corporativas",
            Setor.MercadoFinanceiro => "Financeiro",
            _ => s.ToString()
        };

        private static string NomeCargo(Cargo c) => c switch
        {
            Cargo.Facilitador => "Facilitador",
            Cargo.DiretorDeComunicacao => "Dir. Comunicação",
            Cargo.Investidor => "Investidor",
            Cargo.GestorDeRh => "Gestor RH",
            Cargo.DiretorDeExpansao => "Dir. Expansão",
            Cargo.DiretorDeOperacoes => "Dir. Operações",
            Cargo.EspecialistaEsg => "Esp. ESG",
            Cargo.GestorDePortifolio => "Gest. Portfólio",
            Cargo.Auditor => "Auditor",
            Cargo.NegociadorInternacional => "Neg. Internacional",
            Cargo.Trainee => "Trainee",
            Cargo.ConsultorExterno => "Consultor Ext.",
            _ => c.ToString()
        };

        private static bool MercadoAvancado(Mercado m) => m switch
        {
            Inovacao i => i.InjecaoDeRecursos,
            Tecnologia t => t.ApenasLiderPontua,
            Logistica l => l.ExpansaoParalela,
            Infraestrutura inf => inf.Monopolio,
            EstrategiasCorporativas e => e.OperacoesAfterHours,
            MercadoFinanceiro mf => mf.CashOutDisponivel,
            _ => false
        };
    }
}