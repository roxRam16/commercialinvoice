Imports Ia.Analysis
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports Syn.CustomBrokers.Controllers.Digitalization

<Serializable()>
Public Class CommercialInvoiceAnalysis
    Implements ICommercialInvoiceGeneric, IAnalysisDocument

    Public Property _id As ObjectId Implements ICommercialInvoiceGeneric._id

    Public Property invoicenumber As String Implements ICommercialInvoiceGeneric.invoicenumber

    Public Property invoicedate As String Implements ICommercialInvoiceGeneric.invoicedate

    Public Property invoiceseries As String Implements ICommercialInvoiceGeneric.invoiceseries

    Public Property customername As String Implements ICommercialInvoiceGeneric.customername

    Public Property suppliername As String Implements ICommercialInvoiceGeneric.suppliername

    Public Property invoicecountry As String Implements ICommercialInvoiceGeneric.invoicecountry

    Public Property totalinvoice As Double Implements ICommercialInvoiceGeneric.totalinvoice

    Public Property invoicecurrency As String Implements ICommercialInvoiceGeneric.invoicecurrency

    Public Property customer As Customer Implements ICommercialInvoiceGeneric.customer

    Public Property supplier As Supplier Implements ICommercialInvoiceGeneric.supplier

    Public Property items As List(Of Item) Implements ICommercialInvoiceGeneric.items

    Public Property additionaldetails As AdditionalDetails Implements ICommercialInvoiceGeneric.additionaldetails

    Public Property consigneedetails As ConsigneeDetails Implements ICommercialInvoiceGeneric.consigneedetails

    Public Property processdate As Date Implements IAnalysisDocument.processdate

    Public Property environmentid As Integer Implements IAnalysisDocument.environmentid

    Public Property confidence As Double Implements IAnalysisDocument.confidence

    Public Property info As String Implements IAnalysisDocument.info

    Public Property score As Double Implements IAnalysisDocument.score

    Public Property analysis As Analysis Implements IAnalysisDocument.analysis

End Class
