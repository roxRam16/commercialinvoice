Imports System.IO
Imports System.Reflection.Emit
Imports System.Runtime.CompilerServices
Imports Ia.Pln
Imports MongoDB.Bson
Imports Syn.Documento
Imports Wma.Exceptions
Public Interface IControladorProcesamientoElectronico
#Region "Enum"
    Enum ListaTiposDocumentos
        SIN_DEFINIR = 0
        FACTURA_COMERCIAL_IMPORTACION_PDF = 1
        FACTURA_COMERCIAL_EXPORTACION_PDF = 2
        FACTURA_COMERCIAL_IMPORTACION_CFDI = 3
        FACTURA_COMERCIAL_EXPORTACION_CFDI = 4
    End Enum

#End Region

#Region "Propiedades"
    Property Temperature As Int32
    'Property Transformer As IControllerDocumentAnalyzer.Transformer
    'Property DocumentoCargado As IControllerChatGPT.DocumentoCargado
    Property Estado As TagWatcher
    Property EstadoAsync As Task(Of TagWatcher)
#End Region

#Region "Métodos"
    Sub ReiniciarControlador()
    Function GenerarDocumento(Of T)(ByVal estructura_ As T) As TagWatcher
    Function GuardarEstructuraDocumento(Of T)(ByVal estructura_ As T) As TagWatcher
    Function ProcesarArchivoConIA(ByVal archivo_ As MemoryStream) As TagWatcher
    Function ProcesarArchivosConIA(ByVal listaArchivos_ As List(Of MemoryStream)) As TagWatcher
    Function ProcesarCFDI(ByVal archivo_ As MemoryStream) As TagWatcher
    Function ProcesarCFDIs(ByVal listaArchivos_ As List(Of MemoryStream)) As TagWatcher
    Function GenerarFacturaComercial(ByVal estructura_ As CommercialInvoiceAnalysis, ByVal objectidCliente_ As ObjectId) As TagWatcher
    Function DeserializeCFDI(ByVal xml_ As String) As CFDIFacturaComercial
    Function GenerarCommercialInvoiceDesdeCFDI(cfdi As CFDIFacturaComercial) As CommercialInvoiceAnalysis
#End Region

End Interface
