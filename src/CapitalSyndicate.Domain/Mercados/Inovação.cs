using CapitalSyndicate.Domain.Cartas;
using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Partidas;

namespace CapitalSyndicate.Domain.Mercados
{
    public class Inovacao : Mercado
    {
        private const int LimiteMaoMaximo = 10;

        public bool InjecaoDeRecursos { get; }

        public Inovacao(bool avancado)
        {
            Nome = avancado ? "Pipeline de P&D" : "Laboratório de Inovação";
            Patamares = CriarPatamares(avancado);
            InjecaoDeRecursos = avancado;
        }

        protected override List<Patamar> CriarPatamares(bool avancado)
        {
            List<Patamar> patamares = [];

            if (!avancado)
            {
                int[] pontos = [1, 2, 3, 4, 5, 6, 7, 8, 10];
                int[] requisitos = [1, 1, 1, 2, 2, 2, 3, 3, 3];
                for (int i = 0; i < pontos.Length; i++)
                {
                    patamares.Add(new Patamar(requisitos[i], pontos[i]));
                }
            }
            else
            {
                int[] pontos = [0, 1, 2, 3, 5, 7];
                for (int i = 1; i <= 6; i++)
                {
                    patamares.Add(new Patamar(i, pontos[i - 1]));
                }
            }

            return patamares;
        }

        public override void AoAvancar(Jogador jogador, Partida partida)
        {
            if (!InjecaoDeRecursos)
            {
                return;
            }

            Presenca presenca = jogador.PresencasDeMercado.First(p => p.Mercado == this);
            int limitePatamar = Patamares[presenca.IndicePatamar].Requisito;
            int limiteEfetivo = Math.Min(limitePatamar, LimiteMaoMaximo);

            while (jogador.CartasNaMao.Count < limiteEfetivo
                && jogador.CartasNaMao.Count < LimiteMaoMaximo)
            {
                Carta cartas = partida.Baralho.ComprarCartaDoMonte();
                jogador.CartasNaMao.Add(cartas);
            }
        }
    }
}