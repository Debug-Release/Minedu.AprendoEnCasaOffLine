using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            new OperationManager().RunOperation("BO.Consolidacion.Console", () =>
            {
                new Manager().Start();
            });
            System.Console.Read();
        }
    }
}
