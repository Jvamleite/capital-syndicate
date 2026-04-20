using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Partidas;

namespace CapitalSyndicate.Domain.Mercados
{
    internal abstract class Mercado
    {
        public string Nome { get; set; }
        public List<Patamar> Patamares { get; set; }

        public virtual void AvancarPresenca(Jogador jogador, int escalaProjeto, Partida partida)
        {
            Presenca presenca = jogador.PresencasDeMercado.First(p => p.Mercado == this);

            if (presenca.IndicePatamar >= Patamares.Count - 1)
            {
                throw new Exception("Você já conquistou o maior patamar deste mercado");
            }

            Patamar novoPatamar = Patamares[presenca.IndicePatamar + 1];
            if (novoPatamar.Requisito <= escalaProjeto)
            {
                presenca.Avancar();
                AoAvancar(jogador, partida);
            }
        }

        public virtual int ObterPontuacaoJogador(Jogador jogador, Partida partida)
        {
            Presenca presenca = jogador.PresencasDeMercado.First(p => p.Mercado == this);
            return Patamares[presenca.IndicePatamar].Pontos;
        }

        protected virtual void AoAvancar(Jogador jogador, Partida partida)
        { }

        protected virtual void AntesDaPontuacaoDoTrimestre(Jogador jogador, Partida partida)
        { }

        protected virtual void AposPontuacaoDoTrimestre(Jogador jogador, Partida partida)
        { }

        protected abstract List<Patamar> CriarPatamares(bool avancado);
    }
}