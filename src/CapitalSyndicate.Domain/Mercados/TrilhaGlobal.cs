using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Partidas;

namespace CapitalSyndicate.Domain.Mercados
{
    internal class TrilhaGlobal
    {
        public readonly List<bool> Patamares;
        public readonly Dictionary<Jogador, int> Posicoes = [];

        public TrilhaGlobal(IEnumerable<Jogador> jogadores)
        {
            Patamares = CriarPatamares();
            foreach (Jogador jogador in jogadores)
            {
                Posicoes.Add(jogador, 0);
            }
        }

        private static List<bool> CriarPatamares()
        {
            List<bool> patamares = [];
            int[] memorandosDeEntendimento = [3, 7, 12, 18];
            for (int i = 0; i < 18; i++)
            {
                if (memorandosDeEntendimento.Contains(i))
                {
                    patamares.Add(true);
                }
                else
                {
                    patamares.Add(false);
                }
            }

            return patamares;
        }

        public void AvancarPresenca(Jogador jogador, Partida partida, int escalaProjeto)
        {
            int posicaoAtual = Posicoes[jogador];
            int novaPosicao = Math.Min(posicaoAtual + escalaProjeto, Patamares.Count - 1);

            for (int i = posicaoAtual + 1; i <= novaPosicao; i++)
            {
                if (Patamares[i])
                {
                    List<Presenca> presencasDisponiveis = [.. jogador.PresencasDeMercado];
                    Presenca presenca = partida.Entrada.EscolherPresencaParaMemorado(jogador, presencasDisponiveis);
                    presenca.Avancar();
                }
            }

            Posicoes[jogador] = novaPosicao;
        }

        public int PontuarFimDeTrimestre(Jogador jogador)
        {
            if (Posicoes.Count == 0)
            {
                return 0;
            }

            int maiorPosicao = Posicoes.Values.Max();
            if (Posicoes.TryGetValue(jogador, out int posicao) && posicao == maiorPosicao)
            {
                return 2;
            }

            return 0;
        }
    }
}