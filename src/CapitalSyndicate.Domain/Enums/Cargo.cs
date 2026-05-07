using CapitalSyndicate.Domain.Cartas.Habilidades;

namespace CapitalSyndicate.Domain.Enums
{
    public enum Cargo
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

    public static class CargoExtensions
    {
        private static readonly Dictionary<Cargo, IHabilidadeLider> Habilidades = new()
        {
            { Cargo.Facilitador,             new HabilidadeFacilitador() },
            { Cargo.DiretorDeComunicacao,    new HabilidadeDiretorComunicacao() },
            { Cargo.Investidor,              new HabilidadeInvestidor() },
            { Cargo.GestorDeRh,              new HabilidadeGestorDeRh() },
            { Cargo.DiretorDeExpansao,       new HabilidadeDiretorDeExpansao() },
            { Cargo.DiretorDeOperacoes,      new HabilidadeDiretorDeOperacoes() },
            { Cargo.EspecialistaEsg,         new HabilidadeEspecialistaEsg() },
            { Cargo.GestorDePortifolio,      new HabilidadeGestorPortifolio() },
            { Cargo.Auditor,                 new HabilidadeAuditor() },
            { Cargo.NegociadorInternacional, new HabilidadeNegociadorInternacional() },
            { Cargo.Trainee,                 new HabilidadeTrainee() },
            { Cargo.ConsultorExterno,        new HabilidadeConsultorExterno() },
        };

        public static IHabilidadeLider ObterHabilidade(this Cargo cargo)
            => Habilidades[cargo];
    }
}