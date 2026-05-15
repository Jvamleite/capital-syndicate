using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Partidas;

namespace CapitalSyndicate.Domain.Mercados
{
    public class MercadoFinanceiro : Mercado
    {
        public bool CashOutDisponivel { get; }

        public MercadoFinanceiro(bool avancado)
        {
            Nome = avancado ? "Bolsa de Valores" : "Especulação/Cash Out";
            Patamares = CriarPatamares(avancado);
            CashOutDisponivel = avancado;
        }

        protected override List<Patamar> CriarPatamares(bool avancado)
        {
            List<Patamar> patamares = [];
            if (!avancado)
            {
                int[] pontos = [-2, 2, 0, 6, 3, 10, 5, 15];
                int[] requisitos = [1, 2, 1, 3, 1, 4, 1, 4];
                for (int i = 0; i < pontos.Length; i++)
                {
                    patamares.Add(new Patamar(requisitos[i], pontos[i]));
                }
            }
            else
            {
                int[] pontos = [1, 3, 6, 10, 15, 20];
                for (int i = 1; i <= 6; i++)
                {
                    patamares.Add(new Patamar(i, pontos[i - 1]));
                }
            }

            return patamares;
        }

        public override async void AoAvancar(Jogador jogador, Partida partida)
        {
            if (!CashOutDisponivel)
            {
                return;
            }

            Presenca presenca = jogador.PresencasDeMercado.First(p => p.Mercado == this);
            int pontosOferecidos = Patamares[presenca.IndicePatamar].Pontos;

            if (await partida.Entrada.ConfirmarCashOut(jogador, pontosOferecidos))
            {
                jogador.PontosVitoria += pontosOferecidos;
                presenca.Resetar();
            }
        }
    }
}