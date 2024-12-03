using System.Globalization;
using System.Text.RegularExpressions;

namespace ConsultorioOdontologico.Model
{
    internal class Consulta : IEquatable<Consulta>, IComparable<Consulta>
    {
        public Paciente Paciente { get; }

        public DateTime DataConsulta { get; private set; }

        public DateTime HoraInicial { get; private set; }

        public DateTime HoraFinal { get; private set; }


        public string DataNaoValidada { get; }
        public string HoraInicialNaoValidada {  get; }
        public string HoraFinalNaoValidada { get; }



        public Consulta(Paciente p, string data, string horaInicial, string horaFinal)
        {
            Paciente = p;
            DataNaoValidada = data;
            HoraInicialNaoValidada = horaInicial;
            HoraFinalNaoValidada = horaFinal;
        }

       
        public bool Equals(Consulta? other)
        {
            return other is not null && DataConsulta.Date == other.DataConsulta.Date
             && other.HoraInicial < HoraFinal && other.HoraFinal > HoraInicial;
            // Impedir datas sobrepostas (interseção ou iguais)
        }

        public override bool Equals(object? obj)
        {
            if (obj is Consulta consulta)
            {
                return Equals(consulta);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return DataConsulta.GetHashCode() ^ HoraInicial.GetHashCode() ^ HoraFinal.GetHashCode();
        }

        public int CompareTo(Consulta? other)
        {
            if (other == null) return 1;

            return DataConsulta.CompareTo(other.DataConsulta);
        }
        public Dictionary<string, List<string>> ValidarDados()
        {
            Dictionary<string, List<string>> listaErros = new Dictionary<string, List<string>>();
            DateTime aberturaConsultorio = InformacaoConsultorio.HoraAbertura;
            DateTime encerramentoConsultorio = InformacaoConsultorio.HoraEncerramento;

            Match m = Regex.Match(DataNaoValidada, @"^([0][1-9]|[12][0-9]|3[01])\/(0[1-9]|1[0-2])\/([0-9]{4})$");


            if (m.Success)
            {
                DataConsulta = DateTime.ParseExact(DataNaoValidada, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            else
            {
                listaErros.Add("data", new List<string> { "Erro: Data não corresponde ao formato DD/MM/YYYY" });

            }

            try
            {
                HoraInicial = DateTime.ParseExact(HoraInicialNaoValidada, "HHmm", CultureInfo.InvariantCulture);
                Match m2 = Regex.Match(HoraInicialNaoValidada, @"^([0-1]+[0-9]|2[0-3])(00|15|30|45)$");
                if (!m2.Success)
                {
                    listaErros.Add("horaInicial", new List<string> { "Erro: Hora Inicial não corresponde ao formato HHmm ou não respeita o intervalo de 15 minutos" });
                }

                if (HoraInicial < aberturaConsultorio || HoraInicial > encerramentoConsultorio.AddMinutes(-15))
                {
                    if (listaErros.ContainsKey("horaInicial"))
                    {
                        listaErros["horaInicial"].Add("Erro: Hora inicial fora do horário de atendimento");
                    }
                    else
                    {
                        listaErros.Add("horaInicial", new List<string> { "Erro: Hora inicial fora do horário de atendimento" });
                    }
                }

            

            }
            catch (Exception ex)
            {
                listaErros.Add("horaInicial", new List<string> { "Erro: Hora Inicial não corresponde ao formato HHmm ou não respeita o intervalo de 15 minutos" });
            }


            try
            {
                HoraFinal = DateTime.ParseExact(HoraFinalNaoValidada, "HHmm", CultureInfo.InvariantCulture);
                Match m3 = Regex.Match(HoraFinalNaoValidada, @"^([0-1]+[0-9]|2[0-3])(00|15|30|45)$");
                if (!m3.Success)
                {
                    listaErros.Add("horaFinal", new List<string> { "Erro: Hora Final não corresponde ao formato HHmm ou não respeita o intervalo de 15 minutos" });
                }

                if (HoraFinal < aberturaConsultorio || HoraFinal > encerramentoConsultorio)
                {
                    if (listaErros.ContainsKey("horaFinal"))
                    {
                        listaErros["horaFinal"].Add("Erro: Hora final fora do horário de atendimento");
                    }
                    else
                    {
                        listaErros.Add("horaFinal", new List<string> { "Erro: Hora final fora do horário de atendimento" });
                    }
                }

                if (HoraInicial != DateTime.MinValue && HoraFinal <= HoraInicial)
                {
                    
                    if (listaErros.ContainsKey("horaFinal"))
                    {
                        listaErros["horaFinal"].Add("Erro: Hora Final não pode ser menor do que a hora inicial");
                    }
                    else
                    {
                        listaErros.Add("horaFinal", new List<string> { "Erro: Hora Final não pode ser menor do que a hora inicial" });
                    }
                    
                }

            }
            catch (Exception ex) {
                listaErros.Add("horaFinal", new List<string> { "Erro: Hora Final não corresponde ao formato HHmm ou não respeita o intervalo de 15 minutos" });
            }

            if (DataConsulta != DateTime.MinValue && HoraInicial != DateTime.MinValue)
            {
                DataConsulta = DataConsulta.AddHours(HoraInicial.Hour).AddMinutes(HoraInicial.Minute);
                if (DataConsulta < DateTime.Now)
                    listaErros.Add("Erro-futuro", new List<string> { "Erro: A data da consulta precisa estar no futuro" });

            }



            return listaErros;



        }

    }
}
