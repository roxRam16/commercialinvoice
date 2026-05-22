Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes

Public Interface ICommercialInvoiceCustomsDocument

    <BsonIgnoreIfNull>
    Property _id As ObjectId

    Property invoicenumber As String

    Property invoicedate As String

    Property invoiceseries As String

    '--------------new--------------
    Property operationtype As String

    '--------------new--------------
    Property initialdataload As String

    '--------------new--------------
    Property valuecertificate As String

    '--------------new--------------
    Property valuecertificatedate As String

    '--------------new--------------
    Property idvaluecertificate As ObjectId

    '--------------new--------------
    Property hassubdivision As Boolean

    '--------------new--------------
    Property hasalienation As Boolean

    '--------------new--------------
    Property hasadditions As Boolean


    '--------------new-------------- cvenumericaincortem
    Property incotermnumerickey As String

    '--------------new-------------- cveincortem
    Property incotermkey As String

    '--------------new--------------
    Property incoterm As String

    Property invoiceusd As String

    '--------------new-------------- invoicemercancia
    Property merchandisevalue As Double

    '--------------new-------------- accurrancymercancia
    Property merchandisecurrency As String

    '--------------new--------------
    Property invoiceweight As Double

    '--------------new-------------- invoicebultos
    Property invoicepackages As Double

    '--------------new-------------- pushesorder
    Property purchaseorderref As String

    '--------------new-------------- referenciacustomer
    Property customerreference As String

    Property customername As String
    '--------------new--------------
    Property customerid As ObjectId
    '--------------new-------------- cvecustomername
    Property customernamekey As String

    <BsonIgnoreIfNull>
    Property customer As ICustomer

    '--------------new--------------
    Property supplierid As ObjectId

    '--------------new--------------
    Property idsuppliertaxid As ObjectId

    '--------------new--------------
    Property suppliernamekey As String

    Property suppliername As String

    <BsonIgnoreIfNull>
    Property supplier As ISupplier

    '--------------new--------------
    Property invoicecountryid As String

    '--------------new-------------- cveinvoicecountry
    Property invoicecountrykey As String

    Property invoicecountry As String

    Property totalinvoice As Double

    Property invoicecurrency As String

    Property items As List(Of IItem)


    '--------------new--------------
    Property observations As String


    '--------------new--------------
    Property consigneeobjectid As ObjectId

    '--------------new-------------- claveconsigneed
    Property consigneekey As String

    '--------------new-------------- consigneedname
    Property consigneename As String

    <BsonIgnoreIfNull>
    Property consigneedetails As IConsigneeDetails


    '--------------new--------------
    Property additions As IList(Of IIncrementable)

    <BsonIgnoreIfNull>
    Property marcadopedimento As Boolean

    <BsonIgnoreIfNull>
    Property idpedimentoasociado As ObjectId

    <BsonIgnoreIfNull>
    Property numfactura_subdivision As String

End Interface