using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace MySqlMvc.Libreria
{
    public class MariaDb
    {
        public string Servidor { set; get; } = string.Empty;
        public string Usuario { set; get; } = string.Empty;
        public string Contrasenia { set; get; } = string.Empty;
        public string BaseDatos { set; get; } = string.Empty;

        public void Guardar(MariaDb mariaDb)
        {
            try
            {
                if (!Directory.Exists(@"C:\LJChuello"))
                {
                    Directory.CreateDirectory(@"C:\LJChuello");
                }
                File.WriteAllText(@"C:\LJChuello\LJChuello", JsonConvert.SerializeObject(mariaDb));
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
        }

        public MariaDb Leer()
        {
            MariaDb mariaDb = new MariaDb();
            try
            {
                return JsonConvert.DeserializeObject<MariaDb>(File.ReadAllText(@"C:\LJChuello\LJChuello"));
            }
            catch (Exception)
            {
                return mariaDb;
            }
        }

        public bool Validar(Page page, MariaDb mariaDb)
        {
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection())
                {
                    mySqlConnection.ConnectionString = $"datasource ={mariaDb.Servidor}; username={mariaDb.Usuario}; password={mariaDb.Contrasenia}; database={mariaDb.BaseDatos};";
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand();
                    mySqlCommand.Connection = mySqlConnection;
                    mySqlCommand.CommandType = CommandType.Text;
                    mySqlCommand.CommandText = "SELECT VERSION()";
                    mySqlCommand.ExecuteNonQuery();
                }

                // Guardamos la conexion
                Guardar(mariaDb);

                // Libre de pecados
                return true;
            }
            catch (Exception ex)
            {
                Notificacion.Toas(page, $"Los datos de conexión no son válidos; {ex.Message}");
                return false;
            }
        }

        public List<string> Tables_List(Page page, MariaDb mariaDb)
        {
            List<string> list = new List<string>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection())
                {
                    mySqlConnection.ConnectionString = $"datasource={mariaDb.Servidor}; username={mariaDb.Usuario}; password={mariaDb.Contrasenia}; database={mariaDb.BaseDatos};";
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand();
                    mySqlCommand.Connection = mySqlConnection;
                    mySqlCommand.CommandType = CommandType.Text;
                    mySqlCommand.CommandText = $"SHOW TABLES FROM {mariaDb.BaseDatos}";
                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            list.Add($"{mySqlDataReader[0]}");
                        }
                    }
                }

                // Libre de pecados
                return list;
            }
            catch (Exception ex)
            {
                Notificacion.Toas(page, $"Los datos de conexión no son válidos; {ex.Message}");
                return list;
            }
        }

        public DataTable Table_Details(Page page, MariaDb mariaDb, string tabla)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection())
                {
                    mySqlConnection.ConnectionString = $"datasource={mariaDb.Servidor}; username={mariaDb.Usuario}; password={mariaDb.Contrasenia}; database={mariaDb.BaseDatos};";
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand();
                    mySqlCommand.Connection = mySqlConnection;
                    mySqlCommand.CommandType = CommandType.Text;
                    mySqlCommand.CommandText = $"SHOW FULL COLUMNS FROM {tabla};";
                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);
                    mySqlDataAdapter.Fill(dataTable);
                }

                // Libre de pecados
                return dataTable;
            }
            catch (Exception ex)
            {
                Notificacion.Toas(page, $"Los datos de conexión no son válidos; {ex.Message}");
                return dataTable;
            }
        }
    }
}