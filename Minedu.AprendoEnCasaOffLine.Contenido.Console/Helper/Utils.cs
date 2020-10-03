using OfficeOpenXml;
using System;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Console
{
    public class Utils
    {
        public static DataTable ReadCsvFile(string fileName, char delimiterHeader, char delimiterRow)
        {
            DataTable dtCsv = new DataTable();
            string Fulltext;

            string FileSaveWithPath = fileName;

            using (StreamReader sr = new StreamReader(FileSaveWithPath, Encoding.GetEncoding(28593)))
            {
                while (!sr.EndOfStream)
                {
                    Fulltext = sr.ReadToEnd().ToString();
                    string[] rows = Fulltext.Split(delimiterHeader);
                    for (int i = 0; i < rows.Count() - 1; i++)
                    {
                        string row = rows[i].Replace("\";\"", ",").Replace(";\"", ";").Replace("\";", ";");
                        string[] rowValues = row.Split(delimiterRow);
                        {
                            if (i == 0)
                            {
                                for (int j = 0; j < rowValues.Count(); j++)
                                {
                                    string columName = rowValues[j].Replace("\r", "");
                                    dtCsv.Columns.Add(columName);
                                }
                            }
                            else
                            {
                                DataRow dr = dtCsv.NewRow();
                                for (int k = 0; k < rowValues.Count(); k++)
                                {
                                    string value = rowValues[k].ToString().Replace("\"", "");
                                    dr[k] = value.Trim();
                                }
                                dtCsv.Rows.Add(dr);
                            }
                        }
                    }
                }
            }
            return dtCsv;
        }
        public static DataTable GetHeaderFromExcel(string fileName, string worksheetName)
        {
            var dt = new DataTable();

            Attempts(() =>
            {
                dt.Rows.Clear();

                FileInfo existingFile = new FileInfo(fileName);

                using (ExcelPackage package = new ExcelPackage(existingFile))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[worksheetName];

                    int colStart = worksheet.Dimension.Start.Column;
                    int colEnd = worksheet.Dimension.End.Column;

                    for (int j = colStart; j <= colEnd; j++)
                    {
                        var columnName = worksheet.Cells[1, j].Value.ToString();
                        dt.Columns.Add(CustomNormalization(columnName));
                    }
                }
            });

            return dt;
        }
        public static DataTable GetDataFromExcel(string fileName, string worksheetName, string expresion = null)
        {
            var dt = new DataTable();

            Attempts(() =>
            {
                dt.Rows.Clear();
                using (OleDbConnection conn = new OleDbConnection())
                {
                    conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1;MAXSCANROWS=0'";
                    using (OleDbCommand comm = new OleDbCommand())
                    {
                        if (!string.IsNullOrWhiteSpace(expresion))
                        {
                            comm.CommandText = "SELECT * FROM [" + worksheetName + "$] WHERE " + expresion;
                        }
                        else
                        {
                            comm.CommandText = "SELECT * FROM [" + worksheetName + "$]";
                        }
                        comm.Connection = conn;
                        using (OleDbDataAdapter da = new OleDbDataAdapter())
                        {
                            da.SelectCommand = comm;
                            da.Fill(dt);
                        }
                    }
                }
                foreach (DataColumn col in dt.Columns)
                {
                    col.ColumnName = CustomNormalization(col.ColumnName);
                }

            }, 5);

            return dt;
        }
        public static DataTable GetDataFromExcel2(string fileName, string worksheetName, bool hasHeaderRow = true)
        {
            var dt = new DataTable();

            Attempts(() =>
            {
                dt.Rows.Clear();

                FileInfo existingFile = new FileInfo(fileName);
                using (ExcelPackage package = new ExcelPackage(existingFile))
                {
                    //validate worksheetName
                    if (!package.Workbook.Worksheets.Any(c => c.Name == worksheetName))
                    {
                        throw new Exception("Not found worksheet Name \"" + worksheetName + "\" in " + fileName);
                    }
                    ExcelWorksheet ws = package.Workbook.Worksheets[worksheetName];

                    for (int j = ws.Dimension.Start.Column; j <= ws.Dimension.End.Column; j++)
                    {
                        var columnName = ws.Cells[1, j].Value?.ToString();
                        if (!string.IsNullOrWhiteSpace(columnName))
                        {
                            dt.Columns.Add(CustomNormalization(columnName));
                        }
                        else
                        {
                            columnName = string.Format("Column {0}", j);
                            dt.Columns.Add(columnName);
                        }
                    }
                    var startRow = hasHeaderRow ? 2 : 1;
                    for (var rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                    {
                        var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                        var row = dt.NewRow();
                        foreach (var cell in wsRow) row[cell.Start.Column - 1] = cell.Text;
                        dt.Rows.Add(row);
                    }
                }
            });
            return dt;

        }

        public static DataTable GetDataTableFromTemplate(string templateExcel)
        {
            var dt_data = GetHeaderFromExcel(templateExcel, "Anulaciones");
            /*
            var dt_data = new DataTable("BoAnulaciones");
            dt_data.Columns.Add("RESPONSABLE"); //1
            dt_data.Columns.Add("CONCATENAR"); //2
            dt_data.Columns.Add("NUMEROF12"); //3
            dt_data.Columns.Add("ESTADO_ACTUAL"); //4
            dt_data.Columns.Add("MEDIO_PAGO"); //5
            dt_data.Columns.Add("TIPO_DESPACHO");//6
            dt_data.Columns.Add("COMUNA");//7
            dt_data.Columns.Add("NUMEROF12_1");//8
            dt_data.Columns.Add("NRO_SS");//9
            dt_data.Columns.Add("NIVEL1");//10
            dt_data.Columns.Add("NIVEL2");//11
            dt_data.Columns.Add("NIVEL3");//12
            dt_data.Columns.Add("ESTADO");//13
            dt_data.Columns.Add("SUB_ESTADO");//14
            dt_data.Columns.Add("CANAL_ORIGEN");//15
            dt_data.Columns.Add("SUB_CANAL");//16
            dt_data.Columns.Add("DESCRIPCION");//17
            dt_data.Columns.Add("ESPECIFICACION");//18
            dt_data.Columns.Add("NUMEROF12_2");//19
            dt_data.Columns.Add("FECHA_CREACION");//20
            dt_data.Columns.Add("FECHA_COMPROMISO");//21
            dt_data.Columns.Add("FECHA_SOLUCION");//22
            dt_data.Columns.Add("FECHA_CIERRE");//23
            dt_data.Columns.Add("SKU");//24
            dt_data.Columns.Add("SOLUCION");//25
            dt_data.Columns.Add("NRO_ORDEN_COMPRA");//26
            dt_data.Columns.Add("MOTIVO_PROBLEMA");//27
            dt_data.Columns.Add("EVENTOS_ESPECIALES");//28
            dt_data.Columns.Add("LOGIN_PROPIETARIO");//29
            dt_data.Columns.Add("PROPIETARIO");//30
            dt_data.Columns.Add("DIVISION_PROPIETARIO");//31
            dt_data.Columns.Add("NOMBRE_CREADOR");//32
            dt_data.Columns.Add("DIVISION_CREADOR");//33
            dt_data.Columns.Add("TIPO_DOCUMENTO");//34
            dt_data.Columns.Add("NRO_DOCUMENTO");//35
            dt_data.Columns.Add("NOMBRE");//36
            dt_data.Columns.Add("APELLIDOS");//37
            dt_data.Columns.Add("TIPO_CONTACTO");//38
            dt_data.Columns.Add("CATEGORIA_FAL");//39
            dt_data.Columns.Add("ESTADO_SRX");//40
            dt_data.Columns.Add("TIPO_DESPACHO_SRX");//41
            */
            return dt_data;
        }

        private static string CustomNormalization(string inputString)
        {
            //var inputString = "Mañana será Otro Día";
            var normalizedString = inputString.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            for (int i = 0; i < normalizedString.Length; i++)
            {
                var uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(normalizedString[i]);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(normalizedString[i]);
                }
            }
            string nFin = (sb.ToString().Normalize(NormalizationForm.FormC));
            return nFin;
        }

        public static T GetValueFromDataTable<T>(DataTable dt, string name)
        {
            var value = default(T);
            var item = dt.AsEnumerable().FirstOrDefault(x => x.Field<string>("Nombre")?.ToUpper() == name.ToUpper());
            if (item != null)
            {

                value = (T)Convert.ChangeType(item["Valor"].ToString(), typeof(T), CultureInfo.InvariantCulture);

            }
            return value;
        }

        private static void Attempts(Action operation, int attempt = 5)
        {
            int k = 0;
            while (true)
            {
                try
                {
                    operation();

                    k = 0;
                    break;
                }
                catch
                {
                    if (k == attempt) // Intentos
                    {
                        k = 0;
                        throw;
                    }

                    k += 1;
                    Thread.Sleep(new TimeSpan(0, 0, 1));
                }
                finally
                {
                    if (k == 0)
                    {
                    }
                }
            }
        }
    }
}
