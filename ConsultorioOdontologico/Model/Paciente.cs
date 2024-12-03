using ConsultorioOdontologico.Extensoes;
using System.Text.RegularExpressions;

namespace ConsultorioOdontologico.Model
{
    internal class Paciente : IEquatable<Paciente>
    {
        public string CPF { get; private set; }
        public string Nome { get; private set; }

        public DateTime DataNascimento { get; private set; }

        public string DataNaoValidada { get; private set; }

        public List<Consulta> Consultas { get; }

        public Consulta? ConsultaFutura { get;  set; }

        public Paciente(string cpf, string nome, string data)
        {
            CPF = cpf;
            Nome = nome;
            this.DataNaoValidada = data;

            Consultas = new List<Consulta>();
        }

        public void AtualizarConsulta()
        {
            if (ConsultaFutura != null && DateTime.Now > ConsultaFutura.DataConsulta)
                ConsultaFutura = null;

        }

        public void AdicionarConsulta(Consulta c)
        {
            Consultas.Add(c);
        }

        public bool Equals(Paciente? other)
        {
            return other is not null && other.CPF == CPF;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Paciente paciente)
            {
                return Equals(paciente);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return CPF.GetHashCode();
        }

        public Dictionary<string,string> ValidarDados()
        {
            Dictionary<string, string> listaErros = new Dictionary<string, string>();

            if (!CPF.ehValido())
                listaErros.Add("CPF", "Erro: CPF Inválido");

            if (Nome.Length < 5)
                listaErros.Add("Nome", "Erro: Nome precisa ter pelo menos 5 caracteres.");

            DateTime trezeAnos = DateTime.Now.AddYears(-13);

            // A Barra invertida \ que é usada para escapar / no Regex precisa ser escapada em C# com ela mesma \\
            // entao \/ = \\/ em C#

            // Ao usar @ antes da string o compilador entende que é para ignorar isso e o escape torna-se desnecessário

            Match m = Regex.Match(DataNaoValidada, @"^([0][1-9]|[12][0-9]|3[01])\/(0[1-9]|1[0-2])\/([0-9]{4})$");

            if (!m.Success)
            {
                listaErros.Add("DataNascimento", "Erro: Data de nascimento precisa estar no formato DD/MM/YYYY");
            }
            else
            {
                DateTime aux;

                DateTime.TryParse(DataNaoValidada, out aux);

                DataNascimento = aux;

                if (DataNascimento > trezeAnos)
                {
                    listaErros.Add("DataNascimento", "Paciente precisa ter mais de 13 anos de idade");

                }
            }
           

            return listaErros;
        }
    }
}
