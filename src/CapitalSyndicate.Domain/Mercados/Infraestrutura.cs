using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Partidas;

namespace CapitalSyndicate.Domain.Mercados
{
    public class Infraestrutura : Mercado
    {
        public bool Monopolio { get; }

        public Infraestrutura(bool avancado)
        {
            Nome = avancado ? "Telecomunicações" : "Concessões Públicas";
            Patamares = CriarPatamares(avancado);
            Monopolio = avancado;
        }

        protected override List<Patamar> CriarPatamares(bool avancado)
        {
            int[] pontos = avancado
                ? [2, 4, 6, 8, 10, 12]
                : [0, 0, 4, 7, 9, 12];

            List<Patamar> patamares = [];
            for (int i = 0; i < pontos.Length; i++)
            {
                patamares.Add(new Patamar(i + 1, pontos[i]));
            }

            return patamares;
        }

        public override int ObterPontuacaoJogador(Jogador jogador, Partida partida)
        {
            Presenca presenca = jogador.PresencasDeMercado.First(p => p.Mercado == this);

            if (!Monopolio)
            {
                return Patamares[presenca.IndicePatamar].Pontos;
            }

            List<Presenca> presencasNesseMercado = [.. partida.Jogadores
                .Select(j => j.PresencasDeMercado.FirstOrDefault(p => p.Mercado == this))
                .Where(p => p != null)
                .Cast<Presenca>()];

            int maiorIndice = presencasNesseMercado.Max(p => p.IndicePatamar);
            if (presenca.IndicePatamar < maiorIndice)
            {
                return 0;
            }

            int quantidadeLideres = presencasNesseMercado.Count(p => p.IndicePatamar == maiorIndice);
            int pontosDoTopo = Patamares[maiorIndice].Pontos;

            return quantidadeLideres > 1 ? pontosDoTopo / 2 : pontosDoTopo;
        }
    }
}