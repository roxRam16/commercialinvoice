Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes

Public Class AuxiliarDatosExpedienteElectronico
    <BsonIgnoreIfNull>
    Public Property _id As ObjectId
    <BsonIgnoreIfNull>
    Public Property idcliente As ObjectId

    Public Property razonsocialCliente As String

    Public Property taxidCliente As String

    Public Property idenvironment As Int16

    Public Property environment As String

    Public Property bussinessUnitId As Int16

    Public Property bussinessUnit As String

    <BsonIgnoreIfNull>
    Public Property datosReferencia As List(Of AuxliarDatosReferencia)

    <BsonIgnoreIfNull>
    Public Property messages As List(Of MessagesExpediente)

    Public Property totalReferenciasCerradas As Int16

    Public Property totalReferenciasAbiertas As Int16

    Public Property totalDocumentosSinReferencia As Int16

    <BsonIgnoreIfNull>
    Public Property digitalkeyid As ObjectId

    <BsonIgnoreIfNull>
    Public Property digitalkey As String

    <BsonIgnoreIfNull>
    Public Property ownerid As ObjectId

    <BsonIgnoreIfNull>
    Public Property owner_user As String

    <BsonIgnoreIfNull>
    Public Property owner_name As String

    Public Property fechaApertura As DateTime

    <BsonIgnoreIfNull>
    Public Property ultimaActualizacion As DateTime

End Class

Public Class MessagesExpediente
    Public Property sec As Int16

    Public Property tipo As Int16

    Public Property message As String

    Public Property nivelMessage As String

    Public Property statusMessage As Int16

End Class

Public Class AuxliarDatosReferencia
    Public Property idreferencia As ObjectId

    Public Property referencia As String

    <BsonIgnoreIfNull>
    Public Property totalDocumentosCerrados As Int16

    <BsonIgnoreIfNull>
    Public Property totalDocumentosAbiertos As Int16

    <BsonIgnoreIfNull>
    Public Property estatus As Boolean

End Class
