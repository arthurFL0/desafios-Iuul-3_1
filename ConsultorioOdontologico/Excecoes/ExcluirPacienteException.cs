
namespace ConsultorioOdontologico.Excecoes
{
    internal class ExcluirPacienteException : Exception
    {
        public ExcluirPacienteException() : base("Erro: paciente está agendado") { }
    }
}
