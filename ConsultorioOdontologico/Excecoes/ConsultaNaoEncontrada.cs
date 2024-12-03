

namespace ConsultorioOdontologico.Excecoes
{
    internal class ConsultaNaoEncontrada : Exception
    {
        public ConsultaNaoEncontrada() : base("Erro: agendamento não encontrado") { }
       
    }
}
