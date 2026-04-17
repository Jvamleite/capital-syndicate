using CapitalSyndicate.Domain.Cartas;
using CapitalSyndicate.Domain.Enums;

namespace CapitalSyndicate.Domain.Projetos
{
    internal sealed class Projeto
    {
        public int Escala { get; }
        public Setor SetorFinal { get; }
        public IReadOnlyList<CartaProfissional> Profissionais { get; }
        public CartaProfissional Lider { get; }

        private Projeto(IEnumerable<CartaProfissional> profissionais, CartaProfissional lider, Setor setorFinal)
        {
            Profissionais = [.. profissionais];
            Lider = lider;
            Escala = CalcularEscala(profissionais, lider);
            SetorFinal = setorFinal;
        }

        public static Projeto Criar(List<CartaProfissional> profissionais, CartaProfissional lider, Setor setorFinal)
        {
            if (profissionais is null || profissionais.Count == 0)
            {
                throw new Exception("Selecione pelo menos um profissional para criar o projeto.");
            }

            if (!profissionais.Contains(lider))
            {
                throw new Exception("O líder escolhido deve fazer parte do projeto.");
            }

            if (lider == null)
            {
                throw new Exception("Escolha um líder para o projeto.");
            }

            if (!lider.PodeSerLider())
            {
                throw new Exception("Esse profissional não pode atuar como líder.");
            }

            bool mesmoSetor = profissionais.All(p => p.Setor == profissionais.First().Setor);
            bool mesmoCargo = profissionais.All(p => p.Cargo == profissionais.First().Cargo);

            if (!(mesmoSetor || mesmoCargo))
            {
                throw new Exception("Os profissionais precisam compartilhar o mesmo setor ou cargo.");
            }

            return new Projeto(profissionais, lider, setorFinal);
        }

        private static int CalcularEscala(IEnumerable<CartaProfissional> profissionais, CartaProfissional lider)
        {
            return lider.Cargo == Cargo.FACILITADOR
                ? profissionais.Count() + 1
                : profissionais.Count();
        }
    }
}