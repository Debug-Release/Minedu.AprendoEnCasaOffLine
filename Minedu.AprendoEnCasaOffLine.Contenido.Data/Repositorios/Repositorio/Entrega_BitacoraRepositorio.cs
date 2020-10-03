﻿using Minedu.AprendoEnCasaOffLine.Contenido.Data.Modelo;
using Release.MongoDB.Repository;
using Release.MongoDB.Repository.Base;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Data.Repositorios
{
    public class Entrega_BitacoraRepositorio : CustomBaseRepository<Entrega_Bitacora>, IEntrega_BitacoraRepositorio
    {
        public Entrega_BitacoraRepositorio(IDataContext context) : base(context)
        {
        }
    }
}
