using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.UI;

namespace MySqlMvc.Libreria
{
    public static class Controlador
    {
        private static int iteracion = 0;

        public static string Generar(Page page, List<Estructura> list, string tabla)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                // Cabecera
                stringBuilder.AppendLine($"// Co{tabla}.cs");
                stringBuilder.AppendLine("using System;");
                stringBuilder.AppendLine("using System.Data;");
                stringBuilder.AppendLine("using System.Text;");
                stringBuilder.AppendLine("using System.Threading.Tasks;");
                //stringBuilder.AppendLine("using System.Web.UI;");
                //stringBuilder.AppendLine("using Modelo;");
                stringBuilder.AppendLine("using MySql.Data.MySqlClient;");
                stringBuilder.AppendLine("");
                stringBuilder.AppendLine("namespace Controlador");
                stringBuilder.AppendLine("{");

                stringBuilder.AppendLine($"    /// <summary>");
                stringBuilder.AppendLine($"    /// Clase de {tabla}");
                stringBuilder.AppendLine($"    /// Generado automaticamente");
                stringBuilder.AppendLine($"    /// Leonardo Chuello (ljchuello@gmail.com)");
                stringBuilder.AppendLine($"    /// {DateTime.Now:yyyy-MM-dd}");
                stringBuilder.AppendLine($"    /// </summary>");

                stringBuilder.AppendLine($"    public class Co{tabla}");
                stringBuilder.AppendLine("    {");

                stringBuilder.AppendLine($"        private readonly PoolConexion _poolConexion = new PoolConexion();");
                stringBuilder.AppendLine($"        private readonly string _select = \"SELECT `{string.Join("`, `", list.Select(x => x.Nombre))}`\";");
                stringBuilder.AppendLine($"");

                #region Id

                if (list.Count(x => x.Nombre == "Id") > 0)
                {
                    stringBuilder.AppendLine($"        public async Task<Mo{tabla}> Select_Id_Async(string id)");
                    stringBuilder.AppendLine($"        {{");
                    stringBuilder.AppendLine($"            return await Task.Run(() =>");
                    stringBuilder.AppendLine($"            {{");
                    stringBuilder.AppendLine($"                Mo{tabla} mo{tabla} = new Mo{tabla}();");
                    stringBuilder.AppendLine($"                try");
                    stringBuilder.AppendLine($"                {{");
                    stringBuilder.AppendLine($"                    using (MySqlConnection mySqlConnection = _poolConexion.Devolver())");
                    stringBuilder.AppendLine($"                    {{");
                    stringBuilder.AppendLine($"                        MySqlCommand mySqlCommand = new MySqlCommand();");
                    stringBuilder.AppendLine($"                        mySqlCommand.Connection = mySqlConnection;");
                    stringBuilder.AppendLine($"                        mySqlCommand.CommandType = CommandType.Text;");
                    stringBuilder.AppendLine($"                        mySqlCommand.CommandText = $\"{{_select}} FROM `{tabla}` WHERE `Id` = @Id; \";");
                    stringBuilder.AppendLine($"                        mySqlCommand.Parameters.AddWithValue(\"@Id\", id);");
                    stringBuilder.AppendLine($"                        MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();");
                    stringBuilder.AppendLine($"                        while (mySqlDataReader.Read())");
                    stringBuilder.AppendLine($"                        {{");
                    stringBuilder.AppendLine($"                            mo{tabla} = Maker(mySqlDataReader);");
                    stringBuilder.AppendLine($"                        }}");
                    stringBuilder.AppendLine($"                    }}");
                    stringBuilder.AppendLine($"                    // Libre de pecados");
                    stringBuilder.AppendLine($"                    return mo{tabla};");
                    stringBuilder.AppendLine($"                }}");
                    stringBuilder.AppendLine($"                catch (Exception ex)");
                    stringBuilder.AppendLine($"                {{");
                    stringBuilder.AppendLine($"                    Console.WriteLine(ex);");
                    stringBuilder.AppendLine($"                    return mo{tabla};");
                    stringBuilder.AppendLine($"                }}");
                    stringBuilder.AppendLine($"            }});");
                    stringBuilder.AppendLine($"        }}");
                }

                #endregion

                #region Insert Block

