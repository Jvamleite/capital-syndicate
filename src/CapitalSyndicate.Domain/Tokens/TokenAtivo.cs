using CapitalSyndicate.Domain.Enums;

namespace CapitalSyndicate.Domain.Tokens
{
    internal class TokenAtivo : Token
    {
        public TipoAtivo Tipo { get; set; }

        public static int PontuarConjunto(List<TokenAtivo> ativos)
        {
            int total = 0;

            foreach (TipoAtivo tipo in Enum.GetValues<TipoAtivo>())
            {
                int quantidade = ativos.Count(a => a.Tipo == tipo);
                total += quantidade switch
                {
                    0 => 0,
                    1 => 1,
                    2 => 3,
                    3 => 6,
                    _ => 10
                };
            }

            bool temDiversificacao = Enum.GetValues<TipoAtivo>()
                .All(tipo => ativos.Any(a => a.Tipo == tipo));

            if (temDiversificacao)
            {
                total += 4;
            }

            return total;
        }
    }
}