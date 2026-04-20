using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Partidas;

namespace CapitalSyndicate.Domain.Mercados
{
    internal class Logistica : Mercado
    {
        public bool ExpansaoParalela { get; }
        private readonly Dictionary<Jogador, Presenca> SegundaPresenca = [];

        public Logistica(bool avancado)
        {
            Nome = avancado ? "Operação Multihub" : "Malha de Distribuição";
            Patamares = CriarPatamares(avancado);
            ExpansaoParalela = avancado;
        }

        protected override List<Patamar> CriarPatamares(bool avancado)
        {
            List<Patamar> patamares = [];
            if (!avancado)
            {
                int[] pontos = [1, 2, 3, 5, 7, 9];
                for (int i = 1; i <= 6; i++)
                {
                    patamares.Add(new Patamar(i, pontos[i - 1]));
                }
            }
            else
            {
                int[] pontos = [2, 5, 9, 14, 20];
                int[] requisitos = [1, 2, 3, 4, 5];
                for (int i = 0; i < pontos.Length; i++)
                {
                    patamares.Add(new Patamar(requisitos[i], pontos[i]));
                }
            }

            return patamares;
        }

        private Presenca ObterSegundoMarcador(Jogador jogador)
        {
            if (!SegundaPresenca.TryGetValue(jogador, out Presenca? p))
            {
                p = new Presenca(this, Guid.NewGuid());
                SegundaPresenca[jogador] = p;
            }

            return p;
        }

        public override void AvancarPresenca(Jogador jogador, int escalaProjeto, Partida partida)
        {
            if (!ExpansaoParalela)
            {
                base.AvancarPresenca(jogador, escalaProjeto, partida);
                return;
            }

            Presenca primeiro = jogador.PresencasDeMercado.First(p => p.Mercado == this);
            Presenca segundo = ObterSegundoMarcador(jogador);

            Presenca escolhido = partida.Entrada.EscolherMarcadorLogistica(jogador, primeiro, segundo);

            if (escolhido.IndicePatamar >= Patamares.Count - 1)
            {
                throw new Exception("Esse marcador já está no maior patamar.");
            }

            Patamar novoPatamar = Patamares[escolhido.IndicePatamar + 1];
            if (novoPatamar.Requisito <= escalaProjeto)
            {
                escolhido.Avancar();
                AoAvancar(jogador, partida);
            }
        }

        public override int ObterPontuacaoJogador(Jogador jogador, Partida partida)
        {
            if (!ExpansaoParalela)
            {
                return base.ObterPontuacaoJogador(jogador, partida);
            }

            Presenca primeiro = jogador.PresencasDeMercado.First(p => p.Mercado == this);
            Presenca segundo = ObterSegundoMarcador(jogador);

            int indiceMenor = Math.Min(primeiro.IndicePatamar, segundo.IndicePatamar);
            return Patamares[indiceMenor].Pontos;
        }
    }
}