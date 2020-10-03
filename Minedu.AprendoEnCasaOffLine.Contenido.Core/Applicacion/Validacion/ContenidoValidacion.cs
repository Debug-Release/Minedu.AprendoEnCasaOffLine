using MongoDB.Driver;
using Release.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DTO = Minedu.AprendoEnCasaOffLine.Contenido.Core.Aplicacion;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Core.Aplicacion
{
    public class ContenidoValidacion : ValidacionGenerica
    {
        //private readonly IEntregaRepositorio _entregaRepositorio;

        //public ContenidoValidacion(IEntregaRepositorio entregaRepositorio)
        //{
        //    _entregaRepositorio = entregaRepositorio;
        //}
        //public ValidacionCarga ValidarCargaMasiva(DTO.Entrega[] entregas)
        //{
        //    var vc = new ValidacionCarga();

        //    //Validaciones
        //    Regex expNum = new Regex(@"^[0-9]+$");
        //    var foliosOK = new List<string>();

        //    foreach (var e in entregas)
        //    {
        //        if (string.IsNullOrWhiteSpace(e.folio))
        //        {
        //            AddMgg(e.courier, "El folio es requerido");
        //        }
        //        if (!expNum.IsMatch(e.folio))
        //        {
        //            AddMgg(e.courier, "El formato del folio no es válido " + e.folio);
        //            break;
        //        }
        //        if (string.IsNullOrWhiteSpace(e.dni))
        //        {
        //            AddMgg(e.courier, "El dni es requerido para el folio " + e.folio);
        //        }
        //        if (e.productos.Length == 0)
        //        {
        //            AddMgg(e.courier, "Se requiere al menos un producto para el folio " + e.folio);
        //        }
        //        if (e.estado == null || e.estado.id < 0)
        //        {
        //            AddMgg(e.courier, "El estado es requerido para el folio " + e.folio);
        //        }
        //        if (e.courier == null || e.courier.id <= 0)
        //        {
        //            AddMgg(e.courier, "El courier es requerido para el folio " + e.folio);
        //        }
        //        if (string.IsNullOrWhiteSpace(e.fechaPactada))
        //        {
        //            AddMgg(e.courier, "La fechaPactada es requerida para el folio " + e.folio);
        //        }
        //        var entregaBD = _entregaRepositorio.FirstOrDefault(x => x.folio == e.folio);
        //        if (entregaBD != null)
        //        {
        //            AddMgg(e.courier, "El folio " + e.folio + " ya se encuentra registrado");
        //        }

        //        if (!Msg.Any())
        //        {
        //            foliosOK.Add(e.folio);
        //        }
        //    }
        //    vc.Msg.AddRange(Msg);
        //    vc.Folios.AddRange(foliosOK);

        //    return vc;
        //}

        //public List<string> ValidarRegistroBitacora(DTO.Entrega_Bitacora be, DTO.Courier[] couriers, DTO.Motivo[] motivos)
        //{
        //    var now = DateTime.Now;
        //    if (string.IsNullOrWhiteSpace(be.folio))
        //    {
        //        Msg.Add("El folio es requerido");
        //    }
        //    if (be.motivo == null || be.motivo.id <= 0)
        //    {
        //        Msg.Add("El motivo es requerido");
        //    }

        //    if (be.courier == null || be.courier.id <= 0)
        //    {
        //        Msg.Add("El courier es requerido");
        //    }
        //    if (be.fechaCreacion.Date < now.Date)
        //    {
        //        Msg.Add("La fecha de creación no es válida");
        //    }

        //    if (be.fechaCreacion.TimeOfDay == new TimeSpan(0, 0, 0))
        //    {
        //        Msg.Add("La fecha de creación debe considerar la hora");
        //    }

        //    if (couriers.All(x => x.id != be.courier?.id))
        //    {
        //        Msg.Add("El identificador del courier no es válido:" + be.courier?.id);
        //    }
        //    if (motivos.All(x => x.id != be.motivo?.id))
        //    {
        //        Msg.Add("El identificador del motivo no es válido:" + be.motivo?.id);
        //    }

        //    if (Msg.Any())
        //    {
        //        return Msg;
        //    }

        //    //Mas Validaciones en BD
        //    var entregaBD = _entregaRepositorio.FirstOrDefault(x => x.folio == be.folio);
        //    if (entregaBD == null)
        //    {
        //        Msg.Add("El folio " + be.folio + " no se encuentra registrado");
        //    }
        //    else
        //    {
        //        var estado = (DTO.ESTADO)entregaBD.estado_id;

        //        if (estado == DTO.ESTADO.ENTREGADO && entregaBD.esActivo == false) //Entregado
        //        {
        //            Msg.Add("El folio " + be.folio + " ya se encuentra en estado " + estado.ToString());
        //        }
        //        if (be.courier != null && entregaBD.courier_id != be.courier.id)
        //        {
        //            Msg.Add("El folio " + be.folio + " tiene asignado otro courier");
        //        }
        //    }

        //    return Msg;
        //}
        //private void AddMgg(DTO.Courier courier, string msg)
        //{
        //    Msg.Add(courier.nombre.ToUpper() + ":" + msg);
        //}
    }
    public class ValidacionCarga
    {
        public ValidacionCarga()
        {
            Msg = new List<string>();
            Folios = new List<string>();
        }
        public List<string> Msg { get; set; }
        public List<string> Folios { get; set; }
    }
}
