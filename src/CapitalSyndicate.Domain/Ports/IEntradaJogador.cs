using CapitalSyndicate.Domain.Cartas;
using CapitalSyndicate.Domain.Enums;
using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Mercados;
using CapitalSyndicate.Domain.Partidas;
using CapitalSyndicate.Domain.Projetos;

namespace CapitalSyndicate.Domain.Ports
{
    public interface IEntradaJogador
    {
        bool ConfirmarCashOut(Jogador jogador, int pontosOferecidos);

        Presenca EscolherMarcadorLogistica(Jogador jogador, Presenca primeira, Presenca segunda);

        Projeto? EscolherSegundoProjeto(Jogador jogador, Partida partida);

        Projeto? EscolherProjetoAfterHours(Jogador jogador, Partida partida, Setor setorObrigatorio);

        Presenca EscolherPresencaParaMemorando(Jogador jogador, List<Presenca> presencasDisponiveis);

        Mercado EscolherMercadoExpansao(Jogador jogador, Partida partida);

        int EscolherNumManterCartas(Jogador jogador, Partida partida, int escalaProjeto);

        Carta EscolherCartaParaMercadoDeTalentos(Jogador jogador);
    }
}