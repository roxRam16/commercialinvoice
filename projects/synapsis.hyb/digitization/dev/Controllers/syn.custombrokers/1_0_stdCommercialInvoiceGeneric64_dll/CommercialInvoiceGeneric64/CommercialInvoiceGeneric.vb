Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes

<Serializable()>
Public Class CommercialInvoiceGeneric
    Implements ICommercialInvoiceGeneric
    Public Property _id As ObjectId _
        Implements ICommercialInvoiceGeneric._id

    Public Property invoicenumber As String _
        Implements ICommercialInvoiceGeneric.invoicenumber

    'Public Property invoicedate As Date _
    '    Implements ICommercialInvoice.invoicedate

    Public Property invoicedate As String _
        Implements ICommercialInvoiceGeneric.invoicedate

    Public Property invoiceseries As String _
        Implements ICommercialInvoiceGeneric.invoiceseries

    Public Property customername As String _
        Implements ICommercialInvoiceGeneric.customername

    Public Property suppliername As String _
        Implements ICommercialInvoiceGeneric.suppliername

    Public Property invoicecountry As String _
        Implements ICommercialInvoiceGeneric.invoicecountry

    Public Property totalinvoice As Double _
        Implements ICommercialInvoiceGeneric.totalinvoice

    Public Property invoicecurrency As String _
        Implements ICommercialInvoiceGeneric.invoicecurrency

    Public Property items As List(Of Item) _
        Implements ICommercialInvoiceGeneric.items

    <BsonIgnoreIfNull>
    Public Property origincountryinvoice As String

    Public Property customer As Customer _
        Implements ICommercialInvoiceGeneric.customer

    Public Property supplier As Supplier _
        Implements ICommercialInvoiceGeneric.supplier

    Public Property additionaldetails As AdditionalDetails _
        Implements ICommercialInvoiceGeneric.additionaldetails

    Public Property consigneedetails As ConsigneeDetails _
        Implements ICommercialInvoiceGeneric.consigneedetails

End Class
