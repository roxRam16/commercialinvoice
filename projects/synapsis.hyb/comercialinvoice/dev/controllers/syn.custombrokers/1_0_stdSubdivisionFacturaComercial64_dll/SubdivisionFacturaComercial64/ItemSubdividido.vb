Imports MongoDB.Bson

Public Class ItemSubdividido
    Enum TipoRequisicion
        Completo = 1
        Parcial = 2
    End Enum

    Public Property sec As Int64 '
    Public Property id_producto As ObjectId '
    Public Property numero_partida As Int64 '
    Public Property numero_parte As String '
    Public Property numeropartecompleto As String
    Public Property cantidad_comercial_requerida As Double '
    Public Property unidad_comercial As String '
    Public Property descripcion_partida As String '
    Public Property precio_unitario As Double '
    Public Property moneda_precio_unitario As String '
    Public Property valor_mercancia As Double '
    Public Property moneda_mercancia As String '
    Public Property cantidad_tarifa_requerida As Double '
    Public Property unidad_medida_tarifa As String '
    Public Property tipo_requisicion As TipoRequisicion '
    Public Property estado As Int64 '
    Public Property cve_unidad_medida_comercial As String '
    Public Property idmoneda_valor_mercancia_original As String '
    Public Property idmoneda_precio_unitario_original As String '
    Public Property cve_unidad_medida_tarifa As String '
    Public Property descripcion_merca_original As String '
    Public Property descripcion_merca_cove As String '
    Public Property idmoneda_val_fact_partida As String '
    Public Property moneda_val_fact_partida As String '
    Public Property peso_neto_partida As Double '
    Public Property pais_origen As String '
    Public Property id_pais_origen As String '
    Public Property cve_metodo_val_partida As String '
    Public Property metodo_val_partida As String
    Public Property orden_compra_partida As String '
    Public Property fraccion As String '
    Public Property fraccion_descripcion As String '
    Public Property nico As String '
    Public Property nico_descripcion As String '
    Public Property lote_part As String '
    Public Property numero_serie_part As String '
    Public Property marca_part As String '
    Public Property modelo_part As String '
    Public Property submodelo_part As String '
    Public Property kilometraje_part As String '
    Public Property timestamp_part As String '
    Public Property val_fact_partida As Double '
    Public Property identity As Int32 '

End Class