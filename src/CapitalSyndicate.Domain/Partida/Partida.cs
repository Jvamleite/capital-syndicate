using CapitalSyndicate.Domain.Baralhos;
using CapitalSyndicate.Domain.Cartas;
using CapitalSyndicate.Domain.Enums;
using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Mercados;
using CapitalSyndicate.Domain.Ports;

namespace CapitalSyndicate.Domain.Partidas
{
    public class Partida
    {
        private static readonly Random Random = new();
        private readonly List<CartaCrise> _cartasCrise;

        public List<Jogador> Jogadores { get; } = [];
        public Baralho Baralho { get; }
        public IEntradaJogador Entrada { get; }
        public TrilhaGlobal? TrilhaGlobal { get; }
        public List<Mercado> Mercados { get; }
        public List<Trimestre> Trimestres { get; } = [];
        public int TotalTrimestres { get; }
        public bool Encerrada { get; private set; }
        public Jogador? JogadorQueRevelouUltimaCrise { get; set; }

        public Mercado ObterMercado(Setor setor) => Mercados.First(m => m.GetType().Name == setor.ToString());

        public Mercado ObterMercado(Mercado mercado) => Mercados.First(m => m == mercado);

        public Trimestre ObterTrimestreAtual() => Trimestres[^1];

        public Partida(
            List<Jogador> jogadores,
            List<Mercado> mercados,
            List<Carta> cartasNormais,
            List<CartaCrise> cartasCrise,
            IEntradaJogador entrada,
            bool comTrilhaGlobal = false)
        {
            _cartasCrise = cartasCrise;
            Jogadores = jogadores;
            Mercados = mercados;
            Entrada = entrada;
            TotalTrimestres = jogadores.Count <= 3 ? 2 : 3;
            Baralho = new Baralho(cartasNormais, cartasCrise, jogadores.Count);

            if (comTrilhaGlobal)
            {
                TrilhaGlobal = new TrilhaGlobal(jogadores);
            }
        }

        public bool TodosOsTrimestresEncerrados() =>
            Trimestres.Count == TotalTrimestres
            && Trimestres[^1].Encerrado;

        public void Iniciar()
        {
            InicializarPresencasDeMercado();
            IniciarNovoTrimestre(primeiroTrimestre: true);
        }



        private void InicializarPresencasDeMercado()
        {
            foreach (Jogador jogador in Jogadores)
            {
                foreach (Mercado mercado in Mercados)
                {
                    jogador.PresencasDeMercado.Add(new Presenca(mercado, Guid.NewGuid()));
                }
            }
        }

        public void IniciarNovoTrimestre(bool primeiroTrimestre = false)
        {
            if (!primeiroTrimestre)
            {
                ResetarParaNovoTrimestre();
            }


            List<Jogador> ordemDeTurno = DefinirOrdemDeTurno(primeiroTrimestre);
            int numero = Trimestres.Count + 1;

            Trimestres.Add(new Trimestre(numero, ordemDeTurno));

            foreach (Jogador jogador in ordemDeTurno)
            {
                Carta carta = Baralho.ComprarCartaDoMonte();
                jogador.ComprarCartas([carta]);
            }
        }

        private void ResetarParaNovoTrimestre()
        {
            foreach (Jogador jogador in Jogadores)
            {
                jogador.ProjetosNaMesa.Clear();

                foreach (Presenca presenca in jogador.PresencasDeMercado)
                {
                    presenca.Resetar();
                }
            }

            Baralho.Reiniciar(_cartasCrise, Jogadores.Count);
        }

        private List<Jogador> DefinirOrdemDeTurno(bool primeiroTrimestre)
        {
            if (primeiroTrimestre || JogadorQueRevelouUltimaCrise is null)
            {
                return OrdenarAPartirDe(Jogadores[Random.Next(Jogadores.Count)]);
            }

            return OrdenarAPartirDe(JogadorQueRevelouUltimaCrise);
        }

        private List<Jogador> OrdenarAPartirDe(Jogador primeiro)
        {
            int indice = Jogadores.IndexOf(primeiro);
            return [.. Jogadores.Skip(indice), .. Jogadores.Take(indice)];
        }

        public void Encerrar()
        {
            if (Encerrada)
            {
                return;
            }

            Encerrada = true;
            ResolverHabilidadesDeFimDeJogo();
        }

        private void ResolverHabilidadesDeFimDeJogo()
        {
            foreach (Jogador jogador in Jogadores)
            {
                foreach (Projetos.Projeto projeto in jogador.ProjetosNaMesa)
                {
                    projeto.Gerente.Cargo.ObterHabilidade().AoFimDoJogo(jogador, this);
                }
            }
        }

        public List<Jogador> DefinirVencedor()
        {
            int maiorPontuacao = Jogadores.Max(j => j.PontosVitoria);
            List<Jogador> lideres = [.. Jogadores.Where(j => j.PontosVitoria == maiorPontuacao)];

            if (lideres.Count == 1)
            {
                return lideres;
            }

            return ResolverDesempate(lideres);
        }

        private static List<Jogador> ResolverDesempate(List<Jogador> empatados)
        {
            Dictionary<Jogador, List<int>> escalasPorJogador = empatados.ToDictionary(
                j => j,
                j => ObterEscalasDoProjetos(j).OrderByDescending(e => e).ToList()
            );

            int maxRodadas = escalasPorJogador.Values.Max(e => e.Count);
            for (int i = 0; i < maxRodadas; i++)
            {
                int maiorEscala = empatados.Max(j =>
                    i < escalasPorJogador[j].Count ? escalasPorJogador[j][i] : 0);

                empatados = [.. empatados.Where(j =>
                    i < escalasPorJogador[j].Count && escalasPorJogador[j][i] == maiorEscala)];

                if (empatados.Count == 1)
                {
                    return empatados;
                }
            }

            return empatados;
        }

        private static List<int> ObterEscalasDoProjetos(Jogador jogador)
        {
            return [.. jogador.ProjetosNaMesa.Select(p => p.Escala)];
        }
    }
}