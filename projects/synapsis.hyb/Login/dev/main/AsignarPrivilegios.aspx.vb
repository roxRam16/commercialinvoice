Imports AspNet.Identity.MongoDB
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.Owin
Imports sax.authentication
Imports System.Reflection
Imports System.Security.Claims
Imports System.Threading.Tasks
Imports Wma.Exceptions

Public Class AsignarPrivilegios
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Sub AsignarPrivilegios_Click()


        Dim tagwatcher_ = New TagWatcher With {.Status = TagWatcher.TypeStatus.Ok}

        Dim tagwatcherTask_ As Task(Of TagWatcher) = AsignarPrivilegios(tbEmail.Text, tagwatcher_)

        tagwatcherTask_.Wait()

        tagwatcher_ = tagwatcherTask_.Result

        If tagwatcher_.Status = TagWatcher.TypeStatus.Ok Then

            Session("fallaLogin_") = "Actualización realizada con éxito"

            Response.Redirect("Login.aspx")

        Else

            Session("fallaLogin_") = "error de conexión " & tagwatcher_.ObjectReturned

        End If



    End Sub

    Sub GoStartSesion_Click()

        Response.Redirect("Login.aspx")

    End Sub


    Public Async Function AsignarPrivilegios(email_ As String, tagwatcher_ As TagWatcher) As Task(Of TagWatcher)

        Dim context_ = HttpContext.Current.GetOwinContext()

        Dim userManager_ = context_.GetUserManager(Of ApplicationUserManager)()

        ' Autenticación basada en Claims

        Dim user_ As ApplicationUser = Await userManager_.FindByNameAsync(email_).ConfigureAwait(False)

        Dim indexPage_ = 2

        user_.Claims.RemoveAll(Function(ch) ch.Type.Contains("SYNAPSIS"))

        user_.AddClaim(New Claim("SYNAPSISLocation", "NULL"))

        user_.AddClaim(New Claim("SYNAPSISPanel General_fas fa-chart-bar_1", "http://SynOperations/FrontEnd/Modulos/TraficoAA/ConsultasOperaciones/Ges003-001-Consultas.Principal.aspx"))

        user_.AddClaim(New Claim("SYNAPSISModulos_fa fa-laptop_2", "#"))

        If ckSynapsisPanIzqModClientes.Checked Then

            user_.AddClaim(New Claim("SYNAPSISClientes_ _" & indexPage_, "http://SynCatalogs/FrontEnd/Modulos/TraficoAA/Clientes/Ges022-001-Clientes.aspx"))

            indexPage_ += 1

        End If

        If ckSynapsisPanIzqModApendices.Checked Then

            user_.AddClaim(New Claim("SYNAPSISApendices_ _" & indexPage_, "http://SynOperations/FrontEnd/Modulos/TraficoAA/AltaApendices/Ges022-001-RegistroApendices.aspx"))

            indexPage_ += 1

        End If

        If ckSynapsisPanIzqModPedimentos.Checked Then

            user_.AddClaim(New Claim("SYNAPSISPedimentos_ _" & indexPage_, "http://SynOperations/FrontEnd/Modulos/TraficoAA/MetaforaPedimento/Ges022-001-MetaforaPedimento.aspx"))

            indexPage_ += 1

        End If

        If ckGuiasMaritimas.Checked Then

            user_.AddClaim(New Claim("SYNAPSISGuías Marítimas_ _" & indexPage_, "http://SynReferences/FrontEnd/Modulos/TraficoAA/Guias/Maritimas/Ges022-GuiaMaritima.aspx"))

            indexPage_ += 1

        End If

        If ckGuiasAereas.Checked Then

            user_.AddClaim(New Claim("SYNAPSISGuías Aereas_ _" & indexPage_, "http://SynReferences/FrontEnd/Modulos/TraficoAA/Guias/Aereas/Ges022-GuiaAerea.aspx"))

            indexPage_ += 1

        End If

        If ckSynapsisPanIzqModRegistroReferencias.Checked Then

            user_.AddClaim(New Claim("SYNAPSISReferencias_ _" & indexPage_, "http://SynReferences/FrontEnd/Modulos/TraficoAA/Referencias/Ges022-001-Referencias.aspx"))

            indexPage_ += 1

        End If

        If ckSynapsisPanIzqRevalidacion.Checked Then

            user_.AddClaim(New Claim("SYNAPSISRevalidación_ _" & indexPage_, "http://SynReferences/FrontEnd/Modulos/TraficoAA/Revalidacion/Ges022-001-Revalidacion.aspx"))

            indexPage_ += 1

        End If

        If ckSynapsisPanIzqModViajes.Checked Then

            user_.AddClaim(New Claim("SYNAPSISControl de Viajes_ _" & indexPage_, "http://SynReferences/FrontEnd/Modulos/TraficoAA/ControlViajes/Ges022-001-ControlViajes.aspx"))

            indexPage_ += 1

        End If

        If ckSynapsisConsolidados.Checked Then

            user_.AddClaim(New Claim("SYNAPSISConsolidados_ _" & indexPage_, "http://SynControls/FrontEnd/Modulos/TraficoAA/ControlConsolidados/Ges022-001-ControlConsolidados.aspx"))

            indexPage_ += 1

        End If

        If ckSynapsisCopiasSimples.Checked Then

            user_.AddClaim(New Claim("SYNAPSISCopias Simples_ _" & indexPage_, "http://SynControls/FrontEnd/Modulos/TraficoAA/CopiaSimple/Ges022-001-CopiasSimples.aspx"))

            indexPage_ += 1

        End If

        If ckSynapsisPartesII.Checked Then

            user_.AddClaim(New Claim("SYNAPSISPartes II_ _" & indexPage_, "http://SynControls/FrontEnd/Modulos/TraficoAA/PartesII/Ges022-001-PartesII.aspx"))

            indexPage_ += 1

        End If

        If ckSynapsisPrevios.Checked Then

            user_.AddClaim(New Claim("SYNAPSISProgramación de Previos_ _" & indexPage_, "http://SynControls/FrontEnd/Modulos/TraficoAA/ProgramacionPrevios/Ges022-001-ProgramacionPrevios.aspx"))

            indexPage_ += 1

        End If



        If ckSynapsisPanIzqAcuseValor.Checked Then

            user_.AddClaim(New Claim("SYNAPSISAcuse de Valor_ _" & indexPage_, "http://SynComercialInvoices/FrontEnd/Modulos/TraficoAA/AcusesValor/Ges022-001-AcuseValor.aspx"))

            indexPage_ += 1

        End If

        If ckSynapsisPanIzqModRegistroFacturasExp.Checked Then

            user_.AddClaim(New Claim("SYNAPSISFacturas Exportación_ _" & indexPage_, "http://SynComercialInvoices/FrontEnd/Modulos/TraficoAA/FacturasComerciales/FacturaComercialExportacion/Ges022_FacturaComercialExportacion.aspx"))

            indexPage_ += 1

        End If


        If ckSynapsisPanIzqModRegistroFacturasImp.Checked Then

            user_.AddClaim(New Claim("SYNAPSISFacturas Importación_ _" & indexPage_, "http://SynComercialInvoices/FrontEnd/Modulos/TraficoAA/FacturasComerciales/FacturaComercialImportacion/Ges003-001-FacturasComerciales.aspx"))

            indexPage_ += 1

        End If

        If ckSynapsisSubDivisionFactura.Checked Then

            user_.AddClaim(New Claim("SYNAPSISSubdivión de Facturas_ _" & indexPage_, "http://SynComercialInvoices/FrontEnd/Modulos/TraficoAA/SubdivisionFacturaComercial/Ges022_SubdivisionFacturaComercial.aspx"))

            indexPage_ += 1

        End If

        'If ckSynapsisContenedores.Checked Then

        '    user_.AddClaim(New Claim("SYNAPSISDestinatarios_ _" & indexPage_, "http://SynOperations/FrontEnd/Modulos/TraficoAA/Destinatarios/Ges022_Destinatarios.aspx"))

        '    indexPage_ += 1

        'End If

        If ckSynapsisPanIzqModRegistroProveedoresImp.Checked Then

            user_.AddClaim(New Claim("SYNAPSISProveedores Nacionales_ _" & indexPage_, "http://SynProducts/FrontEnd/Modulos/TraficoAA/Proveedores/ProveedorNacional/Ges022-001-ProveedorNacional.aspx"))

            indexPage_ += 1

        End If

        If ckSynapsisPanIzqModRegistroProveedoresExp.Checked Then

            user_.AddClaim(New Claim("SYNAPSISProveedores Extranjeros_ _" & indexPage_, "http://SynProducts/FrontEnd/Modulos/TraficoAA/Proveedores/ProveedorExtranjero/Ges022-001-ProveedorExtranjero.aspx"))

            indexPage_ += 1

        End If

        If ckSynapsisDestinatarios.Checked Then

            user_.AddClaim(New Claim("SYNAPSISDestinatarios_ _" & indexPage_, "http://SynProducts/FrontEnd/Modulos/TraficoAA/Proveedores/ProveedorExtranjero/Ges022-001-ProveedorExtranjero.aspx"))

            indexPage_ += 1

        End If

        If ckSynapsisPanIzqModRegistroProductos.Checked Then

            user_.AddClaim(New Claim("SYNAPSISProductos_ _" & indexPage_, "http://SynProducts/FrontEnd/Modulos/TraficoAA/Productos/Ges022-001-RegistroProductos.aspx"))

            indexPage_ += 1

        End If

        If ckSynapsisPanIzqModTarifaArancelaria.Checked Then

            user_.AddClaim(New Claim("SYNAPSISTarifa Arencelaria_ _" & indexPage_, "http://SynProducts/FrontEnd/Modulos/TraficoAA/TarifaArancelaria/Ges022-001-TarifaArancelaria.aspx"))

            indexPage_ += 1

        End If

        If ckSynapsisPanIzqModBusquedaGeneral.Checked Then

            user_.AddClaim(New Claim("SYNAPSISBusqueda general_ _" & indexPage_, "http://SynReporting/FrontEnd/Modulos/TraficoAA/BusquedaGeneral/BusquedaGeneral.aspx"))

            indexPage_ += 1

        End If

        If ckSynapsisPanIzqProcesamientoElectronico.Checked Then

            user_.AddClaim(New Claim("SYNAPSISProcesamiento_ _" & indexPage_, "http://SynDigitization/FrontEnd/Modulos/TraficoAA/ProcesamientoElectronicoDocumentos/Ges022_ProcesamientoElectronicoDocumentos.aspx"))

            indexPage_ += 1

        End If

        If ckSynapsisPanIzqCuboDatos.Checked Then

            user_.AddClaim(New Claim("SYNAPSISCubo de Datos_ _" & indexPage_, "http://SynServices/FrontEnd/Modulos/TraficoAA/CuboDatos/Ges022-001-CuboDatos.aspx"))

            indexPage_ += 1

        End If

        If ckbTodas.Checked Then

            user_.AddClaim(New Claim("FAVORITEOFFICE", "1"))

            user_.AddClaim(New Claim("OFFICES", "0,1,2,3,4,5,6,7,8,9,10"))

        Else

            Dim offices_ As String = ""

            Dim FavoriteOffice_ = ""

            If ckbVirtual.Checked Then

                offices_ = "0,"

            End If

            If ckbVeracruz.Checked Then

                offices_ &= "1,"

                FavoriteOffice_ = "1"

            End If

            If ckbManzanillo.Checked Then

                offices_ &= "2,"

                If FavoriteOffice_ = "" Then

                    FavoriteOffice_ = "2"

                End If

            End If

            If ckbLazaroCardenas.Checked Then

                offices_ &= "3,"

                If FavoriteOffice_ = "" Then

                    FavoriteOffice_ = "3"

                End If


            End If

            If ckbAltamira.Checked Then

                offices_ &= "4,"

                If FavoriteOffice_ = "" Then

                    FavoriteOffice_ = "4"

                End If


            End If

            If ckbAICM.Checked Then

                offices_ &= "5,"

                If FavoriteOffice_ = "" Then

                    FavoriteOffice_ = "5"

                End If


            End If

            If ckbAIFA.Checked Then

                offices_ &= "6,"

                If FavoriteOffice_ = "" Then

                    FavoriteOffice_ = "6"

                End If


            End If

            If ckbToluca.Checked Then

                offices_ &= "7,"

                If FavoriteOffice_ = "" Then

                    FavoriteOffice_ = "7"

                End If


            End If

            If ckbNuevoLaredo.Checked Then

                offices_ &= "8,"

                If FavoriteOffice_ = "" Then

                    FavoriteOffice_ = "8"

                End If


            End If

            If ckbColombia.Checked Then

                offices_ &= "9,"

                If FavoriteOffice_ = "" Then

                    FavoriteOffice_ = "9"

                End If


            End If


            If ckbSanLuis.Checked Then

                offices_ &= "10,"

                If FavoriteOffice_ = "" Then

                    FavoriteOffice_ = "10"

                End If


            End If

            If offices_ = "" Then

                offices_ &= "1,"

                If FavoriteOffice_ = "" Then

                    FavoriteOffice_ = "1"

                End If

            End If

            offices_ = offices_.Substring(0, offices_.Length - 1)

            user_.AddClaim(New Claim("FAVORITEOFFICE", FavoriteOffice_))

            user_.AddClaim(New Claim("OFFICES", offices_))

        End If


        If ckAdminSynApsis.Checked Then

            Dim role_ As ApplicationRole

            Dim roleManager_ = New ApplicationRoleManager()

            If Not Await roleManager_.RoleExistsAsync("ADMINSYNAPSIS").ConfigureAwait(False) Then

                role_ = New ApplicationRole

                role_.Name = "ADMINSYNAPSIS"

                Await roleManager_.CreateAsync(role_).ConfigureAwait(False)

            Else

                role_ = roleManager_.FindByName("ADMINSYNAPSIS")

            End If

            user_.AddRole(role_.Id)

            Await userManager_.UpdateAsync(user_)

            'Dim result_ As IdentityResult = Await userManager_.AddToRoleAsync(user_.Id, role_.Id).ConfigureAwait(False)

            'user_.AddRole("ADMINSYNAPSIS")

        End If

        If ckAdminKromBaseWeb.Checked Then

            user_.AddRole("ADMINKROMBASEWEB")

        End If



        Dim result = Await userManager_.UpdateAsync(user_).ConfigureAwait(False)

        If result.Succeeded Then

            Return tagwatcher_

        Else

            tagwatcher_.ObjectReturned = result.Errors(0)

            tagwatcher_.SetError(Me, result.Errors(0))

            Return tagwatcher_

        End If

    End Function

    Sub CargarPermisosAdmin(sender_ As Object, event_ As EventArgs)

        If ckAdminSynApsis.Checked Then

            ckSynapsisPanIzqModClientes.Checked = True

            ckSynapsisPanIzqModApendices.Checked = True

            ckSynapsisPanIzqModPedimentos.Checked = True

            ckGuiasMaritimas.Checked = True

            ckSynapsisPanIzqModRegistroReferencias.Checked = True

            ckSynapsisPanIzqRevalidacion.Checked = True

            ckSynapsisPanIzqModViajes.Checked = True

            ckSynapsisConsolidados.Checked = True

            ckSynapsisCopiasSimples.Checked = True

            ckSynapsisPartesII.Checked = True

            ckSynapsisPrevios.Checked = True

            ckSynapsisPanIzqAcuseValor.Checked = True

            ckSynapsisPanIzqModRegistroFacturasImp.Checked = True

            ckSynapsisPanIzqModRegistroFacturasExp.Checked = True


            ckSynapsisSubDivisionFactura.Checked = True

            ckSynapsisPanIzqModRegistroProveedoresImp.Checked = True

            ckSynapsisPanIzqModRegistroProveedoresExp.Checked = True

            ckSynapsisDestinatarios.Checked = True

            ckSynapsisPanIzqModRegistroProductos.Checked = True

            ckSynapsisPanIzqModTarifaArancelaria.Checked = True

            ckSynapsisPanIzqModBusquedaGeneral.Checked = True

            ckSynapsisPanIzqProcesamientoElectronico.Checked = True

            ckSynapsisPanIzqCuboDatos.Checked = True

        End If

    End Sub

End Class