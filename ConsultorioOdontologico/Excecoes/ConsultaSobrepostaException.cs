namespace ConsultorioOdontologico.Excecoes
{
    internal class ConsultaSobrepostaException : Exception
    {
        public ConsultaSobrepostaException() : base("Erro: Já existe uma consulta agendada nesse horário ") { }
    }
}
