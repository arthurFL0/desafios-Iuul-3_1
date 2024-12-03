namespace ConsultorioOdontologico.Excecoes
{
    internal class PacienteNaoExisteException : Exception
    {
        public PacienteNaoExisteException() : base("Não existe um paciente com o CPF informado.") { }
    }
}
