using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace MySqlMvc.Libreria
{
    public class Modelo
    {
        private int iteracion = 0;
        public string Generar(Page page, List<Estructura> list, string tabla)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine($"// Mo{tabla}.cs");
                stringBuilder.AppendLine("using System;");
                stringBuilder.AppendLine("");
                stringBuilder.AppendLine("namespace Modelo");
                stringBuilder.AppendLine("{");

                stringBuilder.AppendLine($"    /// <summary>");
                stringBuilder.AppendLine($"    /// Clase de {tabla}");
                stringBuilder.AppendLine($"    /// Generado automaticamente");
                stringBuilder.AppendLine($"    /// Leonardo Chuello (ljchuello@gmail.com)");
                stringBuilder.AppendLine($"    /// {DateTime.Now:yyyy-MM-dd}");
                stringBuilder.AppendLine($"    /// </summary>");

                stringBuilder.AppendLine($"    public class Mo{tabla}");
                stringBuilder.AppendLine("    {");

                iteracion = 0;
                foreach (var row in list)
                {
                    if (iteracion++ > 0) { stringBuilder.AppendLine(); }
                    switch (row.TipoDotNet)
                    {
                        case "int":
                            stringBuilder.AppendLine($"        /// <summary>");
                            stringBuilder.AppendLine($"        /// {row.Comentario}");
                            stringBuilder.AppendLine($"        /// </summary>");
                            stringBuilder.AppendLine($"        public int {row.Nombre} {{ set; get; }} = 0;");
                            break;

                        case "decimal":
                            stringBuilder.AppendLine($"        /// <summary>");
                            stringBuilder.AppendLine($"        /// {row.Comentario}");
                            stringBuilder.AppendLine($"        /// </summary>");
                            stringBuilder.AppendLine($"        public decimal {row.Nombre} {{ set; get; }} = 0;");
                            break;

                        case "bool":
                            stringBuilder.AppendLine($"        /// <summary>");
                            stringBuilder.AppendLine($"        /// {row.Comentario}");
                            stringBuilder.AppendLine($"        /// </summary>");
                            stringBuilder.AppendLine($"        public bool {row.Nombre} {{ set; get; }} = false;");
                            break;

                        case "DateTime":
                            stringBuilder.AppendLine($"        /// <summary>");
                            stringBuilder.AppendLine($"        /// {row.Comentario}");
                            stringBuilder.AppendLine($"        /// </summary>");
                            stringBuilder.AppendLine($"        public DateTime {row.Nombre} {{ set; get; }} = new DateTime(1900, 01, 01);");
                            break;

                        default:
                            stringBuilder.AppendLine($"        /// <summary>");
                            stringBuilder.AppendLine($"        /// {row.Comentario}");
                            stringBuilder.AppendLine($"        /// </summary>");
                            stringBuilder.AppendLine($"        public string {row.Nombre} {{ set; get; }} = string.Empty;");
                            break;
                    }
                }

                stringBuilder.AppendLine("    }");
                stringBuilder.AppendLine("}");
                return stringBuilder.ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}