Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes

Public Class FacturaSubdividible
    Enum EstadoSubdivision
        Abierta = 0
        SaldoPendiente = 1
        Cerrada = 2
    End Enum

    Enum TipoCierre
        SinCierre = 0
        CierreAutomatico = 1
        CierreManual = 2
    End Enum

    Public Property id As ObjectId
    Public Property idfacturaoriginal As ObjectId
    Public Property numerofactura As String
    Public Property cliente As String
    Public Property proveedor As String
    Public Property valorfactura_general As Double
    Public Property cve_moneda_valorfactura As String
    Public Property moneda_valorfactura As String
    Public Property valormercancia_general As Double
    Public Property cve_moneda_mercancia As String
    Public Property moneda_mercancia_general As String
    <BsonIgnoreIfNull>
    Public Property actualizacion As Date
    <BsonIgnoreIfNull>
    Public Property status_id As EstadoSubdivision
    <BsonIgnoreIfNull>
    Public Property tipo_cierre As TipoCierre
    <BsonIgnoreIfNull>
    Public Property cierre_manual As DetalleCierreManual
    <BsonIgnoreIfNull>
    Public Property mas_informacion As DetalleMasInformacion
    <BsonIgnoreIfNull>
    Public Property control_saldos As List(Of DetalleControlSaldo)
    Public Property estado As Int64
End Class

