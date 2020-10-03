namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.ViewModel
{
    public class Contenido : Base
    {
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string rutaOrigen { get; set; }
        public EstadoContenido estado { get; set; }
    }
}
