Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes

Public Interface ISupplier

    <BsonIgnoreIfNull>
    Property id As ObjectId

    Property name As String

    Property taxid As String

    Property address As String

    Property city As String

    Property state As String

    Property country As String

    Property zipcode As String

    Property street As String

    Property externalnumber As String

    Property internalnumber As String

    Property locality As String
    '--------------new--------------
    Property email As String
    '--------------new--------------
    Property phone As String

    '--------------new--------------curpsupplier
    Property curp As String

    Property rfc As String

    '--------------new-------------- clavevinculacion
    Property linkagekey As String

    '--------------new-------------- descriptionvinculacion
    Property linkagedescription As String

    '--------------new-------------- cvemetodovaloracion
    Property valuationmethodkey As String

    '--------------new-------------- descriptionmetodovaloracion
    Property valuationmethoddescription As String

    '--------------new-------------- aplicacertificado
    Property appliescertificate As Boolean

    '--------------new--------------nombrecertificador
    Property certifiername As String

    '--------------new-------------- esdestinatario
    Property isconsignee As Boolean

End Interface
