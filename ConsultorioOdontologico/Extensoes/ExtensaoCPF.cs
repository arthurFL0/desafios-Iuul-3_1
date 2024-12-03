using System.Text.RegularExpressions;

namespace ConsultorioOdontologico.Extensoes
{
    internal static class ExtensaoCPF
    {
        public static bool ehValido(this string cpf)
        {
            if (Regex.IsMatch(cpf, "^[0-9]{11}$"))
            {
                if (cpf.Distinct().Count() != 1)
                {

                    char[] digitos = cpf.ToCharArray();
                    int J = int.Parse(digitos[9].ToString());
                    int K = int.Parse(digitos[10].ToString());

                    int multi = 10;

                    int resultJ = 0;
                    int resultK = 0;

                    for (int i = 0; i < 9; i++)
                    {
                        resultJ += (digitos[i] - '0') * multi;
                        multi--;
                    }

                    multi = 11;

                    for (int i = 0; i < 10; i++)
                    {
                        resultK += (digitos[i] - '0') * multi;
                        multi--;
                    }

                    int restoJ = resultJ % 11;
                    int restoK = resultK % 11;

                    resultJ = restoJ == 0 || restoJ == 1 ? 0 : 11 - restoJ;
                    resultK = restoK == 0 || restoK == 1 ? 0 : 11 - restoK;

                    if (J == resultJ && K == resultK)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {
                    return false;
                }


            }
            else
            {
                return false;

            }
        }
    }
}
