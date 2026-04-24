using CapitalSyndicate.Domain.Enums;
using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Projetos;

namespace CapitalSyndicate.Domain.Cartas.Habilidades
{
    internal class HabilidadeConsultorExterno : IHabilidadeLider
    {
        void IHabilidadeLider.AoFimDoTrimestre(Jogador ativo, Projeto projeto)
        {
            projeto.Profissionais.RemoveAll(x => x.Cargo == Cargo.ConsultorExterno);
        }
    }
}