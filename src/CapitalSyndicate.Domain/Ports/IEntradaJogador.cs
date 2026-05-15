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
        Task<bool> ConfirmarCashOut(Jogador jogador, int pontosOferecidos);

        Task<Presenca> EscolherMarcadorLogistica(Jogador jogador, Presenca primeira, Presenca segunda);

        Task<Projeto?> EscolherSegundoProjeto(Jogador jogador, Partida partida);

        Task<Projeto?> EscolherProjetoAfterHours(Jogador jogador, Partida partida, Setor setorObrigatorio);

        Task<Presenca> EscolherPresencaParaMemorando(Jogador jogador, List<Presenca> presencasDisponiveis);

        Task<Mercado> EscolherMercadoExpansao(Jogador jogador, List<Mercado> mercadosValidos);

        Task<int> EscolherNumManterCartas(Jogador jogador, Partida partida, int escalaProjeto);

        Task<Carta> EscolherCartaParaMercadoDeTalentos(Jogador jogador);

        Task<TipoAtivo> EscolherTipoAtivoCorporativo(Jogador jogador);
    }
}