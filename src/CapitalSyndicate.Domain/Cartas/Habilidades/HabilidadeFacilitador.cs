namespace CapitalSyndicate.Domain.Cartas.Habilidades
{
    internal class HabilidadeFacilitador : IHabilidadeLider
    {
        public int ModificarEscalaParaAvanco(int escala) => escala + 1;
    }
}