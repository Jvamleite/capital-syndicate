using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Mercados;
using CapitalSyndicate.Domain.Partidas;
using CapitalSyndicate.Domain.Projetos;

namespace CapitalSyndicate.Domain.Cartas.Habilidades
{
    internal class HabilidadeDiretorDeExpansao : IHabilidadeLider
    {
        async Task IHabilidadeLider.AposAvanco(Jogador ativo, Partida partida, Projeto projeto)
        {
            List<Mercado> mercadosValidos =
                [.. partida.Mercados.Where(m => m.PodeAvancar(ativo, projeto))];

            if (mercadosValidos.Count == 0)
            {
                return;
            }

            Mercado mercadoEscolhido =
                await partida.Entrada.EscolherMercadoExpansao(
                    ativo,
                    mercadosValidos);

            mercadoEscolhido.AvancarPresenca(
                ativo,
                partida);
        }
    }
}