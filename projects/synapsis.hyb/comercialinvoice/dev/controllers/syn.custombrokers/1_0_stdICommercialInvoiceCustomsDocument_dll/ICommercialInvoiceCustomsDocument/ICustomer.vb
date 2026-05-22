Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes

Public Interface ICustomer

    <BsonIgnoreIfNull>
    Property id As ObjectId

    Property name As String

    Property taxid As String

    Property address As String

    Property street As String

    Property city As String

    Property state As String

    Property country As String

    Property zipcode As String

    Property externalnumber As String

    Property internalnumber As String

    Property locality As String
    '--------------new--------------
    Property email As String
    '--------------new--------------
    Property phone As String
    '--------------new--------------
    Property curp As String
    Property rfc As String

End Interface
