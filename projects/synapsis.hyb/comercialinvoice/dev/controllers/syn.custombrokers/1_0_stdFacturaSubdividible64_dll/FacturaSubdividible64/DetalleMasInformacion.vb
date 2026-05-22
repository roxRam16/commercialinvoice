Imports MongoDB.Bson.Serialization.Attributes

Public Class DetalleMasInformacion
    <BsonIgnoreIfNull>
    Public Property fecha_factura_original As Date
    <BsonIgnoreIfNull>
    Public Property numero_acuse_valor As String
    <BsonIgnoreIfNull>
    Public Property FechaCreacion As Date
    <BsonIgnoreIfNull>
    Public Property UsuarioGenerador As String
End Class