                stringBuilder.AppendLine("");
                stringBuilder.AppendLine($"        public string Insert_Block(Mo{tabla} mo{tabla})");
                stringBuilder.AppendLine($"        {{");
                stringBuilder.AppendLine($"            StringBuilder stringBuilder = new StringBuilder();");
                stringBuilder.AppendLine($"            stringBuilder.AppendLine(\"\");");
                stringBuilder.AppendLine($"            stringBuilder.AppendLine(\"--  Insert {tabla}\");");
                stringBuilder.AppendLine($"            stringBuilder.AppendLine(\"INSERT INTO `{tabla}` (\");");

                // Cabecera
                iteracion = 0;
                foreach (var row in list)
                {
                    ++iteracion;
                    stringBuilder.AppendLine(iteracion != list.Count
                        ? $"            stringBuilder.AppendLine(\"`{row.Nombre}`, -- {row.Nombre} | {row.TipoMariaDb} | {row.TipoDotNet}\");"
                        : $"            stringBuilder.AppendLine(\"`{row.Nombre}` -- {row.Nombre} | {row.TipoMariaDb} | {row.TipoDotNet}\");");
                }

                // Values
                stringBuilder.AppendLine($"stringBuilder.AppendLine(\") VALUES (\");");

                //Detalles
                iteracion = 0;
                foreach (var row in list)
                {
                    ++iteracion;
                    stringBuilder.AppendLine(iteracion != list.Count
                        ? $"            stringBuilder.AppendLine($\"'{{PoolConexion.Tools.Remplazar(mo{tabla}.{row.Nombre})}}', -- {row.Nombre} | {row.TipoMariaDb} | {row.TipoDotNet}\");"
                        : $"            stringBuilder.AppendLine($\"'{{PoolConexion.Tools.Remplazar(mo{tabla}.{row.Nombre})}}'); -- {row.Nombre} | {row.TipoMariaDb} | {row.TipoDotNet}\");");
                }

                stringBuilder.AppendLine($"        return stringBuilder.ToString();");
                stringBuilder.AppendLine($"        }}");

                #endregion

                #region Update Block

                if (list.Count(x => x.Nombre.ToLower() == "id") == 1)
                {
                    stringBuilder.AppendLine("");
                    stringBuilder.AppendLine($"        public string Update_Block(Mo{tabla} mo{tabla})");
                    stringBuilder.AppendLine($"        {{");
                    stringBuilder.AppendLine($"            StringBuilder stringBuilder = new StringBuilder();");
                    stringBuilder.AppendLine($"            stringBuilder.AppendLine(\"\");");
                    stringBuilder.AppendLine($"            stringBuilder.AppendLine(\"--  Update {tabla}\");");
                    stringBuilder.AppendLine($"            stringBuilder.AppendLine(\"UPDATE `{tabla}` SET\");");

                    // Cabecera
                    iteracion = 0;
                    var listUpdate01 = list.Where(x => x.Nombre.ToLower() != "id").ToList();
                    foreach (var row in listUpdate01)
                    {
                        iteracion = iteracion + 1;
                        stringBuilder.AppendLine(iteracion != listUpdate01.Count
                            ? $"            stringBuilder.AppendLine($\"`{row.Nombre}` = '{{PoolConexion.Tools.Remplazar(mo{tabla}.{row.Nombre})}}', -- {row.Nombre} | {row.TipoMariaDb} | {row.TipoDotNet}\");"
                            : $"            stringBuilder.AppendLine($\"`{row.Nombre}` = '{{PoolConexion.Tools.Remplazar(mo{tabla}.{row.Nombre})}}' -- {row.Nombre} | {row.TipoMariaDb} | {row.TipoDotNet}\");");
                    }

                    // Where
                    stringBuilder.AppendLine($"            stringBuilder.AppendLine(\"WHERE\");");
                    var rowUpdate = list.FirstOrDefault(x => x.Nombre.ToLower() == "id") ?? new Estructura();
                    stringBuilder.AppendLine($"            stringBuilder.AppendLine($\"`{rowUpdate.Nombre}` = '{{PoolConexion.Tools.Remplazar(mo{tabla}.{rowUpdate.Nombre})}}'; -- {rowUpdate.Nombre} | {rowUpdate.TipoMariaDb} | {rowUpdate.TipoDotNet}\");");
                    stringBuilder.AppendLine($"        return stringBuilder.ToString();");
                    stringBuilder.AppendLine($"        }}");
                }

