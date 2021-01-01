using System;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core
{
    public class AppSettings
    {
        public Setting Settings { get; set; }
        public TokenSetting Token { get; set; }
    }
    public class Setting 
    {

        public string CADENA_CONEXION { get; set; }
        public string HORA_DESCARGA_INICIO { get; set; }
        public TimeSpan HORA_DESCARGA_INTERVALO { get; set; }
        public int SERVIDOR_INTERVALO { get; set; }
        public string RUTA_ARCHIVOS { get; set; }
        public string RUTA_ACK { get; set; }
        public string RUTA_TRAZABILIDAD { get; set; }
        public string RUTA_CONTINUIDAD { get; set; }
        public TimeSpan PROGRAMACION_TODO { get; set; }
    }
    public class TokenSetting
    {
        public string TOKEN_SECRET { get; set; }
        public string TOKEN_USER { get; set; }
        public string TOKEN_PASSWORD { get; set; }
        public string TOKEN_CLIENT_ID { get; set; }
        public int TOKEN_EXPIRES_DAYS { get; set; }
    }
}
