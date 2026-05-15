using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Partidas;
using CapitalSyndicate.Domain.Projetos;

namespace CapitalSyndicate.Domain.Mercados
{
    public class Logistica : Mercado
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

        public override bool PodeAvancar(Jogador jogador, Projeto projeto)
        {
            if (!ExpansaoParalela)
            {
                return base.PodeAvancar(jogador, projeto);
            }

            Presenca primeiro = jogador.PresencasDeMercado.First(p => p.Mercado == this);
            Presenca segundo = ObterSegundoMarcador(jogador);

            return PodeAvancarPresenca(primeiro, projeto.Escala)
                || PodeAvancarPresenca(segundo, projeto.Escala);
        }

        public override async Task AvancarPresenca(Jogador jogador, Partida partida)
        {
            if (!ExpansaoParalela)
            {
                await base.AvancarPresenca(jogador, partida);
                return;
            }

            Presenca primeiro = jogador.PresencasDeMercado.First(p => p.Mercado == this);
            Presenca segundo = ObterSegundoMarcador(jogador);
            Presenca escolhido = await partida.Entrada.EscolherMarcadorLogistica(jogador, primeiro, segundo);

            if (escolhido.IndicePatamar >= Patamares.Count - 1)
            {
                throw new Exception("Esse marcador já está no maior patamar.");
            }

            escolhido.Avancar();
            AoAvancar(jogador, partida);
        }

        private bool PodeAvancarPresenca(Presenca presenca, int escalaProjeto)
        {
            if (presenca.IndicePatamar >= Patamares.Count - 1)
            {
                return false;
            }

            return Patamares[presenca.IndicePatamar + 1].Requisito <= escalaProjeto;
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