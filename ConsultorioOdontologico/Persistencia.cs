using ConsultorioOdontologico.Excecoes;
using ConsultorioOdontologico.Model;
using System.Runtime.CompilerServices;

namespace ConsultorioOdontologico
{
    internal class Persistencia
    {
        private List<Paciente> pacientes;
        private List<Consulta> consultas;
        private bool listaConsultasFoiModificada;
        
        public IReadOnlyCollection<Paciente> PacientesReadOnly
        {
            get { return pacientes.AsReadOnly(); }
        }

        public IReadOnlyCollection<Consulta> ConsultasReadOnly
        {
            get { return consultas.AsReadOnly(); }
        }

        public Persistencia()
        {
            pacientes = new List<Paciente>();
            consultas = new List<Consulta>();
            listaConsultasFoiModificada = false;
        }

 

        public bool SalvarPaciente(Paciente p)
        {
            if (CpfNaoExiste(p)) { 
                pacientes.Add(p);
                return true;
                
            }

            return false;
        }

        public void ExcluirPaciente(string cpf)
        {
            Paciente p = PegarPaciente(cpf);
            p.AtualizarConsulta();
            if(p.ConsultaFutura != null)
            {
                throw new ExcluirPacienteException();

            }

            consultas.RemoveAll(c => p.Consultas.Contains(c));
            pacientes.Remove(p);
        }


        public bool CpfNaoExiste(Paciente p) { 
            
            return !pacientes.Contains(p)? true : throw new PacienteJaExisteException();
        }

        public bool CpfExiste(Paciente p) {
            return pacientes.Contains(p) ? true : throw new PacienteNaoExisteException();

        }

        public bool SalvarConsulta(Consulta c) {
            Paciente? pSalvo = pacientes.Find((p) => p.Equals(c.Paciente));
         

            if (pSalvo == null)
            {
                throw new PacienteNaoExisteException();
            }

            if(consultas.Contains(c))
            {
                throw new ConsultaSobrepostaException();
            }

            pSalvo.AtualizarConsulta();
            if(pSalvo.ConsultaFutura != null)
            {
                throw new ConsultaFuturaException();
            }

            pSalvo.Consultas.Add(c);
            pSalvo.ConsultaFutura = c;
            consultas.Add(c);
            listaConsultasFoiModificada = true;

            return true;
        }

        public IReadOnlyCollection<Consulta> PegarConsultas()
        {
            if (listaConsultasFoiModificada)
            {
                    consultas.Sort();
                    listaConsultasFoiModificada = false;
            }

            return ConsultasReadOnly;
            
        }

        public IReadOnlyCollection<Consulta> PegarConsultasPorPeriodo(DateTime inicio, DateTime fim)
        {
            if (listaConsultasFoiModificada)
            {
                consultas.Sort();
                listaConsultasFoiModificada = false;
            }



            return consultas.Where((c) => c.DataConsulta >= inicio && c.DataConsulta <= fim).ToList().AsReadOnly() ;

        }

       
        public Paciente PegarPaciente(string cpf)
        {
            Paciente? pSalvo = pacientes.Find((p) => p.CPF == cpf);
            if (pSalvo == null)
                throw new PacienteNaoExisteException();

            return pSalvo;
        }

        public IReadOnlyCollection<Paciente> PegarPacientes(string ordenacao)
        {
            List<Paciente> listaPacientes = pacientes;

            if(ordenacao == "cpf")
            {
                listaPacientes.Sort((p, p2) => p.CPF.CompareTo(p2.CPF));
            }else if(ordenacao == "nome")
            {
                listaPacientes.Sort((p, p2) => p.Nome.CompareTo(p2.Nome));
            }

            return listaPacientes.AsReadOnly();
        }

        public void CancelarConsulta(string cpf, DateTime dataConsulta, DateTime horaInicial)
        {
            Paciente? pSalvo = pacientes.Find((p) => p.CPF == cpf);
            if (pSalvo == null)
                throw new PacienteNaoExisteException();
            Consulta? c = pSalvo.ConsultaFutura;
            if (c == null || c.DataConsulta != dataConsulta.AddHours(horaInicial.Hour).AddMinutes(horaInicial.Minute))
                throw new ConsultaNaoEncontrada();

            pSalvo.ConsultaFutura = null;
            int i = consultas.FindIndex((c) => c.Paciente.CPF == cpf && c.DataConsulta == dataConsulta.AddHours(horaInicial.Hour).AddMinutes(horaInicial.Minute));
            consultas.RemoveAt(i);

        }
    }

}
