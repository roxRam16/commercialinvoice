Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes

Public Class SubdivisionFacturaComercial
    Public Property id As ObjectId
    Public Property id_fact_sub As ObjectId
    Public Property sec As Int64
    Public Property numerofactura_subdivision As String
    Public Property numerofactura_original As String
    Public Property creacion As Date
    Public Property actualizacion As Date
    Public Property cantidad_unidad_comercial_total As Double
    Public Property unidad_medida_comercial_total As String
    Public Property valor_mercancia_total As Double
    Public Property moneda_valor_mercancia_total As String
    Public Property generado_por As DetalleUser
    Public Property publicado As Boolean
    Public Property url_subdivision As String
    Public Property items As List(Of ItemSubdividido)
    Public Property idfacturaoriginal As ObjectId
    Public Property valorfactura_general As Double
    Public Property moneda_valorfactura As String
    Public Property cve_moneda_valorfactura As String
    Public Property cve_moneda_mercancia As String
End Class



