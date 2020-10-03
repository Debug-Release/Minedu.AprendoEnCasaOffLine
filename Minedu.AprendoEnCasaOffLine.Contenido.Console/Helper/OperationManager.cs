using System;
using System.IO;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Console
{
    public class OperationManager
    {
        public void RunOperation(string projectName, Action accion)
        {
            var inicio = DateTime.Now;


            System.Console.Title = projectName;
            System.Console.WriteLine("========================== " + projectName + " =======================\n");

            try
            {
                System.Console.WriteLine("Inicio:" + inicio.ToString("dd-MM-yyyy HH:mm:ss") + "\n");

                WriteFullLine("Espere..." + Environment.NewLine, ColorMessageType.Warning);
                accion();
            }
            catch (Exception ex)
            {
                var cex = ex.GetExceptionMessages();

                WriteFullLine(cex, ColorMessageType.Error);
            }
            finally
            {
                var fin = DateTime.Now;
                TimeSpan timeDifference = fin.Subtract(inicio);
                WriteFullLine("\nFin:" + fin.ToString("dd-MM-yyyy HH:mm:ss") + "\n");
                System.Console.WriteLine("\r\nDuracion:" + string.Format("{0}:{1}:{2}", timeDifference.Minutes, timeDifference.Seconds, timeDifference.Milliseconds));
            }
        }

        public static void WriteFullLine(string value, ColorMessageType colorMessageType = ColorMessageType.None)
        {
            switch (colorMessageType)
            {
                case ColorMessageType.None:
                    System.Console.BackgroundColor = ConsoleColor.Black;
                    System.Console.ForegroundColor = ConsoleColor.White;

                    break;
                case ColorMessageType.Success:
                    System.Console.BackgroundColor = ConsoleColor.Black;
                    System.Console.ForegroundColor = ConsoleColor.Green;

                    break;
                case ColorMessageType.Warning:
                    System.Console.BackgroundColor = ConsoleColor.Black;
                    System.Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case ColorMessageType.Error:
                    System.Console.BackgroundColor = ConsoleColor.Black;
                    System.Console.ForegroundColor = ConsoleColor.Red;

                    break;
            }

            System.Console.WriteLine(value ?? "");
            System.Console.ResetColor();
        }
    }
    public enum ColorMessageType : int
    {
        None = 0,
        Success = 1,
        Warning = 2,
        Error = 3
    }
    public static class Extensions
    {
        public static string GetExceptionMessages(this Exception e, string msgs = "")
        {
            if (e == null) return string.Empty;
            if (msgs == "") msgs = e.Message;
            if (e.InnerException != null)
                msgs += "\r\nInnerException: " + GetExceptionMessages(e.InnerException);
            return msgs;
        }
    }
}
