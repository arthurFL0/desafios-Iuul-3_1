namespace ConsultorioOdontologico.Excecoes
{
    internal class PacienteJaExisteException : Exception
    {
        public PacienteJaExisteException() : base("Já existe um paciente cadastrado com o CPF informado.") { }
    }
}
