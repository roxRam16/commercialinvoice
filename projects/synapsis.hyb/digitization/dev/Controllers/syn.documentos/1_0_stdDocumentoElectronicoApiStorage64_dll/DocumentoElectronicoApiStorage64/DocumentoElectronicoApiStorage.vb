Imports System
Imports System.Collections.Generic
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes

' --------- MODELO PRINCIPAL ---------
<Serializable()>
Public Class RootRequest
    Public Property payloads As List(Of DocumentoElectronicoApiStorage)
End Class

<Serializable()>
Public Class DocumentoElectronicoApiStorage
    <BsonIgnoreIfNull>
    Public Property _id As String
    Public Property file_model_64 As List(Of FileBase64)
    Public Property customerid As String
    Public Property customer_name As String
    Public Property taxid As String
    Public Property environment As String
    Public Property environmentid As Integer?
    Public Property business_unit As String
    Public Property business_unitid As Integer
    Public Property section As String
    Public Property use_type As String
    Public Property document_type As Integer
    Public Property content_type As String
    <BsonIgnoreIfNull>
    Public Property use_type_description As String
    <BsonIgnoreIfNull>
    Public Property sub_type As String
    <BsonIgnoreIfNull>
    Public Property description As String
    Public Property file_name_origin As String
    Public Property owner As Owner
    <BsonIgnoreIfNull>
    Public Property content_tag As List(Of String)
    <BsonIgnoreIfNull>
    Public Property bucket_type As String
    Public Property upload_year As Integer
    Public Property month_upload As Integer
    Public Property type_operation As String
End Class


' --------- CLASES AUXILIARES ---------
Public Class FileBase64
    Public Property filename_64 As String
    Public Property content_type As String
    Public Property base64_data As String
End Class

Public Class Owner
    Public Property name As String
    Public Property user As String
    Public Property userid As String
End Class

Public Class MetadataGCS
    <BsonIgnoreIfNull>
    Public Property Bucket As String
    <BsonIgnoreIfNull>
    Public Property SizeBytes As Integer
    <BsonIgnoreIfNull>
    Public Property MimeType As String
    <BsonIgnoreIfNull>
    Public Property ProjectId As String
    <BsonIgnoreIfNull>
    Public Property Path As String
    <BsonIgnoreIfNull>
    Public Property Filename As String
    <BsonIgnoreIfNull>
    Public Property UrlFirmada As String
    <BsonIgnoreIfNull>
    Public Property ExpiresAt As DateTime?
    <BsonIgnoreIfNull>
    Public Property Updated As DateTime?
    <BsonIgnoreIfNull>
    Public Property Generation As String
    <BsonIgnoreIfNull>
    Public Property Metageneration As String
    <BsonIgnoreIfNull>
    Public Property Etag As String
    <BsonIgnoreIfNull>
    Public Property Owner As String
    <BsonIgnoreIfNull>
    Public Property ComponentCount As String
    <BsonIgnoreIfNull>
    Public Property Md5Hash As String
    <BsonIgnoreIfNull>
    Public Property CacheControl As String
    <BsonIgnoreIfNull>
    Public Property ContentType As String
    <BsonIgnoreIfNull>
    Public Property ContentDisposition As String
    <BsonIgnoreIfNull>
    Public Property ContentEncoding As String
    <BsonIgnoreIfNull>
    Public Property ContentLanguage As String
    <BsonIgnoreIfNull>
    Public Property Metadata As String
    <BsonIgnoreIfNull>
    Public Property MediaLink As String
    <BsonIgnoreIfNull>
    Public Property CustomTime As DateTime?
    <BsonIgnoreIfNull>
    Public Property TemporaryHold As String
End Class
