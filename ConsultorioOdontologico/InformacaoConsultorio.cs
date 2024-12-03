

using System.Globalization;

namespace ConsultorioOdontologico
{
    internal class InformacaoConsultorio
    {
        private static string abertura = "0800";
        private static string encerramento = "1900";

        public static DateTime HoraAbertura => DateTime.ParseExact(abertura, "HHmm", CultureInfo.InvariantCulture);

        public static DateTime HoraEncerramento => DateTime.ParseExact(encerramento, "HHmm", CultureInfo.InvariantCulture);
    }

}
