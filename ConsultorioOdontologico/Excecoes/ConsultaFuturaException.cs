
namespace ConsultorioOdontologico.Excecoes
{
    internal class ConsultaFuturaException : Exception
    {
        public ConsultaFuturaException():  base("Erro: O paciente já possui uma consulta futura"){ }
    }
}
