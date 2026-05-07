using CapitalSyndicate.Domain.Enums;
using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Partidas;
using CapitalSyndicate.Domain.Projetos;

namespace CapitalSyndicate.Domain.Mercados
{
    internal class EstrategiasCorporativas : Mercado
    {
        public bool OperacoesAfterHours { get; }

        public EstrategiasCorporativas(bool avancado)
        {
            Nome = avancado ? "Operações After-Hours" : "Fusões e Aquisições";
            Patamares = CriarPatamares(avancado);
            OperacoesAfterHours = avancado;
        }

        protected override List<Patamar> CriarPatamares(bool avancado)
        {
            List<Patamar> patamares = [];
            int[] pontos, requisitos;

            if (!avancado)
            {
                pontos = [2, 4, 6, 8, 11, 14];
                requisitos = [3, 3, 4, 4, 5, 5];
            }
            else
            {
                pontos = [0, 1, 2, 2, 4, 6];
                requisitos = [1, 2, 3, 4, 5, 6];
            }

            for (int i = 0; i < pontos.Length; i++)
            {
                patamares.Add(new Patamar(requisitos[i], pontos[i]));
            }

            return patamares;
        }

        public override void AntesDaPontuacaoDoTrimestre(Jogador jogador, Partida partida)
        {
            if (!OperacoesAfterHours)
            {
                return;
            }

            Presenca presenca = jogador.PresencasDeMercado.First(p => p.Mercado == this);

            if (presenca.IndicePatamar < 0)
            {
                return;
            }

            Projeto? projeto = partida.Entrada.EscolherProjetoAfterHours(jogador, partida, Setor.EstrategiasCorporativa);

            if (projeto is null)
            {
                return;
            }

            if (projeto.Gerente.Setor != Setor.EstrategiasCorporativa)
            {
                throw new InvalidOperationException(
                    $"O gerente do projeto after-hours deve ser do setor {Setor.EstrategiasCorporativa}.");
            }

            jogador.ValidarCartas(projeto);
            jogador.AlocarProfissionais(projeto);

            bool podeAvancar = PodeAvancar(jogador, projeto);
            if (podeAvancar)
            {
                AvancarPresenca(jogador, partida);
                projeto.Gerente.Cargo.ObterHabilidade().AposAvanco(jogador, partida, projeto);
            }
        }
    }
}