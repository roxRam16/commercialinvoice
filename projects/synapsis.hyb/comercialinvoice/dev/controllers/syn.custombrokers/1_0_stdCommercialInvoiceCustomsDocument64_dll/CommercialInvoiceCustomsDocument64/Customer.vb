Imports MongoDB.Bson

Public Class Customer
    Implements ICustomer

    Public Property id As ObjectId Implements ICustomer.id
    Public Property name As String Implements ICustomer.name
    Public Property taxid As String Implements ICustomer.taxid
    Public Property address As String Implements ICustomer.address
    Public Property street As String Implements ICustomer.street
    Public Property city As String Implements ICustomer.city
    Public Property state As String Implements ICustomer.state
    Public Property country As String Implements ICustomer.country
    Public Property zipcode As String Implements ICustomer.zipcode
    Public Property externalnumber As String Implements ICustomer.externalnumber
    Public Property internalnumber As String Implements ICustomer.internalnumber
    Public Property locality As String Implements ICustomer.locality
    Public Property email As String Implements ICustomer.email
    Public Property phone As String Implements ICustomer.phone
    Public Property curp As String Implements ICustomer.curp
    Public Property rfc As String Implements ICustomer.rfc

End Class
