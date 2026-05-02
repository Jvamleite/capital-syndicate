using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Partidas;

namespace CapitalSyndicate.Domain.Mercados
{
    internal class EstrategiasCorporativas : Mercado
    {
        public bool OperacoesAfterHours { get; }

        public EstrategiasCorporativas(bool avancado)
        {
            Nome = avancado ? "Operações After-Hours" : "Fusões e Aquisições";
            Patamares = CriarPatamares(avancado);
            OperacoesAfterHours = avancado;
        }

        protected override List<Patamar> CriarPatamares(bool avancado)
        {
            List<Patamar> patamares = [];
            int[] pontos, requisitos;

            if (!avancado)
            {
                pontos = [2, 4, 6, 8, 11, 14];
                requisitos = [3, 3, 4, 4, 5, 5];
            }
            else
            {
                pontos = [0, 1, 2, 2, 4, 6];
                requisitos = [1, 2, 3, 4, 5, 6];
            }

            for (int i = 0; i < pontos.Length; i++)
            {
                patamares.Add(new Patamar(requisitos[i], pontos[i]));
            }

            return patamares;
        }

        protected override void AntesDaPontuacaoDoTrimestre(Jogador jogador, Partida partida)
        {
            if (!OperacoesAfterHours)
            {
                return;
            }

            Presenca presenca = jogador.PresencasDeMercado.First(p => p.Mercado == this);

            if (presenca.IndicePatamar < 0)
            {
                return;
            }

            partida.Entrada.ExecutarProjetoAdicional(jogador, this);
        }
    }
}