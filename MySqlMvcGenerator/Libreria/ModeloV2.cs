﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace MySqlMvcGenerator.Libreria
{
    public class ModeloV2
    {
        public string Generar(Page page, string tabla, List<Estructura> campos)
        {
            int iteracion = 0;
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine($"// {tabla}.cs");
                stringBuilder.AppendLine("using System;");
                stringBuilder.AppendLine("using System.Text;");
                stringBuilder.AppendLine("using DataCloud.Libreria;");
                stringBuilder.AppendLine("using MySql.Data.MySqlClient;");
                stringBuilder.AppendLine("");
                stringBuilder.AppendLine("namespace DataCloud");
                stringBuilder.AppendLine("{");

                stringBuilder.AppendLine($"    /// <summary>");
                stringBuilder.AppendLine($"    /// Clase de {tabla}");
                stringBuilder.AppendLine($"    /// Generado automaticamente");
                stringBuilder.AppendLine($"    /// Leonardo Chuello (ljchuello@gmail.com)");
                stringBuilder.AppendLine($"    /// {DateTime.Now:yyyy-MM-dd}");
                stringBuilder.AppendLine($"    /// </summary>");

                stringBuilder.AppendLine($"    public class {tabla}");
                stringBuilder.AppendLine("    {");

                stringBuilder.AppendLine($"        private readonly PoolConexion _poolConexion = new PoolConexion();");
                stringBuilder.AppendLine($"");
                stringBuilder.AppendLine($"        private const string Select = \"SELECT {string.Join(", ", campos.Select(x => x.Nombre))} FROM {tabla}\";");
                stringBuilder.AppendLine("");

                #region Campos

                stringBuilder.AppendLine("        #region Field\n");
                foreach (var row in campos)
                {
                    switch (row.TipoDotNet)
                    {
                        case "int":
                            stringBuilder.AppendLine($"        /// <summary>");
                            stringBuilder.AppendLine($"        /// {row.Comentario}");
                            stringBuilder.AppendLine($"        /// </summary>");
                            stringBuilder.AppendLine($"        public int {row.Nombre} {{ set; get; }} = 0;\n");
                            break;

                        case "decimal":
                            stringBuilder.AppendLine($"        /// <summary>");
                            stringBuilder.AppendLine($"        /// {row.Comentario}");
                            stringBuilder.AppendLine($"        /// </summary>");
                            stringBuilder.AppendLine($"        public decimal {row.Nombre} {{ set; get; }} = 0;\n");
                            break;

                        case "bool":
                            stringBuilder.AppendLine($"        /// <summary>");
                            stringBuilder.AppendLine($"        /// {row.Comentario}");
                            stringBuilder.AppendLine($"        /// </summary>");
                            stringBuilder.AppendLine($"        public bool {row.Nombre} {{ set; get; }} = false;\n");
                            break;

                        case "DateTime":
                            stringBuilder.AppendLine($"        /// <summary>");
                            stringBuilder.AppendLine($"        /// {row.Comentario}");
                            stringBuilder.AppendLine($"        /// </summary>");
                            stringBuilder.AppendLine($"        public DateTime {row.Nombre} {{ set; get; }} = new DateTime(1900, 01, 01);\n");
                            break;

                        default:
                            stringBuilder.AppendLine($"        /// <summary>");
                            stringBuilder.AppendLine($"        /// {row.Comentario}");
                            stringBuilder.AppendLine($"        /// </summary>");
                            stringBuilder.AppendLine($"        public string {row.Nombre} {{ set; get; }} = string.Empty;\n");
                            break;
                    }
                }
                stringBuilder.AppendLine("        #endregion");

                #endregion

                #region Methods

                stringBuilder.AppendLine("");
                stringBuilder.AppendLine("        #region Methods");
                stringBuilder.AppendLine("");
                stringBuilder.AppendLine("");
                stringBuilder.AppendLine("");
                stringBuilder.AppendLine("        #endregion");

                #endregion

                #region Bloque

                if (campos.First().Nombre.ToLower() == "id")
                {
                    stringBuilder.AppendLine("");
                    stringBuilder.AppendLine("        #region Block's");

                    #region Insert Block

                    stringBuilder.AppendLine("");
                    stringBuilder.AppendLine($"        public string Insert_Block({tabla} {Cadena.PriMin(tabla)})");
                    stringBuilder.AppendLine($"        {{");
                    stringBuilder.AppendLine($"            StringBuilder stringBuilder = new StringBuilder();");
                    stringBuilder.AppendLine($"            stringBuilder.AppendLine(\"\");");
                    stringBuilder.AppendLine($"            stringBuilder.AppendLine(\"--  Insert {tabla}\");");
                    stringBuilder.AppendLine($"            stringBuilder.AppendLine(\"INSERT INTO {tabla} (\");");

                    // Cabecera
                    iteracion = 0;
                    foreach (var row in campos)
                    {
                        ++iteracion;
                        stringBuilder.AppendLine(iteracion != campos.Count
                            ? $"            stringBuilder.AppendLine(\"{row.Nombre}, -- {row.Nombre} | {row.TipoMariaDb} | {row.TipoDotNet}\");"
                            : $"            stringBuilder.AppendLine(\"{row.Nombre} -- {row.Nombre} | {row.TipoMariaDb} | {row.TipoDotNet}\");");
                    }

                    // Values
                    stringBuilder.AppendLine($"            stringBuilder.AppendLine(\") VALUES (\");");

                    //Detalles
                    iteracion = 0;
                    foreach (var row in campos)
                    {
                        ++iteracion;

                        if (!row.Nulo)
                        {
                            stringBuilder.AppendLine(iteracion != campos.Count
                                ? $"            stringBuilder.AppendLine($\"'{{_poolConexion.Remplazar({Cadena.PriMin(tabla)}.{row.Nombre})}}', -- {row.Nombre} | {row.TipoMariaDb} | {row.TipoDotNet}\");"
                                : $"            stringBuilder.AppendLine($\"'{{_poolConexion.Remplazar({Cadena.PriMin(tabla)}.{row.Nombre})}}'); -- {row.Nombre} | {row.TipoMariaDb} | {row.TipoDotNet}\");");
                        }
                        else
                        {
                            if (iteracion != campos.Count)
                            {
                                stringBuilder.AppendLine($"            stringBuilder.AppendLine(!_poolConexion.Vacia({Cadena.PriMin(tabla)}.{row.Nombre})");
                                stringBuilder.AppendLine($"                 ? $\"'{{_poolConexion.Remplazar({Cadena.PriMin(tabla)}.{row.Nombre})}}', -- {row.Nombre} | {row.TipoMariaDb} | {row.TipoDotNet}\"");
                                stringBuilder.AppendLine($"                : \"NULL, -- {row.Nombre} | {row.TipoMariaDb} | {row.TipoDotNet}\");");
                            }
                            else
                            {
                                stringBuilder.AppendLine($"            stringBuilder.AppendLine(!_poolConexion.Vacia({Cadena.PriMin(tabla)}.{row.Nombre})");
                                stringBuilder.AppendLine($"                 ? $\"'{{_poolConexion.Remplazar({Cadena.PriMin(tabla)}.{row.Nombre})}}', -- {row.Nombre} | {row.TipoMariaDb} | {row.TipoDotNet}\"");
                                stringBuilder.AppendLine($"                : \"NULL -- {row.Nombre} | {row.TipoMariaDb} | {row.TipoDotNet}\");");
                            }
                        }
                    }

                    stringBuilder.AppendLine($"        return stringBuilder.ToString();");
                    stringBuilder.AppendLine($"        }}");

                    #endregion

                    #region Update Block

                    if (campos.Count(x => x.Nombre.ToLower() == "id") == 1)
                    {
                        stringBuilder.AppendLine("");
                        stringBuilder.AppendLine($"        public string Update_Block({tabla} {Cadena.PriMin(tabla)})");
                        stringBuilder.AppendLine($"        {{");
                        stringBuilder.AppendLine($"            StringBuilder stringBuilder = new StringBuilder();");
                        stringBuilder.AppendLine($"            stringBuilder.AppendLine(\"\");");
                        stringBuilder.AppendLine($"            stringBuilder.AppendLine(\"--  Update {tabla}\");");
                        stringBuilder.AppendLine($"            stringBuilder.AppendLine(\"UPDATE {tabla} SET\");");

                        // Cabecera
                        iteracion = 0;
                        var listUpdate01 = campos.Where(x => x.Nombre.ToLower() != "id").ToList();
                        foreach (var row in listUpdate01)
                        {
                            iteracion = iteracion + 1;

                            if (!row.Nulo)
                            {
                                stringBuilder.AppendLine(iteracion != listUpdate01.Count
                                    ? $"            stringBuilder.AppendLine($\"{row.Nombre} = '{{_poolConexion.Remplazar({Cadena.PriMin(tabla)}.{row.Nombre})}}', -- {row.Nombre} | {row.TipoMariaDb} | {row.TipoDotNet}\");"
                                    : $"            stringBuilder.AppendLine($\"{row.Nombre} = '{{_poolConexion.Remplazar({Cadena.PriMin(tabla)}.{row.Nombre})}}' -- {row.Nombre} | {row.TipoMariaDb} | {row.TipoDotNet}\");");
                            }
                            else
                            {
                                if (iteracion != listUpdate01.Count)
                                {
                                    stringBuilder.AppendLine($"            stringBuilder.AppendLine(!_poolConexion.Vacia({Cadena.PriMin(tabla)}.{row.Nombre})");
                                    stringBuilder.AppendLine($"                ? $\"{row.Nombre} = '{{_poolConexion.Remplazar({Cadena.PriMin(tabla)}.{row.Nombre})}}', -- {row.Nombre} | {row.TipoMariaDb} | {row.TipoDotNet}\"");
                                    stringBuilder.AppendLine($"                : \"{row.Nombre} = NULL, -- {row.Nombre} | {row.TipoMariaDb} | {row.TipoDotNet}\");");
                                }
                                else
                                {
                                    stringBuilder.AppendLine($"            stringBuilder.AppendLine(!_poolConexion.Vacia({Cadena.PriMin(tabla)}.{row.Nombre})");
                                    stringBuilder.AppendLine($"                ? $\"{row.Nombre} = '{{_poolConexion.Remplazar({Cadena.PriMin(tabla)}.{row.Nombre})}}' -- {row.Nombre} | {row.TipoMariaDb} | {row.TipoDotNet}\"");
                                    stringBuilder.AppendLine($"                : \"{row.Nombre} = NULL -- {row.Nombre} | {row.TipoMariaDb} | {row.TipoDotNet}\");");
                                }
                            }
                        }

                        // Where
                        stringBuilder.AppendLine($"            stringBuilder.AppendLine(\"WHERE\");");
                        var rowUpdate = campos.FirstOrDefault(x => x.Nombre.ToLower() == "id") ?? new Estructura();
                        stringBuilder.AppendLine($"            stringBuilder.AppendLine($\"{rowUpdate.Nombre} = '{{_poolConexion.Remplazar({Cadena.PriMin(tabla)}.{rowUpdate.Nombre})}}'; -- {rowUpdate.Nombre} | {rowUpdate.TipoMariaDb} | {rowUpdate.TipoDotNet}\");");
                        stringBuilder.AppendLine($"        return stringBuilder.ToString();");
                        stringBuilder.AppendLine($"        }}");
                    }

                    #endregion

                    #region Delete Block

                    if (campos.Count(x => x.Nombre.ToLower() == "id") == 1)
                    {
                        stringBuilder.AppendLine("");
                        stringBuilder.AppendLine($"        public string Delete_Block({tabla} {Cadena.PriMin(tabla)})");
                        stringBuilder.AppendLine($"        {{");
                        stringBuilder.AppendLine($"            StringBuilder stringBuilder = new StringBuilder();");
                        stringBuilder.AppendLine($"            stringBuilder.AppendLine(\"\");");
                        stringBuilder.AppendLine($"            stringBuilder.AppendLine(\"--  Delete {tabla}\");");
                        stringBuilder.AppendLine($"            stringBuilder.AppendLine(\"DELETE FROM {tabla} WHERE\");");
                        var rowDelete = campos.FirstOrDefault(x => x.Nombre.ToLower() == "id") ?? new Estructura();
                        stringBuilder.AppendLine($"            stringBuilder.AppendLine($\"{rowDelete.Nombre} = '{{_poolConexion.Remplazar({Cadena.PriMin(tabla)}.{rowDelete.Nombre})}}'; -- {rowDelete.Nombre} | {rowDelete.TipoMariaDb} | {rowDelete.TipoDotNet}\");");
                        stringBuilder.AppendLine($"        return stringBuilder.ToString();");
                        stringBuilder.AppendLine($"        }}");
                    }

                    #endregion

                    stringBuilder.AppendLine("        #endregion");
                }

                #endregion

                #region Maker

                stringBuilder.AppendLine("\n        #region Maker");

                stringBuilder.AppendLine($"");
                stringBuilder.AppendLine($"        private {tabla} Maker(MySqlDataReader dtReader)");
                stringBuilder.AppendLine($"        {{");
                stringBuilder.AppendLine($"            " +
                                         $"{tabla} {Cadena.PriMin(tabla)} = new {tabla}();");

                foreach (var row in campos)
                {
                    switch (row.TipoDotNet)
                    {
                        case "int":
                            stringBuilder.AppendLine($"            {Cadena.PriMin(tabla)}.{row.Nombre} = dtReader.IsDBNull(dtReader.GetOrdinal(\"{row.Nombre}\")) ? 0 : dtReader.GetInt32(dtReader.GetOrdinal(\"{row.Nombre}\"));");
                            break;

                        case "decimal":
                            stringBuilder.AppendLine($"            {Cadena.PriMin(tabla)}.{row.Nombre} = dtReader.IsDBNull(dtReader.GetOrdinal(\"{row.Nombre}\")) ? 0 : dtReader.GetDecimal(dtReader.GetOrdinal(\"{row.Nombre}\"));");
                            break;

                        case "bool":
                            stringBuilder.AppendLine($"            {Cadena.PriMin(tabla)}.{row.Nombre} = !dtReader.IsDBNull(dtReader.GetOrdinal(\"{row.Nombre}\")) && dtReader.GetBoolean(dtReader.GetOrdinal(\"{row.Nombre}\"));");
                            break;

                        case "DateTime":
                            stringBuilder.AppendLine($"            {Cadena.PriMin(tabla)}.{row.Nombre} = dtReader.IsDBNull(dtReader.GetOrdinal(\"{row.Nombre}\")) ? new DateTime(1900, 01, 01) : dtReader.GetDateTime(dtReader.GetOrdinal(\"{row.Nombre}\"));");
                            break;

                        default:
                            stringBuilder.AppendLine($"            {Cadena.PriMin(tabla)}.{row.Nombre} = dtReader.IsDBNull(dtReader.GetOrdinal(\"{row.Nombre}\")) ? string.Empty : dtReader.GetString(dtReader.GetOrdinal(\"{row.Nombre}\"));");
                            break;
                    }
                }

                stringBuilder.AppendLine($"            return {Cadena.PriMin(tabla)};");
                stringBuilder.AppendLine($"        }}");

                stringBuilder.AppendLine("        #endregion");

                #endregion

                stringBuilder.AppendLine("    }");
                stringBuilder.AppendLine("}");
                return stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                Notificacion.Toas(page, $"Ah ocurridoun error; {ex.Message}");
                return string.Empty;
            }
        }
    }
}