using CapitalSyndicate.Application.Crises.Interfaces;
using CapitalSyndicate.Application.Turnos.Interfaces;
using CapitalSyndicate.Domain.Baralhos;
using CapitalSyndicate.Domain.Cartas;
using CapitalSyndicate.Domain.Enums;
using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Mercados;
using CapitalSyndicate.Domain.Partidas;
using CapitalSyndicate.Domain.Projetos;
using CapitalSyndicate.Domain.Tokens;

namespace CapitalSyndicate.Application.Turnos
{
    public class TurnoService : ITurnoService
    {
        private static readonly HashSet<Cargo> CargosQueGeramToken = [Cargo.Auditor, Cargo.GestorDePortifolio, Cargo.EspecialistaEsg];

        private readonly ICriseService _criseService;

        public TurnoService(ICriseService criseService)
        {
            _criseService = criseService;
        }

        public IEnumerable<Carta> ComprarCartas(int quantidade, Partida partida)
        {
            Trimestre trimestre = partida.ObterTrimestreAtual();
            Turno turno = trimestre.TurnoAtual;

            List<Carta> cartasCompradas = ComprarDoMonte(quantidade, turno.Jogador, partida);

            turno.Jogador.ComprarCartas(cartasCompradas);
            turno.Encerrar();
            trimestre.AvancarTurno();

            return cartasCompradas;
        }

        public IEnumerable<Carta> ExecutarProjeto(Projeto projeto, Partida partida)
        {
            Trimestre trimestre = partida.ObterTrimestreAtual();
            Turno turno = trimestre.TurnoAtual;

            Mercado mercado = ObterMercadoDoProjeto(projeto, turno.Jogador, partida);
            bool podeAvancar = mercado.PodeAvancar(turno.Jogador, projeto);

            List<Carta> cartasDescartadas = [.. ResolverExecucaoDeProjeto(projeto, turno.Jogador, partida, podeAvancar)];

            turno.Encerrar();
            trimestre.AvancarTurno();

            return cartasDescartadas;
        }

        private static IEnumerable<Carta> ResolverExecucaoDeProjeto(Projeto projeto, Jogador jogador, Partida partida, bool podeAvancar)
        {
            jogador.ValidarCartas(projeto);
            jogador.AlocarProfissionais(projeto);

            Mercado mercado = partida.ObterMercado(projeto.SetorFinal);
            if (podeAvancar)
            {
                bool avancoTratadoPelaHabilidade = projeto.Gerente.Cargo
                    is Cargo.NegociadorInternacional
                    or Cargo.DiretorDeExpansao;

                if (!avancoTratadoPelaHabilidade)
                {
                    mercado.AvancarPresenca(jogador, partida);
                }

                projeto.Gerente.Cargo.ObterHabilidade().AposAvanco(jogador, partida, projeto);
            }

            TentarGerarToken(projeto, jogador);

            return jogador.FazerLayoff(projeto, partida);
        }

        private List<Carta> ComprarDoMonte(int quantidade, Jogador jogador, Partida partida)
        {
            Baralho baralho = partida.Baralho;
            int totalAComprar = CalcularTotalAComprar(quantidade, baralho);
            List<Carta> cartasCompradas = [];

            for (int i = 0; i < totalAComprar; i++)
            {
                Carta carta = baralho.ComprarCartaDoMonte();

                if (carta is not CartaCrise crise)
                {
                    cartasCompradas.Add(carta);
                    continue;
                }

                bool trimestreEncerrado = _criseService.Resolver(crise, jogador, partida);
                if (trimestreEncerrado)
                {
                    return cartasCompradas;
                }

                totalAComprar++;
            }

            return cartasCompradas;
        }

        private static Mercado ObterMercadoDoProjeto(Projeto projeto, Jogador jogador, Partida partida) =>
            projeto.Gerente.Cargo == Cargo.DiretorDeExpansao
                ? partida.Entrada.EscolherMercadoExpansao(jogador, partida)
                : partida.ObterMercado(projeto.SetorFinal);

        private static void TentarGerarToken(Projeto projeto, Jogador jogador)
        {
            if (CargosQueGeramToken.Contains(projeto.Gerente.Cargo))
            {
                jogador.Tokens.Add(Token.GerarToken(projeto.Gerente.Cargo));
            }
        }

        private static int CalcularTotalAComprar(int quantidade, Baralho baralho) =>
            quantidade + (baralho.MercadoDeTalentos.Count == 0 ? 1 : 0);
    }
}