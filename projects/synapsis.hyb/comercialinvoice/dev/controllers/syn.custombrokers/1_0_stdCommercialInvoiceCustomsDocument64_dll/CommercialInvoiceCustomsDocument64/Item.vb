Imports MongoDB.Bson

Public Class Item
    Implements IItem

    Public Property sequence As Integer Implements IItem.sequence
    Public Property productid As ObjectId Implements IItem.productid
    Public Property sku As String Implements IItem.sku
    Public Property partnumber As String Implements IItem.partnumber
    Public Property quantity As Integer Implements IItem.quantity
    Public Property unit As String Implements IItem.unit
    Public Property description As String Implements IItem.description
    Public Property total As Double Implements IItem.total
    Public Property currency As String Implements IItem.currency
    Public Property usdvalue As Double Implements IItem.usdvalue
    Public Property value As Double Implements IItem.value
    Public Property discount As Decimal Implements IItem.discount
    Public Property unitprice As Double Implements IItem.unitprice
    Public Property netweight As Double Implements IItem.netweight
    Public Property purchaseorder As String Implements IItem.purchaseorder
    Public Property destinationcountry As String Implements IItem.destinationcountry
    Public Property destinationcountrykey As String Implements IItem.destinationcountrykey
    Public Property origincountry As String Implements IItem.origincountry
    Public Property origincountrykey As String Implements IItem.origincountrykey
    Public Property customsdeclarationdescription As String Implements IItem.customsdeclarationdescription
    Public Property valuecertificatedescription As String Implements IItem.valuecertificatedescription
    Public Property partnumberdescription As String Implements IItem.partnumberdescription
    Public Property partnumberkey As String Implements IItem.partnumberkey
    Public Property commercialquantity As Double Implements IItem.commercialquantity
    Public Property unitquantitycommercial As Integer Implements IItem.unitquantitycommercial
    Public Property commercialunitdescription As String Implements IItem.commercialunitdescription
    Public Property usdcurrency As String Implements IItem.usdcurrency
    Public Property merchandisevalue As Double Implements IItem.merchandisevalue
    Public Property merchandisecurrency As String Implements IItem.merchandisecurrency
    Public Property valueunitprice As Double Implements IItem.valueunitprice
    Public Property currencyunitprice As String Implements IItem.currencyunitprice
    Public Property valuationmethodkey As Integer Implements IItem.valuationmethodkey
    Public Property valuationmethoddescription As String Implements IItem.valuationmethoddescription
    Public Property tarifffraction As String Implements IItem.tarifffraction
    Public Property tarifffractiondescription As String Implements IItem.tarifffractiondescription
    Public Property tariffquantity As Double Implements IItem.tariffquantity
    Public Property tariffunitkey As Integer Implements IItem.tariffunitkey
    Public Property tariffunitdescription As String Implements IItem.tariffunitdescription
    Public Property nico As String Implements IItem.nico
    Public Property nicodescription As String Implements IItem.nicodescription
    Public Property lot As String Implements IItem.lot
    Public Property serial As String Implements IItem.serial
    Public Property brand As String Implements IItem.brand
    Public Property model As String Implements IItem.model
    Public Property submodel As String Implements IItem.submodel
    Public Property mileage As Integer Implements IItem.mileage
    Public Property status As Short Implements IItem.status
    Public Property archived As Boolean Implements IItem.archived
    Public Property valorAgregado As Double Implements IItem.valorAgregado
    Public Property monedaValorAgregado As String Implements IItem.monedaValorAgregado
    Public Property currencyvalueunitprice As String Implements IItem.currencyvalueunitprice

End Class
