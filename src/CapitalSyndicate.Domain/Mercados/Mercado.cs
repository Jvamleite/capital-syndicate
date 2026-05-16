using CapitalSyndicate.Domain.Enums;
using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Partidas;
using CapitalSyndicate.Domain.Projetos;

namespace CapitalSyndicate.Domain.Mercados
{
    public abstract class Mercado
    {
        public string Nome { get; set; } = string.Empty;
        public List<Patamar> Patamares { get; set; } = [];

        public virtual async Task AvancarPresenca(Jogador jogador, Partida partida)
        {
            Presenca presenca = jogador.PresencasDeMercado.First(p => p.Mercado == this);
            presenca.Avancar();
            AoAvancar(jogador, partida);
        }

        public virtual bool PodeAvancar(Jogador jogador, Projeto projeto)
        {
            Presenca presenca = jogador.PresencasDeMercado.First(p => p.Mercado == this);

            if (presenca.IndicePatamar >= Patamares.Count - 1)
            {
                return false;
            }

            int escalaEfetiva = projeto.Gerente.Cargo
                .ObterHabilidade()
                .ModificarEscalaParaAvanco(projeto.Escala);

            Patamar novoPatamar = Patamares[presenca.IndicePatamar + 1];
            return novoPatamar.Requisito <= escalaEfetiva;
        }

        public virtual int ObterPontuacaoJogador(Jogador jogador, Partida partida)
        {
            Presenca presenca = jogador.PresencasDeMercado.First(p => p.Mercado == this);
            return Patamares[presenca.IndicePatamar].Pontos;
        }

        public virtual Task AoAvancar(Jogador jogador, Partida partida)
        { return Task.CompletedTask; }

        public virtual Task AntesDaPontuacaoDoTrimestre(Jogador jogador, Partida partida)
        { return Task.CompletedTask; }

        public virtual void AposPontuacaoDoTrimestre(Jogador jogador, Partida partida)
        { }

        protected abstract List<Patamar> CriarPatamares(bool avancado);
    }
}