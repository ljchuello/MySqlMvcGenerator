﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace MySqlMvcGenerator.Libreria
{
    public class Response
    {
        public string Generar(Page page, string tabla, List<Estructura> campos)
        {
            int iteracion = 0;
            try
            {
                StringBuilder stringBuilder = new StringBuilder();

                #region Campos
                foreach (var row in campos)
                {
                    switch (row.TipoDotNet)
                    {
                        case "int":
                            stringBuilder.AppendLine($"public int {row.Nombre} {{ set; get; }} = 0;");
                            break;

                        case "decimal":
                            stringBuilder.AppendLine($"public decimal {row.Nombre} {{ set; get; }} = 0;");
                            break;

                        case "bool":
                            stringBuilder.AppendLine($"public bool {row.Nombre} {{ set; get; }} = false;");
                            break;

                        case "DateTime":
                            stringBuilder.AppendLine($"public DateTime {row.Nombre} {{ set; get; }} = new DateTime(1900, 01, 01);");
                            break;

                        default:
                            stringBuilder.AppendLine($"public string {row.Nombre} {{ set; get; }} = string.Empty;");
                            break;
                    }
                }
                #endregion

                stringBuilder.AppendLine();

                stringBuilder.AppendLine($"public dynamic Get(dynamic oIn)");
                stringBuilder.AppendLine($"{{");
                stringBuilder.AppendLine($"    dynamic oOut = new dynamic();");
                foreach (Estructura row in campos)
                {
                    stringBuilder.AppendLine($"    oOut.{row.Nombre} = oIn.{row.Nombre};");
                }
                stringBuilder.AppendLine($"    return oOut;");
                stringBuilder.AppendLine($"}}");

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