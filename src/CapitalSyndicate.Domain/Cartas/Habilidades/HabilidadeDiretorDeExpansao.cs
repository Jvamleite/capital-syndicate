using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Mercados;
using CapitalSyndicate.Domain.Partidas;
using CapitalSyndicate.Domain.Projetos;

namespace CapitalSyndicate.Domain.Cartas.Habilidades
{
    internal class HabilidadeDiretorDeExpansao : IHabilidadeLider
    {
        void IHabilidadeLider.AposAvanco(Jogador ativo, Partida partida, Projeto projeto)
        {
            Mercado mercadoEscolhido = partida.Entrada.EscolherMercadoExpansao(ativo, partida);

            if (!mercadoEscolhido.PodeAvancar(ativo, projeto))
            {
                throw new InvalidOperationException(
                    "A escala do projeto não atinge o requisito do mercado escolhido.");
            }

            mercadoEscolhido.AvancarPresenca(ativo, partida);
        }
    }
}