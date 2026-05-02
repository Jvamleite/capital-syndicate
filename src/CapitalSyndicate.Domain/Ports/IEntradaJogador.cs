using CapitalSyndicate.Domain.Cartas;
using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Mercados;
using CapitalSyndicate.Domain.Partidas;

namespace CapitalSyndicate.Domain.Ports
{
    public interface IEntradaJogador
    {
        bool ConfirmarCashOut(Jogador jogador, int pontosOferecidos);

        Presenca EscolherMarcadorLogistica(Jogador jogador, Presenca primeira, Presenca segunda);

        bool ExecutarProjetoAdicional(Jogador jogador, Mercado mercado);

        Presenca EscolherPresencaParaMemorando(Jogador jogador, List<Presenca> presencasDisponiveis);

        Mercado EscolherMercadoExpansao(Jogador jogador, Partida partida);

        int EscolherNumManterCartas(Jogador jogador, Partida partida, int escalaProjeto);

        Carta EscolherCartaParaMercadoDeTalentos(Jogador jogador);
    }
}