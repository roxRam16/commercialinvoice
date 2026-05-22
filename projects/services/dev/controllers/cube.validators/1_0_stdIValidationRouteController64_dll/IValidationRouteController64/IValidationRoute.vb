Imports System.ComponentModel
Imports System.Runtime.Serialization
Imports Syn.Documento
Imports Wma.Exceptions

Public Interface IValidationRoute : Inherits IDisposable


#Region "Enum"
    Enum ValidationRoutes
        Undefined = 0
        <EnumMember> <Description("RUTA DE PEDIMENTO DE IMPORTACIÓN NORMAL POR DEFECTO  CON SYNAPSIS QUALITY")> RUVA1 = 1
        <EnumMember> <Description("RUTA DE PEDIMENTO DE IMPORTACIÓN NORMAL GLOBAL COMPLEMENTARIO CON SYNAPSIS QUALITY")> RUVA2 = 2
        <EnumMember> <Description("RUTA DE PEDIMENTO DE IMPORTACIÓN NORMAL PEDIMENTO CONSOLIDADO CON SYNAPSIS QUALITY")> RUVA3 = 3
        <EnumMember> <Description("RUTA DE PEDIMENTO DE EXPORTACIÓN NORMAL POR DEFECTO CON SYNAPSIS QUALITY")> RUVA4 = 4
        <EnumMember> <Description("RUTA DE PEDIMENTO DE EXPORTACIÓN NORMAL PEDIMENTO CONSOLIDADO CON SYNAPSIS QUALITY")> RUVA5 = 5
        <EnumMember> <Description("RUTA DE PEDIMENTO DE EXPORTACIÓN NORMAL GLOBAL COMPLEMENTARIO CON SYNAPSIS QUALITY")> RUVA6 = 6
        <EnumMember> <Description("RUTA DE PEDIMENTO DE EXPORTACIÓN NORMAL COMPLEMENTARIO CON SYNAPSIS QUALITY")> RUVA7 = 7
        <EnumMember> <Description("RUTA DE PEDIMENTO DE TRÁNSITO NORMAL POR DEFECTO CON SYNAPSIS QUALITY")> RUVA8 = 8
        <EnumMember> <Description("RUTA DE PEDIMENTO DE EXPORTACIÓN VIRTUAL POR DEFECTO CON SYNAPSIS QUALITY")> RUVA9 = 9
        <EnumMember> <Description("RUTA DE PEDIMENTO DE EXPORTACIÓN VIRTUAL PEDIMENTO CONSOLIDADO CON SYNAPSIS QUALITY")> RUVA10 = 10
        <EnumMember> <Description("RUTA DE PEDIMENTO DE IMPORTACIÓN VIRTUAL POR DEFECTO CON SYNAPSIS QUALITY")> RUVA11 = 11
        <EnumMember> <Description("RUTA DE PEDIMENTO DE IMPORTACIÓN VIRTUAL PEDIMENTO CONSOLIDADO CON SYNAPSIS QUALITY")> RUVA12 = 12
        <EnumMember> <Description("RUTA DE PEDIMENTO DE RECTIFICACIÓN DE IMPORTACION NORMAL POR DEFECTO  CON SYNAPSIS QUALITY")> RUVA13 = 13
        <EnumMember> <Description("RUTA DE PEDIMENTO DE RECTIFICACIÓN DE IMPORTACION NORMAL PEDIMENTO CONSOLIDADO CON SYNAPSIS QUALITY")> RUVA14 = 14
        <EnumMember> <Description("RUTA DE PEDIMENTO DE RECTIFICACIÓN DE EXPORTACION NORMAL POR DEFECTO CON SYNAPSIS QUALITY")> RUVA15 = 15
        <EnumMember> <Description("RUTA DE PEDIMENTO DE RECTIFICACIÓN DE EXPORTACION NORMAL COMPLEMENTARIO CON SYNAPSIS QUALITY")> RUVA16 = 16
        <EnumMember> <Description("RUTA DE PEDIMENTO DE RECTIFICACIÓN DE EXPORTACION NORMAL PEDIMENTO CONSOLIDADO CON SYNAPSIS QUALITY")> RUVA17 = 17
        <EnumMember> <Description("RUTA DE PEDIMENTO DE RECTIFICACIÓN DE TRÁNSITO NORMAL POR DEFECTO CON SYNAPSIS QUALITY")> RUVA18 = 18
        <EnumMember> <Description("RUTA DE PEDIMENTO DE RECTIFICACIÓN DE EXPORTACION VIRTUAL POR DEFECTO CON SYNAPSIS QUALITY")> RUVA19 = 19
        <EnumMember> <Description("RUTA DE PEDIMENTO DE RECTIFICACIÓN DE IMPORTACION VIRTUAL POR DEFECTO CON SYNAPSIS QUALITY")> RUVA20 = 20
        <EnumMember> <Description("RUTA DE PEDIMENTO DE IMPORTACIÓN NORMAL POR DEFECTO  AL 100%")> RUVA21 = 21
        <EnumMember> <Description("RUTA DE PEDIMENTO DE IMPORTACIÓN NORMAL GLOBAL COMPLEMENTARIO AL 100%")> RUVA22 = 22
        <EnumMember> <Description("RUTA DE PEDIMENTO DE IMPORTACIÓN NORMAL PEDIMENTO CONSOLIDADO AL 100%")> RUVA23 = 23
        <EnumMember> <Description("RUTA DE PEDIMENTO DE EXPORTACIÓN NORMAL POR DEFECTO AL 100%")> RUVA24 = 24
        <EnumMember> <Description("RUTA DE PEDIMENTO DE EXPORTACIÓN NORMAL PEDIMENTO CONSOLIDADO AL 100%")> RUVA25 = 25
        <EnumMember> <Description("RUTA DE PEDIMENTO DE EXPORTACIÓN NORMAL GLOBAL COMPLEMENTARIO AL 100%")> RUVA26 = 26
        <EnumMember> <Description("RUTA DE PEDIMENTO DE EXPORTACIÓN NORMAL COMPLEMENTARIO AL 100%")> RUVA27 = 27
        <EnumMember> <Description("RUTA DE PEDIMENTO DE TRÁNSITO NORMAL POR DEFECTO AL 100%")> RUVA28 = 28
        <EnumMember> <Description("RUTA DE PEDIMENTO DE EXPORTACIÓN VIRTUAL POR DEFECTO AL 100%")> RUVA29 = 29
        <EnumMember> <Description("RUTA DE PEDIMENTO DE EXPORTACIÓN VIRTUAL PEDIMENTO CONSOLIDADO AL 100%")> RUVA30 = 30
        <EnumMember> <Description("RUTA DE PEDIMENTO DE IMPORTACIÓN VIRTUAL POR DEFECTO AL 100%")> RUVA31 = 31
        <EnumMember> <Description("RUTA DE PEDIMENTO DE IMPORTACIÓN VIRTUAL PEDIMENTO CONSOLIDADO AL 100%")> RUVA32 = 32
        <EnumMember> <Description("RUTA DE PEDIMENTO DE RECTIFICACIÓN DE IMPORTACION NORMAL POR DEFECTO AL 100%")> RUVA33 = 33
        <EnumMember> <Description("RUTA DE PEDIMENTO DE RECTIFICACIÓN DE IMPORTACION NORMAL PEDIMENTO CONSOLIDADO AL 100%")> RUVA34 = 34
        <EnumMember> <Description("RUTA DE PEDIMENTO DE RECTIFICACIÓN DE EXPORTACION NORMAL POR DEFECTO AL 100%")> RUVA35 = 35
        <EnumMember> <Description("RUTA DE PEDIMENTO DE RECTIFICACIÓN DE EXPORTACION NORMAL COMPLEMENTARIO AL 100%")> RUVA36 = 36
        <EnumMember> <Description("RUTA DE PEDIMENTO DE RECTIFICACIÓN DE EXPORTACION NORMAL PEDIMENTO CONSOLIDADO AL 100%")> RUVA37 = 37
        <EnumMember> <Description("RUTA DE PEDIMENTO DE RECTIFICACIÓN DE TRÁNSITO NORMAL POR DEFECTO AL 100%")> RUVA38 = 38
        <EnumMember> <Description("RUTA DE PEDIMENTO DE RECTIFICACIÓN DE EXPORTACION VIRTUAL POR DEFECTO AL 100%")> RUVA39 = 39
        <EnumMember> <Description("RUTA DE PEDIMENTO DE RECTIFICACIÓN DE IMPORTACION VIRTUAL POR DEFECTO AL 100%")> RUVA40 = 40

    End Enum

    Enum ValidationDocuments
        Undefined = 0
        <EnumMember> <Description("VALIDACIÓN DE DOCUMENTOS RUTA 1")> DOVA = 1

    End Enum

    Enum ValidationQuality
        Undefined = 0
        <EnumMember> <Description("CALIDAD SYNAPSYS")> SYNQUALITY = 1
        <EnumMember> <Description("SIN CALIDAD")> STDQUALITY = 2

    End Enum

    Enum RunAt
        Undefined = 0
        <EnumMember> <Description("Ejecutar Recámara del Cubo")> ROOMCUBE = 1
        <EnumMember> <Description("Ejecutar Condiciones Locales")> LOCALCONDITIONS = 2

    End Enum
#End Region

#Region "Properties"

    Property validationtarget As ValidationRoutes
    ReadOnly Property document As DocumentoElectronico
    ReadOnly Property status As TagWatcher
    ReadOnly Property validationstatus As TagWatcher
    ReadOnly Property report As ValidatorReport
    ReadOnly Property validationpanel As validationpanel
    ReadOnly Property folioperacion As String
    ReadOnly Property route(Optional route_ As IValidationRoute.ValidationRoutes? = Nothing,
                            Optional type_ As IValidationRoute.ValidationDocuments? = Nothing) As ValidatorReport
    Default Property ValidationRoute(quality_ As IValidationRoute.ValidationQuality?) As IValidationRoute

#End Region

#Region "Methods"

    Function ValidateDocument(document_ As DocumentoElectronico) As IValidationRoute

    Function Validate(Of T)(document_ As DocumentoElectronico, route_ As ValidationRoutes) As TagWatcher

#End Region

End Interface
