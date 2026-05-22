Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes

Public Interface IConsigneeDetails

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

    Property contact As String

    Property email As String

    Property phone As String

    '--------------new-------------- objectidconsigneed
    Property id As ObjectId

    '--------------new-------------- claveconsigneed
    Property key As String

    '--------------new-------------- objectiddomicilioconsigneed
    Property addressid As ObjectId

    '--------------new-------------- secdomicilioconsigneed
    Property addresssequence As String

    '--------------new-------------- domiciliofiscalconsigneed
    Property taxaddress As String
    Property rfc As String


End Interface
