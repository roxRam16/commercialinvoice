Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes

<Serializable()>
Public Class CommercialInvoiceCustomsDocument
    Implements ICommercialInvoiceCustomsDocument

    Public Property _id As ObjectId Implements ICommercialInvoiceCustomsDocument._id

    Public Property invoicenumber As String Implements ICommercialInvoiceCustomsDocument.invoicenumber

    Public Property invoicedate As String Implements ICommercialInvoiceCustomsDocument.invoicedate

    Public Property invoiceseries As String Implements ICommercialInvoiceCustomsDocument.invoiceseries

    Public Property operationtype As String Implements ICommercialInvoiceCustomsDocument.operationtype

    Public Property initialdataload As String Implements ICommercialInvoiceCustomsDocument.initialdataload

    Public Property valuecertificate As String Implements ICommercialInvoiceCustomsDocument.valuecertificate

    Public Property valuecertificatedate As String Implements ICommercialInvoiceCustomsDocument.valuecertificatedate

    Public Property idvaluecertificate As ObjectId Implements ICommercialInvoiceCustomsDocument.idvaluecertificate

    Public Property hassubdivision As Boolean Implements ICommercialInvoiceCustomsDocument.hassubdivision

    Public Property hasalienation As Boolean Implements ICommercialInvoiceCustomsDocument.hasalienation

    Public Property hasadditions As Boolean Implements ICommercialInvoiceCustomsDocument.hasadditions

    Public Property incotermnumerickey As String Implements ICommercialInvoiceCustomsDocument.incotermnumerickey

    Public Property incotermkey As String Implements ICommercialInvoiceCustomsDocument.incotermkey

    Public Property incoterm As String Implements ICommercialInvoiceCustomsDocument.incoterm

    Public Property invoiceusd As String Implements ICommercialInvoiceCustomsDocument.invoiceusd

    Public Property merchandisevalue As Double Implements ICommercialInvoiceCustomsDocument.merchandisevalue

    Public Property merchandisecurrency As String Implements ICommercialInvoiceCustomsDocument.merchandisecurrency

    Public Property invoiceweight As Double Implements ICommercialInvoiceCustomsDocument.invoiceweight

    Public Property invoicepackages As Double Implements ICommercialInvoiceCustomsDocument.invoicepackages

    Public Property purchaseorderref As String Implements ICommercialInvoiceCustomsDocument.purchaseorderref

    Public Property customerreference As String Implements ICommercialInvoiceCustomsDocument.customerreference

    Public Property customername As String Implements ICommercialInvoiceCustomsDocument.customername

    Public Property customerid As ObjectId Implements ICommercialInvoiceCustomsDocument.customerid

    Public Property customernamekey As String Implements ICommercialInvoiceCustomsDocument.customernamekey

    Public Property customer As ICustomer Implements ICommercialInvoiceCustomsDocument.customer

    Public Property supplierid As ObjectId Implements ICommercialInvoiceCustomsDocument.supplierid

    Public Property idsuppliertaxid As ObjectId Implements ICommercialInvoiceCustomsDocument.idsuppliertaxid

    Public Property suppliernamekey As String Implements ICommercialInvoiceCustomsDocument.suppliernamekey

    Public Property suppliername As String Implements ICommercialInvoiceCustomsDocument.suppliername

    Public Property supplier As ISupplier Implements ICommercialInvoiceCustomsDocument.supplier

    Public Property invoicecountryid As String Implements ICommercialInvoiceCustomsDocument.invoicecountryid

    Public Property invoicecountrykey As String Implements ICommercialInvoiceCustomsDocument.invoicecountrykey

    Public Property invoicecountry As String Implements ICommercialInvoiceCustomsDocument.invoicecountry

    Public Property totalinvoice As Double Implements ICommercialInvoiceCustomsDocument.totalinvoice

    Public Property invoicecurrency As String Implements ICommercialInvoiceCustomsDocument.invoicecurrency

    Public Property items As List(Of IItem) Implements ICommercialInvoiceCustomsDocument.items

    Public Property observations As String Implements ICommercialInvoiceCustomsDocument.observations

    Public Property consigneeobjectid As ObjectId Implements ICommercialInvoiceCustomsDocument.consigneeobjectid

    Public Property consigneekey As String Implements ICommercialInvoiceCustomsDocument.consigneekey

    Public Property consigneename As String Implements ICommercialInvoiceCustomsDocument.consigneename

    Public Property consigneedetails As IConsigneeDetails Implements ICommercialInvoiceCustomsDocument.consigneedetails

    Public Property additions As IList(Of IIncrementable) Implements ICommercialInvoiceCustomsDocument.additions

    Public Property marcadopedimento As Boolean Implements ICommercialInvoiceCustomsDocument.marcadopedimento

    Public Property idpedimentoasociado As ObjectId Implements ICommercialInvoiceCustomsDocument.idpedimentoasociado

    Public Property numfactura_subdivision As String Implements ICommercialInvoiceCustomsDocument.numfactura_subdivision

End Class
