using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Mercados;

namespace CapitalSyndicate.Domain.Ports
{
    internal interface IEntradaJogador
    {
        bool ConfirmarCashOut(Jogador jogador, int pontosOferecidos);

        Presenca EscolherMarcadorLogistica(Jogador jogador, Presenca primeira, Presenca segunda);

        void ExecutarProjetoAdicional(Jogador jogador, Mercado mercado);
    }
}