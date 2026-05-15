using CapitalSyndicate.Domain.Enums;
using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Mercados;
using CapitalSyndicate.Domain.Partidas;
using CapitalSyndicate.Domain.Projetos;

namespace CapitalSyndicate.Domain.Cartas.Habilidades
{
    internal class HabilidadeDiretorDeOperacoes : IHabilidadeLider
    {
        async Task IHabilidadeLider.AposAvanco(Jogador ativo, Partida partida, Projeto projeto)
        {
            Projeto? segundoProjeto = await partida.Entrada.EscolherSegundoProjeto(ativo, partida);

            if (segundoProjeto is null)
            {
                return;
            }

            ativo.ValidarCartas(segundoProjeto);
            ativo.AlocarProfissionais(segundoProjeto);

            Mercado mercado = partida.ObterMercado(segundoProjeto.SetorFinal);
            if (mercado.PodeAvancar(ativo, segundoProjeto))
            {
                await mercado.AvancarPresenca(ativo, partida);

                await segundoProjeto.Gerente.Cargo.ObterHabilidade().AposAvanco(ativo, partida, segundoProjeto);
            }
        }
    }
}