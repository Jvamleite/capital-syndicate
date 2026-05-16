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

        public async Task<IEnumerable<Carta>> ComprarCartas(int quantidade, Partida partida)
        {
            Trimestre trimestre = partida.ObterTrimestreAtual();
            Turno turno = trimestre.TurnoAtual;

            List<Carta> cartasCompradas = await ComprarDoMonte(quantidade, turno.Jogador, partida);

            turno.Jogador.ComprarCartas(cartasCompradas);
            turno.Encerrar();
            trimestre.AvancarTurno();

            return cartasCompradas;
        }

        public async Task<IEnumerable<Carta>> ExecutarProjeto(Projeto projeto, Partida partida)
        {
            Trimestre trimestre = partida.ObterTrimestreAtual();
            Turno turno = trimestre.TurnoAtual;

            Mercado mercado = ObterMercadoDoProjeto(projeto, partida);
            bool podeAvancar = mercado.PodeAvancar(turno.Jogador, projeto);

            List<Carta> cartasDescartadas = [.. await ResolverExecucaoDeProjeto(projeto, turno.Jogador, partida, podeAvancar)];

            turno.Encerrar();
            trimestre.AvancarTurno();

            return cartasDescartadas;
        }

        private static async Task<IEnumerable<Carta>> ResolverExecucaoDeProjeto(Projeto projeto, Jogador jogador, Partida partida, bool podeAvancar)
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
                    await mercado.AvancarPresenca(jogador, partida);
                }

                await projeto.Gerente.Cargo.ObterHabilidade().AposAvanco(jogador, partida, projeto);
            }

            await TentarGerarToken(projeto, jogador, partida);

            IReadOnlyList<Carta> cartasDescartadas = await jogador.FazerLayoff(projeto, partida);

            projeto.Gerente.Cargo.ObterHabilidade().AposLayoff(jogador, partida, projeto);

            return cartasDescartadas;
        }

        private async Task<List<Carta>> ComprarDoMonte(int quantidade, Jogador jogador, Partida partida)
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

                bool trimestreEncerrado = await _criseService.Resolver(crise, jogador, partida);

                await partida.Entrada.NotificarCrise(crise);

                if (trimestreEncerrado)
                {
                    return cartasCompradas;
                }

                totalAComprar++;
            }

            return cartasCompradas;
        }

        private static Mercado ObterMercadoDoProjeto(Projeto projeto, Partida partida) => partida.ObterMercado(projeto.SetorFinal);

        private static async Task TentarGerarToken(
            Projeto projeto,
            Jogador jogador,
            Partida partida)
        {
            Cargo cargo = projeto.Gerente.Cargo;

            if (!CargosQueGeramToken.Contains(cargo))
            {
                return;
            }

            Token token;

            if (cargo == Cargo.GestorDePortifolio)
            {
                TipoAtivo tipo =
                    await partida.Entrada
                        .EscolherTipoAtivoCorporativo(
                            jogador);

                token = new TokenAtivo
                {
                    Tipo = tipo
                };
            }
            else
            {
                token = Token.GerarToken(cargo);
            }

            jogador.Tokens.Add(token);
        }

        private static int CalcularTotalAComprar(int quantidade, Baralho baralho) =>
            quantidade + (baralho.MercadoDeTalentos.Count == 0 ? 1 : 0);
    }
}