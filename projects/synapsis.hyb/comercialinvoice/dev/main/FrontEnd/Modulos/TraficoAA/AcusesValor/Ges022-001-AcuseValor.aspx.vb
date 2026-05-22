Imports System.IO
Imports System.Security.Claims
Imports System.Security.Cryptography.X509Certificates
Imports Syn.CustomBrokers.SDKs.ControllerSDKCustomsSettings
Imports Syn.CustomBrokers.SDKs.IControllerSDKCustomsSettings
Imports Syn.Exceptions
Imports Gsol
Imports Gsol.krom
Imports Gsol.Web.Components
Imports MongoDB.Bson
Imports MongoDB.Driver
Imports Rec.Globals
Imports Rec.Globals.Controllers
Imports Rec.Globals.Controllers.Empresas
Imports Rec.Globals.InstitucionBancaria
Imports Rec.Globals.Utils
Imports SAT
Imports Sax.Web
Imports Syn.CustomBrokers.Controllers
Imports Syn.Documento
Imports Syn.Documento.Componentes
Imports Syn.Nucleo.Recursos
Imports Syn.Nucleo.RecursosComercioExterior
Imports Syn.Nucleo.RecursosComercioExterior.CamposAcuseValor
Imports Syn.Nucleo.RecursosComercioExterior.CamposFacturaComercial
Imports Syn.Operaciones
Imports Syn.Utils
Imports VUCEM
Imports Wma.Exceptions
Imports Wma.Exceptions.TagWatcher
Imports Wma.Exceptions.TagWatcher.TypeStatus

Public Class Ges022_001_AcuseValor
    Inherits ControladorBackend

#Region "████████████████████████████████████████   Atributos locales  ██████████████████████████████████████"
    '    ██                 Defina en esta región sus atributos o propiedades locales                      ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

    Private _controladorProveedor As New CtrlProveedoresOperativos

    Private _icontroladorMonedas As IControladorMonedas

    Private _icontroladorAcuseValor As IControladorAcuseValor

    Private _icontroladorFactura As IControladorFacturaComercial

    Private _icontroladorEmpresa As Rec.Globals.Controllers.Empresas.IControladorEmpresas

    Private _organismo As New Syn.Utils.Organismo

    Private _controladorUnidadesMedida As New ControladorUnidadesMedida

    Private _sistema As New Syn.Utils.Organismo


#End Region

