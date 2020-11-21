namespace Minedu.AprendoEnCasaOffLine.Contenido.Core
{
    //[JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EstadoContenido
    {
        Pendiente = 0,
        Cargando = 1,
        Cargado = 2,
        Error = 99
    }
    //[JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EstadoDescarga
    {
        Pendiente = 0,
        Programado = 1,
        Descargando = 2,
        Descargado = 3,
        Error = 99
    }
}
