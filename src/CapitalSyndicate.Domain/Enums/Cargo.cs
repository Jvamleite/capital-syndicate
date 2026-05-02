using CapitalSyndicate.Domain.Cartas.Habilidades;

namespace CapitalSyndicate.Domain.Enums
{
    internal enum Cargo
    {
        Facilitador,
        DiretorDeComunicacao,
        Investidor,
        GestorDeRh,
        DiretorDeExpansao,
        DiretorDeOperacoes,
        EspecialistaEsg,
        GestorDePortifolio,
        Auditor,
        NegociadorInternacional,
        Trainee,
        ConsultorExterno
    }

    internal static class CargoExtensions
    {
        private static readonly Dictionary<Cargo, IHabilidadeLider> Habilidades = new()
        {
                { Cargo.ConsultorExterno, new HabilidadeConsultorExterno() },
                { Cargo.Auditor, new HabilidadeAuditor() },
                { Cargo.EspecialistaEsg, new HabilidadeEspecialistaEsg() },
                { Cargo.GestorDePortifolio, new HabilidadeGestorPortifolio() },
                { Cargo.Investidor, new HabilidadeInvestidor() },
                { Cargo.DiretorDeComunicacao, new HabilidadeDiretorComunicacao() },
        };

        public static IHabilidadeLider ObterHabilidade(this Cargo cargo)
            => Habilidades[cargo];
    }
}