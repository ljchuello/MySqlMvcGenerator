using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using Newtonsoft.Json;

namespace MySqlMvc.Libreria
{
    public class Estructura
    {
        public string Nombre { set; get; } = string.Empty;
        public string Comentario { set; get; } = string.Empty;
        public string TipoMariaDb { set; get; } = string.Empty;
        public string TipoDotNet { set; get; } = string.Empty;
        public bool Where { set; get; } = false;

        public List<Estructura> Devolver(Page page, DataTable dataTable)
        {
            List<Estructura> lista = new List<Estructura>();
            try
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    Estructura estructura = new Estructura();

                    estructura.Nombre = $"{row["Field"]}";
                    estructura.Comentario = $"{row["Comment"]}";

                    if ($"{row["key"]}".Length > 0)
                    {
                        estructura.Where = true;
                    }

                    estructura.TipoMariaDb = $"{row["Type"]}";
                    estructura.TipoMariaDb = estructura.TipoMariaDb.Replace("0", string.Empty);
                    estructura.TipoMariaDb = estructura.TipoMariaDb.Replace("1", string.Empty);
                    estructura.TipoMariaDb = estructura.TipoMariaDb.Replace("2", string.Empty);
                    estructura.TipoMariaDb = estructura.TipoMariaDb.Replace("3", string.Empty);
                    estructura.TipoMariaDb = estructura.TipoMariaDb.Replace("4", string.Empty);
                    estructura.TipoMariaDb = estructura.TipoMariaDb.Replace("5", string.Empty);
                    estructura.TipoMariaDb = estructura.TipoMariaDb.Replace("6", string.Empty);
                    estructura.TipoMariaDb = estructura.TipoMariaDb.Replace("7", string.Empty);
                    estructura.TipoMariaDb = estructura.TipoMariaDb.Replace("8", string.Empty);
                    estructura.TipoMariaDb = estructura.TipoMariaDb.Replace("9", string.Empty);
                    estructura.TipoMariaDb = estructura.TipoMariaDb.Replace(",", string.Empty);
                    estructura.TipoMariaDb = estructura.TipoMariaDb.Replace(".", string.Empty);
                    estructura.TipoMariaDb = estructura.TipoMariaDb.Replace("(", string.Empty);
                    estructura.TipoMariaDb = estructura.TipoMariaDb.Replace(")", string.Empty);

                    switch (estructura.TipoMariaDb.ToLower())
                    {
                        case "tinyint":
                            estructura.TipoDotNet = "bool";
                            break;

                        case "int":
                            estructura.TipoDotNet = "int";
                            break;

                        case "decimal":
                            estructura.TipoDotNet = "decimal";
                            break;

                        case "datetime":
                            estructura.TipoDotNet = "DateTime";
                            break;

                        default:
                            estructura.TipoDotNet = "string";
                            break;
                    }

                    lista.Add(estructura);
                }
            }
            catch (Exception ex)
            {
                Notificacion.Toas(page, $"Ah ocurrido un error; {ex.Message}");
            }

            string a = JsonConvert.SerializeObject(lista, Formatting.Indented);
            return lista;
        }
    }
}