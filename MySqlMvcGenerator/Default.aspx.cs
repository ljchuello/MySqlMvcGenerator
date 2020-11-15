using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using MySqlMvcGenerator.Libreria;

namespace MySqlMvcGenerator
{
    public partial class Default : Page
    {
        private MariaDb _mariaDb = new MariaDb();
        private readonly Estructura _estructura = new Estructura();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Leemos
                _mariaDb = _mariaDb.Leer();
                txtServidor.Text = _mariaDb.Servidor;
                txtUsuario.Text = _mariaDb.Usuario;
                txtContrasenia.Text = _mariaDb.Contrasenia;
                txtBaseDatos.Text = _mariaDb.BaseDatos;

                // Evitamos el doble clic
                UControl.EvitarDobleEnvioButton(this, btnConectar);
                UControl.EvitarDobleEnvioButton(this, btnGenerar);
            }
        }

        protected void btnConectar_OnServerClick(object sender, EventArgs e)
        {
            try
            {
                // txtServidor
                if (Cadena.Vacia(txtServidor.Text))
                {
                    Notificacion.Toas(this, "Debe ingresar el servidor");
                    return;
                }

                // txtUsuario
                if (Cadena.Vacia(txtUsuario.Text))
                {
                    Notificacion.Toas(this, "Debe ingresar el usuario");
                    return;
                }

                // txtBaseDatos
                if (Cadena.Vacia(txtBaseDatos.Text))
                {
                    Notificacion.Toas(this, "Debe ingresar la contraseña");
                    return;
                }

                // Seteamos
                _mariaDb.Servidor = txtServidor.Text;
                _mariaDb.Usuario = txtUsuario.Text;
                _mariaDb.Contrasenia = txtContrasenia.Text;
                _mariaDb.BaseDatos = txtBaseDatos.Text;

                // Validamos
                if (!_mariaDb.Validar(this, _mariaDb))
                {
                    return;
                }

                // Libre de pecados
                Notificacion.Toas(this, "Conexión exitosa");

                // Llenamos las tablas
                List<string> tablas = _mariaDb.Tables_List(this, _mariaDb);

                // Listamos
                ddlTabla.DataSource = tablas;
                ddlTabla.DataBind();
            }
            catch (Exception ex)
            {
                Notificacion.Toas(this, $"Ah ocurrido un error; {ex.Message}");
            }
        }

        protected void btnGenerar_OnServerClick(object sender, EventArgs e)
        {
            try
            {
                // Validamos la conexion
                _mariaDb = new MariaDb();
                _mariaDb.Servidor = txtServidor.Text;
                _mariaDb.Usuario = txtUsuario.Text;
                _mariaDb.Contrasenia = txtContrasenia.Text;
                _mariaDb.BaseDatos = txtBaseDatos.Text;

                // Validamos la conexion
                if (!_mariaDb.Validar(this, _mariaDb))
                {
                    return;
                }

                // Obtenemos los detalles de la tabla
                DataTable dataTable = _mariaDb.Table_Details(this, _mariaDb, ddlTabla.SelectedValue);

                // Procesamos
                List<Estructura> lista = _estructura.Devolver(this, dataTable);

                // Libre de pecados
                Notificacion.Toas(this, $"Se ha generado la clase de {ddlTabla.SelectedValue}");
            }
            catch (Exception ex)
            {
                Notificacion.Toas(this, $"Ah ocurrido un error; {ex.Message}");
            }
        }
    }
}