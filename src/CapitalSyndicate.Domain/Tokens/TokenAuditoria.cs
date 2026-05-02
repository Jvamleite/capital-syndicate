namespace CapitalSyndicate.Domain.Tokens
{
    internal class TokenAuditoria : Token
    {
        private static readonly int[] _valoresPossiveis = [-2, -1, 0, 1, 2, 3];

        public TokenAuditoria()
        {
            Valor = _valoresPossiveis[Random.Shared.Next(_valoresPossiveis.Length)];
        }
    }
}