#Region "██████ Vinculación c/capas inf █████████       SAX      ████████████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████

    'EVENTO INICIALIZADOR
    Public Overrides Sub Inicializa()

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Configure la barra de búsquedas para el módulo                                            '
        ' Asigne una instancia de su clase constructura "Preasignación" en la propiedad DataObject  '
        ' Asigne n cantidad de filtros u opciones de consulta para su documento "Preasignación"     '
        '  -defina la seccion donde quiere consultar                                                '
        '  -defina el campo que debe consultar en la seccio dada                                    '
        '  -defina un titulo a los resultados de su filtro                                          '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        With Buscador

            .DataObject = New ConstructorAcuseValor(True)
            .addFilter(SeccionesAcuseValor.SAcuseValor1, CamposAcuseValor.CA_NUMERO_ACUSEVALOR, "ACUSE DE VALOR")
            .addFilter(SeccionesAcuseValor.SAcuseValor1, CamposFacturaComercial.CA_NUMERO_FACTURA, "FACTURA")
            .addFilter(SeccionesAcuseValor.SAcuseValor2, CamposAcuseValor.CA_RAZON_SOCIAL_EMISOR, "Emisor")
            .addFilter(SeccionesAcuseValor.SAcuseValor3, CamposAcuseValor.CA_RAZON_SOCIAL_DESTINATARIO_ACUSE, "Destinatario")
            .addFilter(SeccionesAcuseValor.SAcuseValor4, CamposAcuseValor.CA_DESCRIPCION_PARTIDA_ACUSEVALOR, "Descripción A.V.")

        End With

        'scAsignadoA.DataEntity = New Ejecutivos()

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' SeccionesClientes.SCS1         = ID de la sección en nuestro documento donde se quiere buscar              '
        ' CamposClientes.CA_RAZON_SOCIAL = ID del campo dentro de la sección asignada donde se realizara la búsqueda '
        ' "Cliente"                      = Titulo personalizado para el filtro                                       '
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' NOTAS A CONSIDERAR                                                                                               '
        ' -----------------------------------------------------------------------------------------------------------------'
        ' SESIONES: para el uso de secciones utilice los métodos:                                                          '
        '                                                                                                                  '
        ' SetVars(ByVal var_ As String, Optional ByVal value_ As Object = Nothing)                                         '
        ' GetVars(ByVal var_ As String, Optional ByVal defaultValue_ As Object = Nothing)                                  '
        '                                                                                                                  '
        ' -----------------------------------------------------------------------------------------------------------------'                                                                                                                 
        ' BOTONERA ESTADO INICIAL: si se desea tener un estado inicial de la botera distinto a lo que ofrece por defecto,  '
        ' sobreescriba el método Public Overridable Sub InicializaBotonera() y asigne la modalidad deseada                 '
        '                                                                                                                  '
        ' Formulario.Modality = FormControl.ButtonBarModality.Open                                                         '
        '                                                                                                                  '
        ' Formulario es una propiedad global que hace referencia a nuestro formulario en el marcado.                       '
        ' asegúrate que dicha asignación ocurra solo cuando no hay postback, coloquelo dentro del siguiente IF             '
        ' If Not Page.IsPostBack Then ..... EndIf                                                                          '
        '                                                                                                                  '
        ' -----------------------------------------------------------------------------------------------------------------'                                                                                                                 
        ' BOTONERA CAMBIO DE ESTADO: Si se desea cambiar el estado de la botonera en cualquier otro momento como           '
        ' al desencadenar un evento utilice el método PreparaBotonera(ByVal modality_ As [Enum]) y asigne                  '
        ' el estado deseado                                                                                                '
        '                                                                                                                  '
        ' -----------------------------------------------------------------------------------------------------------------'                                                                                                                 
        ' TARJETEROS CAMBIO DE ESTADO: para cambiar el estado en un tarjetero utilice el siguiente método                  '
        '                                                                                                                  '
        ' PreparaTarjetero(ByVal modality_ As [Enum], ByRef tarjetero_ As PillboxControl)                                  '
        ' Designe la modalidad y el ID de su PillboxControl                                                                '
        '                                                                                                                  '
        ' -----------------------------------------------------------------------------------------------------------------'                                                                                                                 
        ' ACTIVAR/DESACTIVAR FORMULARIO                                                                                    '
        ' si desea activar o desactivar los controles en el formulario en algun caso especial utilice el siguiente método  '
        '                                                                                                                  '
        ' ActivaControles(Optional ByVal activar_ As Boolean = True)                                                       '
        '                                                                                                                  '
        ' -----------------------------------------------------------------------------------------------------------------'
        ' MOSTRAR MENSAJES                                                                                                 '
        ' DisplayMessage(ByVal message_ As String, Optional ByVal status_ As StatusMessage = StatusMessage.Success)        '
        '                                                                                                                  '
        ' message_  = contenido del mensaje a mostrar al usuario                                                           '
        ' status_   = por defecto siempre es success                                                                       '
        ' -----------------------------------------------------------------------------------------------------------------'
        ' VENTANAS DE DIALOGO                                                                                              '
        ' DisplayAlert(ByVal title_ As String,                                                                             '
        '                    ByVal message_ As String,                                                                     '
        '                    ByVal argument_ As String,                                                                    '
        '                    Optional accept_ As String = "Entendido",                                                     '
        '                    Optional reject_ As String = Nothing)                                                         '
        '                                                                                                                  '
        ' title_  = contenido del título de la ventana de dialogo                                                          '
        ' message_ = contenido del mensaje de  la ventana de dialogo                                                       '
        ' argument_ = valor custom por el programador para evaluarlo y realizar acciones a conveniencia                    '
        ' accept_ = titulo del boton por defecto del dialogo                                                               '
        ' reject_ = titulo del boton de cancelar, cuando se definen ambos botones en automatico se convierte               '
        ' en una ventana de conformación y sus eventos son capturables en el código para realizar alguna tarea             '
        '                                                                                                                  '
        ' todas la ventanas de dialogo ejecutaran los siguientes métodos he alli donde la propiedad arguement_             '
        ' tiene sentido, sobre escriba los métodos en su código                                                            '
        '                                                                                                                  '
        ' Public Overridable Sub AceptaConfirmacion(argument_ As String)                                                   '
        ' Public Overridable Sub RechazaConfirmacion(argument_ As String)                                                  '
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        _icontroladorAcuseValor = New ControladorAcuseValor


        _icontroladorFactura = New ControladorFacturaComercial(1, True)

        _icontroladorMonedas = New ControladorMonedas

        _controladorProveedor = New CtrlProveedoresOperativos

        SetVars("_AcuseValorFindBar", Nothing)

        'Dim userclaim_ As ClaimsPrincipal = TryCast(Request.RequestContext.HttpContext.User,
        '                                ClaimsPrincipal)

        'Dim idUser_ = userclaim_.Claims.Where(Function(ch) ch.Type.Contains("nameidentifier")).First.Value

        'Dim algo = 3

        CycleLifeType = LifeCycleTypes.Automatic

    End Sub

    Sub New()




    End Sub


    Public Overrides Function AgregarComponentesBloqueadosInicial() As List(Of WebControl)

        Dim lista_ As New List(Of WebControl)

        lista_.Add(icDireccionProveedor)

        lista_.Add(icIDFiscalProveedor)

        lista_.Add(icDireccionProveedor)

        lista_.Add(icIDFiscalProveedor)

        Return lista_

    End Function

    'ASIGNACION PARA CONTROLES AUTOMÁTICOS
    Public Overrides Function Configuracion() As TagWatcher


        'Datos Generales
        'Case SeccionesACUSEVALOR.SACUSEVALOR1

        [Set](scTipoDocumento, CP_TIPO_DOCUMENTO_ACUSEVALOR)

        '                                    Item(CamposFacturaComercial.CA_NUMERO_FACTURA, Texto, longitud_:=40),
        [Set](dbcNumFacturaAcuseValor, CA_NUMERO_FACTURA, propiedadDelControl_:=PropiedadesControl.Valor, esRequerido_:=True)
        '                                    Item(CamposACUSEVALOR.CA_NUMERO_ACUSEVALOR, Texto, longitud_:=40),
        [Set](dbcNumFacturaAcuseValor, CA_NUMERO_ACUSEVALOR, propiedadDelControl_:=PropiedadesControl.ValueDetail)
        '                                    Item(CamposFacturaComercial.CP_TIPO_OPERACION, Texto, longitud_:=11),
        [Set](IIf(swcTipoOperacion.Checked, "Importación", "Exportación"), CamposFacturaComercial.CP_TIPO_OPERACION)
        '                                    Item(CamposACUSEVALOR.CP_TIPO_DOCUMENTO_ACUSEVALOR, Texto, longitud_:=40),

        '                                    Item(CamposFacturaComercial.CA_MONEDA_FACTURACION, Texto, longitud_:=20),
        [Set](scTipoMoneda, CA_MONEDA_FACTURACION)
        '                                    Item(CamposFacturaComercial.CA_FECHA_FACTURA, Fecha),
        [Set](icFechaExpedicion, CA_FECHA_FACTURA)
        '                                    Item(CamposACUSEVALOR.CA_FECHA_ACUSEVALOR, Fecha),
        '                                    Item(CamposFacturaComercial.CA_APLICA_SUBDIVISION, Texto, longitud_:=3),
        [Set](IIf(swcSubdivision.Checked, "SÍ", "NO"), CamposFacturaComercial.CA_APLICA_SUBDIVISION)
        '                                    Item(CamposACUSEVALOR.CA_RELACION_FACTURA_ACUSEVALOR, Texto, longitud_:=100)
        [Set](IIf(swcRelacionFactura.Checked, "Sí", "NO"), CamposAcuseValor.CA_RELACION_FACTURA_ACUSEVALOR)
        '                                    Item(CamposFacturaComercial.CA_APLICA_CERTIFICADO, Texto, longitud_:=3),
        [Set](IIf(swcCertificadoOrigen.Checked, "SÍ", "NO"), CamposFacturaComercial.CA_APLICA_CERTIFICADO)
        '                                    Item(CamposACUSEVALOR.CA_NUMERO_EXPORTADOR_ACUSEVALOR, Texto, longitud_:=100),
        [Set](icExpotadorAutorizado, CA_NUMERO_EXPORTADOR_ACUSEVALOR)
        '                                    Item(CamposACUSEVALOR.CA_OBSERVACIONES_ACUSEVALOR, Texto, longitud_:=450)
        [Set](icObservaciones, CA_OBSERVACIONES_ACUSEVALOR)
        '                                    Item(CamposDomicilio.CA_DOMICILIO_FISCAL, Texto, longitud_:=450)
        '                                    Item(CamposDomicilio.CA_CALLE, Texto, longitud_:=80)

        'Datos del proveedor
        'Case SeccionesACUSEVALORl.SACUSEVALOR2
        '                                     Item(CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR, Texto, longitud_:=120)
        [Set](fbcProveedor, CamposAcuseValor.CA_RAZON_SOCIAL_EMISOR,, propiedadDelControl_:=PropiedadesControl.Text, esRequerido_:=True)

        ' [Set](fbcProveedor, CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR, propiedadDelControl_:=PropiedadesControl.Text)
        '                                     Item(CamposDomicilio.CA_DOMICILIO_FISCAL, Texto, longitud_:=450)
        ' [Set](icIDFiscalProveedor, CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR)

        [Set](icDireccionProveedor, seccion_:=SeccionesAcuseValor.SAcuseValor2, campo_:=CamposDomicilio.CA_DOMICILIO_FISCAL, esRequerido_:=True)
        '                                     Item(CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR, Texto, longitud_:=11)
        '                                     Item(CamposProveedorOperativo.CA_RFC_PROVEEDOR, Texto, longitud_:=13)
        '                                     Item(CamposDomicilio.CA_CALLE, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_NUMERO_EXTERIOR, Texto, longitud_:=10)
        '                                     Item(CamposDomicilio.CA_NUMERO_INTERIOR, Texto, longitud_:=10)
        '                                     Item(CamposDomicilio.CA_NUMERO_EXT_INT, Texto, longitud_:=20)
        '                                     Item(CamposDomicilio.CA_CODIGO_POSTAL, Texto, longitud_:=10)
        '                                     Item(CamposDomicilio.CA_COLONIA, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_LOCALIDAD, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_CIUDAD, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_MUNICIPIO, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA, Texto, longitud_:=3)
        '                                     Item(CamposDomicilio.CA_ENTIDAD_FEDERATIVA, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_CVE_PAIS, Texto, longitud_:=3)
        '                                     Item(CamposDomicilio.CA_PAIS, Texto, longitud_:=80)
        '                                     Item(CamposFacturaComercial.CA_CVE_VINCULACION, Entero)

        'Datos del destinatario
        'Case SeccionesACUSEVALORl.SACUSEVALOR3
        [Set](fbcDestinatario, CamposAcuseValor.CA_RAZON_SOCIAL_DESTINATARIO_ACUSE, propiedadDelControl_:=PropiedadesControl.Text, esRequerido_:=True)
        '                                     Item(CamposDomicilio.CA_DOMICILIO_FISCAL, Texto, longitud_:=450)
        '                                     Item(CamposDestinatario.CA_RAZON_SOCIAL, Texto, longitud_:=120)
        [Set](icDireccionDestinatario, seccion_:=SeccionesAcuseValor.SAcuseValor3, campo_:=CamposDomicilio.CA_DOMICILIO_FISCAL, esRequerido_:=True)
        '                                     Item(CamposDestinatario.CA_TAX_ID, Texto, longitud_:=11)
        '                                     Item(CamposDestinatario.CA_RFC_DESTINATARIO, Texto, longitud_:=13)
        '                                     Item(CamposDomicilio.CA_DOMICILIO_FISCAL, Texto, longitud_:=450)
        '                                     Item(CamposDomicilio.CA_CALLE, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_NUMERO_EXTERIOR, Texto, longitud_:=10)
        '                                     Item(CamposDomicilio.CA_NUMERO_INTERIOR, Texto, longitud_:=10)
        '                                     Item(CamposDomicilio.CA_CODIGO_POSTAL, Texto, longitud_:=10)
        '                                     Item(CamposDomicilio.CA_COLONIA, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_LOCALIDAD, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_CIUDAD, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_MUNICIPIO, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA, Texto, longitud_:=3)
        '                                     Item(CamposDomicilio.CA_ENTIDAD_FEDERATIVA, Texto, longitud_:=80)
        '                                     Item(CamposDomicilio.CA_CVE_PAIS, Texto, longitud_:=3)
        '                                     Item(CamposDomicilio.CA_PAIS, Texto, longitud_:=80)

        ' Partidas - Factura-ACUSEVALOR
        'Case SeccionesACUSEVALOR.SACUSEVALOR4
        '                                     Item(CamposFacturaComercial.CP_NUMERO_PARTIDA, Entero)
        '[Set](lbNumeroACUSEVALOR, CP_NUMERO_PARTIDA)
        If pbPartidasAcuseValor.PageIndex > 0 Then

            lbNumeroAcuseValor.Text = pbPartidasAcuseValor.PageIndex.ToString()

        End If

        '                                     Item(CamposACUSEVALOR.CA_DESCRIPCION_PARTIDA_ACUSEVALOR, Texto, longitud_:=250),

        [Set](icDescripcionAcuseValor, CA_DESCRIPCION_PARTIDA_ACUSEVALOR, propiedadDelControl_:=PropiedadesControl.Ninguno, esRequerido_:=True)
        '                                     Item(CamposFacturaComercial.CA_PRECIO_UNITARIO_PARTIDA, Real, cantidadEnteros_:=15, cantidadDecimales_:=5),
        [Set](icPrecioUnitarioAcuseValor, CA_PRECIO_UNITARIO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno, esRequerido_:=True)
        '                                     Item(CamposFacturaComercial.CA_CANTIDAD_COMERCIAL_PARTIDA, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
        [Set](icCantidadAcuseValor, CA_CANTIDAD_COMERCIAL_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno, esRequerido_:=True)
        '                                     Item(CamposACUSEVALOR.CA_UNIDAD_MEDIDA_FACTURA_PARTIDA_ACUSEVALOR, Texto, longitud_:=80),
        [Set](scUnidadAcuseValor, CA_UNIDAD_MEDIDA_FACTURA_PARTIDA_ACUSEVALOR, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '                                     Item(CamposFacturaComercial.CA_UNIDAD_MEDIDA_TARIFA_PARTIDA, Texto, longitud_:=3)
        '                                     Item(CamposFacturaComercial.CP_MONEDA_FACTURA_PARTIDA, Texto, longitud_:=80),
        [Set](scMonedaPrecioUnitarioPartida, CP_MONEDA_FACTURA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '                                     Item(CamposFacturaComercial.CA_VALOR_MERCANCIA_PARTIDA, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
        [Set](icValorFacturaPartida, CA_VALOR_MERCANCIA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno, esRequerido_:=True)
        '                                     Item(CamposACUSEVALOR.CA_VALOR_MERCANCIA_PARTIDA_DOLARES_ACUSEVALOR, Real, cantidadEnteros_:=18, cantidadDecimales_:=5),
        [Set](icValorDolaresPartida, CA_VALOR_MERCANCIA_PARTIDA_DOLARES_ACUSEVALOR, propiedadDelControl_:=PropiedadesControl.Ninguno, esRequerido_:=True)

        ' Partida - detalle mercancía
        '                                     Item(CamposFacturaComercial.CA_MARCA_PARTIDA, Texto, longitud_:=80),
        [Set](icMarcaAcuseValor, CA_MARCA_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '                                     Item(CamposFacturaComercial.CA_MODELO_PARTIDA, Texto, longitud_:=80),
        [Set](icModeloAcuseValor, CA_MODELO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '                                     Item(CamposFacturaComercial.CA_SUBMODELO_PARTIDA, Texto, longitud_:=80),
        [Set](icSubmodeloAcuseValor, CA_SUBMODELO_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)
        '                                     Item(CamposFacturaComercial.CA_NUMERO_SERIE_PARTIDA, Texto, longitud_:=80)
        [Set](icNumeroSerieAcuseValor, CA_NUMERO_SERIE_PARTIDA, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](pbPartidasAcuseValor, Nothing, seccion_:=SeccionesAcuseValor.SAcuseValor4)
        ' Configuración
        'Case SeccionesACUSEVALOR.SACUSEVALOR5
        '                                     Item(CamposACUSEVALOR.CA_SELLO_ACUSEVALOR, Real, cantidadEnteros_:=18, cantidadDecimales_:=5)
        [Set](scSelloCliente, CA_SELLO_ACUSEVALOR, esRequerido_:=True)
        '                                     Item(CamposACUSEVALOR.CA_RFC_CONSULTA_ACUSEVALOR, Texto, longitud_:=3)
        [Set](icPatenteAduanal, CA_PATENTE_ACUSEVALOR, esRequerido_:=True)

        '                                     Item(CamposFacturaComercial.CA_MONEDA_SEGUROS, Texto, longitud_:=3)

        '                                     Item(CamposFacturaComercial.CA_MONEDA_SEGUROS, Texto, longitud_:=3)

        [Set](icRFCConsulta, CA_RFC_CONSULTA_ACUSEVALOR, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icRazonSocialConsulta, CP_RAZON_SOCIAL_CONSULTA_ACUSEVALOR, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](icEmailConsulta, CP_EMAIL_CONSULTA_ACUSEVALOR, propiedadDelControl_:=PropiedadesControl.Ninguno)

        [Set](ccDatosConsulta, Nothing, seccion_:=SeccionesAcuseValor.SAcuseValor6)

        Return New TagWatcher(Ok)

    End Function

    Public Overrides Sub BotoneraClicNuevo()

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Esta metodo se manda llamar al dar clic en la opción nuevo (+) '
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''

        'Dim Instituciones_ As IControladorInstitucionBancaria = New ControladorInstitucionBancaria

        'Dim algo_ = Instituciones_.BuscarBancos(New Dictionary(Of IControladorInstitucionBancaria.CamposBusquedaSimple, Object) From {{IControladorInstitucionBancaria.CamposBusquedaSimple.CLAVEUSOBUSCAACTUALIZA, "jkhk"}}, IControladorInstitucionBancaria.Modalidades.Externo)

        swcTipoOperacion.Checked = True

        scTipoDocumento.Value = "1"

        icFechaExpedicion.Value = DateTime.UtcNow.Date.ToString("yyyy-MM-dd")

        'dbcNumFacturaAcuseValor.ValueDetail = "017025159NLV5"

        SetVars("_scTipoMoneda", scTipoMoneda)

        Dim listaSelectOption_ = New List(Of SelectOption)


        Dim monedas_ = _icontroladorMonedas.BuscarMonedas(New List(Of String) From {"USD",
                                                                                    "EUR",
                                                                                    "MXN",
                                                                                    "CHF",
                                                                                    "JPY",
                                                                                    "CNY"},,
                                                                                    "cveISO4217")



        listaSelectOption_ = _organismo.ObtenerSelectOption(scTipoMoneda,
                                                            monedas_.Select(Of ValorProvisionalOption)(Function(e) New ValorProvisionalOption With {
                            .Id = e._id,
                            .Valor = e.nombremonedaesp & " | " & e.aliasmoneda.Find(Function(ef) ef.Clave = "cveISO4217").Valor
                           }).ToList)


        SetVars("_Monedas", monedas_)

        scTipoMoneda.DataSource = listaSelectOption_

        scMonedaPrecioUnitarioPartida.DataSource = listaSelectOption_

        scTipoMoneda.Value = listaSelectOption_(0).Value

        scMonedaPrecioUnitarioPartida.Value = listaSelectOption_(0).Value

        listaSelectOption_ = _organismo.ObtenerSelectOption(scUnidadAcuseValor,
                                                            _controladorUnidadesMedida.
                                                            BuscarUnidadesCOVE(New List(Of String) From {"C62_1",
                                                                                                         "KGM",
                                                                                                         "CS",
                                                                                                         "SET",
                                                                                                         "C62_2",
                                                                                                         "KT",
                                                                                                         "TNE",
                                                                                                         "LM",
                                                                                                         "MIL",
                                                                                                         "MQ",
                                                                                                         "MTK",
                                                                                                         "BX",
                                                                                                         "LTR",
                                                                                                         "GRM"}).
                                                                                                         Select(Of ValorProvisionalOption)(Function(e) New ValorProvisionalOption With {
                                                                                                              .Id = e._id,
                                                                                                              .Valor = e.nombreoficialesp
                                                                                                          }).ToList)

        scUnidadAcuseValor.DataSource = listaSelectOption_

        scUnidadAcuseValor.Value = listaSelectOption_(0).Value

        MostrarFactor()

        'scTipoMoneda.ToolTip = "Factor: $" &
        '                         monedas_(0).factoresmoneda(0).valordefault &
        '                         " al " &
        '                         monedas_(0).factoresmoneda(0).fecha.ToString("dd-MM-yyyy")

        PreparaTarjetero(PillboxControl.ToolbarModality.Simple, pbPartidasAcuseValor)

        icDireccionDestinatario.Enabled = False

        icDireccionProveedor.Enabled = False

        icIDFiscalDestinatario.Enabled = False

        icIDFiscalProveedor.Enabled = False

        swcCertificadoOrigen.Enabled = True

        swcSubdivision.Enabled = True


        SetVars("prueba_", False)

    End Sub


    Public Overrides Sub BotoneraClicGuardar()

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Este método se manda llamar al dar clic en el boton Guardar                             '
        ' Llamamos el método "ProcesarTransaccion" pasando el tipo de nuestra clase constructora  '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim acuseValorFindBar_ As ConstructorAcuseValor = GetVars("_AcuseValorFindBar")

        Dim sinCambio_ As Boolean

        If acuseValorFindBar_ IsNot Nothing Then

            If acuseValorFindBar_.Seccion(SeccionesAcuseValor.SAcuseValor1).
                                  Attribute(CamposAcuseValor.CA_NUMERO_ACUSEVALOR).
                                  Valor Is Nothing Then

                sinCambio_ = True

            Else

                'DisplayMessage("Este Documento ya tenía el Número de AcuseValor -" &
                '               acuseValorFindBar_.Seccion(SeccionesAcuseValor.SAcuseValor1).
                '               Attribute(CamposAcuseValor.CA_NUMERO_ACUSEVALOR).Valor &
                '               "- Por lo que se procederá a hacer una Adenda", StatusMessage.Info)

            End If

        End If

        If Not ProcesarTransaccion(Of ConstructorAcuseValor)().Status = TypeStatus.Errors Then

            If OperacionGenerica IsNot Nothing Then

                '  _icontroladorAcuseValor.ActualizarIDS(OperacionGenerica.Id)

            End If



            '  OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Id = OperacionGenerica.Id.ToString

        End If

    End Sub

    Public Overrides Sub BotoneraClicPublicar()

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Esta metodo se manda llamar al dar clic en la opción Publicar '
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''

        With Buscador

            'Dim AcuseValorFindBar_ = GetVars("_AcuseValorFindBar")

            OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Id = OperacionGenerica.Id.ToString

            OperacionGenerica.Borrador.Folder.DocumentosAsociados = New List(Of DocumentoAsociado)



            Dim documentoElectronico_ As DocumentoElectronico = OperacionGenerica.
                                                                Borrador.
                                                                Folder.
                                                                ArchivoPrincipal.
                                                                Dupla.
                                                                Fuente

            If documentoElectronico_.Campo(CamposAcuseValor.CP_TIPO_SELLO).Valor = 0 Then

                'AQUI VA LO DE LA OBTENCIÓN DEL SELLO DEL AGENTE ADUANAL

            End If

            Dim idCLiente_ As ObjectId = documentoElectronico_.
                                         Campo(CamposAcuseValor.CP_ID_DESTINATARIO_ACUSE).
                                         Valor

            Using controladorDocumento_ = New ControladorDocumento

                Dim bulkCamposPedidos = _organismo.ObtenerCamposSeccionExterior(New List(Of ObjectId) From {idCLiente_},
                                                                             New ConstructorCliente, New Dictionary(Of [Enum],
                                                                             List(Of [Enum])) From {{SeccionesClientes.SCS5,
                                                                             New List(Of [Enum]) From {CamposClientes.CP_CONTRASENIA_SELLOS,
                                                                                                       CamposClientes.CP_CVE_WEB_SERVICES_SELLOS,
                                                                                                       CamposClientes.CP_RUTA_ARCHIVO_SER_SELLOS,
                                                                                                       CamposClientes.CP_RUTA_ARCHIVO_KEY_SELLOS}},
                                                                                                       {SeccionesClientes.SCS1,
                                                                             New List(Of [Enum]) From {CamposClientes.CA_RFC_CLIENTE}}})




                Dim certPassword_ = DirectCast(bulkCamposPedidos(idCLiente_).Item(0), Campo).Valor

                Dim webServicePassoword_ = DirectCast(bulkCamposPedidos(idCLiente_).Item(1), Campo).Valor

                Dim certByte_ As Byte() = controladorDocumento_.
                                          GetDocument(DirectCast(bulkCamposPedidos(idCLiente_).Item(2),
                                                                 Campo).
                                                                 Valor).
                                          ObjectReturned

                Dim keyByte_ As Byte() = controladorDocumento_.GetDocument(DirectCast(bulkCamposPedidos(idCLiente_).Item(3), Campo).Valor).ObjectReturned 'File.ReadAllBytes("C:\SAX\CSD_VUCEM_UMA011214255_20250411_122343.key") 

                Dim userName_ = DirectCast(bulkCamposPedidos(idCLiente_).Item(4), Campo).Valor

                _icontroladorAcuseValor.EnvironmentOnline = ListaEmpresas.Value


                Dim coveResult_ = _icontroladorAcuseValor.GenerarAcuseValor(OperacionGenerica.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente,
                                                                            certByte_,
                                                                            keyByte_,
                                                                            userName_,
                                                                            certPassword_,
                                                                            webServicePassoword_).
                                                           ObjectReturned

                If coveResult_.Contains("COVE") Then

                    dbcNumFacturaAcuseValor.ValueDetail = coveResult_

                    StatusPublicar = New TagWatcher With {.Status = TypeStatus.Ok}

                    DisplayMessage("SU ACUSE DE VALOR HA SIDO GENERADO", StatusMessage.Info)


                Else

                    StatusPublicar = New TagWatcher With {.Status = TypeStatus.Errors}

                    DisplayMessage(coveResult_, StatusMessage.Fail)

                End If

            End Using

        End With

    End Sub

    Public Overrides Sub BotoneraClicEditar()

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Esta metodo se manda llamar al dar clic en la opción Seguir Editando '
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        PreparaTarjetero(PillboxControl.ToolbarModality.Simple, pbPartidasAcuseValor)

        PreparaBotonera(FormControl.ButtonbarModality.Draft)

    End Sub

    Public Overrides Sub BotoneraClicBorrar()

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Esta metodo se manda llamar al dar clic en la opción Borrar'
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    End Sub

    Public Overrides Sub BotoneraClicArchivar()

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Esta metodo se manda llamar al dar clic en la opción Archivar '
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    End Sub

    Public Overrides Sub BotoneraClicOtros(ByVal IndexSelected_ As Integer)

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Esta metodo se manda llamar al dar clic en cualquiera de las opciones del      '
        ' dropdown en la botonera; recibe el valor indice del boton al que se le ha dado '
        ' clic                                                                           '
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        'Dim factormonedas_ As Dictionary(Of String, FactorMonedaPrincipal) = GetVars("_FactoresMonedas")
        'If factormonedas_ IsNot Nothing Then
        'DisplayMessage("Factor: $" & factormonedas_(scTipoMoneda.Value.ToString).valorfactor & " al " & factormonedas_(scTipoMoneda.Value.ToString).Fecha.ToString("dd-MM-yyyy"))
        'End If
        Select Case IndexSelected_

            Case 7

                If dbcNumFacturaAcuseValor.ValueDetail = "" Then

                    DisplayMessage("No hay Acuse de Valor generado para adendar", StatusMessage.Fail)

                Else

                    With Buscador

                        _icontroladorAcuseValor.EnvironmentOnline = ListaEmpresas.Value

                        Dim TagWatcher_ = _icontroladorAcuseValor.GenerarAcuseValor(GetVars("_AcuseValorFindBar"), "", "", "", "", True)

                        If TagWatcher_.ObjectReturned <> "" Then

                            DisplayMessage("SU ACUSE DE VALOR HA SIDO ADENDADO", StatusMessage.Info)

                            dbcNumFacturaAcuseValor.ValueDetail = TagWatcher_.ObjectReturned

                        End If

                    End With


                End If

            Case 8

                LimpiarTodo()


            Case 9

                Response.Redirect("/DescargarAcuseHandler.ashx?idacuseValor=" & OperacionGenerica.Id.ToString & "&acuseValor=" & dbcNumFacturaAcuseValor.ValueDetail & "&onlyXML=NO")

                DisplayMessage("Se ha descargado con éxito " & dbcNumFacturaAcuseValor.ValueDetail & ".pdf", StatusMessage.Info)

            Case 10

                If If(OperacionGenerica Is Nothing, "", OperacionGenerica.FolioOperacion) = "" Then

                    DisplayMessage("Debes seleccionar el acuse de valor a imprimir", StatusMessage.Fail)

                Else

                    Response.Redirect("/DescargarAcuseHandler.ashx?idacuseValor=" & OperacionGenerica.Id.ToString & "&acuseValor=" & dbcNumFacturaAcuseValor.ValueDetail & "&onlyXML=SI")

                    DisplayMessage("Se ha descargado con éxito " & dbcNumFacturaAcuseValor.ValueDetail & ".xml", StatusMessage.Info)

                End If

            Case 11

                If If(OperacionGenerica Is Nothing, "", OperacionGenerica.FolioOperacion) = "" Then

                    DisplayMessage("Debes seleccionar el acuse de valor a imprimir", StatusMessage.Fail)

                Else

                    DisplayMessage("Generando Representación Impresa")

                    Response.Redirect("/DescargarAcuseHandler.ashx?idacuseValor=" & OperacionGenerica.Id.ToString & "&representacionImpresa=SI&acuseValor=" & dbcNumFacturaAcuseValor.ValueDetail, False)

                    Context.ApplicationInstance.CompleteRequest()

                    'Dim url_ As String = "/DescargarHandler.ashx?idacuseValor=" & OperacionGenerica.Id.ToString & "&representacionImpresa=SI&acuseValor=" & dbcNumFacturaAcuseValor.ValueDetail

                    'ClientScript.RegisterStartupScript(
                    '    Me.GetType(),
                    '    "descargar",
                    '    "document.getElementById('frameDescarga').src='" & url_ & "';",
                    '    True
                    ')
                End If

            Case 12

                'Dim algo_ = _organismo.SeparacionPalabras("TECNIFOS S.A.", "", "", "", "itext")

                Dim valor_ = fbcProveedor.Value

                _icontroladorMonedas = New ControladorMonedas

                Dim monedilla_ As ObjectId? = ObjectId.Parse("635acf21a8210bfa0d5842c0")

                Dim algo = _icontroladorMonedas.BuscarMonedas("", monedilla_)




                Dim ivucemAxtions_ As IVUCEMActions = New VUCEMActions

                Dim fileName_ = dbcNumFacturaAcuseValor.Value.Substring(dbcNumFacturaAcuseValor.Value.LastIndexOf("\") + 1)

                Dim algo_ = ivucemAxtions_.SubmitEdocument(fileName_,
                                           File.ReadAllBytes(dbcNumFacturaAcuseValor.Value),
                                           "sergio.flores@kromaduanal.com",
                                           170,
                                           "FOAS820112T75",
                                           ObjectId.Parse("6990a55d645351e24006d713"))

                'Dim algo_ = "314214971"
                'Dim algo2_ = ivucemAxtions_.StatusEdocument(algo_,
                '               ObjectId.Parse("68c30d46cd76f8bd10400077"))




                'Dim algo_ = "01702618CW9B123"
                'Dim algo2_ = ivucemAxtions_.GetAcknowledgmentEdocumentPDFAPI(algo_,
                '               ObjectId.Parse("68c30d46cd76f8bd10400077"))

                'Dim fileName_ = dbcNumFacturaAcuseValor.Value.Substring(dbcNumFacturaAcuseValor.Value.LastIndexOf("\") + 1)

                'Dim algo_ = ivucemAxtions_.GetAuthoritEdocumentXml(fileName_,
                '                           File.ReadAllBytes(dbcNumFacturaAcuseValor.Value),
                '                           "sergio.flores@kromaduanal.com",
                '                           170,
                '                           "FOAS820112T75",
                '                           ObjectId.Parse("68c30d46cd76f8bd10400077"))


                Dim x = 5

        End Select

    End Sub

    'EVENTOS PARA LA INSERCIÓN DE DATOS
    Public Overrides Function AntesRealizarInsercion(ByVal session_ As IClientSessionHandle) As TagWatcher

        Dim tagwatcher_ As TagWatcher

        '      ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas con transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒
        If session_ IsNot Nothing Then

            '  ██████inicio███████        Logica de negocios local
            '████████████████████████
            Dim idFactura_ = GetVars("IDS")

            If idFactura_ Is Nothing Then

                idFactura_ = ""

            End If

            If idFactura_.ToString = "" Then

                _icontroladorFactura.EnvironmentOnline = ListaEmpresas.Value

                tagwatcher_ = _icontroladorFactura.
                              ListaCamposFacturaComercial(dbcNumFacturaAcuseValor.Value.ToString,
                                                          New Dictionary(Of [Enum], List(Of [Enum])) _
                                                          From {{SeccionesFacturaComercial.SFAC1,
                                                          New List(Of [Enum]) From
                                                          {CamposFacturaComercial.CA_NUMERO_FACTURA}}})

                Dim resultado_ As Dictionary(Of String, List(Of Nodo)) = tagwatcher_.ObjectReturned

                If resultado_ IsNot Nothing Then

                    If resultado_(dbcNumFacturaAcuseValor.Value.ToString).Count > 0 Then

                        idFactura_ = DirectCast(resultado_(dbcNumFacturaAcuseValor.Value.ToString).
                                                       Item(1),
                                                       Campo).
                                                       Valor

                    Else

                        idFactura_ = ObjectId.GenerateNewId.ToString

                    End If



                Else



                End If

            End If

            If idFactura_.ToString <> "" Then

                [Set](New ObjectId(idFactura_.ToString), CP_ID_FACTURA_ACUSEVALOR)


            End If

            If fbcProveedor.Value <> "" Then

                [Set](New ObjectId(fbcProveedor.Value), CP_ID_EMISOR)

            End If

            If fbcDestinatario.Value <> "" Then

                [Set](New ObjectId(fbcDestinatario.Value), CP_ID_DESTINATARIO_ACUSE)

            End If


            [Set](ObjectId.GenerateNewId, CP_ID_ACUSEVALOR)

            'Dim loginUsuario_ As Dictionary(Of String, String) = Session("DatosUsuario")

            '[Set](loginUsuario_("WebServiceUserID"), CP_EMAIL_CONSULTA_ACUSEVALOR)

            'pb_PartidasCOVE.DeletePillbox()
            '  ████████fin█████████       Logica de negocios local       ███████████████████████

            tagwatcher_ = New TagWatcher

            tagwatcher_.SetOK()


        Else  '▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas sin transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ 

            tagwatcher_ = New TagWatcher

            tagwatcher_.SetOK()

        End If

        Return tagwatcher_


    End Function

    Public Overrides Function RealizarInsercion_ProcesoInterno(ByRef documentoElectronico_ As DocumentoElectronico) As TagWatcher

        Dim controladorSecuencias_ As New ControladorSecuencia

        Dim tagwatcher_ As TagWatcher = controladorSecuencias_.Generar(SecuenciasComercioExterior.AcusesValor.ToString, 1, 1, 1, 1, Statements.GetOfficeOnline()._id)
        Dim secuencia_ As Rec.Globals.Utils.Secuencia = DirectCast(tagwatcher_.ObjectReturned, Rec.Globals.Utils.Secuencia)
        'With documentoElectronico_
        '    .Campo(CamposFacturaComercial.CP_TIPO_OPERACION).Valor = 2
        '    .Campo(CamposFacturaComercial.CP_TIPO_OPERACION).ValorPresentacion = "Exportacion"
        '    If lbModoCapturaIA.Visible = True Then
        '        .Campo(CP_TIPO_CARGA_DATOS).Valor = 1
        '        .Campo(CP_TIPO_CARGA_DATOS).ValorPresentacion = "Carga IA"
        '    Else
        '        .Campo(CP_TIPO_CARGA_DATOS).Valor = 2
        '        .Campo(CP_TIPO_CARGA_DATOS).ValorPresentacion = "Carga manual"
        '    End If
        '    .UsuarioGenerador = loginUsuario_("Nombre")
        '    .Id = secuencia_._id.ToString
        '    .IdDocumento = secuencia_.sec
        '    .FolioDocumento = dbcNumFacturaAcuseValor.Value
        '    .FolioOperacion = secuencia_.sec
        '    .TipoPropietario = SecuenciasComercioExterior.FacturasComerciales.ToString
        '    .NombrePropietario = fbcCliente.Text
        '    .IdPropietario = datosCliente_("cveEmpresaCliente") 'se debe agregar desde el cliente
        '    .ObjectIdPropietario = New ObjectId(fbcCliente.Value)
        'End With

        'Dim respuesta_ As TagWatcher = secuencia_.Generar().Result

        Dim documentoElectronicoaux_ As DocumentoElectronico

        If swcTipoOperacion.Checked Then

            Dim proveedores_ As List(Of AuxiliarProveedor) = GetVars("_proveedores")


            Dim proveedor_ = proveedores_.Find(Function(ch) ch.idtaxid = fbcProveedor.Value)

            With documentoElectronico_

                .FolioDocumento = dbcNumFacturaAcuseValor.Value

                With .Seccion(SeccionesAcuseValor.SAcuseValor2)

                    If proveedor_ IsNot Nothing Then

                        .Campo(CamposAcuseValor.CP_ID_EMISOR).Valor = ObjectId.Parse(proveedor_.idtaxid)


                        If proveedor_._taxid <> "" Then

                            .Campo(CamposAcuseValor.CA_TAX_ID_EMISOR).Valor = proveedor_._taxid

                            .Campo(CA_TIPO_IDENTIFICADOR_EMISOR).Valor = 0

                        End If

                        If proveedor_._rfc <> "" Then

                            .Campo(CamposAcuseValor.CA_RFC_EMISOR).Valor = proveedor_._rfc

                            .Campo(CA_TIPO_IDENTIFICADOR_EMISOR).Valor = 1

                        End If

                        If proveedor_._domicilio IsNot Nothing Then

                            .Campo(CamposDomicilio.CP_ID_DOMICILIO).Valor = proveedor_._domicilio._iddomicilio

                            .Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor = proveedor_._domicilio.domicilioPresentacion

                            .Campo(CamposDomicilio.CA_CALLE).Valor = proveedor_._domicilio.calle

                            .Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor = proveedor_._domicilio.numeroexterior

                            .Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor = proveedor_._domicilio.numerointerior

                            .Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor = proveedor_._domicilio.codigopostal

                            .Campo(CamposDomicilio.CA_COLONIA).Valor = proveedor_._domicilio.colonia

                            .Campo(CamposDomicilio.CA_LOCALIDAD).Valor = proveedor_._domicilio.localidad

                            .Campo(CamposDomicilio.CA_CIUDAD).Valor = proveedor_._domicilio.ciudad

                            .Campo(CamposDomicilio.CA_MUNICIPIO).Valor = proveedor_._domicilio.municipio

                            .Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor = proveedor_._domicilio.cveEntidadfederativa

                            .Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor = proveedor_._domicilio.entidadfederativa


                            .Campo(CamposDomicilio.CA_CVE_PAIS).Valor = proveedor_._cvepais

                            Dim pais_ = If(proveedor_._pais, "")

                            If pais_ = "" Then

                                Dim paises_ As New List(Of Pais)

                                ControladorPaises.BuscarPaises(paises_, proveedor_._cvepais)

                                pais_ = paises_(0).nombrepaisesp

                            End If

                            .Campo(CamposDomicilio.CA_PAIS).Valor = pais_

                        End If

                    End If

                End With

                documentoElectronicoaux_ = GetVars("_ClienteAcuseValor")

                With .Seccion(SeccionesAcuseValor.SAcuseValor3)

                    If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposClientes.CA_TAX_ID).Valor IsNot Nothing Then

                        .Campo(CamposAcuseValor.CA_TAX_ID_DESTINATARIO_ACUSE).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposClientes.CA_TAX_ID).Valor

                        .Campo(CamposAcuseValor.CA_TIPO_IDENTIFICADOR_DESTINATARIO_ACUSE).Valor = 0
                    End If

                    If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposClientes.CA_RFC_CLIENTE).Valor IsNot Nothing Then

                        .Campo(CamposAcuseValor.CA_RFC_DESTINATARIO_ACUSE).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposClientes.CA_RFC_CLIENTE).Valor

                        .Campo(CamposAcuseValor.CA_TIPO_IDENTIFICADOR_DESTINATARIO_ACUSE).Valor = 1

                    End If

                    If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor IsNot Nothing Then

                        .Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor

                        .Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).ValorPresentacion = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).ValorPresentacion

                    End If

                    If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CALLE).Valor IsNot Nothing Then

                        .Campo(CamposDomicilio.CA_CALLE).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CALLE).Valor

                    End If

                    If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor IsNot Nothing Then

                        .Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor

                    End If

                    If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor IsNot Nothing Then

                        .Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor

                    End If

                    If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_NUMERO_EXT_INT).Valor IsNot Nothing Then

                        .Campo(CamposDomicilio.CA_NUMERO_EXT_INT).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_NUMERO_EXT_INT).Valor

                    End If

                    If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor IsNot Nothing Then

                        .Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor

                    End If


                    If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_COLONIA).Valor IsNot Nothing Then

                        .Campo(CamposDomicilio.CA_COLONIA).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_COLONIA).Valor

                    End If

                    If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_LOCALIDAD).Valor IsNot Nothing Then

                        .Campo(CamposDomicilio.CA_LOCALIDAD).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_LOCALIDAD).Valor

                    End If

                    If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_LOCALIDAD).Valor IsNot Nothing Then

                        .Campo(CamposDomicilio.CA_CIUDAD).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CIUDAD).Valor

                    End If

                    If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_MUNICIPIO).Valor IsNot Nothing Then

                        .Campo(CamposDomicilio.CA_MUNICIPIO).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_MUNICIPIO).Valor

                    End If

                    If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor IsNot Nothing Then

                        .Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor

                    End If

                    Dim idEntidadFederativaCliente_ As ObjectId

                    If ObjectId.TryParse(documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor, idEntidadFederativaCliente_) Then

                        .Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).ValorPresentacion
                    Else

                        .Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor

                    End If


                    If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CVE_PAIS).Valor IsNot Nothing Then

                        .Campo(CamposDomicilio.CA_CVE_PAIS).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CVE_PAIS).Valor

                    End If

                    If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_PAIS).Valor IsNot Nothing Then

                        .Campo(CamposDomicilio.CA_PAIS).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_PAIS).Valor

                    End If

                End With

                'DE MOMENTO documentoElectronicoaux_ con tiene el ID del Cliente para estas pruebas de importación

                If scSelloCliente.Value = "0" Then

                    .Campo(CamposAcuseValor.CP_ID_AGENTE_SELLO).Valor = ObjectId.Parse(documentoElectronicoaux_.Id)

                    .Campo(CamposAcuseValor.CP_TIPO_SELLO).Valor = 0


                Else

                    .Campo(CamposAcuseValor.CP_ID_CLIENTE_SELLO).Valor = ObjectId.Parse(scSelloCliente.Value)

                    .Campo(CamposAcuseValor.CP_TIPO_SELLO).Valor = 1

                End If

                .NombreCliente = documentoElectronicoaux_.NombreCliente

                With .Attribute(CamposFacturaComercial.CA_MONEDA_FACTURACION)

                    .Valor = scTipoMoneda.Value

                    .ValorPresentacion = scTipoMoneda.Text

                End With

                .IdDocumento = secuencia_.sec

                .IdCliente = documentoElectronicoaux_.IdCliente

                .FolioOperacion = secuencia_.sec

                .ObjectIdPropietario = New ObjectId(fbcDestinatario.Value)

                Dim rfcCliente_ As String

                Dim tipoFigura_ As Integer

                If swcTipoOperacion.Checked Then

                    rfcCliente_ = icIDFiscalDestinatario.Value

                    tipoFigura_ = 5

                Else

                    rfcCliente_ = icIDFiscalProveedor.Value

                    tipoFigura_ = 4

                End If

                If scSelloCliente.Text = rfcCliente_ Then

                    .Campo(CamposAcuseValor.CA_TIPO_FIGURA).Valor = tipoFigura_

                Else

                    .Campo(CamposAcuseValor.CA_TIPO_FIGURA).Valor = 1

                End If





            End With

        Else

            'Dim proveedores_ As List(Of AuxiliarProveedor) = GetVars("_proveedores")


            'Dim proveedor_ = proveedores_.Find(Function(ch) ch.id = fbcProveedor.Value)

            With documentoElectronico_

                .FolioDocumento = dbcNumFacturaAcuseValor.Value

                documentoElectronicoaux_ = GetVars("_ClienteAcuseValor")

                With .Seccion(SeccionesAcuseValor.SAcuseValor2)

                    If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposClientes.CA_TAX_ID).Valor IsNot Nothing Then

                        .Campo(CamposAcuseValor.CA_TAX_ID_EMISOR).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposClientes.CA_TAX_ID).Valor

                        .Campo(CamposAcuseValor.CA_TIPO_IDENTIFICADOR_EMISOR).Valor = 0
                    End If

                    If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposClientes.CA_RFC_CLIENTE).Valor IsNot Nothing Then

                        .Campo(CamposAcuseValor.CA_RFC_EMISOR).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposClientes.CA_RFC_CLIENTE).Valor

                        .Campo(CamposAcuseValor.CA_TIPO_IDENTIFICADOR_EMISOR).Valor = 1

                    End If

                    If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor IsNot Nothing Then

                        .Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor

                        .Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).ValorPresentacion = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).ValorPresentacion

                    End If

                    If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CALLE).Valor IsNot Nothing Then

                        .Campo(CamposDomicilio.CA_CALLE).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CALLE).Valor

                    End If

                    If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor IsNot Nothing Then

                        .Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor

                    End If

                    If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor IsNot Nothing Then

                        .Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor

                    End If

                    If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_NUMERO_EXT_INT).Valor IsNot Nothing Then

                        .Campo(CamposDomicilio.CA_NUMERO_EXT_INT).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_NUMERO_EXT_INT).Valor

                    End If

                    If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor IsNot Nothing Then

                        .Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor

                    End If


                    If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_COLONIA).Valor IsNot Nothing Then

                        .Campo(CamposDomicilio.CA_COLONIA).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_COLONIA).Valor

                    End If

                    If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_LOCALIDAD).Valor IsNot Nothing Then

                        .Campo(CamposDomicilio.CA_LOCALIDAD).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_LOCALIDAD).Valor

                    End If

                    If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_LOCALIDAD).Valor IsNot Nothing Then

                        .Campo(CamposDomicilio.CA_CIUDAD).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CIUDAD).Valor

                    End If

                    If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_MUNICIPIO).Valor IsNot Nothing Then

                        .Campo(CamposDomicilio.CA_MUNICIPIO).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_MUNICIPIO).Valor

                    End If

                    If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor IsNot Nothing Then

                        .Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor

                    End If

                    Dim idEntidadFederativaCliente_ As ObjectId

                    If ObjectId.TryParse(documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor, idEntidadFederativaCliente_) Then

                        .Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).ValorPresentacion
                    Else

                        .Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor

                    End If

                    If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CVE_PAIS).Valor IsNot Nothing Then

                        .Campo(CamposDomicilio.CA_CVE_PAIS).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CVE_PAIS).Valor

                    End If

                    If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_PAIS).Valor IsNot Nothing Then

                        .Campo(CamposDomicilio.CA_PAIS).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_PAIS).Valor

                    End If

                End With

                Dim proveedores_ As List(Of AuxiliarProveedor) = GetVars("_proveedores")


                Dim proveedor_ = proveedores_.Find(Function(ch) ch.id = fbcDestinatario.Value)

                With .Seccion(SeccionesAcuseValor.SAcuseValor3)

                    If proveedor_ IsNot Nothing Then

                        .Campo(CamposAcuseValor.CP_ID_DESTINATARIO_ACUSE).Valor = ObjectId.Parse(proveedor_.id)


                        If proveedor_._taxid <> "" Then

                            .Campo(CamposAcuseValor.CA_TAX_ID_DESTINATARIO_ACUSE).Valor = proveedor_._taxid

                            .Campo(CA_TIPO_IDENTIFICADOR_DESTINATARIO_ACUSE).Valor = 0

                        End If

                        If proveedor_._rfc <> "" Then

                            .Campo(CamposAcuseValor.CA_RFC_DESTINATARIO_ACUSE).Valor = proveedor_._rfc

                            .Campo(CA_TIPO_IDENTIFICADOR_DESTINATARIO_ACUSE).Valor = 1

                        End If

                        If proveedor_._domicilio IsNot Nothing Then

                            .Campo(CamposDomicilio.CP_ID_DOMICILIO).Valor = proveedor_._domicilio._iddomicilio

                            .Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor = proveedor_._domicilio.domicilioPresentacion

                            .Campo(CamposDomicilio.CA_CALLE).Valor = proveedor_._domicilio.calle

                            .Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor = proveedor_._domicilio.numeroexterior

                            .Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor = proveedor_._domicilio.numerointerior

                            .Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor = proveedor_._domicilio.codigopostal

                            .Campo(CamposDomicilio.CA_COLONIA).Valor = proveedor_._domicilio.colonia

                            .Campo(CamposDomicilio.CA_LOCALIDAD).Valor = proveedor_._domicilio.localidad

                            .Campo(CamposDomicilio.CA_CIUDAD).Valor = proveedor_._domicilio.ciudad

                            .Campo(CamposDomicilio.CA_MUNICIPIO).Valor = proveedor_._domicilio.municipio

                            .Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor = proveedor_._domicilio.cveEntidadfederativa

                            .Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor = proveedor_._domicilio.entidadfederativa

                            .Campo(CamposDomicilio.CA_CVE_PAIS).Valor = proveedor_._cvepais

                            Dim pais_ = If(proveedor_._pais, "")

                            If pais_ = "" Then

                                Dim paises_ As New List(Of Pais)

                                ControladorPaises.BuscarPaises(paises_, proveedor_._cvepais)

                                pais_ = paises_(0).nombrepaisesp

                            End If

                            .Campo(CamposDomicilio.CA_PAIS).Valor = pais_

                        End If

                    End If

                End With

                'DE MOMENTO documentoElectronicoaux_ con tiene el ID del Cliente para estas pruebas de importación

                If scSelloCliente.Value = "0" Then

                    .Campo(CamposAcuseValor.CP_ID_AGENTE_SELLO).Valor = ObjectId.Parse(documentoElectronicoaux_.Id)

                    .Campo(CamposAcuseValor.CP_TIPO_SELLO).Valor = 0


                Else

                    .Campo(CamposAcuseValor.CP_ID_CLIENTE_SELLO).Valor = ObjectId.Parse(scSelloCliente.Value)

                    .Campo(CamposAcuseValor.CP_TIPO_SELLO).Valor = 1

                End If

                .NombreCliente = documentoElectronicoaux_.NombreCliente

                With .Attribute(CamposFacturaComercial.CA_MONEDA_FACTURACION)

                    .Valor = scTipoMoneda.Value

                    .ValorPresentacion = scTipoMoneda.Text

                End With

                .IdDocumento = secuencia_.sec

                .IdCliente = documentoElectronicoaux_.IdCliente

                .FolioOperacion = secuencia_.sec

                .ObjectIdPropietario = New ObjectId(fbcDestinatario.Value)

                Dim rfcCliente_ As String

                Dim tipoFigura_ As Integer

                If swcTipoOperacion.Checked Then

                    rfcCliente_ = icIDFiscalDestinatario.Value

                    tipoFigura_ = 5

                Else

                    rfcCliente_ = icIDFiscalProveedor.Value

                    tipoFigura_ = 4

                End If

                If scSelloCliente.Text = rfcCliente_ Then

                    .Campo(CamposAcuseValor.CA_TIPO_FIGURA).Valor = tipoFigura_

                Else

                    .Campo(CamposAcuseValor.CA_TIPO_FIGURA).Valor = 1

                End If

            End With

        End If

        SetVars("_AcuseValorFindBar", New ConstructorAcuseValor(True, documentoElectronico_))

        Return New TagWatcher(Ok)
    End Function

    Public Overrides Function DespuesRealizarInsercion() As TagWatcher




        __SYSTEM_MODULE_FORM.Modality = FormControl.ButtonbarModality.Draft
        'PreparaTarjetero(PillboxControl.ToolbarModality.Simple, pbPartidasAcuseValor)
        Return New TagWatcher(Ok)

    End Function


    'EVENTOS PARA MODIFICACIÓN DE DATOS
    Public Overrides Function AntesRealizarModificacion(ByVal session_ As IClientSessionHandle) As TagWatcher

        Dim tagwatcher_ As TagWatcher

        '     ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas con transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒

        If session_ IsNot Nothing Then

            '  ██████inicio███████        Logica de negocios local      ████████████████████████

            tagwatcher_ = New TagWatcher

            tagwatcher_.SetOK()


        Else  '▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ Operaciones atómicas sin transacción ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ 

            tagwatcher_ = New TagWatcher

            tagwatcher_.SetOK()

        End If

        Return tagwatcher_

    End Function

    Public Overrides Function RealizarModificacion_ProcesoInterno(ByRef documentoElectronico_ As DocumentoElectronico) As TagWatcher

        With documentoElectronico_

            If .Campo(CamposAcuseValor.CP_ID_AGENTE_SELLO).Valor <> scSelloCliente.Value Then

                If scSelloCliente.Value = "0" Then

                    .Campo(CamposAcuseValor.CP_ID_AGENTE_SELLO).Valor = ObjectId.Parse(scSelloCliente.Value)

                    .Campo(CamposAcuseValor.CP_TIPO_SELLO).Valor = 0


                Else

                    .Campo(CamposAcuseValor.CP_ID_CLIENTE_SELLO).Valor = ObjectId.Parse(scSelloCliente.Value)

                    .Campo(CamposAcuseValor.CP_TIPO_SELLO).Valor = 1


                End If

            End If


        End With

        Dim documentoElectronicoaux_ As DocumentoElectronico

        If swcTipoOperacion.Checked Then

            Dim razonSocoialProvedor_ As String = documentoElectronico_.Campo(CamposAcuseValor.CA_RAZON_SOCIAL_EMISOR).Valor

            razonSocoialProvedor_ = razonSocoialProvedor_.Split("|")(0)

            Dim proveedores_ As List(Of AuxiliarProveedor) = _controladorProveedor.ObtenerProveedoresPorRazonSocialPais(razonSocoialProvedor_).ObjectReturned


            Dim proveedor_ = proveedores_.Find(Function(ch) ch._razonsocial.Contains(fbcProveedor.Text))

            With documentoElectronico_

                .FolioDocumento = dbcNumFacturaAcuseValor.Value

                With .Seccion(SeccionesAcuseValor.SAcuseValor2)

                    If proveedor_ IsNot Nothing Then

                        .Campo(CamposAcuseValor.CP_ID_EMISOR).Valor = ObjectId.Parse(proveedor_.idtaxid)


                        If proveedor_._taxid <> "" Then

                            .Campo(CamposAcuseValor.CA_TAX_ID_EMISOR).Valor = proveedor_._taxid

                            .Campo(CA_TIPO_IDENTIFICADOR_EMISOR).Valor = 0

                        End If

                        If proveedor_._rfc <> "" Then

                            .Campo(CamposAcuseValor.CA_RFC_EMISOR).Valor = proveedor_._rfc

                            .Campo(CA_TIPO_IDENTIFICADOR_EMISOR).Valor = 1

                        End If

                        If proveedor_._domicilio IsNot Nothing Then

                            .Campo(CamposDomicilio.CP_ID_DOMICILIO).Valor = proveedor_._domicilio._iddomicilio

                            .Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor = proveedor_._domicilio.domicilioPresentacion

                            .Campo(CamposDomicilio.CA_CALLE).Valor = proveedor_._domicilio.calle

                            .Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor = proveedor_._domicilio.numeroexterior

                            .Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor = proveedor_._domicilio.numerointerior

                            .Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor = proveedor_._domicilio.codigopostal

                            .Campo(CamposDomicilio.CA_COLONIA).Valor = proveedor_._domicilio.colonia

                            .Campo(CamposDomicilio.CA_LOCALIDAD).Valor = proveedor_._domicilio.localidad

                            .Campo(CamposDomicilio.CA_CIUDAD).Valor = proveedor_._domicilio.ciudad

                            .Campo(CamposDomicilio.CA_MUNICIPIO).Valor = proveedor_._domicilio.municipio

                            .Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor = proveedor_._domicilio.cveEntidadfederativa

                            .Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor = proveedor_._domicilio.entidadfederativa


                            .Campo(CamposDomicilio.CA_CVE_PAIS).Valor = proveedor_._cvepais

                            Dim pais_ = If(proveedor_._pais, "")

                            If pais_ = "" Then

                                Dim paises_ As New List(Of Pais)

                                ControladorPaises.BuscarPaises(paises_, proveedor_._cvepais)

                                pais_ = paises_(0).nombrepaisesp

                            End If

                            .Campo(CamposDomicilio.CA_PAIS).Valor = pais_

                        End If

                    End If

                End With

                Dim idCliente_ As ObjectId = documentoElectronico_.Campo(CamposAcuseValor.CP_ID_DESTINATARIO_ACUSE).Valor

                Dim buscarCliente_ As New ControladorBusqueda(Of ConstructorCliente)

                Dim tagwatcherCliente_ As TagWatcher = buscarCliente_.ObtenerDocumento(idCliente_.ToString)

                If tagwatcherCliente_.Status = TypeStatus.Ok Then

                    Dim clienteAcuseValor_ As ConstructorCliente = DirectCast(tagwatcherCliente_.ObjectReturned.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente, ConstructorCliente)

                    documentoElectronicoaux_ = clienteAcuseValor_

                    With .Seccion(SeccionesAcuseValor.SAcuseValor3)

                        If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposClientes.CA_TAX_ID).Valor IsNot Nothing Then

                            .Campo(CamposAcuseValor.CA_TAX_ID_DESTINATARIO_ACUSE).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposClientes.CA_TAX_ID).Valor

                            .Campo(CamposAcuseValor.CA_TIPO_IDENTIFICADOR_DESTINATARIO_ACUSE).Valor = 0
                        End If

                        If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposClientes.CA_RFC_CLIENTE).Valor IsNot Nothing Then

                            .Campo(CamposAcuseValor.CA_RFC_DESTINATARIO_ACUSE).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposClientes.CA_RFC_CLIENTE).Valor

                            .Campo(CamposAcuseValor.CA_TIPO_IDENTIFICADOR_DESTINATARIO_ACUSE).Valor = 1

                        End If

                        If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor IsNot Nothing Then

                            .Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor

                            .Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).ValorPresentacion = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).ValorPresentacion

                        End If

                        If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CALLE).Valor IsNot Nothing Then

                            .Campo(CamposDomicilio.CA_CALLE).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CALLE).Valor

                        End If

                        If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor IsNot Nothing Then

                            .Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor

                        End If

                        If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor IsNot Nothing Then

                            .Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor

                        End If

                        If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_NUMERO_EXT_INT).Valor IsNot Nothing Then

                            .Campo(CamposDomicilio.CA_NUMERO_EXT_INT).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_NUMERO_EXT_INT).Valor

                        End If

                        If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor IsNot Nothing Then

                            .Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor

                        End If


                        If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_COLONIA).Valor IsNot Nothing Then

                            .Campo(CamposDomicilio.CA_COLONIA).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_COLONIA).Valor

                        End If

                        If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_LOCALIDAD).Valor IsNot Nothing Then

                            .Campo(CamposDomicilio.CA_LOCALIDAD).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_LOCALIDAD).Valor

                        End If

                        If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_LOCALIDAD).Valor IsNot Nothing Then

                            .Campo(CamposDomicilio.CA_CIUDAD).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CIUDAD).Valor

                        End If

                        If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_MUNICIPIO).Valor IsNot Nothing Then

                            .Campo(CamposDomicilio.CA_MUNICIPIO).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_MUNICIPIO).Valor

                        End If

                        If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor IsNot Nothing Then

                            .Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor

                        End If

                        Dim idEntidadFederativaCliente_ As ObjectId

                        If ObjectId.TryParse(documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor, idEntidadFederativaCliente_) Then

                            .Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).ValorPresentacion
                        Else

                            .Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor

                        End If


                        If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CVE_PAIS).Valor IsNot Nothing Then

                            .Campo(CamposDomicilio.CA_CVE_PAIS).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CVE_PAIS).Valor

                        End If

                        If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_PAIS).Valor IsNot Nothing Then

                            .Campo(CamposDomicilio.CA_PAIS).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_PAIS).Valor

                        End If

                    End With

                End If


                'DE MOMENTO documentoElectronicoaux_ con tiene el ID del Cliente para estas pruebas de importación

                If scSelloCliente.Value = "0" Then

                    If documentoElectronicoaux_ IsNot Nothing Then

                        .Campo(CamposAcuseValor.CP_ID_AGENTE_SELLO).Valor = ObjectId.Parse(documentoElectronicoaux_.Id)

                    End If



                    .Campo(CamposAcuseValor.CP_TIPO_SELLO).Valor = 0


                Else

                    .Campo(CamposAcuseValor.CP_ID_CLIENTE_SELLO).Valor = ObjectId.Parse(scSelloCliente.Value)

                    .Campo(CamposAcuseValor.CP_TIPO_SELLO).Valor = 1

                End If

                .NombreCliente = documentoElectronicoaux_.NombreCliente

                With .Attribute(CamposFacturaComercial.CA_MONEDA_FACTURACION)

                    .Valor = scTipoMoneda.Value

                    .ValorPresentacion = scTipoMoneda.Text

                End With



                Dim rfcCliente_ As String

                Dim tipoFigura_ As Integer

                If swcTipoOperacion.Checked Then

                    rfcCliente_ = icIDFiscalDestinatario.Value

                    tipoFigura_ = 5

                Else

                    rfcCliente_ = icIDFiscalProveedor.Value

                    tipoFigura_ = 4

                End If

                If scSelloCliente.Text = rfcCliente_ Then

                    .Campo(CamposAcuseValor.CA_TIPO_FIGURA).Valor = tipoFigura_

                Else

                    .Campo(CamposAcuseValor.CA_TIPO_FIGURA).Valor = 1

                End If





            End With

        Else

            'Dim proveedores_ As List(Of AuxiliarProveedor) = GetVars("_proveedores")


            'Dim proveedor_ = proveedores_.Find(Function(ch) ch.id = fbcProveedor.Value)

            With documentoElectronico_

                .FolioDocumento = dbcNumFacturaAcuseValor.Value

                Dim idCliente_ As ObjectId = documentoElectronico_.Campo(CamposAcuseValor.CP_ID_EMISOR).Valor

                Dim buscarCliente_ As New ControladorBusqueda(Of ConstructorCliente)

                Dim tagwatcherCliente_ As TagWatcher = buscarCliente_.ObtenerDocumento(idCliente_.ToString)

                If tagwatcherCliente_.Status = TypeStatus.Ok Then

                    Dim clienteAcuseValor_ As ConstructorCliente = DirectCast(tagwatcherCliente_.ObjectReturned.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente, ConstructorCliente)

                    documentoElectronicoaux_ = clienteAcuseValor_

                    With .Seccion(SeccionesAcuseValor.SAcuseValor2)

                        If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposClientes.CA_TAX_ID).Valor IsNot Nothing Then

                            .Campo(CamposAcuseValor.CA_TAX_ID_EMISOR).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposClientes.CA_TAX_ID).Valor

                            .Campo(CamposAcuseValor.CA_TIPO_IDENTIFICADOR_EMISOR).Valor = 0
                        End If

                        If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposClientes.CA_RFC_CLIENTE).Valor IsNot Nothing Then

                            .Campo(CamposAcuseValor.CA_RFC_EMISOR).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposClientes.CA_RFC_CLIENTE).Valor

                            .Campo(CamposAcuseValor.CA_TIPO_IDENTIFICADOR_EMISOR).Valor = 1

                        End If

                        If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor IsNot Nothing Then

                            .Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor

                            .Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).ValorPresentacion = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).ValorPresentacion

                        End If

                        If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CALLE).Valor IsNot Nothing Then

                            .Campo(CamposDomicilio.CA_CALLE).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CALLE).Valor

                        End If

                        If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor IsNot Nothing Then

                            .Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor

                        End If

                        If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor IsNot Nothing Then

                            .Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor

                        End If

                        If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_NUMERO_EXT_INT).Valor IsNot Nothing Then

                            .Campo(CamposDomicilio.CA_NUMERO_EXT_INT).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_NUMERO_EXT_INT).Valor

                        End If

                        If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor IsNot Nothing Then

                            .Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor

                        End If


                        If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_COLONIA).Valor IsNot Nothing Then

                            .Campo(CamposDomicilio.CA_COLONIA).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_COLONIA).Valor

                        End If

                        If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_LOCALIDAD).Valor IsNot Nothing Then

                            .Campo(CamposDomicilio.CA_LOCALIDAD).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_LOCALIDAD).Valor

                        End If

                        If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_LOCALIDAD).Valor IsNot Nothing Then

                            .Campo(CamposDomicilio.CA_CIUDAD).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CIUDAD).Valor

                        End If

                        If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_MUNICIPIO).Valor IsNot Nothing Then

                            .Campo(CamposDomicilio.CA_MUNICIPIO).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_MUNICIPIO).Valor

                        End If

                        If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor IsNot Nothing Then

                            .Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor

                        End If

                        Dim idEntidadFederativaCliente_ As ObjectId

                        If ObjectId.TryParse(documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor, idEntidadFederativaCliente_) Then

                            .Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).ValorPresentacion
                        Else

                            .Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor

                        End If

                        If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CVE_PAIS).Valor IsNot Nothing Then

                            .Campo(CamposDomicilio.CA_CVE_PAIS).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_CVE_PAIS).Valor

                        End If

                        If documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_PAIS).Valor IsNot Nothing Then

                            .Campo(CamposDomicilio.CA_PAIS).Valor = documentoElectronicoaux_.Seccion(SeccionesClientes.SCS1).Campo(CamposDomicilio.CA_PAIS).Valor

                        End If

                    End With

                End If


                Dim razonSocialProvedor_ = documentoElectronico_.Campo(CamposAcuseValor.CA_TAX_ID_DESTINATARIO_ACUSE).Valor

                Dim proveedores_ As List(Of AuxiliarProveedor) = _controladorProveedor.ObtenerProveedoresPorRazonSocialPais(razonSocialProvedor_).ObjectReturned


                Dim proveedor_ = proveedores_.Find(Function(ch) ch._razonsocial.Contains(razonSocialProvedor_))


                With .Seccion(SeccionesAcuseValor.SAcuseValor3)

                    If proveedor_ IsNot Nothing Then

                        .Campo(CamposAcuseValor.CP_ID_EMISOR).Valor = ObjectId.Parse(proveedor_.idtaxid)


                        If proveedor_._taxid <> "" Then

                            .Campo(CamposAcuseValor.CA_TAX_ID_EMISOR).Valor = proveedor_._taxid

                            .Campo(CA_TIPO_IDENTIFICADOR_EMISOR).Valor = 0

                        End If

                        If proveedor_._rfc <> "" Then

                            .Campo(CamposAcuseValor.CA_RFC_EMISOR).Valor = proveedor_._rfc

                            .Campo(CA_TIPO_IDENTIFICADOR_EMISOR).Valor = 1

                        End If

                        If proveedor_._domicilio IsNot Nothing Then

                            .Campo(CamposDomicilio.CP_ID_DOMICILIO).Valor = proveedor_._domicilio._iddomicilio

                            .Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor = proveedor_._domicilio.domicilioPresentacion

                            .Campo(CamposDomicilio.CA_CALLE).Valor = proveedor_._domicilio.calle

                            .Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor = proveedor_._domicilio.numeroexterior

                            .Campo(CamposDomicilio.CA_NUMERO_INTERIOR).Valor = proveedor_._domicilio.numerointerior

                            .Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor = proveedor_._domicilio.codigopostal

                            .Campo(CamposDomicilio.CA_COLONIA).Valor = proveedor_._domicilio.colonia

                            .Campo(CamposDomicilio.CA_LOCALIDAD).Valor = proveedor_._domicilio.localidad

                            .Campo(CamposDomicilio.CA_CIUDAD).Valor = proveedor_._domicilio.ciudad

                            .Campo(CamposDomicilio.CA_MUNICIPIO).Valor = proveedor_._domicilio.municipio

                            .Campo(CamposDomicilio.CA_CVE_ENTIDAD_FEDERATIVA).Valor = proveedor_._domicilio.cveEntidadfederativa

                            .Campo(CamposDomicilio.CA_ENTIDAD_FEDERATIVA).Valor = proveedor_._domicilio.entidadfederativa

                            .Campo(CamposDomicilio.CA_CVE_PAIS).Valor = proveedor_._cvepais

                            Dim pais_ = If(proveedor_._pais, "")

                            If pais_ = "" Then

                                Dim paises_ As New List(Of Pais)

                                ControladorPaises.BuscarPaises(paises_, proveedor_._cvepais)

                                pais_ = paises_(0).nombrepaisesp

                            End If

                            .Campo(CamposDomicilio.CA_PAIS).Valor = pais_

                        End If

                    End If

                End With

                'DE MOMENTO documentoElectronicoaux_ con tiene el ID del Cliente para estas pruebas de importación

                If scSelloCliente.Value = "0" Then

                    .Campo(CamposAcuseValor.CP_ID_AGENTE_SELLO).Valor = ObjectId.Parse(documentoElectronicoaux_.Id)

                    .Campo(CamposAcuseValor.CP_TIPO_SELLO).Valor = 0


                Else

                    .Campo(CamposAcuseValor.CP_ID_CLIENTE_SELLO).Valor = ObjectId.Parse(scSelloCliente.Value)

                    .Campo(CamposAcuseValor.CP_TIPO_SELLO).Valor = 1

                End If

                .NombreCliente = documentoElectronicoaux_.NombreCliente

                With .Attribute(CamposFacturaComercial.CA_MONEDA_FACTURACION)

                    .Valor = scTipoMoneda.Value

                    .ValorPresentacion = scTipoMoneda.Text

                End With

                Dim rfcCliente_ As String

                Dim tipoFigura_ As Integer

                If swcTipoOperacion.Checked Then

                    rfcCliente_ = icIDFiscalDestinatario.Value

                    tipoFigura_ = 5

                Else

                    rfcCliente_ = icIDFiscalProveedor.Value

                    tipoFigura_ = 4

                End If

                If scSelloCliente.Text = rfcCliente_ Then

                    .Campo(CamposAcuseValor.CA_TIPO_FIGURA).Valor = tipoFigura_

                Else

                    .Campo(CamposAcuseValor.CA_TIPO_FIGURA).Valor = 1

                End If

            End With

        End If

        SetVars("_AcuseValorFindBar", New ConstructorAcuseValor(True, documentoElectronico_))


        'SetVars("_AcuseValorFindBar", New ConstructorAcuseValor(True, documentoElectronico_))
        Return New TagWatcher With {.Status = TypeStatus.Ok}

    End Function

    Public Overrides Function DespuesRealizarModificacion() As TagWatcher

        'MsgBox("LLEGA AQUÍ")
        Return New TagWatcher(Ok)


    End Function

    'EVENTOS PARA PRESENTACIÓN DE DATOS EN FRONTEND
    Public Overrides Function PreparaModificacion_ProcesoInterno(ByRef documentoElectronico_ As DocumentoElectronico) As TagWatcher

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Esta metodo se manda llamar justo al seleccionar uno de los resultados de la busqueda general       '
        ' Aqui ocurre el llenado del formulario                                                               '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''
        Return New TagWatcher With {.Status = TypeStatus.Ok}
    End Function

    Public Overrides Function DespuesBuquedaGeneralConDatos_ProcesoInterno() As TagWatcher



        Dim operacionGenerica_ As OperacionGenerica = GetVars("_operacionGenerica")

        operacionGenerica_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Id = operacionGenerica_.Id.ToString

        Dim constructorAcuseValor_ = New ConstructorAcuseValor(True, operacionGenerica_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente)

        SetVars("_AcuseValorFindBar", constructorAcuseValor_)
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Esta metodo se manda llamar al realizar una consulta en la barra de busqueda y obtenemos resultados '
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


        If operacionGenerica_ IsNot Nothing Then


            Select Case constructorAcuseValor_.Campo(CamposAcuseValor.CA_TIPO_IDENTIFICADOR_EMISOR).Valor


                Case "0"

                    icIDFiscalProveedor.Value = constructorAcuseValor_.Campo(CamposAcuseValor.CA_TAX_ID_EMISOR).Valor

                Case "1"

                    icIDFiscalProveedor.Value = constructorAcuseValor_.Campo(CamposAcuseValor.CA_RFC_EMISOR).Valor

                Case "2"

                    icIDFiscalProveedor.Value = constructorAcuseValor_.Campo(CamposAcuseValor.CA_CURP_EMISOR).Valor

                Case "3"

                    icIDFiscalProveedor.Value = "Sin TAXID"

            End Select

            Select Case constructorAcuseValor_.Campo(CamposAcuseValor.CA_TIPO_IDENTIFICADOR_DESTINATARIO_ACUSE).Valor

                Case "0"

                    icIDFiscalDestinatario.Value = constructorAcuseValor_.Campo(CamposAcuseValor.CA_TAX_ID_DESTINATARIO_ACUSE).Valor

                Case "1"

                    icIDFiscalDestinatario.Value = constructorAcuseValor_.Campo(CamposAcuseValor.CA_RFC_DESTINATARIO_ACUSE).Valor

                Case "2"

                    icIDFiscalDestinatario.Value = constructorAcuseValor_.Campo(CamposAcuseValor.CA_CURP_DESTINATARIO_ACUSE).Valor

                Case "3"

                    icIDFiscalDestinatario.Value = "Sin TAXID"

            End Select



            If constructorAcuseValor_.Campo(CamposFacturaComercial.CP_TIPO_OPERACION).Valor IsNot Nothing Then

                If constructorAcuseValor_.Campo(CamposFacturaComercial.CP_TIPO_OPERACION).Valor = "Importación" Then

                    swcTipoOperacion.Checked = True


                Else

                    swcTipoOperacion.Checked = False

                End If

            End If

        End If

        Dim aplicaCertificado_ As String = constructorAcuseValor_.Campo(CamposFacturaComercial.CA_APLICA_CERTIFICADO).Valor

        If aplicaCertificado_ IsNot Nothing Then

            If aplicaCertificado_.ToUpper = "SI" OrElse
                   aplicaCertificado_.ToUpper = "SÍ" OrElse
                   aplicaCertificado_.ToUpper = "Sí" Then


                swcCertificadoOrigen.Checked = True

            Else

                swcCertificadoOrigen.Checked = False

            End If

        End If

        Dim subdivision_ As String = constructorAcuseValor_.Campo(CamposFacturaComercial.CA_APLICA_SUBDIVISION).Valor

        If subdivision_ IsNot Nothing Then

            If subdivision_.ToUpper = "SI" OrElse
                   subdivision_.ToUpper = "SÍ" OrElse
                   subdivision_.ToUpper = "Sí" Then


                swcSubdivision.Checked = True

            Else

                swcSubdivision.Checked = False

            End If

        End If

        Return New TagWatcher With {.Status = TypeStatus.Ok}

    End Function

    Public Overrides Function DespuesBuquedaGeneralSinDatos_ProcesoInterno() As TagWatcher

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Esta metodo se manda llamar al realizar una consulta en la barra de busqueda y no obtenemos resultados '
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''
        Return New TagWatcher With {.Status = TypeStatus.Ok}

    End Function


    'EVENTOS DE MANTENIMIENTO
    Public Overrides Sub LimpiaSesion()

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Esta metodo se manda llamar la primera vez que carga la página y despues de culminar una transacción '
        ' importante limpies tus variables de sessión aqui                                                     '
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    End Sub

    Public Overrides Sub Limpiar()

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Esta metodo se manda llamar la primera vez que carga la página y despues de culminar una transacción '
        ' importante limpies tus controles asigando un Value o DataSource en Nothing                           '
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    End Sub




