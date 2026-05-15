using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Partidas;

namespace CapitalSyndicate.Domain.Mercados
{
    public class Tecnologia : Mercado
    {
        public bool ApenasLiderPontua { get; }

        public Tecnologia(bool avancado)
        {
            Nome = avancado ? "Plataforma Dominante" : "Corrida Algorítmica";
            Patamares = CriarPatamares(avancado);
            ApenasLiderPontua = avancado;
        }

        protected override List<Patamar> CriarPatamares(bool avancado)
        {
            List<Patamar> patamares = [];
            int[] pontos, requisitos;

            if (!avancado)
            {
                pontos = [2, 3, 5, 8, 12, 17];
                requisitos = [2, 2, 4, 4, 6, 6];
            }
            else
            {
                pontos = [1, 2, 3, 4];
                requisitos = [1, 2, 4, 6];
            }

            for (int i = 0; i < pontos.Length; i++)
            {
                patamares.Add(new Patamar(requisitos[i], pontos[i]));
            }

            return patamares;
        }

        public override int ObterPontuacaoJogador(Jogador jogador, Partida partida)
        {
            Presenca presenca = jogador.PresencasDeMercado.First(p => p.Mercado == this);

            if (!ApenasLiderPontua)
            {
                return Patamares[presenca.IndicePatamar].Pontos;
            }

            List<Presenca> presencasNesseMercado = ObterPresencasNesseMercado(partida);
            int maiorIndice = presencasNesseMercado.Max(p => p.IndicePatamar);

            if (presenca.IndicePatamar < maiorIndice)
            {
                return 0;
            }

            return Patamares[maiorIndice].Pontos;
        }

        public override void AposPontuacaoDoTrimestre(Jogador jogador, Partida partida)
        {
            if (!ApenasLiderPontua)
            {
                return;
            }

            Presenca presenca = jogador.PresencasDeMercado.First(p => p.Mercado == this);
            int maiorIndice = ObterPresencasNesseMercado(partida).Max(p => p.IndicePatamar);

            if (presenca.IndicePatamar == maiorIndice)
            {
                presenca.Resetar();
            }
        }

        private List<Presenca> ObterPresencasNesseMercado(Partida partida) =>
            [.. partida.Jogadores
                .Select(j => j.PresencasDeMercado.FirstOrDefault(p => p.Mercado == this))
                .Where(p => p != null)
                .Cast<Presenca>()];
    }
}