using CapitalSyndicate.Domain.Cartas;
using CapitalSyndicate.Domain.Enums;

namespace CapitalSyndicate.Domain.Projetos
{
    internal sealed class Projeto
    {
        public int Escala { get; }
        public Setor SetorFinal { get; }
        public IReadOnlyList<CartaProfissional> Profissionais { get; }
        public CartaProfissional Gerente { get; }

        private Projeto(IEnumerable<CartaProfissional> profissionais, CartaProfissional gerente)
        {
            Profissionais = [.. profissionais];
            Gerente = gerente;
            Escala = CalcularEscala(profissionais, gerente);
            SetorFinal = gerente.Setor;
        }

        public static Projeto Criar(List<CartaProfissional> profissionais, CartaProfissional gerente)
        {
            if (profissionais is null || profissionais.Count == 0)
            {
                throw new Exception("Selecione pelo menos um profissional para criar o projeto.");
            }

            if (!profissionais.Contains(gerente))
            {
                throw new Exception("O gerente escolhido deve fazer parte do projeto.");
            }

            if (gerente == null)
            {
                throw new Exception("Escolha um gerente para o projeto.");
            }

            if (!gerente.PodeSerLider())
            {
                throw new Exception("Esse profissional não pode atuar como líder.");
            }

            bool mesmoSetor = profissionais.All(p => p.Setor == profissionais[0].Setor);
            bool mesmoCargo = profissionais.All(p => p.Cargo == profissionais[0].Cargo);

            if (!(mesmoSetor || mesmoCargo))
            {
                throw new Exception("Os profissionais precisam compartilhar o mesmo setor ou cargo.");
            }

            return new Projeto(profissionais, gerente);
        }

        private static int CalcularEscala(IEnumerable<CartaProfissional> profissionais, CartaProfissional gerente)
        {
            return gerente.Cargo == Cargo.Facilitador
                ? profissionais.Count() + 1
                : profissionais.Count();
        }
    }
}