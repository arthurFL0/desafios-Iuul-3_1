using ConsultorioOdontologico.Model;

namespace ConsultorioOdontologico.Controladores
{
    internal class ControladoraPaciente
    {
        private Persistencia persistencia;

        public ControladoraPaciente(Persistencia p)
        {
            persistencia = p;
        }

        public bool CadastrarPaciente(string cpf, string nome, string data,out Dictionary<string,string> listaErros)
        {

            Paciente p = new Paciente (cpf, nome, data);
            listaErros = p.ValidarDados();
            if (listaErros.Count > 0)
                return false;
            

            return persistencia.SalvarPaciente(p);
        }

        public bool VerificaCPF(string cpf, bool deveExistir)
        {
            Paciente p = new Paciente(cpf, "", "");
            return deveExistir ? persistencia.CpfExiste(p) : persistencia.CpfNaoExiste(p);
        }

        public Paciente PegarPaciente(string cpf)
        {
            return persistencia.PegarPaciente(cpf);
        }

        public IReadOnlyCollection<Paciente> PegarPacientes(string ordenacao)
        {
            return persistencia.PegarPacientes(ordenacao);
        }

        public void ExcluirPaciente(string cpf)
        {
            persistencia.ExcluirPaciente(cpf);
        }

    }
}
