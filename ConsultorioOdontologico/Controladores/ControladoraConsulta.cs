using ConsultorioOdontologico.Model;

namespace ConsultorioOdontologico.Controladores
{
    internal class ControladoraConsulta
    {
        private Persistencia persistencia;

        public ControladoraConsulta(Persistencia persistencia)
        {
            this.persistencia = persistencia;
        }

        public bool CadastrarConsulta(Paciente p, string data, string horaInicial, string horaFinal, out Dictionary<string, List<String>> listaErros)
        {
            Consulta c = new Consulta(p, data, horaInicial, horaFinal);
            listaErros = c.ValidarDados();
            if (listaErros.Count > 0)
                return false;


           return persistencia.SalvarConsulta(c);
            
        }

        public void CancelarConsulta(string cpf, DateTime dataConsulta, DateTime horaInicial)
        {
            persistencia.CancelarConsulta(cpf,dataConsulta,horaInicial);
        }

        public IReadOnlyCollection<Consulta> PegarConsultas()
        {
            return persistencia.PegarConsultas();
        }

        public IReadOnlyCollection<Consulta> PegarConsultasPorPeriodo(string dataInicio, string dataFim)
        {
            return persistencia.PegarConsultasPorPeriodo(DateTime.Parse(dataInicio),DateTime.Parse(dataFim));
        }
    }
}
