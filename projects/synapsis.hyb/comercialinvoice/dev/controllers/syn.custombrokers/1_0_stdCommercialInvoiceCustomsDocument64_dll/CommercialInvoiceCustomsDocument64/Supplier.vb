Imports MongoDB.Bson

Public Class Supplier
    Implements ISupplier

    Public Property id As ObjectId Implements ISupplier.id
    Public Property name As String Implements ISupplier.name
    Public Property taxid As String Implements ISupplier.taxid
    Public Property address As String Implements ISupplier.address
    Public Property city As String Implements ISupplier.city
    Public Property state As String Implements ISupplier.state
    Public Property country As String Implements ISupplier.country
    Public Property zipcode As String Implements ISupplier.zipcode
    Public Property street As String Implements ISupplier.street
    Public Property externalnumber As String Implements ISupplier.externalnumber
    Public Property internalnumber As String Implements ISupplier.internalnumber
    Public Property locality As String Implements ISupplier.locality
    Public Property email As String Implements ISupplier.email
    Public Property phone As String Implements ISupplier.phone
    Public Property curp As String Implements ISupplier.curp
    Public Property rfc As String Implements ISupplier.rfc
    Public Property linkagekey As String Implements ISupplier.linkagekey 'cve vinculacion
    Public Property linkagedescription As String Implements ISupplier.linkagedescription 'descripcion vinculacion
    Public Property valuationmethodkey As String Implements ISupplier.valuationmethodkey 'cve metodo valoracion
    Public Property valuationmethoddescription As String Implements ISupplier.valuationmethoddescription 'descripcion metodo valoracion
    Public Property appliescertificate As Boolean Implements ISupplier.appliescertificate 'aplica certificado
    Public Property certifiername As String Implements ISupplier.certifiername 'nombre del certificador
    Public Property isconsignee As Boolean Implements ISupplier.isconsignee 'el proveedor actua como destinario

End Class
