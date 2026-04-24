using CapitalSyndicate.Application.Crises.Interfaces;
using CapitalSyndicate.Domain.Cartas;
using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Partidas;

namespace CapitalSyndicate.Application.Crises
{
    public class CriseService : ICriseService
    {
        public bool Resolver(CartaCrise crise, Jogador jogador, Partida partida)
        {
            Trimestre trimestre = partida.ObterTrimestreAtual();

            trimestre.CrisesReveladas++;
            crise.Resolver(jogador, partida);

            return trimestre.Encerrado;
        }
    }
}