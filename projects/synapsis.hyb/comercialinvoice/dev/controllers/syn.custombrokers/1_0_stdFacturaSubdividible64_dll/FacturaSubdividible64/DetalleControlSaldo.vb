Imports MongoDB.Bson

Public Class DetalleControlSaldo
    Public Property sec As Int64
    Public Property idProducto As ObjectId '
    Public Property numeropartida As Int64 '
    Public Property numeroparte As String '
    Public Property numeropartecompleto As String
    Public Property descripcion As String '
    Public Property cantidad_comercial_original As Double '
    Public Property cantidad_comercial_disponible As Double
    Public Property cve_unidad_medida_comercial As String
    Public Property unidad_medida_comercial As String '
    Public Property valor_mercancia_original As Double '
    Public Property idmoneda_valor_mercancia_original As String
    Public Property moneda_valor_mercancia_original As String '
    Public Property precio_unitario_original As Double '
    Public Property idmoneda_precio_unitario_original As String
    Public Property moneda_precio_unitario_original As String '
    Public Property disponible As Boolean
    Public Property unidad_parcializable As Boolean
    Public Property cantidad_tarifa_original As Double
    Public Property cve_unidad_medida_tarifa As String
    Public Property unidad_medida_tarifa As String
    Public Property descripcion_merca_original As String
    Public Property descripcion_merca_cove As String
    Public Property val_fact_partida As Double
    Public Property idmoneda_val_fact_partida As String
    Public Property moneda_val_fact_partida As String
    Public Property peso_neto_partida As Double
    Public Property pais_origen As String
    Public Property id_pais_origen As String
    Public Property cve_metodo_val_partida As String
    Public Property metodo_val_partida As String
    Public Property orden_compra_partida As String
    Public Property fraccion As String
    Public Property fraccion_descripcion As String
    Public Property nico As String
    Public Property nico_descripcion As String
    Public Property lote_part As String
    Public Property numero_serie_part As String
    Public Property marca_part As String
    Public Property modelo_part As String
    Public Property submodelo_part As String
    Public Property kilometraje_part As String
    Public Property timestamp_part As String
    Public Property identity As Int32
End Class
