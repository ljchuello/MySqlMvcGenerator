<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MySqlMvcGenerator.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <%-- Metas --%>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <%-- Title --%>
    <title>MariaDB MVC</title>

    <%-- Google Icon Fonts --%>
    <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet" />

    <%-- FontAwesome --%>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.13.0/css/all.min.css" rel="stylesheet" />

    <%-- jQuery --%>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.5.0/jquery.min.js"></script>

    <%-- Materializecss --%>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/materialize/1.0.0/css/materialize.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/materialize/1.0.0/js/materialize.min.js"></script>

    <!-- base -->
    <link href="/script/base/css/stylesheet.css" rel="stylesheet" />
    <script src="/script/base/js/javascript.js"></script>

</head>
<body>
    <form id="form1" runat="server">

        <asp:ScriptManager runat="server"></asp:ScriptManager>

        <script>

            //On Page Load
            $(function () {
                InitV2();
            });

            //On UpdatePanel Refresh
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            if (prm != null) {
                prm.add_endRequest(function (sender, e) {
                    if (sender._postBackSettings.panelsToUpdate != null) {
                        InitV2();
                    }
                });
            };
        </script>

        <div class="row">
            <div class="col s12">
                <ul class="tabs">
                    <li class="tab col s3"><a class="active" href="#test1">Principal</a></li>
                    <li class="tab col s3"><a href="#test2">File.cs</a></li>
                </ul>
            </div>

            <div id="test1" class="col s12">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>

                        <div class="col s6">
                            <div class="input-field col s12">
                                <asp:TextBox ID="txtServidor" runat="server"></asp:TextBox>
                                <label for="<%=txtServidor.ClientID%>">Servidor</label>
                            </div>

                            <div class="input-field col s12">
                                <asp:TextBox ID="txtUsuario" runat="server"></asp:TextBox>
                                <label for="<%=txtUsuario.ClientID%>">Usuario</label>
                            </div>

                            <div class="input-field col s12">
                                <asp:TextBox ID="txtContrasenia" runat="server"></asp:TextBox>
                                <label for="<%=txtContrasenia.ClientID%>">Contrasenia</label>
                            </div>

                            <div class="input-field col s12">
                                <asp:TextBox ID="txtBaseDatos" runat="server"></asp:TextBox>
                                <label for="<%=txtBaseDatos.ClientID%>">Base de datos</label>
                            </div>

                            <div class="col s12">
                                <button id="btnConectar" runat="server" class="btn blue col s12" onserverclick="btnConectar_OnServerClick">
                                    Conectar
                                </button>
                            </div>

                        </div>

                        <div class="col s6">

                            <div class="input-field col s12">
                                <asp:DropDownList ID="ddlTabla" runat="server"></asp:DropDownList>
                                <label for="<%=ddlTabla.ClientID%>">Tabla</label>
                            </div>

                            <div class="col s12">
                                <button id="btnGenerar" runat="server" class="btn green col s12" onserverclick="btnGenerar_OnServerClick">
                                    Generar
                                </button>
                            </div>

                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div id="test2" class="col s12">File.cs</div>

        </div>

    </form>
</body>
</html>