#End Region

#Region "████████████████  QUINTA CAPA  █████████       Reglas locales         ██████████████████████████████"
    '    ██                                                                                                ██
    '    ██                 Defina en esta región su lógica de negocio para este módulo                    ██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████
    'Dim illenamonedas As Int32 = 2

    Public Sub CargarFacturasRepetidas(listaResultado_ As List(Of Nodo))

        Dim listasResultado_ As New Dictionary(Of String, List(Of Nodo))

        Dim total_ = listaResultado_.Count

        Dim indice_ = 0

        Dim selectOptions_ = New List(Of SelectOption)

        While total_ > indice_

            Dim value_ = DirectCast(listaResultado_(3 + indice_), Campo).Valor.ToString &
                                    DirectCast(listaResultado_(10 + indice_), Campo).Valor.ToString

            Dim razonSocialProveedor_ = DirectCast(listaResultado_(11 + indice_), Campo).ValorPresentacion.ToString.Split("|")(0)

            Dim razonSocialCliente_ = DirectCast(listaResultado_(4 + indice_), Campo).ValorPresentacion.ToString.Split("|")(0)

            Dim facturaClienteProveedor_ = DirectCast(listaResultado_(0 + indice_), Campo).Valor.ToString & " - " &
                                           razonSocialCliente_ & " - " &
                                          razonSocialProveedor_

            listasResultado_.Add(value_,
                                     listaResultado_.GetRange(indice_, 16))

            selectOptions_.Add(New SelectOption With {.Value = value_,
                                                      .Text = facturaClienteProveedor_})

            SetVars("FacturasCargadas_", listasResultado_)

            indice_ += 16

        End While


        dbcNumFacturaAcuseValor.Visible = False

        scNumeroFactura.Visible = True

        scNumeroFactura.DataSource = selectOptions_

        DisplayMessage("Se encontro repetido el Folio " & dbcNumFacturaAcuseValor.Value & " seleccione el que desea cargar", StatusMessage.Info)



    End Sub

    Public Sub CargarFacturaSeleccionada(sender_ As Object, event_ As EventArgs)

        If sender_.Value <> "" Then

            Dim listasResultado_ = GetVars("FacturasCargadas_")

            If listasResultado_ IsNot Nothing Then

                scNumeroFactura.Visible = False

                dbcNumFacturaAcuseValor.Visible = True

                CargarFacturas(listasResultado_(sender_.Value))

            End If



        End If

    End Sub


    Public Sub CargarFacturas(listaResultado_ As List(Of Nodo))

        ' Dim listaResultado_ = resultado_(dbcNumFacturaAcuseValor.Value.ToString)



        Dim listas_ = New List(Of ObjectId) From {ObjectId.Parse(DirectCast(listaResultado_(15), Campo).Valor)}

        If _icontroladorFactura.ObtenerFacturasComercialesPublicadas(listas_).ObjectReturned Is Nothing Then

            DisplayMessage("Se encontró la factura con folio " &
                           Chr(34) &
                           dbcNumFacturaAcuseValor.Value &
                           Chr(34) &
                           " pero no  está publicada",
                           StatusMessage.Fail)

        Else

            icFechaExpedicion.Value = Date.Parse(DirectCast(listaResultado_.Find(Function(ch_) DirectCast(ch_, Campo).Nombre = "CA_FECHA_FACTURA"), Campo).Valor).ToString("yyyy-MM-dd")

            If DirectCast(listaResultado_.Find(Function(ch_) DirectCast(ch_, Campo).Nombre = "CP_TIPO_OPERACION"), Campo).Valor = "1" Then

                swcTipoOperacion.Checked = True

            Else

                swcTipoOperacion.Checked = False

            End If

            Dim idFactura_ = DirectCast(listaResultado_.Item(15), Campo).ValorPresentacion

            SetVars("IDS", idFactura_)

            Dim idMoneda = ObjectId.Parse(DirectCast(listaResultado_.Find(Function(ch_) DirectCast(ch_, Campo).Nombre = "CA_MONEDA_FACTURACION"), Campo).Valor)

            scTipoMoneda.DataSource.RemoveAll(Function(ch) ch.Value <> "")

            Dim monedas_ = _icontroladorMonedas.BuscarMonedas(New List(Of String),
                                                         New List(Of ObjectId) From {idMoneda}, "cveISO4217")

            scTipoMoneda.DataSource = _organismo.
                                   ObtenerSelectOption(scTipoMoneda,
                                                       monedas_.
                                                       Select(Of ValorProvisionalOption)(Function(chi) New ValorProvisionalOption With {
                                .Id = chi._id,
                                .Valor = chi.nombremonedaesp & " | " &
                                chi.aliasmoneda.Find(Function(ef) ef.Clave = "cveISO4217").Valor
                               }).ToList)

            If scTipoMoneda.DataSource.Count > 0 Then

                scTipoMoneda.Value = idMoneda.ToString

            End If

            SetVars("_Monedas", monedas_)

            Dim fbcEmisor_ As FindboxControl = If(swcTipoOperacion.Checked, fbcProveedor, fbcDestinatario)

            Dim icDirecconEmisor_ As InputControl = If(swcTipoOperacion.Checked, icDireccionProveedor, icDireccionDestinatario)

            Dim icFiscalEmisor_ As InputControl = If(swcTipoOperacion.Checked, icIDFiscalProveedor, icIDFiscalDestinatario)

            Dim fbcDestinatario_ As FindboxControl = If(swcTipoOperacion.Checked, fbcDestinatario, fbcProveedor)

            Dim icDirecconDestinatario_ As InputControl = If(swcTipoOperacion.Checked, icDireccionDestinatario, icDireccionProveedor)

            Dim icFiscalDestinatario_ As InputControl = If(swcTipoOperacion.Checked, icIDFiscalDestinatario, icIDFiscalProveedor)

            Dim identificacionPersona_, tipoIdentificador_ As String

            identificacionPersona_ = DirectCast(listaResultado_.Find(Function(ch_) DirectCast(ch_, Campo).Nombre = "CA_TAX_ID_PROVEEDOR"), Campo).ValorPresentacion

            identificacionPersona_ = If(identificacionPersona_, "")



            If identificacionPersona_ <> "" Then

                tipoIdentificador_ = "TAXID"

            Else

                identificacionPersona_ = If(DirectCast(listaResultado_.Find(Function(ch_) DirectCast(ch_, Campo).Nombre = "CA_RFC_PROVEEDOR"), Campo).Valor Is Nothing, "",
                                        DirectCast(listaResultado_.Find(Function(ch_) DirectCast(ch_, Campo).Nombre = "CA_RFC_PROVEEDOR"), Campo).Valor.ToString)
                If identificacionPersona_ <> "" Then

                    'identificacionPersona_ = DirectCast(listaResultado_.Item(12), Campo).Valor.ToString

                    tipoIdentificador_ = "RFC"
                Else

                    identificacionPersona_ = DirectCast(listaResultado_.Find(Function(ch_) DirectCast(ch_, Campo).Nombre = "CA_RAZON_SOCIAL_PROVEEDOR"), Campo).Valor.ToString

                    tipoIdentificador_ = "RAZONSOCIAL"

                    Dim iindiceaux_ As Int32 = identificacionPersona_.IndexOf("|")

                    If iindiceaux_ > 0 Then

                        identificacionPersona_ = identificacionPersona_.Substring(0, iindiceaux_ - 1)

                    Else

                        iindiceaux_ = identificacionPersona_.LastIndexOf("-")

                        If iindiceaux_ > 0 Then

                            identificacionPersona_ = identificacionPersona_.Substring(0, iindiceaux_ - 1)

                        End If

                    End If

                End If

            End If

            If identificacionPersona_ <> "" Then

                If swcTipoOperacion.Checked Then

                    _controladorProveedor._TipoOperacion = CtrlProveedoresOperativos.TipoOperacion.Importacion

                Else

                    _controladorProveedor._TipoOperacion = CtrlProveedoresOperativos.TipoOperacion.Exportacion

                End If

                Dim razonSocialProveedor_ = If(DirectCast(listaResultado_.Find(Function(ch_) DirectCast(ch_, Campo).Nombre = "CA_RAZON_SOCIAL_PROVEEDOR"), Campo).ValorPresentacion Is Nothing, "",
                                        DirectCast(listaResultado_.Find(Function(ch_) DirectCast(ch_, Campo).Nombre = "CA_RAZON_SOCIAL_PROVEEDOR"), Campo).ValorPresentacion.ToString)


                razonSocialProveedor_ = razonSocialProveedor_.Split(" ")(0)

                Dim proveedores_ As List(Of AuxiliarProveedor) = _controladorProveedor.ObtenerProveedoresPorRazonSocialPais(razonSocialProveedor_).ObjectReturned

                Dim lista_ = New List(Of SelectOption)

                If proveedores_ IsNot Nothing Then

                    For Each proveedor_ In proveedores_

                        lista_.Add(New SelectOption With {.Value = proveedor_.idtaxid, .Text = proveedor_._razonsocial})

                    Next

                    SetVars("_proveedores", proveedores_)

                    fbcEmisor_.Value = proveedores_(0).idtaxid

                    fbcEmisor_.Text = proveedores_(0)._razonsocial & " | " & proveedores_(0)._taxid

                    icDirecconEmisor_.Value = proveedores_(0)._domicilio.domicilioPresentacion

                    If proveedores_(0)._taxid IsNot Nothing Then

                        If proveedores_(0)._taxid <> "" Then

                            icFiscalEmisor_.Value = proveedores_(0)._taxid

                        Else

                            icFiscalEmisor_.Value = proveedores_(0)._rfc

                        End If

                    Else

                        icFiscalEmisor_.Value = proveedores_(0)._rfc

                    End If

                End If

                'Dim buscarProveedor_ As New ControladorBusqueda(Of ConstructorProveedoresOperativos)

                'Dim idProveedor_ = DirectCast(listaResultado_.Find(Function(ch_) DirectCast(ch_, Campo).Nombre = "CP_ID_PROVEEDOR"), Campo).Valor

                'Dim tagwatcherPtoveedor_ As TagWatcher = buscarProveedor_.ObtenerDocumento(idProveedor_)

                'Dim proveedorAcuseValor_ = New ConstructorProveedoresOperativos(True, tagwatcherPtoveedor_.ObjectReturned.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente)

                'If proveedorAcuseValor_ IsNot Nothing Then

                '    proveedorAcuseValor_.NombrePropietario = tagwatcherPtoveedor_.ObjectReturned.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.NombrePropietario

                '    fbcEmisor_.Value = proveedorAcuseValor_.Id.ToString

                '    fbcEmisor_.Text = proveedorAcuseValor_.NombrePropietario & " | " & proveedorAcuseValor_.FolioDocumento

                '    If proveedorAcuseValor_.Seccion(SeccionesProvedorOperativo.SPRO2).
                '    Campo(CamposProveedorOperativo.CA_DOMICILIO_FISCAL) IsNot Nothing Then

                '        If proveedorAcuseValor_.Seccion(SeccionesProvedorOperativo.SPRO2).
                '        Campo(CamposProveedorOperativo.CA_DOMICILIO_FISCAL).ValorPresentacion IsNot Nothing Then

                '            icDirecconEmisor_.Value = proveedorAcuseValor_.Seccion(SeccionesProvedorOperativo.SPRO2).
                '                                      Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).ValorPresentacion

                '        Else

                '            icDirecconEmisor_.Value = proveedorAcuseValor_.Seccion(SeccionesProvedorOperativo.SPRO2).
                '                                      Campo(CamposDomicilio.CA_CALLE).Valor &
                '                                      " #" &
                '                                      proveedorAcuseValor_.Seccion(SeccionesProvedorOperativo.SPRO2).
                '                                      Campo(CamposDomicilio.CA_NUMERO_EXTERIOR).Valor &
                '                                     " CP:  " &
                '                                     proveedorAcuseValor_.Seccion(SeccionesProvedorOperativo.SPRO2).
                '                                     Campo(CamposDomicilio.CA_CODIGO_POSTAL).Valor &
                '                                     " " &
                '                                     proveedorAcuseValor_.Seccion(SeccionesProvedorOperativo.SPRO2).
                '                                     Campo(CamposDomicilio.CA_COLONIA).Valor &
                '                                     " ," &
                '                                     proveedorAcuseValor_.Seccion(SeccionesProvedorOperativo.SPRO2).
                '                                     Campo(CamposDomicilio.CA_PAIS).Valor

                '        End If

                '        If tipoIdentificador_ = "RAZONSOCIAL" Then

                '            If proveedorAcuseValor_.Seccion(SeccionesProvedorOperativo.SPRO2).
                '            Campo(CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR).Valor IsNot Nothing Then

                '                icFiscalEmisor_.Value = proveedorAcuseValor_.Seccion(SeccionesProvedorOperativo.SPRO2).
                '                                         Campo(CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR).ValorPresentacion

                '            Else
                '                icFiscalEmisor_.Value = proveedorAcuseValor_.Seccion(SeccionesProvedorOperativo.SPRO2).
                '                                         Campo(CamposProveedorOperativo.CA_RFC_PROVEEDOR).Valor

                '            End If

                '        Else

                '            icFiscalEmisor_.Value = identificacionPersona_

                '        End If

                '    End If

                'End If

                ' SetVars("_ProveedorAcuseValor", proveedorAcuseValor_)

            End If

            If DirectCast(listaResultado_.Item(5), Campo).Valor IsNot Nothing Then

                identificacionPersona_ = DirectCast(listaResultado_.Item(5), Campo).Valor.ToString

                tipoIdentificador_ = "TAXID"

            Else

                If DirectCast(listaResultado_.Item(6), Campo).Valor IsNot Nothing Then

                    identificacionPersona_ = DirectCast(listaResultado_.Item(6), Campo).Valor.ToString

                    tipoIdentificador_ = "RFC"

                Else


                    identificacionPersona_ = DirectCast(listaResultado_.Item(4), Campo).Valor.ToString

                    tipoIdentificador_ = "RAZONSOCIAL"

                    Dim iindiceAux_ As Int32 = identificacionPersona_.IndexOf("|")

                    If iindiceAux_ > 0 Then

                        identificacionPersona_ = identificacionPersona_.Substring(0, iindiceAux_ - 1)

                    Else

                        iindiceAux_ = identificacionPersona_.LastIndexOf("-")

                        If iindiceAux_ > 0 Then

                            identificacionPersona_ = identificacionPersona_.Substring(0, iindiceAux_ - 1)

                        End If

                    End If

                End If

            End If

            If identificacionPersona_ <> "" Then

                If tipoIdentificador_ = "RAZONSOCIAL" Then

                    Dim constructorCliente_ As New ControladorBusqueda(Of ConstructorCliente)
                    Dim listaClientes_ As List(Of SelectOption) = constructorCliente_.Buscar(identificacionPersona_,
                                                                  New Filtro With {.IdSeccion = SeccionesClientes.SCS1, .IdCampo = CamposClientes.CA_RAZON_SOCIAL})


                End If


            End If

            Dim buscarCliente_ As New ControladorBusqueda(Of ConstructorCliente)

            Dim idCliente_ = DirectCast(listaResultado_.Find(Function(ch_) DirectCast(ch_, Campo).Nombre = "CP_OBJECTID_CLIENTE"), Campo).Valor

            Dim tagwatcherCliente_ As TagWatcher = buscarCliente_.ObtenerDocumento(idCliente_.ToString)

            If tagwatcherCliente_.Status = TypeStatus.Ok Then

                Dim clienteAcuseValor_ As ConstructorCliente = DirectCast(tagwatcherCliente_.ObjectReturned.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente, ConstructorCliente)

                With clienteAcuseValor_

                    If tipoIdentificador_ = "RAZONSOCIAL" Then

                        icFiscalDestinatario_.Value = clienteAcuseValor_.Seccion(SeccionesClientes.SCS1).
                                                                       Attribute(CamposClientes.CA_TAX_ID).Valor

                        If icFiscalDestinatario_.Value = "" Then

                            icFiscalDestinatario_.Value = clienteAcuseValor_.Seccion(SeccionesClientes.SCS1).
                                                                           Attribute(CamposClientes.CA_RFC_CLIENTE).Valor

                        End If

                    Else

                        icFiscalDestinatario_.Value = identificacionPersona_

                    End If

                    fbcDestinatario_.Value = idCliente_.ToString

                    fbcDestinatario_.Text = .NombreCliente & " | " & .FolioDocumento

                    icDirecconDestinatario_.Value = .Seccion(SeccionesClientes.SCS1).
                                                  Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor

                    scSelloCliente.DataSource = New List(Of SelectOption) From {New SelectOption With {.Value = idCliente_.ToString,
                                                                                                    .Text = icFiscalDestinatario_.Value.ToString},
                                                                             New SelectOption With {.Value = "0", .Text = "AGENTE ADUANAL"}}

                    scSelloCliente.Value = idCliente_.ToString

                    Dim clientetemporal_ = .Seccion(SeccionesClientes.SCS2).Campo(CamposClientes.CP_CVE_PATENTE_ADUANAL)

                    Dim patenteaduanal_ As String

                    If clientetemporal_ Is Nothing Then

                        patenteaduanal_ = ""

                    Else

                        If clientetemporal_.Valor Is Nothing Then

                            patenteaduanal_ = ""

                        Else

                            patenteaduanal_ = clientetemporal_.Valor

                        End If

                    End If

                    If patenteaduanal_ <> "" Then

                        Dim _recursosAduanales = New ControladorRecursosAduanales

                        Dim modalidadpatente_ = _recursosAduanales.BuscarRecursosAduanales(1)

                        '_ = _recursosAduanales.aduanaspatentes

                        'Dim algo_ = _recursosAduanales.aduanaspatentes

                        Dim patente_ As String = ""

                        Dim modalidadAduana_ = ""

                        For Each campo_ In .Seccion(SeccionesClientes.SCS2).Nodos

                            Dim valor_ = campo_.Campo(CamposClientes.CP_CVE_ADUANA_SECCION).Valor

                            If modalidadpatente_ IsNot Nothing Then

                                modalidadAduana_ = modalidadpatente_.aduanaspatentes.Find(Function(ch) ch._idaduanaseccion = valor_).ciudad


                            End If


                            If modalidadAduana_.Contains(ListaEmpresas.Text) Then

                                patente_ = campo_.Campo(CamposClientes.CP_CVE_PATENTE_ADUANAL).Valor

                                Exit For

                            End If

                        Next

                        icPatenteAduanal.Value = patente_

                        patente_ = If(patente_ = "", patente_, patente_.Substring(0, 4))

                        ccDatosConsulta.ClearRows()

                        Dim rfcAgente_ = ""

                        Dim services_ = Statements.GetService("project", 18, Sax.SaxStatements.SettingTypes.Projects, 3)

                        Dim key_ = services_.environmentsettings(0).claims(0).key

                        Dim value_ = services_.environmentsettings(0).claims(0).value

                        Dim sdk_ As IControllerCustomsSettingsClient = New ControllerCustomsSettingsClient(
                        apiKey_:=value_,
                       version_:=key_
                        )

                        'Dim options_ As New BrokerConfigurationClientOptions(
                        '          value_,
                        '           key_
                        '          )

                        'pbPartidasAcuseValor.setValueInvisible(Of Integer)(icMarcaAcuseValor, 1, 3)

                        'Dim sdk_ As New BrokerConfigurationClient(options_)

                        Try

                            If patente_ <> "" Then

                                Dim dto_ = sdk_.GetBrokerConfiguration(GetEnvironment(__SYSTEM_ENVIRONMENT.Value), ListaEmpresas.Value, patente_)

                                rfcAgente_ = dto_.Data.customsOfficeGeneralData.agentTaxId

                                ccDatosConsulta.SetRow(Sub(catalogRow_ As CatalogRow)

                                                           'Define el valor Llave de tu fila

                                                           Dim loginUsuario_ As Dictionary(Of String, String) = Session("DatosUsuario")

                                                           catalogRow_.SetIndice(ccDatosConsulta.KeyField, 0)

                                                           'Define el valor de una columna de la fila

                                                           'Dim optionList_ = New List(Of SelectOption) From {New SelectOption With {.Value = "HOLA", .Text = "MUNDO"},
                                                           '                                                           New SelectOption With {.Value = "HOLA2", .Text = "MUNDO2"},
                                                           '                                                           New SelectOption With {.Value = "HOLA3", .Text = "MUNDO3"}}

                                                           Dim icRFCConsulta_ As New InputControl With {.ID = "icRFCConsulta",
                                                                                                               .Value = rfcAgente_,
                                                                                                               .Type = InputControl.InputType.Text}

                                                           Dim icRazonSocialConsulta_ As New InputControl With {.ID = "icRazonSocialConsulta",
                                                                                                               .Value = "",
                                                                                                               .Type = InputControl.InputType.Text}

                                                           Dim icEmailConsulta As New InputControl With {.ID = "icEmailConsulta",
                                                                                                               .Value = loginUsuario_("WebServiceUserID"),
                                                                                                               .Type = InputControl.InputType.Text}


                                                           catalogRow_.SetColumn(icRFCConsulta_, rfcAgente_)

                                                           catalogRow_.SetColumn(icRazonSocialConsulta_, "")

                                                           catalogRow_.SetColumn(icEmailConsulta, loginUsuario_("WebServiceUserID"))

                                                       End Sub)

                            End If

                        Catch ex As Exception

                            rfcAgente_ = ""

                        End Try

                        ccDatosConsulta.CatalogDataBinding()

                    Else

                        icPatenteAduanal.Value = ""

                    End If

                End With

                SetVars("_ClienteAcuseValor", clienteAcuseValor_)

            End If

            Dim partidasFactura_ = listaResultado_.Item(14)

            Dim pillboxControl_ As PillboxControl = DirectCast(pbPartidasAcuseValor, PillboxControl)

            pillboxControl_.ClearRows()

            Dim unidadesT_ As New List(Of UnidadMedida)

            Dim indice_ = 0

            For Each partidaFactura_ In partidasFactura_.Nodos

                pillboxControl_.SetPillbox(Sub(pillbox_ As PillBox)

                                               Dim descripcionesAcuseValor_ As String = ""

                                               Dim cantidadAcuseValor_ As String = ""

                                               Dim precioUnitario_ As String = ""

                                               Dim total_ As String = ""

                                               Dim totalDolares_ As String = ""

                                               Dim monedaFactura_ As String = ""

                                               Dim marca_ As String = ""

                                               Dim modelo_ As String = ""

                                               Dim subModelo_ As String = ""

                                               Dim numeroSerie_ As String = ""

                                               Dim unidades_ As New List(Of UnidadMedida)

                                               pillbox_.SetIndice(pillboxControl_.KeyField,
                                                              indice_)

                                               pillbox_.SetFiled(False)

                                               descripcionesAcuseValor_ = If(DirectCast(partidaFactura_.Nodos.Find(Function(ch_) DirectCast(ch_.Nodos(0), Campo).Nombre = "CA_DESCRIPCION_PARTE_PARTIDA").Nodos(0), Campo).Valor, "")

                                               cantidadAcuseValor_ = If(DirectCast(partidaFactura_.Nodos.Find(Function(ch_) DirectCast(ch_.Nodos(0), Campo).Nombre = "CA_CANTIDAD_COMERCIAL_PARTIDA").Nodos(0), Campo).Valor, "")

                                               precioUnitario_ = If(DirectCast(partidaFactura_.Nodos.Find(Function(ch_) DirectCast(ch_.Nodos(0), Campo).Nombre = "CA_PRECIO_UNITARIO_PARTIDA").Nodos(0), Campo).Valor, "")

                                               total_ = If(DirectCast(partidaFactura_.Nodos.Find(Function(ch_) DirectCast(ch_.Nodos(0), Campo).Nombre = "CA_VALOR_FACTURA_PARTIDA").Nodos(0), Campo).Valor, "")

                                               If DirectCast(partidaFactura_.Nodos(16).Nodos(0), Campo).Valor IsNot Nothing Then

                                                   unidades_ = _controladorUnidadesMedida.BuscarUnidadesCOVE((DirectCast(partidaFactura_.Nodos(16).Nodos(0), Campo).ValorPresentacion),, 1)

                                               Else

                                                   unidades_ = _controladorUnidadesMedida.BuscarUnidadesCOVE("PIEZA",, 1)

                                               End If

                                               If DirectCast(partidaFactura_.Nodos(23).Nodos(0), Campo).Valor IsNot Nothing Then

                                                   numeroSerie_ = DirectCast(partidaFactura_.Nodos(23).Nodos(0), Campo).Valor

                                               End If

                                               If DirectCast(partidaFactura_.Nodos(24).Nodos(0), Campo).Valor IsNot Nothing Then

                                                   marca_ = DirectCast(partidaFactura_.Nodos(24).Nodos(0), Campo).Valor

                                               End If

                                               If DirectCast(partidaFactura_.Nodos(25).Nodos(0), Campo).Valor IsNot Nothing Then

                                                   modelo_ = DirectCast(partidaFactura_.Nodos(25).Nodos(0), Campo).Valor

                                               End If

                                               If DirectCast(partidaFactura_.Nodos(26).Nodos(0), Campo).Valor IsNot Nothing Then

                                                   subModelo_ = DirectCast(partidaFactura_.Nodos(26).Nodos(0), Campo).Valor

                                               End If

                                               scMonedaPrecioUnitarioPartida.DataSource = scTipoMoneda.DataSource

                                               If scMonedaPrecioUnitarioPartida.DataSource.Count > 0 Then

                                                   scMonedaPrecioUnitarioPartida.Value = scTipoMoneda.Value

                                                   monedaFactura_ = scMonedaPrecioUnitarioPartida.Text

                                               Else

                                                   scMonedaPrecioUnitarioPartida.DataSource = New List(Of SelectOption) From
                                                                                              {New SelectOption With {.Value = "",
                                                                                                                      .Text = ""}}
                                               End If

                                               If cantidadAcuseValor_ <> "" And precioUnitario_ <> "" Then

                                                   If total_ = "" Then

                                                       total_ = Convert.ToString(Convert.ToDouble(cantidadAcuseValor_) *
                                                                             Convert.ToDouble(precioUnitario_))

                                                   End If

                                                   totalDolares_ = Convert.ToString(Convert.ToDouble(total_) *
                                                                                    _icontroladorMonedas.
                                                                                    Monedas.
                                                                                    Find(Function(se) se._id.ToString = scMonedaPrecioUnitarioPartida.Value.ToString).
                                                                                    factoresmoneda(0).valordefault)

                                               End If

                                               pillbox_.SetControlValue(icDescripcionAcuseValor, descripcionesAcuseValor_)

                                               pillbox_.SetControlValue(icCantidadAcuseValor, cantidadAcuseValor_)

                                               pillbox_.SetControlValue(icPrecioUnitarioAcuseValor, precioUnitario_)

                                               pillbox_.SetControlValue(icValorFacturaPartida, total_)

                                               pillbox_.SetControlValue(icValorDolaresPartida, totalDolares_)

                                               pillbox_.SetControlValue(scMonedaPrecioUnitarioPartida,
                                                                        New SelectOption With {.Value = scMonedaPrecioUnitarioPartida.Value,
                                                                                               .Text = scMonedaPrecioUnitarioPartida.Text})

                                               If unidades_.Count = 0 Then

                                                   unidades_ = _controladorUnidadesMedida.BuscarUnidadesCOVE("PIEZA", 1)

                                               End If

                                               unidades_ = unidades_.Union(unidadesT_).ToList

                                               unidadesT_ = unidades_

                                               Dim ltselecoption_ = New List(Of SelectOption)

                                               For Each ltunidad_ In unidades_

                                                   ltselecoption_.Add(New SelectOption With {.Value = ltunidad_._id.ToString,
                                                                                             .Text = ltunidad_.nombreoficialesp.ToUpper})

                                               Next

                                               scUnidadAcuseValor.DataSource = ltselecoption_

                                               scUnidadAcuseValor.Value = unidades_(0)._id.ToString

                                               pillbox_.SetControlValue(scUnidadAcuseValor,
                                                                        New SelectOption With {.Value = scUnidadAcuseValor.Value,
                                                                                               .Text = scUnidadAcuseValor.Text})

                                               pillbox_.SetControlValue(icNumeroSerieAcuseValor, numeroSerie_)

                                               pillbox_.SetControlValue(icMarcaAcuseValor, marca_)

                                               pillbox_.SetControlValue(icModeloAcuseValor, modelo_)

                                               pillbox_.SetControlValue(icSubmodeloAcuseValor, subModelo_)

                                           End Sub)

                indice_ += 1
            Next

            pbPartidasAcuseValor = pillboxControl_

            pbPartidasAcuseValor.PillBoxDataBinding()

            Dim datos = New List(Of Dictionary(Of String, Object))

            Dim identidad_ As Int32 = 1


            pbPartidasAcuseValor.DataSource.ToList().ForEach(Sub(c As Dictionary(Of String, Object))

                                                                 c.Item(pbPartidasAcuseValor.KeyField) = 0

                                                                 c.Add("identidad", Str(identidad_))

                                                                 c.Add("estado", 1)

                                                                 datos.Add(c)

                                                                 identidad_ = identidad_ + 1

                                                             End Sub)

            pbPartidasAcuseValor.DataSource = datos

            MostrarFactor()

            BloqueaObligatoriosFactura()

            DisplayMessage("Factura  " &
                       Chr(34) &
                       dbcNumFacturaAcuseValor.Value.ToString &
                       Chr(34) &
                       " cargada satisfactoriamente",
                       StatusMessage.Info)


        End If

    End Sub

    Public Sub dbc_NumFacturaAcuseValor_Click(sender As Object, e As EventArgs)

        'Dim monedas_ = _icontroladorMonedas.BuscarMonedas(token_:="", formato_:="cveISO4217", 181)

        If Not Checallenado() Then

            If dbcNumFacturaAcuseValor.Value.ToString <> "" Then

                _icontroladorFactura.EnvironmentOnline = ListaEmpresas.Value

                Dim tagwatcher_ = _icontroladorFactura.ListaCamposFacturaComercial(dbcNumFacturaAcuseValor.Value.ToString,
                                                                                  New Dictionary(Of [Enum], List(Of [Enum])) _
                        From {{SeccionesFacturaComercial.SFAC1, New List(Of [Enum]) From {CamposFacturaComercial.CA_NUMERO_FACTURA,
                                                                                          CamposFacturaComercial.CA_FECHA_FACTURA,
                                                                                          CamposFacturaComercial.CP_TIPO_OPERACION,
                                                                                          CamposClientes.CP_OBJECTID_CLIENTE,
                                                                                          CamposClientes.CA_RAZON_SOCIAL,
                                                                                          CamposClientes.CA_TAX_ID,
                                                                                          CamposClientes.CA_RFC_CLIENTE,
                                                                                          CamposFacturaComercial.CA_MONEDA_FACTURACION,
                                                                                          CamposFacturaComercial.CP_APLICA_ENAJENACION,
                                                                                          CamposFacturaComercial.CA_APLICA_SUBDIVISION
                                }},
                                {SeccionesFacturaComercial.SFAC2, New List(Of [Enum]) From {CamposProveedorOperativo.CP_ID_PROVEEDOR,
                                                                                            CamposProveedorOperativo.CA_RAZON_SOCIAL_PROVEEDOR,
                                                                                            CamposProveedorOperativo.CA_TAX_ID_PROVEEDOR,
                                                                                            CamposProveedorOperativo.CA_RFC_PROVEEDOR
                                }},
                                {SeccionesFacturaComercial.SFAC4, Nothing}})

                Dim resultado_ As Dictionary(Of String, List(Of Nodo)) = tagwatcher_.ObjectReturned



                If resultado_ IsNot Nothing Then

                    If resultado_(dbcNumFacturaAcuseValor.Value.ToString).Count > 0 Then

                        Dim listaresultado_ = resultado_(dbcNumFacturaAcuseValor.Value.ToString)

                        If listaresultado_.Count > 16 Then

                            CargarFacturasRepetidas(listaresultado_)

                        Else

                            CargarFacturas(listaresultado_)


                        End If




                    Else

                        DisplayMessage("No se encontró un Documento con Folio " &
                               Chr(34) &
                               dbcNumFacturaAcuseValor.Value.ToString &
                               Chr(34) & "En esta Oficina",
                               StatusMessage.Fail)

                    End If


                End If



            Else

                DisplayMessage("Falta especificar el Folio del Documento",
                               StatusMessage.Fail)

            End If


        End If

    End Sub

    Public Sub sc_TipoDocumento_Click(sender As Object, e As EventArgs)

    End Sub

    Public Sub sc_TipoMoneda_Click(sender As Object, e As EventArgs)


        Dim monedas_ = _icontroladorMonedas.
                       BuscarMonedas(New List(Of String) From {"USD",
                                                               "EUR",
                                                               "MXN",
                                                               "CHF",
                                                               "JPY",
                                                               "CNY"},,
                                                               "cveISO4217")

        sender.dataSource = _organismo.ObtenerSelectOption(scTipoMoneda,
                                                           monedas_.
                                                           Select(Of ValorProvisionalOption)(Function(chi) New ValorProvisionalOption With {
                                                          .Id = chi._id,
                                                          .Valor = chi.nombremonedaesp &
                                                          " | " &
                                                          chi.aliasmoneda.Find(Function(ch) ch.Clave = "cveISO4217").Valor
                                                           }).ToList)

        SetVars("_Monedas", monedas_)

    End Sub

    Public Sub sc_TipoMoneda_SelectedIndesxChanged(sender As Object, e As EventArgs)

        scMonedaPrecioUnitarioPartida.DataSource = scTipoMoneda.DataSource

        scMonedaPrecioUnitarioPartida.Value = scTipoMoneda.Value

        MostrarFactor()

    End Sub
    Public Sub sc_TipoMoneda_TextChanged(sender As Object, e As EventArgs)

        Dim monedas_ = _icontroladorMonedas.BuscarMonedas(CType(sender.SuggestedText, String),,
                                                          "cveISO4217",
                                                          7)

        sender.dataSource = _organismo.ObtenerSelectOption(sender,
                                                           monedas_.
                                                           Select(Of ValorProvisionalOption)(Function(chi) New ValorProvisionalOption With {
                                                          .Id = chi._id,
                                                          .Valor = chi.nombremonedaesp &
                                                          " | " &
                                                          chi.aliasmoneda.Find(Function(ef) ef.Clave = "cveISO4217").Valor
                                                          }).ToList)
        SetVars("_Monedas", monedas_)



    End Sub



    Public Sub swc_CertificadoOrigen_CheckedChanged(sender As Object, e As EventArgs)

        icExpotadorAutorizado.Visible = swcCertificadoOrigen.Checked

    End Sub

    Protected Sub pb_PartidasAcuseValor_CheckedChange(sender As Object, e As EventArgs)

    End Sub

    Protected Sub pb_PartidasAcuseValor_Click(sender As Object, e As EventArgs)

        Select Case pbPartidasAcuseValor.ToolbarAction

            Case PillboxControl.ToolbarActions.Nuevo

                'OBTENEMOS LA MONEDA UTILIZADA EN LA PÁGINA ANTERIOR DEL PASTILLERO POR SI ES LA QUE VA USAR EL USUARIO
                Dim sc_ValorAnterior_ = pbPartidasAcuseValor.DataSource(pbPartidasAcuseValor.PageIndex - 2)

                Dim sValue_ As String = ""

                Dim sText_ As String = ""

                For Each sc_Valor In sc_ValorAnterior_

                    If sc_Valor.Key = "scMonedaPrecioUnitarioPartida" Then

                        Dim scadena_ = sc_Valor.ToJson.ToString

                        scadena_ = scadena_.Substring(scadena_.IndexOf("Value"))

                        Dim indice_ = scadena_.IndexOf(":") + 3

                        Dim indicet_ = scadena_.IndexOf(",")

                        sValue_ = scadena_.Substring(indice_, indicet_ - indice_ - 1)

                        scadena_ = scadena_.Substring(scadena_.IndexOf("Text"))

                        indice_ = scadena_.IndexOf(":") + 3

                        indicet_ = scadena_.IndexOf("}") - 1

                        sText_ = scadena_.Substring(indice_, indicet_ - indice_ - 1)

                    End If

                Next

                scMonedaPrecioUnitarioPartida.DataSource = New List(Of SelectOption) From {New SelectOption With {.Value = sValue_,
                                                                                                                   .Text = sText_}}

                scMonedaPrecioUnitarioPartida.Value = scMonedaPrecioUnitarioPartida.DataSource(0).Value

                'OBTENEMOS LA UNIDAD DE MEDIDA UTILIZADA EN LA PÁGINA ANTERIOR DEL PASTILLERO POR SI ES LA QUE VA USAR EL USUARIO

                sValue_ = ""

                sText_ = ""

                For Each sc_Valor In sc_ValorAnterior_

                    If sc_Valor.Key = "scUnidadAcuseValor" Then

                        Dim scadena_ = sc_Valor.ToJson.ToString

                        scadena_ = scadena_.Substring(scadena_.IndexOf("Value"))

                        Dim indice_ = scadena_.IndexOf(":") + 3

                        Dim indicet_ = scadena_.IndexOf(",")

                        sValue_ = scadena_.Substring(indice_, indicet_ - indice_ - 1)

                        scadena_ = scadena_.Substring(scadena_.IndexOf("Text"))

                        indice_ = scadena_.IndexOf(":") + 3

                        indicet_ = scadena_.IndexOf("}") - 1

                        sText_ = scadena_.Substring(indice_, indicet_ - indice_ - 1)

                    End If

                Next

                scUnidadAcuseValor.DataSource = New List(Of SelectOption) From {New SelectOption With {.Value = sValue_,
                                                                                                        .Text = sText_}}

                scUnidadAcuseValor.Value = scUnidadAcuseValor.DataSource(0).Value

                lbNumeroAcuseValor.Text = (pbPartidasAcuseValor.PageIndex).ToString()

            Case PillboxControl.ToolbarActions.Borrar

            Case PillboxControl.ToolbarActions.Archivar

            Case Else

        End Select

    End Sub

    Protected Sub fbc_Proveedor_TextChanged(sender As Object, e As EventArgs)

        If swcTipoOperacion.Checked Then

            _controladorProveedor._TipoOperacion = CtrlProveedoresOperativos.TipoOperacion.Importacion

            If sender.Text <> "" Then

                Dim busqueda_ = sender.Text.ToString

                Dim spacePosition_ = busqueda_.IndexOf(" ")

                If spacePosition_ >= 0 Then

                    busqueda_ = busqueda_.Substring(0, spacePosition_ - 1)

                End If

                Dim proveedores_ As List(Of AuxiliarProveedor) = _controladorProveedor.ObtenerProveedoresPorRazonSocialPais(busqueda_).ObjectReturned

                Dim lista_ = New List(Of SelectOption)

                If proveedores_ IsNot Nothing Then

                    For Each proveedor_ In proveedores_

                        lista_.Add(New SelectOption With {.Value = proveedor_.idtaxid, .Text = proveedor_._razonsocial})

                    Next

                    SetVars("_proveedores", proveedores_)

                Else

                    icDireccionProveedor.Value = ""

                    icIDFiscalProveedor.Value = ""

                End If

                sender.DataSource = lista_

            End If

        Else


            Dim ConstructorCliente_ As New ControladorBusqueda(Of ConstructorCliente)

            Dim listaClientes_ As List(Of SelectOption) = ConstructorCliente_.Buscar(sender.Text,
                                                                              New Filtro With {.IdSeccion = SeccionesClientes.SCS1, .IdCampo = CamposClientes.CA_RAZON_SOCIAL})
            sender.DataSource = listaClientes_

            If listaClientes_ Is Nothing Then

                icDireccionDestinatario.Value = ""

                icIDFiscalDestinatario.Value = ""

            End If

        End If




    End Sub


    Protected Sub fbc_Proveedor_Click(sender As Object, e As EventArgs)

        If sender.Value.ToString <> "" Then
            'Aquí buscamos al proveedor y su dirección

            If swcTipoOperacion.Checked Then

                Select Case sender.ID

                    Case "fbcProveedor"

                        ClickProveedores(sender)

                    Case "fbcDestinatario"

                        ClickClientes(sender)

                    Case Else

                End Select

            Else

                Select Case sender.ID

                    Case "fbcProveedor"

                        ClickClientes(sender)

                    Case "fbcDestinatario"

                        ClickProveedores(sender)
                    Case Else

                End Select

            End If

        Else

            Select Case sender.ID

                Case "fbcProveedor"

                    icDireccionProveedor.Value = ""

                    icIDFiscalProveedor.Value = ""

                Case "fbcDestinatario"

                    icDireccionDestinatario.Value = ""

                    icIDFiscalDestinatario.Value = ""

                Case Else

            End Select

        End If

    End Sub


    Public Sub fbc_Proveedor_ClickClose(sender_ As Object, event_ As EventArgs)

        icDireccionProveedor.Value = ""

        icIDFiscalProveedor.Value = ""

    End Sub

    Sub ClickProveedores(sender_ As FindboxControl)

        Dim proveedores_ As List(Of AuxiliarProveedor) = GetVars("_proveedores")

        Dim buscarProveedor_ As New ControladorBusqueda(Of ConstructorProveedoresOperativos)

        Dim proveedor_ = proveedores_.Find(Function(ch) ch.idtaxid = sender_.Value)

        'SE USARÁ ObtenerProveedoresPorRazonSocialPais

        SetVars("_ProveedorAcuseValor", proveedor_)

        If proveedor_ IsNot Nothing Then

            If swcTipoOperacion.Checked Then

                icDireccionProveedor.Value = proveedor_._domicilio.domicilioPresentacion

                If proveedor_._taxid <> "" Then

                    icIDFiscalProveedor.Value = proveedor_._taxid

                End If

            Else

                icDireccionDestinatario.Value = proveedor_._domicilio.domicilioPresentacion


                If proveedor_._taxid <> "" Then

                    icIDFiscalDestinatario.Value = proveedor_._taxid

                Else

                    icIDFiscalDestinatario.Value = proveedor_._rfc

                End If

            End If

        Else

            If swcTipoOperacion.Checked Then

                icDireccionProveedor.Value = ""

                icIDFiscalProveedor.Value = ""


            Else

                icDireccionDestinatario.Value = ""


                icIDFiscalDestinatario.Value = ""

            End If

        End If

    End Sub

    Sub ClickClientes(sender As FindboxControl)

        If sender.Text.ToString <> "" Then

            Dim buscarCliente_ As New ControladorBusqueda(Of ConstructorCliente)

            Dim tagwatcherCliente_ As TagWatcher = buscarCliente_.ObtenerDocumento(sender.Value)

            If tagwatcherCliente_.Status = TypeStatus.Ok Then

                Dim clienteAcuseValor_ As ConstructorCliente = DirectCast(tagwatcherCliente_.ObjectReturned.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente, ConstructorCliente)

                clienteAcuseValor_.Id = tagwatcherCliente_.ObjectReturned.iD.ToString

                SetVars("_ClienteAcuseValor", clienteAcuseValor_)

                With clienteAcuseValor_

                    If .NombreCliente IsNot Nothing Then

                        If swcTipoOperacion.Checked Then

                            icDireccionDestinatario.Value = .Seccion(SeccionesClientes.SCS1).
                                              Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor

                            If .Seccion(SeccionesClientes.SCS1).Campo(CamposClientes.CA_TAX_ID).Valor IsNot Nothing Then

                                If .Seccion(SeccionesClientes.SCS1).Campo(CamposClientes.CA_TAX_ID).Valor = "" Then

                                    icIDFiscalDestinatario.Value = .Seccion(SeccionesClientes.SCS1).
                                                                                 Campo(CamposClientes.CA_RFC_CLIENTE).Valor

                                Else

                                    icIDFiscalDestinatario.Value = .Seccion(SeccionesClientes.SCS1).
                                                                                 Campo(CamposClientes.CA_TAX_ID).Valor

                                End If

                            Else

                                If .Seccion(SeccionesClientes.SCS1).
                                                Campo(CamposClientes.CA_RFC_CLIENTE).Valor IsNot Nothing Then

                                    icIDFiscalDestinatario.Value = .Seccion(SeccionesClientes.SCS1).
                                                                                 Campo(CamposClientes.CA_RFC_CLIENTE).Valor

                                End If

                            End If

                        Else

                            icDireccionProveedor.Value = .Seccion(SeccionesClientes.SCS1).
                                              Campo(CamposDomicilio.CA_DOMICILIO_FISCAL).Valor

                            If .Seccion(SeccionesClientes.SCS1).Campo(CamposClientes.CA_TAX_ID).Valor IsNot Nothing Then

                                If .Seccion(SeccionesClientes.SCS1).Campo(CamposClientes.CA_TAX_ID).Valor = "" Then

                                    icIDFiscalProveedor.Value = .Seccion(SeccionesClientes.SCS1).
                                                                                 Campo(CamposClientes.CA_RFC_CLIENTE).Valor

                                Else

                                    icIDFiscalProveedor.Value = .Seccion(SeccionesClientes.SCS1).
                                                                                 Campo(CamposClientes.CA_TAX_ID).Valor

                                End If

                            Else

                                If .Seccion(SeccionesClientes.SCS1).
                                                Campo(CamposClientes.CA_RFC_CLIENTE).Valor IsNot Nothing Then

                                    icIDFiscalProveedor.Value = .Seccion(SeccionesClientes.SCS1).
                                                                                 Campo(CamposClientes.CA_RFC_CLIENTE).Valor

                                End If

                            End If


                        End If

                        scSelloCliente.DataSource = New List(Of SelectOption) From {New SelectOption With {.Value = clienteAcuseValor_.Id.ToString, .Text = icIDFiscalDestinatario.Value.ToString}, New SelectOption With {.Value = "0", .Text = "AGENTE ADUANAL"}}

                        scSelloCliente.Value = clienteAcuseValor_.Id.ToString


                        If .Seccion(SeccionesClientes.SCS2).
                                        Campo(CamposClientes.CP_CVE_PATENTE_ADUANAL) IsNot Nothing Then

                            If .Seccion(SeccionesClientes.SCS2).
                                        Campo(CamposClientes.CP_CVE_PATENTE_ADUANAL).ValorPresentacion IsNot Nothing Then



                                Dim patente_ As String = ""

                                Dim modalidadAduana_ = ""

                                For Each campo_ In .Seccion(SeccionesClientes.SCS2).Nodos

                                    modalidadAduana_ = campo_.Campo(CamposClientes.CP_CVE_ADUANA_SECCION).ValorPresentacion

                                    If modalidadAduana_.Contains(ListaEmpresas.Text) Then

                                        patente_ = campo_.
                                        Campo(CamposClientes.CP_CVE_PATENTE_ADUANAL).Valor

                                        Exit For

                                    End If

                                Next

                                icPatenteAduanal.Value = patente_

                                patente_ = If(patente_ = "", patente_, patente_.Substring(0, 4))

                                ccDatosConsulta.ClearRows()

                                Dim rfcAgente_ = ""

                                Dim services_ = Statements.GetService("project", 18, Sax.SaxStatements.SettingTypes.Projects, 3)

                                Dim key_ = services_.environmentsettings(0).claims(0).key

                                Dim value_ = services_.environmentsettings(0).claims(0).value
                                Dim sdk_ As IControllerCustomsSettingsClient = New ControllerCustomsSettingsClient(
                                    apiKey_:=value_,
                                   version_:=key_
                                    )

                                Try

                                    If patente_ <> "" Then

                                        Dim dto_ = sdk_.GetBrokerConfiguration(GetEnvironment(__SYSTEM_ENVIRONMENT.Value), ListaEmpresas.Value, patente_)

                                        rfcAgente_ = dto_.Data.customsOfficeGeneralData.agentTaxId

                                        ccDatosConsulta.SetRow(Sub(catalogRow_ As CatalogRow)

                                                                   'Define el valor Llave de tu fila

                                                                   Dim loginUsuario_ As Dictionary(Of String, String) = Session("DatosUsuario")

                                                                   catalogRow_.SetIndice(ccDatosConsulta.KeyField, 0)

                                                                   'Define el valor de una columna de la fila

                                                                   'Dim optionList_ = New List(Of SelectOption) From {New SelectOption With {.Value = "HOLA", .Text = "MUNDO"},
                                                                   '                                                           New SelectOption With {.Value = "HOLA2", .Text = "MUNDO2"},
                                                                   '                                                           New SelectOption With {.Value = "HOLA3", .Text = "MUNDO3"}}

                                                                   Dim icRFCConsulta_ As New InputControl With {.ID = "icRFCConsulta",
                                                                                                                           .Value = rfcAgente_,
                                                                                                                           .Type = InputControl.InputType.Text}

                                                                   Dim icRazonSocialConsulta_ As New InputControl With {.ID = "icRazonSocialConsulta",
                                                                                                                           .Value = "",
                                                                                                                           .Type = InputControl.InputType.Text}

                                                                   Dim icEmailConsulta As New InputControl With {.ID = "icEmailConsulta",
                                                                                                                           .Value = loginUsuario_("WebServiceUserID"),
                                                                                                                           .Type = InputControl.InputType.Text}


                                                                   catalogRow_.SetColumn(icRFCConsulta_, rfcAgente_)

                                                                   catalogRow_.SetColumn(icRazonSocialConsulta_, "")

                                                                   catalogRow_.SetColumn(icEmailConsulta, loginUsuario_("WebServiceUserID"))

                                                               End Sub)
                                    Else

                                        DisplayMessage("Este cliente no tiene patente dada de alta para esta aduana\modalidad", StatusMessage.Info)

                                    End If

                                Catch ex As Exception

                                    rfcAgente_ = ""

                                End Try


                                ccDatosConsulta.CatalogDataBinding()

                            Else

                                icPatenteAduanal.Value = ""

                            End If

                        Else

                            icPatenteAduanal.Value = ""

                        End If

                    End If

                End With

            End If

        Else

            If swcTipoOperacion.Checked Then

                icDireccionDestinatario.Value = ""


                icIDFiscalDestinatario.Value = ""


            Else



                icDireccionProveedor.Value = ""

                icIDFiscalProveedor.Value = ""

            End If

        End If

    End Sub

    Protected Sub fbc_Destinatario_TextChanged(sender As Object, e As EventArgs)

        If swcTipoOperacion.Checked Then

            Dim ConstructorCliente_ As New ControladorBusqueda(Of ConstructorCliente)

            Dim listaClientes_ As List(Of SelectOption) = ConstructorCliente_.Buscar(sender.Text,
                                                                                  New Filtro With {.IdSeccion = SeccionesClientes.SCS1, .IdCampo = CamposClientes.CA_RAZON_SOCIAL})
            sender.DataSource = listaClientes_

        Else

            _controladorProveedor._TipoOperacion = CtrlProveedoresOperativos.TipoOperacion.Exportacion

            If sender.Text <> "" Then

                Dim busqueda_ = sender.Text.ToString

                Dim spacePosition_ = busqueda_.IndexOf(" ")

                If spacePosition_ >= 0 Then

                    busqueda_ = busqueda_.Substring(0, spacePosition_ - 1)

                End If

                Dim proveedores_ As List(Of AuxiliarProveedor) = _controladorProveedor.ObtenerProveedoresPorRazonSocialPais(busqueda_).ObjectReturned

                Dim lista_ = New List(Of SelectOption)

                If proveedores_ IsNot Nothing Then

                    For Each proveedor_ In proveedores_

                        lista_.Add(New SelectOption With {.Value = proveedor_.id, .Text = proveedor_._razonsocial})

                    Next

                    SetVars("_proveedores", proveedores_)

                End If

                sender.DataSource = lista_

            End If

        End If



    End Sub


    Protected Sub sc_UnidadAcuseValor_TextChanged(sender As Object, e As EventArgs)

        Dim listaUnidadesComponente_ As List(Of SelectOption)

        Dim listaUnidadMedida_ As List(Of UnidadMedida) = _controladorUnidadesMedida.BuscarUnidadesCOVE(sender.SuggestedText)

        If listaUnidadMedida_ Is Nothing Then

            listaUnidadesComponente_ = New List(Of SelectOption)

        Else

            listaUnidadesComponente_ = _organismo.ObtenerSelectOption(scUnidadAcuseValor,
                                                                      listaUnidadMedida_.
                                                                      Select(Function(chi) New ValorProvisionalOption With {
                                                                        .Id = chi._id,
                                                                        .Valor = chi.nombreoficialesp
                                                                      }).ToList)

        End If

        sender.Datasource = listaUnidadesComponente_



    End Sub


    Protected Sub sc_UnidadAcuseValor_Click(sender As Object, e As EventArgs)

        Dim listaunidades As List(Of SelectOption)

        listaunidades = _organismo.ObtenerSelectOption(scUnidadAcuseValor,
                                                       _controladorUnidadesMedida.
                                                       BuscarUnidadesCOVE(New List(Of String) From {"C62_1",
                                                                                                      "KGM",
                                                                                                      "CS",
                                                                                                      "SET",
                                                                                                      "C62_2",
                                                                                                      "KT",
                                                                                                      "TNE",
                                                                                                      "LM",
                                                                                                      "MIL",
                                                                                                      "MQ",
                                                                                                      "MTK",
                                                                                                      "BX",
                                                                                                      "LTR",
                                                                                                      "GRM"}).
                                                                                                      Select(Of ValorProvisionalOption)(Function(chi) New ValorProvisionalOption With {
                                                                                                          .Id = chi._id,
                                                                                                          .Valor = chi.nombreoficialesp
                                                                                                      }).ToList)

        sender.DataSource = listaunidades



    End Sub


    Public Function Checallenado() As Boolean

        Dim bchecador_ As Boolean = True

        If fbcProveedor.Value.ToString = "" Then

            bchecador_ = False

        End If

        Return bchecador_

    End Function

    Public Sub LimpiarTodo()

        Dim listaSelectOption_ As List(Of SelectOption)

        dbcNumFacturaAcuseValor.Value = ""

        dbcNumFacturaAcuseValor.ValueDetail = ""

        swcCertificadoOrigen.Value = False

        swcTipoOperacion.Checked = True

        swcRelacionFactura.Checked = False

        swcSubdivision.Checked = False

        scTipoDocumento.Value = "1"

        icFechaExpedicion.Value = DateTime.UtcNow.Date.ToString("yyyy-MM-dd")

        Dim monedas_ As List(Of MonedaGlobal) = GetVars("_Monedas")


        monedas_ = _icontroladorMonedas.BuscarMonedas(New List(Of String) From {"USD",
                                                                                "EUR",
                                                                                "MXN",
                                                                                "CHF",
                                                                                "JPY",
                                                                                "CNY"},,
                                                                                "cveISO4217")

        scTipoMoneda.DataSource = _organismo.ObtenerSelectOption(scTipoMoneda,
                                                                  monedas_.
                                                                  Select(Of ValorProvisionalOption)(Function(e) New ValorProvisionalOption With {
                                                                 .Id = e._id,
                                                                 .Valor = e.nombremonedaesp &
                                                                  " | " &
                                                                  e.aliasmoneda.Find(Function(ef) ef.Clave = "cveISO4217").Valor
                                                                  }).ToList)

        scMonedaPrecioUnitarioPartida.DataSource = scTipoMoneda.DataSource

        scTipoMoneda.Value = scTipoMoneda.DataSource(0).Value

        scMonedaPrecioUnitarioPartida.Value = scTipoMoneda.DataSource(0).Value

        listaSelectOption_ = _organismo.ObtenerSelectOption(scUnidadAcuseValor, _controladorUnidadesMedida.BuscarUnidadesCOVE(New List(Of String) From {"C62_1", "KGM", "CS", "SET", "C62_2", "KT", "TNE", "LM", "MIL", "MQ", "MTK", "BX", "LTR", "GRM"}).Select(Of ValorProvisionalOption)(Function(e) New ValorProvisionalOption With {
                             .Id = e._id,
                            .Valor = e.nombreoficialesp
                           }).ToList)

        icObservaciones.Value = ""

        icExpotadorAutorizado.Value = ""

        icDireccionProveedor.Value = ""

        fbcProveedor.Text = ""

        fbcProveedor.Value = ""

        icIDFiscalProveedor.Value = ""

        fbcDestinatario.Text = ""

        fbcDestinatario.Value = ""

        icIDFiscalDestinatario.Value = ""

        icDireccionDestinatario.Value = ""

        scUnidadAcuseValor.DataSource = listaSelectOption_

        scUnidadAcuseValor.Value = listaSelectOption_(0).Value

        icDescripcionAcuseValor.Value = ""

        icCantidadAcuseValor.Value = ""

        icPrecioUnitarioAcuseValor.Value = ""

        icValorFacturaPartida.Value = ""

        icValorDolaresPartida.Value = ""

        icMarcaAcuseValor.Value = ""

        icModeloAcuseValor.Value = ""

        icSubmodeloAcuseValor.Value = ""

        icNumeroSerieAcuseValor.Value = ""

        icPatenteAduanal.Value = ""

        scSelloCliente.DataSource = New List(Of SelectOption)

        ccDatosConsulta.ClearRows()

        DesbloqueaObligatoriosFactura()

        MostrarFactor()

        icDireccionProveedor.Enabled = False

        icIDFiscalProveedor.Enabled = False

        icIDFiscalDestinatario.Enabled = False

        icDireccionDestinatario.Enabled = False

    End Sub

    Public Sub MostrarFactor()

        Dim monedas_ As List(Of MonedaGlobal) = GetVars("_Monedas")

        If monedas_ IsNot Nothing Then

            Dim moneda_ = monedas_.Find(Function(ch) ch._id.ToString = scTipoMoneda.Value.ToString)

            If moneda_ IsNot Nothing Then

                If moneda_.aliasmoneda.FindAll(Function(ch) ch.Valor = "MXN").Count > 0 Then

                    scTipoMoneda.ToolTip = "Factor: $" &
                                            (1 / moneda_.tiposdecambio(0).valordefault).ToString("C") &
                                            " al " &
                                             moneda_.tiposdecambio(0).fecha.ToString("dd-MM-yyyy")

                Else


                    scTipoMoneda.ToolTip = "Factor: $" &
                                            moneda_.factoresmoneda(0).valordefault &
                                            " al " &
                                            moneda_.factoresmoneda(0).fecha.ToString("dd-MM-yyyy")

                End If

                scTipoMoneda.ToolTipExpireTime = 0
                scTipoMoneda.ToolTipStatus = IUIControl.ToolTipTypeStatus.OkInfo
                scTipoMoneda.ToolTipModality = IUIControl.ToolTipModalities.Ondemand
                scTipoMoneda.ShowToolTip()

            End If



        End If

    End Sub

    Public Sub BloqueaObligatoriosFactura()


        dbcNumFacturaAcuseValor.Enabled = False

        swcTipoOperacion.Enabled = False

        ' scTipoDocumento.Enabled = False

        scTipoMoneda.Enabled = False

        icFechaExpedicion.Enabled = False

        swcSubdivision.Enabled = False

        fbcProveedor.Enabled = False

        icDireccionProveedor.Enabled = False

        icIDFiscalProveedor.Enabled = False

        fbcDestinatario.Enabled = False

        icIDFiscalDestinatario.Enabled = False

        icDireccionDestinatario.Enabled = False

        scMonedaPrecioUnitarioPartida.Enabled = False

        icValorFacturaPartida.Enabled = False

        scSelloCliente.Enabled = False

        icPatenteAduanal.Enabled = False

    End Sub

    Public Sub DesbloqueaObligatoriosFactura()


        dbcNumFacturaAcuseValor.Enabled = True

        swcTipoOperacion.Enabled = True

        scTipoDocumento.Enabled = True

        scTipoMoneda.Enabled = True

        icFechaExpedicion.Enabled = True

        swcSubdivision.Enabled = True

        fbcProveedor.Enabled = True

        fbcDestinatario.Enabled = True

        scMonedaPrecioUnitarioPartida.Enabled = True

        icValorFacturaPartida.Enabled = True

        scSelloCliente.Enabled = True

        icPatenteAduanal.Enabled = True

        icDireccionProveedor.Enabled = False

        icIDFiscalProveedor.Enabled = False

        icIDFiscalDestinatario.Enabled = False

        icDireccionDestinatario.Enabled = False

    End Sub

    Sub EnroqueProveedorCliente(sender_ As Object, event_ As EventArgs)

        'Dim destinatarioValue_ = fbcDestinatario.Value

        'Dim destinatarioText_ = fbcDestinatario.Text

        'Dim destinatarioDomicilio_ = icDireccionDestinatario.Value

        'Dim destinatarioFiscal_ = icIDFiscalDestinatario.Value

        'Dim emisorValue_ = fbcProveedor.Value

        'Dim emisorText_ = fbcProveedor.Text

        'Dim emisorDomicilio_ = icDireccionProveedor.Value

        'Dim emisorFiscal_ = icIDFiscalProveedor.Value

        'If destinatarioValue_ <> "" And destinatarioValue_ <> Nothing Then

        '    fbcProveedor.DataSource = New List(Of SelectOption) From
        '                                                              {New SelectOption With
        '                                                                                {.Value = destinatarioValue_,
        '                                                                                .Text = destinatarioText_}}
        '    fbcProveedor.Value = destinatarioValue_

        '    icDireccionProveedor.Value = destinatarioDomicilio_

        '    icIDFiscalProveedor.Value = destinatarioFiscal_


        'End If

        'If emisorValue_ <> "" And emisorValue_ <> Nothing Then

        '    fbcDestinatario.DataSource = New List(Of SelectOption) From
        '                                                              {New SelectOption With
        '                                                                                {.Value = emisorValue_,
        '                                                                                .Text = emisorText_}}
        '    fbcDestinatario.Value = emisorValue_

        '    icDireccionDestinatario.Value = emisorDomicilio_

        '    icIDFiscalDestinatario.Value = emisorFiscal_


        'End If


    End Sub

    Public Sub CalcularTotal(sender_ As Object, event_ As EventArgs)

        If icCantidadAcuseValor.Value IsNot Nothing AndAlso
            icCantidadAcuseValor.Value <> "" AndAlso
            icPrecioUnitarioAcuseValor.Value IsNot Nothing AndAlso
            icPrecioUnitarioAcuseValor.Value <> "" Then

            Dim cantidad_, precioUnitario As Double

            If Double.TryParse(icCantidadAcuseValor.Value, cantidad_) AndAlso Double.TryParse(icPrecioUnitarioAcuseValor.Value.Replace(",", "").Replace("$", ""), precioUnitario) Then

                icValorFacturaPartida.Value = (cantidad_ * precioUnitario).ToString

                Dim nombreMoneda_ = scMonedaPrecioUnitarioPartida.Text.Split("|")

                Dim claveMoneda_ As String

                If nombreMoneda_.Count = 2 Then

                    claveMoneda_ = nombreMoneda_(1).Trim

                Else

                    claveMoneda_ = nombreMoneda_(0).Trim

                End If

                Dim factores_ = _icontroladorMonedas.ObtenerFactorCambio(claveMoneda_, fechaCambio_:=icFechaExpedicion.Value.Replace("-", "/"))

                If factores_ IsNot Nothing AndAlso factores_.Count > 0 Then

                    icValorDolaresPartida.Value = (cantidad_ * precioUnitario * factores_(0).factor).ToString

                End If

            End If

        End If


    End Sub

    Function GetEnvironment(empresa_ As Int16)

        Select Case empresa_

            Case 1, 2, 3, 8, 10

                Return 1

            Case 5, 6

                Return 2

            Case 4, 7, 8

                Return 3

            Case Else

                Return 1

        End Select

    End Function

#End Region

#Region "██████ Vinculación sexta capa  █████████       SAX      ████████████████████████████████████████████"
    '    ██                                                                                                ██
    '    ██ Defina en esta región todo lo que involucre el uso de controladores externos al contexto actual██
    '    ██                                                                                                ██
    '    ████████████████████████████████████████████████████████████████████████████████████████████████████


#End Region


End Class
