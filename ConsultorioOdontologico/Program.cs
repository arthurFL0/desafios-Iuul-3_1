using ConsultorioOdontologico.Controladores;
namespace ConsultorioOdontologico
{

    internal class Program
    {
        static void Main(string[] args)
        {
            Persistencia p = new Persistencia();
            InterfaceConsole c = new InterfaceConsole(new ControladoraPaciente(p), new ControladoraConsulta(p));
            c.Iniciar();
            
        }
    }
}
