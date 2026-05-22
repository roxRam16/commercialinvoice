Imports MongoDB.Bson

Public Class ConsigneeDetails
    Implements IConsigneeDetails

    Public Property name As String Implements IConsigneeDetails.name
    Public Property taxid As String Implements IConsigneeDetails.taxid
    Public Property address As String Implements IConsigneeDetails.address
    Public Property city As String Implements IConsigneeDetails.city
    Public Property state As String Implements IConsigneeDetails.state
    Public Property country As String Implements IConsigneeDetails.country
    Public Property zipcode As String Implements IConsigneeDetails.zipcode
    Public Property street As String Implements IConsigneeDetails.street
    Public Property externalnumber As String Implements IConsigneeDetails.externalnumber
    Public Property internalnumber As String Implements IConsigneeDetails.internalnumber
    Public Property locality As String Implements IConsigneeDetails.locality
    Public Property contact As String Implements IConsigneeDetails.contact
    Public Property email As String Implements IConsigneeDetails.email
    Public Property phone As String Implements IConsigneeDetails.phone
    Public Property id As ObjectId Implements IConsigneeDetails.id
    Public Property key As String Implements IConsigneeDetails.key
    Public Property addressid As ObjectId Implements IConsigneeDetails.addressid
    Public Property addresssequence As String Implements IConsigneeDetails.addresssequence
    Public Property taxaddress As String Implements IConsigneeDetails.taxaddress
    Public Property rfc As String Implements IConsigneeDetails.rfc

End Class
