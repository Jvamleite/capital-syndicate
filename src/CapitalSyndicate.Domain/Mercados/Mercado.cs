using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Partidas;
using CapitalSyndicate.Domain.Projetos;

namespace CapitalSyndicate.Domain.Mercados
{
    internal abstract class Mercado
    {
        public string Nome { get; set; } = string.Empty;
        public List<Patamar> Patamares { get; set; } = [];

        public virtual void AvancarPresenca(Jogador jogador, Partida partida)
        {
            Presenca presenca = jogador.PresencasDeMercado.First(p => p.Mercado == this);
            presenca.Avancar();
            AoAvancar(jogador, partida);
        }

        public bool PodeAvancar(Jogador jogador, Projeto projeto)
        {
            Presenca presenca = jogador.PresencasDeMercado.First(p => p.Mercado == this);

            if (presenca.IndicePatamar >= Patamares.Count - 1)
            {
                return false;
            }

            int escalaProjeto = projeto.Gerente.Cargo == Enums.Cargo.Facilitador ? projeto.Escala + 1 : projeto.Escala;

            Patamar novoPatamar = Patamares[presenca.IndicePatamar + 1];
            return novoPatamar.Requisito <= escalaProjeto;
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