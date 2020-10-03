using Minedu.AprendoEnCasaOffLine.Contenido.Console.Proxy;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using DTO = Minedu.AprendoEnCasaOffLine.Contenido.Entities;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Console
{
    public class Manager
    {
        //Variables
        private static string _url;

        private static string _rutaArchivos;
        private static string _formatoArchivo;
        private static string _formatoFecha;
        private static List<string> _columnas;
        private static string _folderTempDiario;


        private DeliveryApiProxy _deliveryApiProxy;

        public Manager()
        {
            InitLocalVariables();
        }

        public void Start()
        {
            CargaMasivaFolios();
            //RegistraMotivos();
        }

        #region Metodos Privados

        private void InitLocalVariables()
        {
            var apps = ConfigurationManager.AppSettings;

            _url = apps["UrlApi"];
            _rutaArchivos = apps["RutaArchivos"];
            _formatoArchivo = apps["FormatoArchivo"];
            _formatoFecha = apps["FormatoFecha"];
            _deliveryApiProxy = new DeliveryApiProxy(_url);
            _columnas = new List<string>
            {
                "COPECREA",
                "NDOCUMEN",
                "F12",
                "DPERSONA",
                "DRAZSOCI",
                "DDIRECCI",
                "DDBARRIO",
                "DPROVINC",
                "FREPARTO",
                "CREFEREN",
                "DARTICUL",
                "QCANTIDA"
            };

            string folderTemp = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Temp");

            if (!Directory.Exists(folderTemp))
            {
                Directory.CreateDirectory(folderTemp);
            }
            _folderTempDiario = Path.Combine(folderTemp, DateTime.Now.ToString("dd-MM-yyyy"));
            if (!Directory.Exists(_folderTempDiario))
            {
                Directory.CreateDirectory(_folderTempDiario);
            }
        }

        private void CargaMasivaFolios()
        {
            var now = DateTime.Now;
            var lstEntregas = new List<DTO.Entrega>();

            //Leyendo Couriers permitidos
            var couriers = _deliveryApiProxy.Couriers();
            System.Console.WriteLine("  => INICIO\n");

            _formatoFecha = now.ToString(_formatoFecha);

            foreach (var c in couriers)
            {
                string formatoArchivo = string.Format(_formatoArchivo, c.nombre.ToUpper(), _formatoFecha);
                string excelPath = Path.Combine(_rutaArchivos, formatoArchivo);
                string excelNewPath = Path.Combine(_folderTempDiario, Path.GetFileName(excelPath));

                System.Console.WriteLine("  => Leyendo archivo :" + excelPath);

                //Lectura de Archivo por courier
                if (!File.Exists(excelPath))
                {
                    OperationManager.WriteFullLine("  => No existe archivo a procesar con el nombre :" + excelPath, ColorMessageType.Warning);
                    continue;
                }
                else
                {
                    //Copy to Local                
                    System.IO.File.Copy(excelPath, excelNewPath, true);
                }

                var source = Utils.GetDataFromExcel2(excelNewPath, "Hoja1");

                if (source.Rows.Count > 0)
                {
                    var err = ValidarFormato(source);
                    if (err.Any())
                    {
                        foreach (var r in err)
                        {
                            OperationManager.WriteFullLine("  => No existe la columna :" + r, ColorMessageType.Error);
                        }
                        continue;
                    }

                    //Filtro por courier
                    var data = source.AsEnumerable().Where(x => x.Field<string>("COPECREA").ToUpper() == c.nombre.ToUpper()).ToList();

                    if (data.Any())
                    {
                        //DateTime FECHA_COMPRA = DateTime.ParseExact(row["FECHA_COMPRA"].ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.CreateSpecificCulture("es-PE"));

                        var group = data.GroupBy(r => new
                        {
                            NDOCUMEN = r["NDOCUMEN"],
                            F12 = r["F12"],
                            DPERSONA = r["DPERSONA"],
                            DRAZSOCI = r["DRAZSOCI"],
                            DDIRECCI = r["DDIRECCI"],
                            DDBARRIO = r["DDBARRIO"],
                            DPROVINC = r["DPROVINC"],
                            FREPARTO = r["FREPARTO"],
                            COPECREA = r["COPECREA"]
                        })
                        .Select(y => new DTO.Entrega
                        {
                            esActivo = true,
                            //fechaCreacion = now,
                            fechaCreacion = DateTime.Parse(y.Key.FREPARTO.ToString()),
                            ordenCompra = y.Key.NDOCUMEN.ToString(),
                            subOrdenCompra = y.Key.NDOCUMEN.ToString(),
                            folio = y.Key.F12.ToString(),
                            dni = y.Key.DPERSONA.ToString(),
                            cliente = y.Key.DRAZSOCI.ToString(),
                            direccion = y.Key.DDIRECCI.ToString(),
                            distrito = y.Key.DDBARRIO.ToString(),
                            provincia = y.Key.DPROVINC.ToString(),
                            fechaPactada = y.Key.FREPARTO.ToString(),
                            courier = couriers.First(t => t.nombre.ToUpper() == y.Key.COPECREA.ToString().ToUpper()),
                            estado = new DTO.Estado { id = 0 }, //0 = PENDIENTE
                            productos = y.Select(z => new DTO.Producto
                            {
                                sku = z["CREFEREN"].ToString(),
                                descripcion = z["DARTICUL"].ToString(),
                                cantidad = z["QCANTIDA"].ToString()
                            }).ToArray()
                        })
                        .ToArray();

                        lstEntregas.AddRange(group);

                        System.Console.WriteLine("  => Registros a cargar de " + c.nombre.ToUpper() + ":" + group.Length + "\n");
                        System.Console.WriteLine("  => Inicia envio de " + c.nombre.ToUpper() + "\n");

                        try
                        {
                            var cr = _deliveryApiProxy.CargaMasiva(lstEntregas);
                            OperationManager.WriteFullLine("  => " + cr.Msg, cr.Success ? ColorMessageType.Success : ColorMessageType.Warning);

                            foreach (var s in cr.Errors)
                            {
                                OperationManager.WriteFullLine("  => " + s.Value, cr.Success ? ColorMessageType.Success : ColorMessageType.Warning);
                            }
                        }
                        catch (Exception ex)
                        {
                            var cex = ex.GetExceptionMessages();
                            OperationManager.WriteFullLine("  => " + cex, ColorMessageType.Error);
                        }
                        System.Console.WriteLine("\n  => Termina envio de " + c.nombre.ToUpper() + "\n");
                        lstEntregas.Clear();

                        System.Threading.Thread.Sleep(new TimeSpan(0, 0, 20));
                    }
                }
            }

            System.Console.WriteLine("\n  => FIN");
        }

        private void RegistraMotivos()
        {

            try
            {
                var lstMotivos = new List<DTO.Motivo>
                {
                    new DTO.Motivo {estado = new DTO.Estado { id=1},id=1,descripcion="En Ruta" },

                    new DTO.Motivo {estado = new DTO.Estado { id=2},id=2,descripcion="Entrega Efectiva" },
                    new DTO.Motivo {estado = new DTO.Estado { id=2},id=3,descripcion="Entrega a Terceros" },

                    new DTO.Motivo {estado = new DTO.Estado { id=3},id=4,descripcion="Ausente" },
                    new DTO.Motivo {estado = new DTO.Estado { id=3},id=4,descripcion="Anulará Compra" },
                    new DTO.Motivo {estado = new DTO.Estado { id=3},id=6,descripcion="Dirección errada" },
                    new DTO.Motivo {estado = new DTO.Estado { id=3},id=7,descripcion="Domicilio no corresponde" },
                    new DTO.Motivo {estado = new DTO.Estado { id=3},id=8,descripcion="Fraude" },
                    new DTO.Motivo {estado = new DTO.Estado { id=3},id=9,descripcion="Fuera de Cobertura" },
                    new DTO.Motivo {estado = new DTO.Estado { id=3},id=10,descripcion="Modelo no elegido" },
                    new DTO.Motivo {estado = new DTO.Estado { id=3},id=11,descripcion="No conocen al cliente" },
                    new DTO.Motivo {estado = new DTO.Estado { id=3},id=12,descripcion="Producto dañado" },
                    new DTO.Motivo {estado = new DTO.Estado { id=3},id=13,descripcion="Producto faltante" },
                    new DTO.Motivo {estado = new DTO.Estado { id=3},id=14,descripcion="Producto incompleto" },
                    new DTO.Motivo {estado = new DTO.Estado { id=3},id=15,descripcion="Producto robado" },
                    new DTO.Motivo {estado = new DTO.Estado { id=3},id=16,descripcion="Reprogramado" },
                    new DTO.Motivo {estado = new DTO.Estado { id=3},id=17,descripcion="Zona Intransitable" },
                    new DTO.Motivo {estado = new DTO.Estado { id=3},id=18,descripcion="Zona Peligrosa" },

                };
                var cr = _deliveryApiProxy.RegistraMotivos(lstMotivos);
                OperationManager.WriteFullLine("  => " + cr.Msg, cr.Success ? ColorMessageType.Success : ColorMessageType.Warning);

                foreach (var s in cr.Errors)
                {
                    OperationManager.WriteFullLine("  => " + s.Value, cr.Success ? ColorMessageType.Success : ColorMessageType.Warning);
                }
            }
            catch (Exception ex)
            {
                var cex = ex.GetExceptionMessages();
                OperationManager.WriteFullLine("  => " + cex, ColorMessageType.Error);
            }

        }

        private List<string> ValidarFormato(DataTable dt)
        {
            string[] columnNames = dt.Columns.Cast<DataColumn>().Select(x => x.ColumnName.ToUpper()).ToArray();

            return _columnas.Where(item => !columnNames.Contains(item)).ToList();
        }
        #endregion Metodos Privados
    }
}
