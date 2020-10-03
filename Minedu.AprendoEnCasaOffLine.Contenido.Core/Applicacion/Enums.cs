using System.Text.Json.Serialization;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EstadoContenido
    {
        Pendiente = 0,
        Cargando = 1,
        Cargado = 2
    }
}
