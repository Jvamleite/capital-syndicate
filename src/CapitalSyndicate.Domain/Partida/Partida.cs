using CapitalSyndicate.Domain.Baralhos;
using CapitalSyndicate.Domain.Enums;
using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Mercados;
using CapitalSyndicate.Domain.Ports;

namespace CapitalSyndicate.Domain.Partidas
{
    internal class Partida
    {
        public List<Jogador> Jogadores { get; }
        public Baralho Baralho { get; }
        public IEntradaJogador Entrada { get; }
        public TrilhaGlobal? TrilhaGlobal { get; }
        public List<Mercado> Mercados { get; }

        public Mercado ObterMercado(Setor setor) => Mercados.First(m => nameof(m) == setor.ToString());
    }
}