using System;
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
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine($"// Mo{tabla}.cs");
                stringBuilder.AppendLine("using System;");
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

                stringBuilder.AppendLine("        #region Block's");



                stringBuilder.AppendLine("        #endregion");

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