                #endregion

                #region Delete Block

                if (list.Count(x => x.Nombre.ToLower() == "id") == 1)
                {
                    stringBuilder.AppendLine("");
                    stringBuilder.AppendLine($"        public string Delete_Block(Mo{tabla} mo{tabla})");
                    stringBuilder.AppendLine($"        {{");
                    stringBuilder.AppendLine($"            StringBuilder stringBuilder = new StringBuilder();");
                    stringBuilder.AppendLine($"            stringBuilder.AppendLine(\"\");");
                    stringBuilder.AppendLine($"            stringBuilder.AppendLine(\"--  Delete {tabla}\");");
                    stringBuilder.AppendLine($"            stringBuilder.AppendLine(\"DELETE FROM `{tabla}` WHERE\");");
                    var rowDelete = list.FirstOrDefault(x => x.Nombre.ToLower() == "id") ?? new Estructura();
                    stringBuilder.AppendLine($"            stringBuilder.AppendLine($\"`{rowDelete.Nombre}` = '{{PoolConexion.Tools.Remplazar(mo{tabla}.{rowDelete.Nombre})}}'; -- {rowDelete.Nombre} | {rowDelete.TipoMariaDb} | {rowDelete.TipoDotNet}\");");
                    stringBuilder.AppendLine($"        return stringBuilder.ToString();");
                    stringBuilder.AppendLine($"        }}");
                }

                #endregion

                #region Maker

                stringBuilder.AppendLine($"");
                stringBuilder.AppendLine($"        private Mo{tabla} Maker(MySqlDataReader dtReader)");
                stringBuilder.AppendLine($"        {{");
                stringBuilder.AppendLine($"            try");
                stringBuilder.AppendLine($"            {{");
                stringBuilder.AppendLine($"                Mo{tabla} mo{tabla} = new Mo{tabla}();");

                foreach (var row in list)
                {
                    switch (row.TipoDotNet)
                    {
                        case "int":
                            stringBuilder.AppendLine($"                mo{tabla}.{row.Nombre} = dtReader.IsDBNull(dtReader.GetOrdinal(\"{row.Nombre}\")) ? 0 : dtReader.GetInt32(dtReader.GetOrdinal(\"{row.Nombre}\"));");
                            break;

                        case "decimal":
                            stringBuilder.AppendLine($"                mo{tabla}.{row.Nombre} = dtReader.IsDBNull(dtReader.GetOrdinal(\"{row.Nombre}\")) ? 0 : dtReader.GetDecimal(dtReader.GetOrdinal(\"{row.Nombre}\"));");
                            break;

                        case "bool":
                            stringBuilder.AppendLine($"                mo{tabla}.{row.Nombre} = !dtReader.IsDBNull(dtReader.GetOrdinal(\"{row.Nombre}\")) && dtReader.GetBoolean(dtReader.GetOrdinal(\"{row.Nombre}\"));");
                            break;

                        case "DateTime":
                            stringBuilder.AppendLine($"                mo{tabla}.{row.Nombre} = dtReader.IsDBNull(dtReader.GetOrdinal(\"{row.Nombre}\")) ? new DateTime(1900, 01, 01) : dtReader.GetDateTime(dtReader.GetOrdinal(\"{row.Nombre}\"));");
                            break;

                        default:
                            stringBuilder.AppendLine($"                mo{tabla}.{row.Nombre} = dtReader.IsDBNull(dtReader.GetOrdinal(\"{row.Nombre}\")) ? string.Empty : dtReader.GetString(dtReader.GetOrdinal(\"{row.Nombre}\"));");
                            break;
                    }
                }

                stringBuilder.AppendLine($"                return mo{tabla};");
                stringBuilder.AppendLine($"            }}");
                stringBuilder.AppendLine($"            catch (Exception ex)");
                stringBuilder.AppendLine($"            {{");
                stringBuilder.AppendLine($"                Console.WriteLine(ex);");
                stringBuilder.AppendLine($"                return new Mo{tabla}();");
                stringBuilder.AppendLine($"            }}");
                stringBuilder.AppendLine($"        }}");

                #endregion

                stringBuilder.AppendLine("    }");
                stringBuilder.AppendLine("}");
                return stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}