using CapitalSyndicate.Domain.Baralhos;
using CapitalSyndicate.Domain.Enums;
using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Mercados;
using CapitalSyndicate.Domain.Ports;

namespace CapitalSyndicate.Domain.Partidas
{
    public class Partida
    {
        public List<Jogador> Jogadores { get; }
        public Baralho Baralho { get; }
        public IEntradaJogador Entrada { get; }
        public TrilhaGlobal? TrilhaGlobal { get; }
        public List<Mercado> Mercados { get; }
        public List<Trimestre> Trimestres { get; } = [];

        public Mercado ObterMercado(Setor setor) => Mercados.First(m => nameof(m) == setor.ToString());

        public Mercado ObterMercado(Mercado mercado) => Mercados.First(m => m == mercado);

        public Trimestre ObterTrimestreAtual() => Trimestres[^1];
    }
}