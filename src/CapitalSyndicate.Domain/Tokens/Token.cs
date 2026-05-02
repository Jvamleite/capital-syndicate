using CapitalSyndicate.Domain.Enums;

namespace CapitalSyndicate.Domain.Tokens
{
    public abstract class Token
    {
        public int Valor { get; set; }

        public virtual int ObterPontuacao() => Valor;

        public static Token GerarToken(Cargo cargo) => cargo switch
        {
            Cargo.Auditor => new TokenAuditoria(),
            Cargo.EspecialistaEsg => new TokenEsg(),
            _ => new TokenAtivo()
        };
    }
}